using drz.Updater;

using System;
using System.Reflection;

namespace UnWrapperTest
{
    internal class ProgramUnTest
    {
        static void Main(string[] args)
        {
            https://stackoverflow.com/questions/372865/path-combine-for-urls
            //Uri baseUri = new Uri("http://www.contoso.com");
            //Uri myUri = new Uri(baseUri, "catalog/shownew.htm");


            string prg = "plotspds";
            var baseUri=new Uri(@"http://doctorraz.ucoz.ru/"+prg);
            string xml = "dil.xml";
            string zip = "dil.zip";
            //x del
            Uri myUri = new Uri(baseUri, xml );
            Uri myUri2 = new Uri(baseUri, zip );
           
           

            Assembly asm = Assembly.GetExecutingAssembly();
            //string sasmFullPath = "string.Empty";
            string sasmFullPath = asm.Location;

            //https://doctorraz.ucoz.ru/PlotSPDS/versioninfo.txt
            string sUrl = @"http://doctorraz.ucoz.ru/";
            string sAsmFullPath="";
            try
            {
                
                using (UnWrapper un = new UnWrapper(sasmFullPath))
                {
                    sAsmFullPath = un.sMainAssemblyFullPatch;
                    un.sUrlDomain = sUrl;
                   if(!un.isOk)
                    {
                        Console.WriteLine(un.sErr);
                        Console.ReadKey();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                //think сюда логгер
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                //Console.WriteLine(ex.Data);
            }
           
            Console.WriteLine("Patch:\t"+sAsmFullPath);
            Console.ReadKey();

        }
    }
}
