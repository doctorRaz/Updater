using System;
using System.Collections.Generic;
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
        /// <summary>
        /// The projects
        /// </summary>
        [XmlArrayItem("Project", IsNullable = false)]
        public List<rootProject> Projects;

        /// <summary>
        /// The modules
        /// </summary>
        [XmlArrayItem("Module", IsNullable = false)]
        public List<rootModule> Modules;

        /// <summary>
        /// The packages
        /// </summary>
        [XmlArrayItem("Package", IsNullable = false)]
        public List<rootPackage> Packages;

        /// <summary>
        /// The descriptions
        /// </summary>
        [XmlArrayItem("Description", IsNullable = false)]
        public List<rootDescription> Descriptions;

        /// <summary>
        /// дата числом, хз зачем
        /// </summary>
        [XmlAttribute()]
        public int DateCreate{ get; set; }

        /// <summary>
        /// дата время Wrapped
        /// </summary>
        [XmlAttribute()]
        public string DateTime{ get; set; }
   
        /// <summary>
        /// только имя и расширение zip файла /будет лежать рядом с XML
        /// </summary>
        [XmlAttribute()]
        public string FileNameZIP{ get; set; }

        /// <summary>
        /// пароль для ZIP
        /// </summary>
        [XmlAttribute()]
        public string PasswordZIP{ get; set; }

        /// <summary>
        /// на всякий
        /// </summary>
        [XmlText()]
        public string Val{ get; set; }

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
        /// Проект
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

        /// <summary>
        /// The company name
        /// </summary>
        [XmlAttribute()]
        public string CompanyName;

        /// <summary>
        /// The comments
        /// </summary>
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
        /// <summary>
        /// счетчик строк
        /// </summary>
        [XmlText()]
        public string Counter;

        /// <summary>
        /// Номер строки
        /// </summary>
        [XmlAttribute()]
        public int Row;

        /// <summary>
        ///Содержимое
        /// </summary>
        [XmlAttribute()]
        public string Content;
    }

}
