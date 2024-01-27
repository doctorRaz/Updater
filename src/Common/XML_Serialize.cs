using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace drz.Serialize_test
{

    // Примечание. Для запуска созданного кода может потребоваться NET Framework версии 4.5 или более поздней версии и .NET Core или Standard версии 2.0 или более поздней.
    /// <remarks/>
    [Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public partial class root
    {
        /// <remarks/>
        [XmlArrayItem("Project", IsNullable = false)]
        public List<rootProject> Projects;

        /// <remarks/>
        [XmlArrayItem("Module", IsNullable = false)]
        public List<rootModule> Modules;

        /// <remarks/>
        [XmlAttribute()]
        public ushort Date;

        /// <remarks/>
        [XmlAttribute()]
        public string test;
        
    }

    /// <remarks/>
    [Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class rootProject
    {
        /// <remarks/>
        [XmlAttribute()]
        public string RefPath;


        /// <remarks/>
        [XmlAttribute()]
        public string FileName;

        /// <remarks/>
        [XmlAttribute()]
        public bool root;

        [XmlAttribute()]
        public string Version;

        /// <remarks/>
        [XmlText()]
        public string Value; 


    }

    /// <remarks/>
    [Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class rootModule
    {

        /// <remarks/>
        [XmlAttribute()]
        public string RefPath;


        /// <remarks/>
        [XmlAttribute()]
        public string FileName;

    }



}
