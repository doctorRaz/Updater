using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
//using System.Management;
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

        /// <summary> директория приложения </summary>
        string sDirFiles { get; set; }

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



        //***R

        /// <summary>
        /// писатель обновления, что нового
        /// </summary>
        internal bool XmlDescriptorWriter
        {
            get
            {


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

                //think обавлять файл пакадж в архив

                //!Packages
                XElement Packages = new XElement("Packages");
                ROOT.Add(Packages);


                return true;
            }
        }

        /// <summary>
        ////писатель свойств файлов
        /// </summary>
        internal bool XmlPropWriter
        {
            get
            {
                #region OFD Prop
                ofd.Multiselect = true;
                ofd.Title = "Выбери главный файл проекта, можно несколько";
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
                //***
                //!сразу проверим годится ли файл есть ли там версия
                foreach (string item in sFilePrgs)
                {
                    sFilePrg = item;
                    Console.WriteLine("Project: "+ sFilePrg);
                    if (string.IsNullOrEmpty(versionInfPrj.FileVersion))
                    {
                        sErr = "Неподходящий файл для главного файла проекта!"
                            + "\n"
                            + sFilePrg;
                        return false;
                    }
                }



                //***
                //! ставим директорию приложения
                sDirFiles = Directory.GetParent(sFilePrg).FullName;

                #region XML      
                /// <summary>имя ХМЛ 
                /// <br>в каталоге приложения</br></summary>
                string sDirParentXML = Directory.GetParent(sDirFiles).FullName;

                /// <summary> имя XML</summary>
                string sShortNameXML = versionInfPrj.ProductName + ".packagedescription";//.xml";//? имя приложения PlotSPDS

                //!полный путь к XML 
                sFullNameXML = Path.Combine(sDirParentXML, sShortNameXML);

                #endregion

                //***

                #region Маски исключений

                string sExcludedSupportedExt;
                List<string> arrExcludedSupportedExt = new List<string>();
                arrExcludedSupportedExt.Add("*.bak");
                arrExcludedSupportedExt.Add("*.package");
                arrExcludedSupportedExt.Add("*.packagedescription");
                sExcludedSupportedExt = string.Join(",", arrExcludedSupportedExt);

                #endregion

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
                //? пишем только имя файла который обновить
                //? подумать над обновлением пакадже , типа флаг подниматься уровнем выше, либо относительный путь от файла стартера, пусть файл проекта передает свой путь параметром обновлятору
                //? забивать имена файлов, в словарик, если дубликат, то выход с ошибкой, что дубликаты имен

                foreach (string file in Directory.GetFiles(sDirFiles, "*.*", SearchOption.AllDirectories).Where(s => !sExcludedSupportedExt.Contains(Path.GetExtension(s).ToLower())))
                {
                    //? в словарик что бы пропускать одинаковые имена

                    sFilePrg = file;

                    //Console.WriteLine(sFilePrg);
                    //если файл проекта
                    if (sFilePrgs.Contains(file))
                    {// если это файл проекта 
                        #region Projects

                        XElement Project = new XElement("Project", versionInfPrj.FileDescription);
                        Projects.Add(Project);
                        //! атрибуты
                        Project.Add(new XAttribute("RefPath", Path.GetRelativePath(sDirFiles, sFilePrg)));//think NF не умеет GetRelativePath
                        Project.Add(new XAttribute("FileName", Path.GetFileName(sFilePrg)));
                        Project.Add(new XAttribute("FileDescription", versionInfPrj.FileDescription));
                        Project.Add(new XAttribute("OriginalFilename", versionInfPrj.OriginalFilename));
                        Project.Add(new XAttribute("InternalName", versionInfPrj.InternalName));
                        Project.Add(new XAttribute("FileVersion", versionInfPrj.FileVersion));
                        Project.Add(new XAttribute("ProductVersion", versionInfPrj.ProductVersion));
                        Project.Add(new XAttribute("LegalTrademarks", versionInfPrj.LegalTrademarks));
                        Project.Add(new XAttribute("LegalCopyright", versionInfPrj.LegalCopyright));
                        Project.Add(new XAttribute("CompanyName", versionInfPrj.CompanyName));
                        Project.Add(new XAttribute("Comments", versionInfPrj.Comments));

                        #endregion
                    }
                    else
                    {
                        #region Module

                        XElement Module = new XElement("Module"/*, versionInfoMod.ProductName*/);
                        Modules.Add(Module);
                        Module.Add(new XAttribute("RefPath", Path.GetRelativePath(sDirFiles, sFilePrg)));
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