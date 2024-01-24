using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace drz.Updater
{
    internal class Utils
    {
        internal   string sConsolMesag;

        internal   ConsoleKey ConsoleKeyY=ConsoleKey.Y;
        internal   ConsoleKey ConsoleKeyN=ConsoleKey.N;
        internal   ConsoleKey ConsoleKeyOther=ConsoleKey.N;

        string sConsoleKeyY=>ConsoleKeyY.ToString();
        string sConsoleKeyN=>ConsoleKeyN.ToString();

        /// <summary>
        /// Чтение кнопок консоли
        /// </summary>
        /// <param name="sConsolMesag">Выводимое сообщение</param>н
        /// <returns>кнопка введенная пользователем</returns>
        internal   ConsoleKey ConsoleReadKey
        {
            get
            {
                ConsoleKey response;
                do
                {
                    Console.Write(sConsolMesag + " [y/n] ");
                    response = Console.ReadKey(/*false*/).Key;   // true is intercept key (dont show), false is show
                    if (response != ConsoleKey.Enter)
                        Console.WriteLine();

                } while (response != ConsoleKeyY && response != ConsoleKeyN);

                return response;
            }
        }
    }
}
