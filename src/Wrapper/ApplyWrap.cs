/*
Правитть апли врап 
1. добавить настройки:
* список игнорируемых расширений
* список включаемых расширений
~переделать хмл на серилизацию, добавить полей с запасом~
добавить класс описания обновения
добавить класс пути имени zip
добавить упаковщик
------
писать скачивалку
проверятель 
распаковщик
заменятель файлов


#При генерации обновления:

 ~1. К названию XML файла добавлять **minor** программы~
2. К названию zip добавлять **minor** программы 
>Все обновления внутри **minor** только главный модуль программы + модули имеющие _FileVersion_. 

Соответственно каждый минор обновляется из своего zip+minor
* Изменения minor если добавились или изменились фалы не имеющие _FileVersion_ (lsp, cfg, dll(c++), 
* При след обновлении при изменении minor++ обновка тянется из другого zip++minor (в котором только модули имеющие версию)
и так по кругу..
>т.е. например четные минор только обновления основных модулей
>нечетные минор обновление вспомогательных версий переход на четную

соответственно пока все модули не будут обновлены минор++ не произойдет


   пишем только имя файла который обновить
   подумать над обновлением пакадже , типа флаг подниматься уровнем выше, либо относительный путь от файла стартера, пусть файл проекта передает свой путь параметром обновлятору
   забивать имена файлов, в словарик, если дубликат, то выход с ошибкой, что дубликаты имен
  в словарик что бы пропускать одинаковые имена

после распаковки пытаться положить файлы по путям или тупо искать поиском?? если больше чем один чего делать?

zip
https://www.dotnetspider.com/resources/43506-How-create-zip-file-C-NET-with-secured.aspx
https://github.com/haf/DotNetZip.Semverd
/zip


/*
обновлятор обновляет с конца, главный файл обновлять последним
главный файл по версии признак, что нужно обновление
либо файл пустышку dll чтоб с нее считывать версию

*/



//***

using drz.Updater;
using drz.XMLSerialize;

using Ionic.Zip;
//using Ionic.Zlib;

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
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;



//https://learn.microsoft.com/ru-ru/dotnet/core/project-sdk/msbuild-props#assembly-attribute-properties
#if NET
[assembly: AssemblyInformationalVersion("Wrapper Prep info")]
[assembly: AssemblyTitle("Wrapper Prep")]
#endif

namespace drz.Updater
{

    /// <summary>
    /// Старт
    /// </summary>
    public static class Command
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            var filename = @"d:\@Developers\В работе\!Текущее\Programmers\!NET\Updater\@resourse\русские символы\PlotSPDSn.dll";//полный
            
            var sDirFiles = @"d:\@Developers\В работе\!Текущее\Programmers\!NET\Updater";//папка выше
            var PathtoFile=Directory.GetParent(filename).FullName;//каталог файла

            var RefPath = PathNetCore.GetRelativePath(sDirFiles, PathtoFile);

            //zip
            var NameOfZipFileTocreate = @"d:\@Developers\В работе\!Текущее\Programmers\!NET\Updater\@resourse\русские символы\Plot.zip";



            using (ZipFile zip = new ZipFile())
            {
                zip.AlternateEncoding = System.Text.Encoding.GetEncoding("UTF-8");
                zip.AlternateEncodingUsage = ZipOption.Always;
                zip.Encryption = EncryptionAlgorithm. WinZipAes256;
                zip.Password = "0";

                //zip.
                zip.AddFile(filename,RefPath);
                zip.Comment = "This zip was created at " + System.DateTime.Now.ToString("G") ;
                zip.Save(NameOfZipFileTocreate);
            }
            { }

            // extract entries that use encryption
            using (ZipFile zip = ZipFile.Read(NameOfZipFileTocreate))
            {
                //zip.Password = "ккк";
                zip.ExtractAll("extractDir");
            }

            /* чтение
            string sFilDesc = @"d:\@Developers\В работе\!Текущее\Programmers\!NET\Updater\@resourse\bunle test\changelog_beta.txt";

            //  
            using (StreamReader reader = new StreamReader(sFilDesc, Encoding.Default))
            {
                string line;
                int iRow = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    iRow++;
                    Debug.WriteLine(iRow + " " + line);
                }
            }


            */
            /* masks
            string[] masks = new[]
                {
                  "*.exe",
                  "*.dll",
                  "*.cfg",
                  "*.cuix",
                  "*.lsp",
                  "*.xml",
                  "*.json",
                 };


            string path = @"d:\@Developers\В работе\!Текущее\Programmers\!NET\Updater\@resourse\";
            var directory = new  DirectoryInfo (path );
            //var masks = new[] { "*.mp3", "*.wav" };
            var files = masks.SelectMany(directory.EnumerateFiles );

            { }

            */
            /* понты  
            //https://www.dotnetperls.com/console-color

            // Demonstrate all colors and backgrounds.
            Type type = typeof(ConsoleColor);
            Console.ForegroundColor = ConsoleColor.White;
            foreach (var name in Enum.GetNames(type))
            {
                Console.BackgroundColor = (ConsoleColor)Enum.Parse(type, name);
                Console.WriteLine(name);
            }
            Console.BackgroundColor = ConsoleColor.Black;
            foreach (var name in Enum.GetNames(type))
            {
                Console.ForegroundColor = (ConsoleColor)Enum.Parse(type, name);
                Console.WriteLine(name);
            }
        */

            //***
            #region Console
            Console.Title = "Wrapper";
            //!console read
            Utils.ConsoleRequest ConsRead = new Utils.ConsoleRequest();
            ConsoleKey ckRes;
            #endregion

            Wrapper Wrap = new Wrapper();

            //! интересуемся писать в XML все файлы или только выбранные
            ConsRead.sConsolMesag = "Писать в XML все файлы-Y, только выбранные- N??";
            ckRes = ConsRead.ConsoleReadKey;

            if (ckRes == ConsoleKey.Escape) return; //esc =quit
            if (ckRes == ConsoleKey.Y)//писать все файлы
            {
                Wrap.bWrapAllFile = true;
            }

            //! читаем свойства файлов обновления и пишем в XML
            if (!Wrap.WrapperProjectAndModules)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.WriteLine(Wrap.sErr);
                Console.ResetColor();

                Console.WriteLine("Press any key");
                Console.ReadKey();
                return;
            }

            //!интересуемся нужны ли файлы пакадж
            ConsRead.sConsolMesag = "Нужно ли добавить в обновление файлы Package??";
            ckRes = ConsRead.ConsoleReadKey;

            if (ckRes == ConsoleKey.Escape) return; //esc =quit
            if (ckRes == ConsoleKey.Y)
            {
                //! читаем свойства файлов Package и пишем в XML
                if (!Wrap.WrapperPackage)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.WriteLine(Wrap.sErr);

                    Console.ResetColor();

                    Console.WriteLine("Продолжаем сборку");
                    Console.WriteLine("Press any key");
                    Console.ReadKey();
                    //return;
                }
            }

            //!если все хорошо запросить файл описания обновки what news
            ConsRead.sConsolMesag = "Добавить в XML описание обновлений из файла??";

            ckRes = ConsRead.ConsoleReadKey;

            if (ckRes == ConsoleKey.Escape) return; //esc =quit
            if (ckRes == ConsoleKey.Y)
            {
                //!читаем описание из текстового файла в XML
                if (!Wrap.WrapperDescription)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.BackgroundColor = ConsoleColor.Blue;

                    Console.WriteLine(Wrap.sErr);

                    Console.ResetColor();

                    Console.WriteLine("Продолжаем сборку");
                    Console.WriteLine("Press any key");

                    Console.ReadKey();
                    //return;
                }
            }

            //BUG не работает вывод XML в консоль
            // вывести в консоль
            //ConsRead.sConsolMesag = "НАпечатать собранный XML в консоль??";

            //ckRes = ConsRead.ConsoleReadKey;
            //if (ckRes == ConsoleKey.Escape) return; //esc =quit
            //if (ckRes == ConsoleKey.Y)
            //{
            //    var xxx = Wrap.ROOT;
            //    Console.WriteLine(Wrap.ROOT.ToString());

            //}

            //?сохраним ZIP в метод, пароль имя приложения/или не парить мозг а "00000"
            // пароль записать в XML

            if (!Wrap.WrapperZIP)
            {
                Console.WriteLine(Wrap.sErr);
                Console.WriteLine("Press any key");
                Console.ReadKey();
                return;
            }




            //и др. служебную информацию

            //!сохраним в файл
            XmlSerializer xms = new XmlSerializer(typeof(root));

            using (FileStream fs = new FileStream(Wrap.sFullNameXML, FileMode.Create/*OpenOrCreate*/))
            {
                xms.Serialize(fs, Wrap.ROOT);
            }

            Console.WriteLine("\tXML saved");
            Console.WriteLine("Press any key");
            Console.ReadKey();
            return;


        }

    }

}

