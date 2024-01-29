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


namespace drz.Updater
{
    /// <summary>
    ///Подготовка пакета обновления
    ///описание в XML и упаковка
    /// </summary>
    public partial class Wrapper

    {
        #region DescriptorWriter

        #endregion
        /// <summary>
        /// писатель обновления, что нового
        /// </summary>
        internal bool WrapperDescription
        {
            get
            {
                //undone описатель обновления

                //!Descriptions

                ofd.Multiselect = true;
                ofd.Title = "Выбери файл описания";
                ofd.Filter = "Все файлы (*.*)|*.*|"
                             + ".txt (*.txt)|"
                             + "*.txt";
                ofd.FilterIndex = 2;
                ofd.RestoreDirectory = false;

                //!получим файл описания         
                if (ofd.ShowDialog() != DialogResult.OK)
                {
                    sErr = "Файл Обновления не загружен!\nПользователь отказался!";
                    return false;
                }
                sFilePrg = ofd.FileName;

                return true;
            }
        }

    }
}

