using drz.Updater;

using Ionic.Zip;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;


namespace drz.Updater
{
    /// <summary>
    ///Подготовка пакета обновления
    ///описание в XML и упаковка
    /// </summary>
    public partial class Wrapper

    {
        #region ZIP

        /// <summary>
        /// упаковщик
        /// </summary>
        internal bool WrapperZIP
        {//https://github.com/haf/DotNetZip.Semverd
         //https://github.com/haf/DotNetZip.Semverd
            get
            {
//                using (ZipFile zip = new ZipFile("MyZipFile.zip")
//                {
////zip.AddFiles(arrFiletoZIP);

//                    //zip.Add("c:\\images\\personal\\7440-N49th.png");
//                    //              zip.AddFile("c:\\Desktop\\2008-Regional-Sales-Report.pdf");
//                    //              zip.AddFile("ReadMe.txt");
//                    //              zip.Save();
//                };



            //undone упаковщик
            var rr = arrFiletoZIP;//список для упаковки, писать в один каталог, дубликатов быть не должно
                                  //think писать с относительным путем, тогда и извлекать по относительному из zip
                var sZip = sFullNameZIP;//куда писать zip

                //! после успешной упаковки дописать имя zip в XML (лежит рядом с XML)
                ROOT.FileNameZIP = Path.GetFileName(sFullNameZIP);

                return true;
            }
        }
        #endregion


    }
}

