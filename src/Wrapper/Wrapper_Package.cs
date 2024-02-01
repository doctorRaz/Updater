using drz.XMLSerialize;

using System.IO;
using System.Windows.Forms;


namespace drz.Updater
{
    /// <summary>
    ///Подготовка пакета обновления
    ///описание в XML и упаковка
    /// </summary>
    public partial class Wrapper

    {
        #region PackageWriter

        /// <summary>
        /// писатель свойств Package /загрузчик приложения/
        /// </summary>
        internal bool WrapperPackage
        {
            get
            {
                #region OFD Package
                ofd.Multiselect = true;
                ofd.Title = "Выбери файлы Package, можно несколько";
                ofd.Filter = "Все файлы (*.*)|*.*|"
                             + ".package (*.package)|"
                             + "*.package";
                ofd.FilterIndex = 2;
                ofd.RestoreDirectory = true;

                //!получим файлы Package            
                if (ofd.ShowDialog() != DialogResult.OK)
                {
                    sErr = "Файл Package не загружен! Пользователь отказался!";
                    return false;
                }

                sFilePrgs = ofd.FileNames;
                sFilePrg = ofd.FileName;

                #endregion

                foreach (string filе in sFilePrgs)
                {
                    sFilePrg = filе;

                    #region Package
                    rootPackage Package = new rootPackage
                    {
#if NF
                        RefPath = PathNetCore.GetRelativePath(sDirFiles, sFilePrg),//заглушка для фрэймворка, относительный путь
#else
                        RefPath = Path.GetRelativePath(sDirFiles, sFilePrg),
#endif
                        FileName = Path.GetFileName(sFilePrg),
                    };
                    Packages.Add(Package);
                    #endregion

                    //! список файлов для упаковки
                    arrFiletoZIP.Add(sFilePrg);
                }

                return true;
            }
        }

        #endregion

    }
}

