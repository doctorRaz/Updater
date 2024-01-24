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
                      
            //***R
                        
            //think получать текстовое описание  обновления, what news

            //собираем это добро в XML


            //окно настроек
            //var wpfPrep = new WpfPrep();
            //bool? retval = wpfPrep.ShowDialog();
        }

    }

}

