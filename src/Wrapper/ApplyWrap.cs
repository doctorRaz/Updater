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
            //https://www.dotnetperls.com/console-color

            /* понты  
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
            Console.Title = "Wrapper";

            Wrapper Wrap = new Wrapper();

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

            //!console read
            Utils.ConsoleRequest ConsRead = new Utils.ConsoleRequest();
            ConsoleKey ckRes;

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
                    Console.WriteLine(Wrap.sErr);
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

            if (!Wrap.WrapperZIP)
            {
                Console.WriteLine(Wrap.sErr);
                Console.WriteLine("Press any key");
                Console.ReadKey();
                return;
            }


            //! дописать имя zip в XML (ледит рядом с XML
            Wrap.ROOT.FileNameZIP = Path.GetFileName(Wrap.sFullNameZIP);

            //и др. служебную информацию

            //!сохраним в файл
            XmlSerializer xms = new XmlSerializer(typeof(root));

            if (File.Exists(Wrap.sFullNameXML)) File.Delete(Wrap.sFullNameXML);

            using (FileStream fs = new FileStream(Wrap.sFullNameXML, FileMode.OpenOrCreate))
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

