/*
 В названии XML файла добавлять номер версии программы, кроме имени также номер версии программы добавлять в а зип архив. Таким образом при обновление минорной ветки. Все обновления будут копиться только. В 1 ветке. При переходе. При изменении минорной версии обновления будут подтягиваться из другой ветки.
 
 //undone рабочий код, но нужна отладка по взаимодействию совместно с распаковщиком 


=================================================================================
=================================================================================
//todo общее
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


#При генерации обновления: Как будет...

 ~1. К названию XML файла добавлять **minor** программы~
 ~2. К названию zip добавлять **minor** программы~
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

using drz.XMLSerialize;
//using Ionic.Zlib;

using System;
using System.IO;
using System.Reflection;
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
                Wrap.isWrapAllFile = true;
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

            //сохраним ZIP
            // пароль записать в XML

            if (!Wrap.WrapperZIP)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.BackgroundColor = ConsoleColor.Blue;

                Console.WriteLine(Wrap.sErr);
                Console.WriteLine("Press any key");
                Console.ReadKey();
                Console.ResetColor();

                Console.WriteLine("Press any key");
                Console.ReadKey();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.WriteLine("ZIP archived:\t" + Wrap.sFullNameZIP);

            //!сохраним в файл
            XmlSerializer xms = new XmlSerializer(typeof(root));

            using (FileStream fs = new FileStream(Wrap.sFullNameXML, FileMode.Create/*OpenOrCreate*/))
            {
                xms.Serialize(fs, Wrap.ROOT);
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.WriteLine("XML saved:\t" + Wrap.sFullNameXML);

            Console.ResetColor();

            //открыть каталог 
            ConsRead.sConsolMesag = "Открыть каталог с файлами??";

            ckRes = ConsRead.ConsoleReadKey;
 
            if (ckRes == ConsoleKey.Y)
            {
                //undone открыть папку с zip и XML
            }

            Console.WriteLine("Press any key");
            Console.ReadKey();
            return;

        }

    }

}

