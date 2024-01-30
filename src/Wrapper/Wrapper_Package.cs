using drz.Updater;
using drz.XMLSerialize;

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

