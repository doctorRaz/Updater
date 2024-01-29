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
        #region ZIP

        /// <summary>
        /// упаковщик
        /// </summary>
        internal bool WrapperZIP
        {
            get
            {
                //undone упаковщик
                var rr = arrFiletoZIP;
                var sZip = sFullNameZIP;
           

                return true;
            }
        }
        #endregion


    }
}

