using drz.Updater;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UnWrapperTest
{
    internal class ProgramUnTest
    {
        static void Main(string[] args)
        {

            Assembly asm = Assembly.GetExecutingAssembly();
            string sAsmFulPath  ;
            string sUrl = @"http://doctorraz.ucoz.ru/";

            using (UnWrapper un = new UnWrapper(asm, sUrl))
            {
                sAsmFulPath= un.sAsmFullPath;               
            }
            Console.WriteLine(sAsmFulPath);
            Console.ReadKey();

        }
    }
}
