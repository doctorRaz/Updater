using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace drz.Updater
{
    /// <summary>вспомогательные утилиты
    /// <br>только для писателя и паковщика</br></summary>
    internal partial class Utils
    {
        /// <summary>
        /// Сообщение запрос консоли
        /// </summary>
        internal string sConsolMesag;

        internal ConsoleKey ConsoleKeyY = ConsoleKey.Y;
        internal ConsoleKey ConsoleKeyN = ConsoleKey.N;

        string sConsoleKeyY => ConsoleKeyY.ToString();
        string sConsoleKeyN => ConsoleKeyN.ToString();

        /// <summary>Чтение кнопок консоли <br>
        ///https://stackoverflow.com/questions/37359161/how-would-i-make-a-yes-no-prompt-in-console-using-c
        ///</br>
        /// </summary>
        /// <param name="sConsolMesag">Выводимое сообщение</param>н
        /// <returns>кнопка введенная пользователем</returns>
        internal ConsoleKey ConsoleReadKey
        {
            get
            {
                ConsoleKey response;
                do
                {
                    Console.Write(sConsolMesag + " [" + sConsoleKeyY + "/" + sConsoleKeyN + "] ");
                    response = Console.ReadKey(/*false*/).Key;   // true is intercept key (dont show), false is show
                    if (response != ConsoleKey.Enter)
                        Console.WriteLine();

                } while (response != ConsoleKeyY && response != ConsoleKeyN);

                return response;
            }
        }
    }
}
