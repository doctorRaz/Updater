using System;

namespace drz.Updater
{
    /// <summary>вспомогательные утилиты
    /// <br>только для писателя и паковщика</br></summary>
    internal partial class Utils
    {

        internal class ConsoleRequest
        {


            /// <summary>
            /// Сообщение запрос консоли
            /// </summary>
            internal string sConsolMesag;

            internal ConsoleKey ConsoleKeyY = ConsoleKey.Y;
            internal ConsoleKey ConsoleKeyN = ConsoleKey.N;
            internal ConsoleKey ConsoleKeyEsc = ConsoleKey.Escape;

            string sConsoleKeyY => ConsoleKeyY.ToString();
            string sConsoleKeyN => ConsoleKeyN.ToString();
            string sConsoleKeyEsc => ConsoleKeyEsc.ToString();

            /// <summary>
            /// Чтение кнопок консоли <br>
            /// https://stackoverflow.com/questions/37359161/how-would-i-make-a-yes-no-prompt-in-console-using-c
            /// </br>
            /// </summary>
            /// <value>
            /// The console read key.
            /// </value>
            internal ConsoleKey ConsoleReadKey
            {
                get
                {
                    ConsoleKey response;
                    do
                    {
                        Console.Write(sConsolMesag + " [" + sConsoleKeyY + "/" + sConsoleKeyN + "], " + sConsoleKeyEsc + " - Quit: ");
                        response = Console.ReadKey(/*false*/).Key;   // true is intercept key (dont show), false is show
                        if (response != ConsoleKey.Enter)
                            Console.WriteLine();

                    } while (response != ConsoleKeyY && response != ConsoleKeyN && response != ConsoleKeyEsc);

                    return response;
                }
            }
        }
    }
}