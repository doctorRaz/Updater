/*
#При генерации обновления:

1. К названию XML файла добавлять **minor** программы
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

после распаковки пытаться положиь файлы по путям или тупо искать поиском?? если больше чем один чего делать?



*/

//***
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

            XmlWriter UC = new XmlWriter();

            //! читаем свойства файлов обновления и пишем в XML
            if (!UC.XmlPropWriter)
            {
                Console.WriteLine(UC.sErr);
                Console.WriteLine("Press any key");
                Console.ReadKey();
                return;
            }

            //!интересуемся нужны ли файлы пакадж
            Utils UT = new Utils();
            UT.sConsolMesag = "Нужно ли добавить в обновление файлы Package??";

            if (UT.ConsoleReadKey == ConsoleKey.Y)
            {
                //! читаем свойства файлов Package и пишем в XML
                if (!UC.XmlPackageWriter)
                {
                    Console.WriteLine(UC.sErr);
                    Console.WriteLine("Press any key");
                    Console.ReadKey();
                    return;
                }
            }

            //!если все хорошо запросить файл описания обновки what news
            UT.sConsolMesag = "Добавить в XML описание обновлений из файла??";
            if (UT.ConsoleReadKey == ConsoleKey.Y)
            {
                //!читаем описание из текстового файла в XML
                if (!UC.XmlDescriptorWriter)
                {
                    Console.WriteLine(UC.sErr);
                    Console.WriteLine("Press any key");
                    Console.ReadKey();
                    return;
                }
            }

            //! вывести в консоль
            UT.sConsolMesag = "НАпечатать собранный XML в консоль??";
            if (UT.ConsoleReadKey == ConsoleKey.Y)
            {
                Console.WriteLine(UC.XDOC.ToString());
            }


            //think предложить упаковку файлов собранных в XML в zip с паролем
            //выбор места сохранения файла
            //дописать этот zip в XML

            //!сохранимся
            //think тут тоже добавить запрос сохранить по умолчанию или куда скажет юзер
            UC.XDOC.Save(UC.sFullNameXML);

            //think прибить папку в темпе


            Console.WriteLine("\tXML saved");
            Console.WriteLine("Press any key");
            Console.ReadKey();
            return;

    
        }

    }

}

