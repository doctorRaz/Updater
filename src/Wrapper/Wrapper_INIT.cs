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
        #region INIT CLASS

        public Wrapper()
        {
            XDOC = new XDocument();
            ROOT = new XElement("root");
            XDOC.Add(ROOT);//цепляем к doc
            ofd = new OpenFileDialog();
            arrFiletoZIP = new List<string>();//пустой массив
        }

        #region Пути имена

        /// <summary>
        /// Список файлов для архивации
        /// </summary>
        internal List<string> arrFiletoZIP { get; set; }

        /// <summary> директория приложения 
        /// <br> задаем один раз потом не меняем</br>
        /// <br> относительно этого пути будет все собираться</br>
        /// </summary>
        internal string sDirFiles { get; set; }

        /// <summary>
        /// директория над каталогом приложения
        /// </summary>
        internal string sDirParent { get; set; }


        #region Product

        /// <summary>
        /// название продукта
        /// </summary>
        internal string sProductName { get; set; }

        /// <summary>
        /// Версия главного файла мажор.минор
        /// </summary>
        internal string sMajorMinorVersion
        {
            get
            {
                Version vFileVersion = new Version(sVersion);
                //до минора версия
                return vFileVersion.Major + "." + vFileVersion.Minor;
            }
        }

        /// <summary>
        /// полная версия главного файла
        /// </summary>
        internal string sVersion { get; set; }


        //think при запросе на сервер приложение само собирает из своих ProductName и MajorMinorVersion и запрашивает такой файл на сервере
        /// <summary>
        /// имя файла для ZIP и XLS
        /// </summary>
        internal string sShortName => sProductName + "_" + sMajorMinorVersion;

        #endregion

        /// <summary>
        /// папочка куда предварительно копируем компоненты приложения
        /// </summary>
        internal string sDirTempProduct { get; set; }

        /// <summary>файл приложения, библиотек и прочих файлов</summary>
        string sFilePrg { get; set; }

        /// <summary>файлы приложения, библиотек и прочих файлов</summary>
        string[] sFilePrgs { get; set; }

        #endregion

        /// <summary>инфа о файле проекта </summary>        
        FileVersionInfo versionInfPrj => FileVersionInfo.GetVersionInfo(sFilePrg);

        /// <summary>Сообщения о ошибках</summary>
        internal string sErr { get; set; }

        #region XML

        /// <summary>
        /// XMLdoc
        /// </summary>
        internal XDocument XDOC { get; set; }

        /// <summary>
        /// root
        /// </summary>
        XElement ROOT { get; set; }

        /// <summary>
        /// расширение XML
        /// </summary>
        const string sXMLext = ".xmlpack";

        /// <summary>
        /// Имя по умолчанию для сохранения XML
        /// </summary>
        internal string sFullNameXML => Path.Combine(sDirParent, sShortName + sXMLext);// { get; set; }

        #endregion

        #region ZIP

        /// <summary>
        /// Имя по умолчанию для сохранения XML
        /// </summary>
        internal string sFullNameZIP => Path.Combine(sDirParent, sShortName + ".zip");// { get; set; }

        #endregion



        #region OFD

        /// <summary>файлы приложения</summary>
        internal OpenFileDialog ofd { get; set; }


        #endregion

        #region Маски исключений
        /// Маски исключений файлов, которые не брать
        /// </summary>
        List<string> arrExcludedSupportedExt
        {
            get
            {
                return new List<string>
         {
             "*.bak",
             "*.pdb",
             "*.json",
             "*.package",
             sXMLext
         };
            }
        }

        #endregion

        #endregion


    }
}

