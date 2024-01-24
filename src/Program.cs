using drz.Updater;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;


//https://learn.microsoft.com/ru-ru/dotnet/core/project-sdk/msbuild-props#assembly-attribute-properties
#if NET
[assembly: AssemblyInformationalVersion("Updater Prep info")]
[assembly: AssemblyTitle("Updater Prep Title000")]
#endif

namespace drz.UpdatePrep
{
    public static class Command
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {

            //***
          

            //***

            XmlWriter UC = new XmlWriter();

            //! читаем свойства файлов обновления и пишем в XML
            if (!UC.XmlPropWriter)
            {
                //MessageBox.Show(UC.sErr, "UpCust", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(UC.sErr);
                Console.ReadLine();
                return;
            }

            //интересуемся нужны ли файлы пакадж

            //do
            //{
            //    Console.Write("Нужно ли добавить в обновление файлы Package?? [y/n] ");
            //    response = Console.ReadKey(/*false*/).Key;   // true is intercept key (dont show), false is show
            //    if (response != ConsoleKey.Enter)
            //        Console.WriteLine();

            //} while (response != ConsoleKey.Y && response != ConsoleKey.N);

            //var isPackag = MessageBox.Show("Нужно ли добавить в обновление файлы Package?", "UpCust", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            Utils UT = new Utils();
            UT.sConsolMesag = "Нужно ли добавить в обновление файлы Package??";

            if (UT.ConsoleReadKey == ConsoleKey.Y)

            {
                //! читаем свойства файлов Package и пишем в XML
                if (!UC.XmlPackageWriter)
                {
                    Console.WriteLine(UC.sErr);
                    Console.ReadLine();
                    //MessageBox.Show(UC.sErr, "UpCust", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            //!если все хорошо запросить файл описания обновки


            //do//? вытащить в класс
            //{
            //    Console.Write("Добавить в XML описание обновлений из файла?? [y/n] ");
            //    response = Console.ReadKey(/*false*/).Key;   // true is intercept key (dont show), false is show
            //    if (response != ConsoleKey.Enter)
            //        Console.WriteLine();

            //} while (response != ConsoleKey.Y && response != ConsoleKey.N);

            UT.sConsolMesag = "Добавить в XML описание обновлений из файла??";
            if (UT.ConsoleReadKey == ConsoleKey.Y)
            {
                //!читаем описание из текста в XML
                if (!UC.XmlDescriptorWriter)
                {
                    Console.WriteLine(UC.sErr);
                    Console.ReadLine();
                    return;
                }
            }




            //! вывести в консоль
            //ConsoleKey response;
            //do
            //{
            //    Console.Write("Вывести XML в консоль?? [y/n] ");
            //    response = Console.ReadKey(/*false*/).Key;   // true is intercept key (dont show), false is show
            //    if (response != ConsoleKey.Enter)
            //        Console.WriteLine();

            //} while (response != ConsoleKey.Y && response != ConsoleKey.N);

            UT.sConsolMesag = "Вывести XML в консоль??";

            if (UT.ConsoleReadKey == ConsoleKey.Y)
            {
                Console.WriteLine(UC.XDOC.ToString());
            }

            //!сохранимся
            //think тут тоже запрос сохранить по умолчанию или куда скажет юзер
            UC.XDOC.Save(UC.sFullNameXML);

            //предложить упаковку файлов собранных в XML в zip с паролем
            Console.WriteLine("\tXML saved");
            Console.WriteLine("Press any key");
            Console.ReadKey ();
            return;

            //---

            //***R

            #region Assembly

            Assembly asm = Assembly.GetExecutingAssembly();

            string sTitleAttribute = ((AssemblyTitleAttribute)Attribute.GetCustomAttribute(asm,
                                                                   typeof(AssemblyTitleAttribute),
                                                                   false)).Title;

            //MessageBox.Show(sTitleAttribute);

            /// <summary>Описание программы </summary>
            string sDescription = (Attribute.GetCustomAttribute(asm,
                                                                typeof(AssemblyDescriptionAttribute),
                                                                false) as AssemblyDescriptionAttribute).Description;//!описание

            /// <summary>Конфигурация программы </summary>
#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
            string sConfiguration = (Attribute.GetCustomAttribute(asm,
                                                                  typeof(AssemblyConfigurationAttribute),
                                                                  false) as AssemblyConfigurationAttribute).Configuration;

            /// <summary>Компания </summary>
            string sCompany = (Attribute.GetCustomAttribute(
              asm,
              typeof(AssemblyCompanyAttribute),
              false) as AssemblyCompanyAttribute).Company;

            /// <summary>Продукт </summary>
            string sProduct = (Attribute.GetCustomAttribute(
                 asm,
                 typeof(AssemblyProductAttribute),
                 false) as AssemblyProductAttribute).Product;

            /// <summary>копирайт</summary>
            string sCopyright = (Attribute.GetCustomAttribute(
               asm,
               typeof(AssemblyCopyrightAttribute),
               false) as AssemblyCopyrightAttribute).Copyright;

            /// <summary>торговая марка</summary>
            string sTrademark = (Attribute.GetCustomAttribute(
               asm,
               typeof(AssemblyTrademarkAttribute),
               false) as AssemblyTrademarkAttribute).Trademark;

            /// <summary>ProductVersion - Версия программы
            /// <br>для идентификации в лицензии пограмма для нк или АК</br>
            /// </summary>
            string sInformationalVersionAttribut = (Attribute.GetCustomAttribute(
                      asm,
                      typeof(AssemblyInformationalVersionAttribute),
                      false) as AssemblyInformationalVersionAttribute).InformationalVersion;

            System.Version sysVersion = asm.GetName().Version;

            #endregion

            OpenFileDialog ofd = new OpenFileDialog
            {
                Multiselect = false,
                Title = "Выбери файл приложения",
                Filter = "Все файлы (*.*)|*.*|"
                         + ".NET assemblies (*.exe;*.dll)|"
                         + "*.exe;*.dll",
                FilterIndex = 2,
                RestoreDirectory = true
            };


            //выберем главный файл приложения
            string sErr;

            string sFilePrg;
        test:
            if (ofd.ShowDialog() != DialogResult.OK)
            {
                sFilePrg = string.Empty;
                sErr = "Файл приложения не загружен";
                return;
            }

            sFilePrg = ofd.FileName;
            //x прибить sFilePrg
            //sFilePrg = @"d:\@Developers\В работе\!Текущее\Programmers\!NET\!PlotSPDS\bin\000\PlotSPDSn mod.dll";

            //!директория приложения
            string sDirFiles = Directory.GetParent(sFilePrg).FullName;

            //реальное имя файла проекта
            string sNamePrj = Path.GetFileName(sFilePrg);

            //инфа файла проекта
            FileVersionInfo versionInfPrj = FileVersionInfo.GetVersionInfo(sFilePrg);

            //имя ХМЛ как имя главного файла, но на уровень выше
            //идем на уровень выше
            string sDirParentXML = Directory.GetParent(sDirFiles).FullName;
            //имя XML
            string sShortNameXML = Path.GetFileNameWithoutExtension(sFilePrg) + ".xml";
            //!полный путь к XML
            string suFullNameXML = Path.Combine(sDirParentXML, sShortNameXML);

            goto test;
            #region Header XML

            //тут начинаем писать xml
            XDocument Xdoc = new XDocument();
            XElement root = new XElement("root");
            Xdoc.Add(root);//цепляем к doc
                           //think возможно надо добавить дату и прочие служебные данные
            #endregion



            XElement Project = new XElement("Project", versionInfPrj.ProductName);
            root.Add(Project);
            //его атрибуты
            Project.Add(new XAttribute("FileName", Path.GetFileName(versionInfPrj.FileName)));
            Project.Add(new XAttribute("FileDescription", versionInfPrj.FileDescription));
            Project.Add(new XAttribute("OriginalFilename", versionInfPrj.OriginalFilename));
            Project.Add(new XAttribute("InternalName", versionInfPrj.InternalName));
            Project.Add(new XAttribute("FileVersion", versionInfPrj.FileVersion));
            Project.Add(new XAttribute("ProductVersion", versionInfPrj.ProductVersion));
            Project.Add(new XAttribute("LegalTrademarks", versionInfPrj.LegalTrademarks));
            Project.Add(new XAttribute("LegalCopyright", versionInfPrj.LegalCopyright));
            Project.Add(new XAttribute("CompanyName", versionInfPrj.CompanyName));
            Project.Add(new XAttribute("Comments", versionInfPrj.Comments));



            //!Запись формируем хмл
            // linq to xml https://habr.com/ru/articles/24673/

            #region Пример            

            //Создание вложенными конструкторами.
            //think добавить флаг использовать лицензию для автокад и нанокад

            //            XDocument Xdoc = new XDocument(
            //                    new XElement("root",
            ///*Пользователь*/
            //            new XElement("USER",
            //                new XAttribute("name", dsf.sUN),
            //                new XAttribute("family", dsf.sUF),
            //                new XAttribute("mail", dsf.sUserMail),
            //                new XAttribute("pay", dsf.sPay)),
            ///*Продукт*/             new XElement("PRODUCT",
            //                            new XAttribute("description", dsf.sProgDescription),
            //                            new XAttribute("version", dsf.sProductVersion),
            //                            new XAttribute("CheckingVersion", dsf.IsCheckingVersion),
            //                            new XAttribute("dateentry", dsf.sDateCreate)), /*дата запроса уже в формате 13.11.2023 14:50:55 преобразовано в dsf */
            ///*Запрос*/              new XElement("GENERATOR",
            //                            new XAttribute("licenseexpiration", dsf.sDateEnd),
            //                            new XAttribute("issued", DateTime.Now.ToString("g"))), /* дата время создания лицензии*/
            ///*Система*/             new XElement("HOST",
            //                            new XElement("SYS",
            //                                 new XAttribute("caption", sCaption),
            //                                 new XAttribute("csname", dsf.sSysName),
            //                                 new XAttribute("UUID", sUUID)),
            //                            /*    new XAttribute("systemdrive", sSystemDrive),
            //                                new XAttribute("volumeid", dsf.sVolID),
            //                                new XAttribute("physicaldrive", sHddName),
            //                                new XAttribute("serialnumber", dsf.sHddID)),*/
            //                            new XElement("BaseBoard",
            //                                new XAttribute("serialnumber", sSerialNumberBB)),
            //                            new XElement("BIOS",
            //                                new XAttribute("serialnumber", sSerialNumberBIOS)),
            //                            new XElement("Processor",
            //                                new XAttribute("serialnumber", ProcessorId))),
            //                        new XElement("NSSIGN",                          //!эта секция должна быть зашифрована
            //                                                                        //в боевой порграмме читаем только ее
            //                            new XAttribute("description", dsf.sProgDescription),
            //                            new XAttribute("version", dsf.sProductVersion),
            //                            new XAttribute("CheckingVersion", dsf.IsCheckingVersion),
            //                            new XAttribute("name", dsf.sUN),
            //                            new XAttribute("family", dsf.sUF),
            //                            new XAttribute("dateend", dsf.dDRE),
            //                            new XAttribute("volname", dsf.sVolName),
            //                            new XAttribute("volid", dsf.sVolID),
            //                            new XAttribute("hddname", dsf.sHddName),
            //                            new XAttribute("hddid", dsf.sHddID))));

            //#if DEBUG
            //            Console.WriteLine(Xdoc.ToString());
            //#endif
            //            //сохраняем XML
            //            Xdoc.Save(sFilName);


            #endregion

            //hack САМЫЙ БЫСТРЫЙ
            //https://stackoverflow.com/questions/163162/can-you-call-directory-getfiles-with-multiple-filters
            string supportedExtensions = "*.exe,*.dll";

            foreach (string file in Directory.GetFiles(sDirFiles, "*.*", SearchOption.AllDirectories).Where(s => !supportedExtensions.Contains(Path.GetExtension(s).ToLower())))
            {
                if (string.Compare(file, sFilePrg, true) == 0)
                {// если файл проекта то пропускаем
                    //FileVersionInfo versionInfo2 = FileVersionInfo.GetVersionInfo(file);

                    continue;
                }

                FileVersionInfo versionInfoMod = FileVersionInfo.GetVersionInfo(file);
                XElement Module = new XElement("Module", versionInfoMod.ProductName);
                Project.Add(Module);

                Module.Add(new XAttribute("FileName", Path.GetFileName(versionInfoMod.FileName)));
                Module.Add(new XAttribute("FileDescription", versionInfoMod.FileDescription));
                Module.Add(new XAttribute("OriginalFilename", versionInfoMod.OriginalFilename));
                Module.Add(new XAttribute("InternalName", versionInfoMod.InternalName));
                Module.Add(new XAttribute("FileVersion", versionInfoMod.FileVersion));
                Module.Add(new XAttribute("ProductVersion", versionInfoMod.ProductVersion));
                Module.Add(new XAttribute("LegalTrademarks", versionInfoMod.LegalTrademarks));
                Module.Add(new XAttribute("LegalCopyright", versionInfoMod.LegalCopyright));
                Module.Add(new XAttribute("CompanyName", versionInfoMod.CompanyName));
                Module.Add(new XAttribute("Comments", versionInfoMod.Comments));



                /*

                var sComments = versionInfo.Comments;
                var sCompanyName = versionInfo.CompanyName;
                var version = versionInfo.FileVersion;
ProductName				"PlotSPDS"											string
FileDescription			"PlotSPDSn"											string
OriginalFilename		"PlotSPDSn.dll"										string
InternalName			"PlotSPDSn.dll"										string
FileName				"d:\\@Developers\\В работе\\!Текущее\\Programmers\\!NET\\!PlotSPDS\\bin\\000\\PlotSPDSn mod.dll"	string
FileVersion				"1.0.8780.26549"									string
ProductVersion			"PlotSPDS for nanoCAD"								string  
LegalTrademarks			"©doctorRAZ 2014-2024"								string
LegalCopyright			"Разыграев Андрей"									string
CompanyName				"doctorRaz@gmail.com"								string
Comments				"Автоматическая печать из nanoCAD форматов СПДС"	string
                Version vCopy = new Version(version);//версия копии

                Debug.WriteLine("    -" + file);
            Debug.WriteLine("-----------------------------");
                */
            }
            Debug.WriteLine(Xdoc.ToString());

            Xdoc.Save(suFullNameXML);

            //***



            //think получать текстовое описание  обновления, what news

            //собираем это добро в XML


            //окно настроек
            //var wpfPrep = new WpfPrep();
            //bool? retval = wpfPrep.ShowDialog();
        }



    }


}


/*
    class FinderFile
    {
        internal string[] FindFiles(string directory, string filters, SearchOption searchOption)
        {
            if (!Directory.Exists(directory)) return new string[] { };

            var include = (from filter in filters.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries) where !string.IsNullOrEmpty(filter.Trim()) select filter.Trim());
            var exclude = (from filter in include where filter.Contains(@"!") select filter);

            include = include.Except(exclude);

            if (include.Count() == 0) include = new string[] { "*" };

            var rxfilters = from filter in exclude select string.Format("^{0}$", filter.Replace("!", "").Replace(".", @"\.").Replace("*", ".*").Replace("?", "."));
            Regex regex = new Regex(string.Join("|", rxfilters.ToArray()));

            List<Thread> workers = new List<Thread>();
            List<string> files = new List<string>();

            foreach (string filter in include)
            {
                Thread worker = new Thread(
                    new ThreadStart(
                        delegate
                        {
                            string[] allfiles = Directory.GetFiles(directory, filter, searchOption);
                            if (exclude.Count() > 0)
                            {
                                lock (files)
                                    files.AddRange(allfiles.Where(p => !regex.Match(p).Success));
                            }
                            else
                            {
                                lock (files)
                                    files.AddRange(allfiles);
                            }
                        }
                    ));

                workers.Add(worker);

                worker.Start();
            }

            foreach (Thread worker in workers)
            {
                worker.Join();
            }

            return files.ToArray();

        }



    }
}
*/
/*
//дальше получаем все exe, dll директории и поддиректорий
//? исключить главный файл???его обработать отдельно?
var files = Directory.GetFiles(sDirFiles,
                                "*.*",
                                SearchOption.AllDirectories)
.Where(s => s.EndsWith(".exe") || s.EndsWith(".dll")).ToArray();


string[] filters = new[] { "*.exe", "*.dll" };
string[] filePaths = filters.SelectMany(f => Directory.GetFiles(sDirFiles, f, SearchOption.AllDirectories)).ToArray();


//проходим и св словарик снимаем с них имя файла, имя Product, версию, дату компиляции
foreach (var file in filters.SelectMany(f => Directory.GetFiles(sDirFiles, f, SearchOption.AllDirectories)))
//foreach (var file in filePaths)
{
    //if (string.Compare(file, sFilePrg, true) == 0)
    //{// если файл проекта то пропускаем
    //    continue;
    //}
    //Debug.WriteLine("");
    //Debug.WriteLine(file);
}
//***

FinderFile ff = new FinderFile();

foreach (string file in ff.FindFiles(sDirFiles, @"!sFilePrg,*.exe, *.dll", SearchOption.AllDirectories))
{
    //if (string.Compare(file, sFilePrg, true) == 0)
    //{// если файл проекта то пропускаем
    //    continue;
    //}
    //Debug.WriteLine("");
    //Debug.WriteLine(file);

}
Debug.WriteLine("-----------------------------");

*/