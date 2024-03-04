using Ionic.Zip;

using System.IO;


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
         //https://documentation.help/DotNetZip/CSharp.htm
            get
            {

                //http://doctorraz.ucoz.ru/PlotSPDS/plot_test.zip без пароля не проходит
                //http://doctorraz.ucoz.ru/PlotSPDS/wrapper.nf.zip
                //http://doctorraz.ucoz.ru/PlotSPDS/plot_test.rar

                //password save
                ROOT.PasswordZIP = sProductName;

                //! имя zip в XML (лежит рядом с XML)
                ROOT.FileNameZIP = Path.GetFileName(sFullNameZIP);

                using (ZipFile zip = new ZipFile())
                {
                    zip.AlternateEncoding = System.Text.Encoding.GetEncoding("UTF-8");
                    zip.AlternateEncodingUsage = ZipOption.Always;
                    zip.Encryption = EncryptionAlgorithm.WinZipAes256;
                    zip.Password = ROOT.PasswordZIP;

                    //перебираем файлы для упаковки
                    foreach (string file in arrFiletoZIP)
                    {
                        //каталог упаковываемого файла
                        string PathtoFile = Directory.GetParent(file).FullName;

                        //относительный путь для упаковки
#if NF
                        string RefPathZip = PathNetCore.GetRelativePath(sDirFiles, PathtoFile);//NF не умеет GetRelativePath
#else
                         string   RefPathZip = Path.GetRelativePath(sDirFiles, PathtoFile);//! NF не умеет GetRelativePath
#endif

                        //добавим в архив
                        zip.AddFile(file, RefPathZip);
                    }

                    //zip.Comment = "This zip was created at " + System.DateTime.Now.ToString("G");

                    zip.Save(sFullNameZIP);
                }
                /*
                //x применить для модуля распаковщика и прибить
                // удалять каталог перед извлечением
                Directory.Delete(sShortName, true);
                // extract entries that use encryption
                using (ZipFile zip = ZipFile.Read(sFullNameZIP))
                {
                    //think выдергивать файлы по одному
                    zip.Password = ROOT.PasswordZIP;
                    zip.ExtractAll(sShortName, ExtractExistingFileAction.OverwriteSilently);
                }
                */
                return true;
            }
        }
        #endregion


    }
}

