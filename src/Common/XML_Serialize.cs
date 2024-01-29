using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace drz.XMLSerialize
{

    /// <summary>
    /// Корень XML
    /// </summary>
    [Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public partial class root
    {
        [XmlArrayItem("Project", IsNullable = false)]
        public List<rootProject> Projects;

        [XmlArrayItem("Module", IsNullable = false)]
        public List<rootModule> Modules;


        /// <summary>
        /// Чисто для отладки потом изменить
        /// </summary>
        [XmlAttribute()]
        public ushort Date;

        /// <summary>
        /// Тестовое свойство
        /// </summary>
        [XmlAttribute()]
        public string test;

        /// <summary>
        /// тестовое значение
        /// </summary>
        [XmlText()]
        public string Value;

    }

    /// <summary>
    /// Класс свойств проекта (все файлы которые имеют FileVersion)
    /// </summary>
    [Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class rootProject

    {
        /// <summary>
        /// The value
        /// </summary>
        [XmlText()]
        public string Project;

        /// <summary>
        /// The reference path
        /// </summary>
        [XmlAttribute()]
        public string RefPath;

        /// <summary>
        /// The file name
        /// </summary>
        [XmlAttribute()]
        public string FileName;

        /// <summary>
        /// The product name/
        /// </summary>
        [XmlAttribute()]
        public string ProductName;

        /// <summary>
        /// The file description/
        /// </summary>
        [XmlAttribute()]
        public string FileDescription;

        /// <summary>
        /// The original filename
        /// </summary>
        [XmlAttribute()]
        public string OriginalFilename;

        /// <summary>
        /// The internal name
        /// </summary>
        [XmlAttribute()]
        public string InternalName;

        /// <summary>
        /// The version
        /// </summary>
        [XmlAttribute()]
        public string FileVersion;

        /// <summary>
        /// The product version
        /// </summary>
        [XmlAttribute()]
        public string ProductVersion;

        /// <summary>
        /// The legal trademarks
        /// </summary>
        [XmlAttribute()]
        public string LegalTrademarks;

        /// <summary>
        /// The legal copyright
        /// </summary>
        [XmlAttribute()]
        public string LegalCopyright;

        [XmlAttribute()]
        public string CompanyName;

        [XmlAttribute()]
        public string Comments;

        /// <summary>
        /// The root начальный файл проекта
        /// </summary>
        [XmlAttribute()]
        public bool root;

    }

    /// <summary>
    /// Свойства файлов проекта не имеющие FileVersion
    /// </summary>
    [Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class rootModule
    {
        /// <summary>
        /// The reference path, относительно главного файла проекта (корень каталога проекта)
        /// </summary>
        [XmlAttribute()]
        public string RefPath;

        /// <summary>
        /// The file name
        /// </summary>
        [XmlAttribute()]
        public string FileName;

    }

    /// <summary>
    /// Свойства файлов package
    /// </summary>
    [Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class rootPackage
    {
        /// <summary>
        /// The reference path, относительно главного файла проекта (корень каталога проекта)
        /// </summary>
        [XmlAttribute()]
        public string RefPath;

        /// <summary>
        /// The file name
        /// </summary>
        [XmlAttribute()]
        public string FileName;
    }


    /// <summary>
    /// Описание обновления
    /// </summary>
    [Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true)]

    public partial class rootDescription
    {

    }

}
