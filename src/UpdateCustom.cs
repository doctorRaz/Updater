using drz.Updater;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;


namespace drz.UpdatePrep
{
    /// <summary>
    ///Подготовка пакета обновления
    ///описание в XML и упаковка
    /// </summary>
    public class XmlWriter
    {
        public XmlWriter()
        {
            XDOC = new XDocument();
            ROOT = new XElement("root");
            XDOC.Add(ROOT);//цепляем к doc
            ofd = new OpenFileDialog();
        }

        #region Пути

        /// <summary> директория приложения 
        /// <br> задаем один раз потом не меняем</br>
        /// <br> относительно этого пути будет все собираться</br>
        /// </summary>
        internal string sDirFiles { get; set; }

        /// <summary>
        /// папочка куда предварительно копируем компоненты приложения
        /// </summary>
        internal string sDirTemp { get; set; }

        /// <summary>файл приложения, библиотек и прочих файлов</summary>
        string sFilePrg { get; set; }

        /// <summary>файлы приложения, библиотек и прочих файлов</summary>
        string[] sFilePrgs { get; set; }

        #endregion

        /// <summary>инфа о файле проекта </summary>        
        FileVersionInfo versionInfPrj => FileVersionInfo.GetVersionInfo(sFilePrg);

        /// <summary>Сообщения о ошибках</summary>
        internal string sErr { get; set; }

        #region XML

        /// <summary>
        /// XMLdoc
        /// </summary>
        internal XDocument XDOC { get; set; }

        /// <summary>
        /// root
        /// </summary>
        XElement ROOT { get; set; }

        /// <summary>
        /// Имя по умолчанию для сохранения XML
        /// </summary>
        internal string sFullNameXML { get; set; }

        #endregion

        #region OFD

        /// <summary>файлы приложения</summary>
        internal OpenFileDialog ofd { get; set; }


        #endregion

        #region Маски исключений
        /// Маски исключений файлов, которые не брать
        /// </summary>
        List<string> arrExcludedSupportedExt
        {
            get
            {
                return new List<string>
                {
                    "*.bak",
                    "*.pdb",
                    "*.json",
                    "*.package",
                    "*.packagedescription"
                };
            }
        }

        #endregion

        //***R

        /// <summary>
        /// писатель обновления, что нового
        /// </summary>
        internal bool XmlDescriptorWriter
        {
            get
            {
                //? описатель обновления

                //!Descriptions
                XElement Descriptions = new XElement("Descriptions");
                ROOT.Add(Descriptions);


                return true;
            }
        }

        /// <summary>
        /// писатель свойств пакадже
        /// </summary>
        internal bool XmlPackageWriter
        {
            get
            {
                #region OFD Package
                ofd.Multiselect = true;
                ofd.Title = "Выбери файлы Package, можно несколько";
                ofd.Filter = "Все файлы (*.*)|*.*|"
                             + ".package (*.package)|"
                             + "*.package";
                ofd.FilterIndex = 2;
                ofd.RestoreDirectory = true;

                //!получим файлы Package            
                if (ofd.ShowDialog() != DialogResult.OK)
                {
                    sErr = "Файл Package не загружен!\nПользователь отказался!";
                    return false;
                }

                sFilePrgs = ofd.FileNames;
                sFilePrg = ofd.FileName;

                #endregion

                //!Packages
                XElement Packages = new XElement("Packages");
                ROOT.Add(Packages);

                foreach (string fil in sFilePrgs)
                {
                    //!Package
                    //think добавлять файл пакадж в папку для архива
                    sFilePrg = fil;
                    XElement Package = new XElement("Package"/*, versionInfoMod.ProductName*/);
                    Packages.Add(Package);
#if NF
                    Package.Add(new XAttribute("RefPath", PathNetCore.GetRelativePath(sDirFiles, sFilePrg)));//заглушка для фрэмворка, относительный путь
#else
                    Package.Add(new XAttribute("RefPath", Path.GetRelativePath(sDirFiles, sFilePrg)));
#endif
                    Package.Add(new XAttribute("FileName", Path.GetFileName(sFilePrg)));
                }

                return true;
            }
        }

        /// <summary>
        ////писатель свойств файлов
        /// </summary>
        internal bool XmlPropWriter
        {//think растащить все по классам
            get
            {
                #region OFD Prop
                ofd.Multiselect = false;
                ofd.Title = "Выбери главный файл проекта";
                ofd.Filter = "Все файлы (*.*)|*.*|"
                             + ".NET assemblies (*.exe;*.dll)|"
                             + "*.exe;*.dll";
                ofd.FilterIndex = 2;
                ofd.RestoreDirectory = true;

                //!получим файлы приложения            
                if (ofd.ShowDialog() != DialogResult.OK)
                {
                    sErr = "Файл DLL не загружен!\nПользователь отказался!";
                    return false;
                }
                sFilePrgs = ofd.FileNames;
                sFilePrg = ofd.FileName;

                #endregion

                #region проверка годится ли файл есть ли там FileVersion

                foreach (string item in sFilePrgs)
                {
                    sFilePrg = item;
                    Console.WriteLine("Project: " + sFilePrg);
                    if (string.IsNullOrEmpty(versionInfPrj.FileVersion))
                    {
                        sErr = "Неподходящий файл для главного файла проекта!"
                            + "\n"
                            + sFilePrg;
                        return false;
                    }
                }

                #endregion

                //фильтр исключаемых масок
                string sExcludedSupportedExt = string.Join(",", arrExcludedSupportedExt);

                //!+ корневая директория приложения /прибита гвоздями/
                sDirFiles = Directory.GetParent(sFilePrg).FullName;




                #region XML file name      
                //директория XML над каталогом приложения
                string sDirParent = Directory.GetParent(sDirFiles).FullName;

                string sProductName = versionInfPrj.ProductName;
                //до минора версия
                Version vFileVersion = new Version(versionInfPrj.FileVersion);

                string sMinorVersion = vFileVersion.Major + "." + vFileVersion.Minor;

                //имя файла XML
                string sShortNameXML = sProductName + "_" + sMinorVersion + ".packagedescription";//.xml";//? имя приложения PlotSPDS

                //!полный путь к XML 
                sFullNameXML = Path.Combine(sDirParent, sShortNameXML);

                #endregion
                //***

                //? проверить есть ли папочка в темпе, если есть прибить и создать по новой, не забыть по окончании опять прибить,
                //? подсмотреть в печати
                //собираем путь папочки  темпе
                //темп продукт минор
                string sDirTemp = Path.GetTempPath();
                string sDirFolderTmp = Path.Combine(Path.GetTempPath(), sProductName + "_" + sMinorVersion);
                bool isExistDirFolderTmp = Directory.Exists(sDirFolderTmp);

                #region Header XML

                //think возможно надо добавить дату и прочие служебные данные

                //!Projects
                XElement Projects = new XElement("Projects");
                ROOT.Add(Projects);

                //!Modules
                XElement Modules = new XElement("Modules");
                ROOT.Add(Modules);



                #endregion

                //пошли перебирать файлы
                foreach (string file in Directory.GetFiles(sDirFiles, "*.*", SearchOption.AllDirectories).Where(s => !sExcludedSupportedExt.Contains(Path.GetExtension(s).ToLower())))
                {
                    //think возможно здесь же их собирать в темп/имя приложения для архивации

                    sFilePrg = file;

                    //!+ если файл имеет FileVersion то в Project иначе в модули
                    if (!string.IsNullOrEmpty(versionInfPrj.FileVersion))
                    {
                        #region Projects
                        XElement Project = new XElement("Project", versionInfPrj.FileDescription);
                        Projects.Add(Project);

                        //! атрибуты пока тянем все подряд из доступных
#if NF
                        Project.Add(new XAttribute("RefPath", PathNetCore.GetRelativePath(sDirFiles, sFilePrg)));//NF не умеет GetRelativePath
#else
                        Project.Add(new XAttribute("RefPath", Path.GetRelativePath(sDirFiles, sFilePrg)));//think NF не умеет GetRelativePath
#endif
                        Project.Add(new XAttribute("FileName", Path.GetFileName(sFilePrg)));
                        Project.Add(new XAttribute("ProductName", versionInfPrj.ProductName));
                        Project.Add(new XAttribute("FileDescription", versionInfPrj.FileDescription));
                        Project.Add(new XAttribute("OriginalFilename", versionInfPrj.OriginalFilename));
                        Project.Add(new XAttribute("InternalName", versionInfPrj.InternalName));
                        Project.Add(new XAttribute("FileVersion", versionInfPrj.FileVersion));
                        Project.Add(new XAttribute("ProductVersion", versionInfPrj.ProductVersion));
                        Project.Add(new XAttribute("LegalTrademarks", versionInfPrj.LegalTrademarks));
                        Project.Add(new XAttribute("LegalCopyright", versionInfPrj.LegalCopyright));
                        //Project.Add(new XAttribute("CompanyName", versionInfPrj.CompanyName));
                        //Project.Add(new XAttribute("Comments", versionInfPrj.Comments));

                        //!если файл выбранного проекта
                        if (sFilePrgs.Contains(file))
                        {
                            //признак, что файл в корне каталога и был выбран разработчиком
                            Project.Add(new XAttribute("root", true));
                        }
                        else
                        {
                            Project.Add(new XAttribute("root", false));
                        }
                        #endregion
                    }
                    else
                    {
                        #region Module

                        XElement Module = new XElement("Module"/*, versionInfoMod.ProductName*/);
                        Modules.Add(Module);
#if NF
                        Module.Add(new XAttribute("RefPath", PathNetCore.GetRelativePath(sDirFiles, sFilePrg)));//заглушка для фрэмворка, относительный путь
#else
                        Module.Add(new XAttribute("RefPath", Path.GetRelativePath(sDirFiles, sFilePrg)));
#endif
                        Module.Add(new XAttribute("FileName", Path.GetFileName(sFilePrg)));

                        #endregion
                    }
                }
                Debug.WriteLine(XDOC.ToString());
                return true;
            }
        }
    }


}


/*
обновлятор обновляет с конца, главный файл обновлять последним
главный файл по версии признак, что нужно обновление
либо файл пустышку dll чтоб с нее считывать версию

*/