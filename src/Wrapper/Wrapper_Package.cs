using drz.Updater;

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


namespace drz.UpdatePrep
{
    /// <summary>
    ///Подготовка пакета обновления
    ///описание в XML и упаковка
    /// </summary>
    public partial class Wrapper

    {
        #region PackageWriter

        /// <summary>
        /// писатель свойств пакадже /загрузчик приложения/
        /// </summary>
        internal bool XmlPackageWriter
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
                    sErr = "Файл Package не загружен!\nПользователь отказался!";
                    return false;
                }

                sFilePrgs = ofd.FileNames;
                sFilePrg = ofd.FileName;

                #endregion

                //!Packages
                XElement Packages = new XElement("Packages");
                ROOT.Add(Packages);

                foreach (string fil in sFilePrgs)
                {
                    //!Package
                    //think добавлять файл пакадж в папку для архива
                    sFilePrg = fil;
                    XElement Package = new XElement("Package"/*, versionInfoMod.ProductName*/);
                    Packages.Add(Package);
#if NF
                    Package.Add(new XAttribute("RefPath", PathNetCore.GetRelativePath(sDirFiles, sFilePrg)));//заглушка для фрэмворка, относительный путь
#else
                    Package.Add(new XAttribute("RefPath", Path.GetRelativePath(sDirFiles, sFilePrg)));
#endif
                    Package.Add(new XAttribute("FileName", Path.GetFileName(sFilePrg)));

                    //! список файлов для упаковки
                    arrFiletoZIP.Add(sFilePrg);
                }

                return true;
            }
        }

        #endregion


    }
}

