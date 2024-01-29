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
        #region DescriptorWriter

        /// <summary>
        /// писатель обновления, что нового
        /// </summary>
        internal bool XmlDescriptorWriter
        {
            get
            {
                //? описатель обновления

                //!Descriptions
                XElement Descriptions = new XElement("Descriptions");
                //ROOT.Add(Descriptions);


                return true;
            }
        }
        #endregion


    }
}

