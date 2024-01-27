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
    public partial class Wrapper

    {
                #region FilePropWriter

        /// <summary>
        ////писатель свойств файлов
        /// </summary>
        internal bool XmlFilePropWriter
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


                #region Set general properties /прибито гвоздями/

                //!+ корневая директория приложения
                sDirFiles = Directory.GetParent(sFilePrg).FullName;

                //!+директория над каталогом приложения
                sDirParent = Directory.GetParent(sDirFiles).FullName;

                //!+продукт 
                sProductName = versionInfPrj.ProductName.ToLower();

                //!+полная версия файла
                sVersion = versionInfPrj.FileVersion;

                //Version vFileVersion = new Version(sVersion);

                ////до минора версия
                //sMajorMinorVersion = vFileVersion.Major + "." + vFileVersion.Minor;

                ////!полное имя XML 
                //sFullNameXML = Path.Combine(sDirParent, sShortName + sXMLext);           

                ////!полное имя zip 
                //sFullNameZIP = Path.Combine(sDirParent, sShortName + ".zip");

                //радуем юзера, что пошли формировать XML
                Console.WriteLine("Формирую XML для {0} {1}", sProductName, sVersion);
                Console.WriteLine(sFullNameXML);



                #endregion


                //***

                //x проверить есть ли папочка в темпе, если есть прибить и создать по новой, не забыть по окончании опять прибить,

                //собираем путь папочки  темпе
                //темп продукт минор
                //string sDirFolderTmp = Path.Combine(Path.GetTempPath(), sProductName + "_" + sMajorMinorVersion);
                //bool isExistDirFolderTmp = Directory.Exists(sDirFolderTmp);

                #region Header XML

                //think возможно надо добавить дату и прочие служебные данные

                //!Projects
                XElement Projects = new XElement("Projects");
                ROOT.Add(Projects);

                //!Modules
                XElement Modules = new XElement("Modules");
                ROOT.Add(Modules);

                #endregion

                //фильтр исключаемых масок
                string sExcludedSupportedExt = string.Join(",", arrExcludedSupportedExt);

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
                    //! список файлов для упаковки
                    arrFiletoZIP.Add(sFilePrg);
                }
                Debug.WriteLine(XDOC.ToString());
                return true;
            }
        }

        #endregion


    }
}

