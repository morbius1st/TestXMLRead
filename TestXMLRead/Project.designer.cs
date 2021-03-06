// ------------------------------------------------------------------------------
//  <auto-generated>
//    Generated by Xsd2Code. Version 3.4.0.32989
//    <NameSpace>TestXMLRead</NameSpace><Collection>List</Collection><codeType>CSharp</codeType><EnableDataBinding>False</EnableDataBinding><EnableLazyLoading>False</EnableLazyLoading><TrackingChangesEnable>False</TrackingChangesEnable><GenTrackingClasses>False</GenTrackingClasses><HidePrivateFieldInIDE>True</HidePrivateFieldInIDE><EnableSummaryComment>True</EnableSummaryComment><VirtualProp>False</VirtualProp><IncludeSerializeMethod>True</IncludeSerializeMethod><UseBaseClass>True</UseBaseClass><GenBaseClass>True</GenBaseClass><GenerateCloneMethod>True</GenerateCloneMethod><GenerateDataContracts>True</GenerateDataContracts><CodeBaseTag>Net40</CodeBaseTag><SerializeMethodName>Serialize</SerializeMethodName><DeserializeMethodName>Deserialize</DeserializeMethodName><SaveToFileMethodName>SaveToFile</SaveToFileMethodName><LoadFromFileMethodName>LoadFromFile</LoadFromFileMethodName><GenerateXMLAttributes>True</GenerateXMLAttributes><OrderXMLAttrib>True</OrderXMLAttrib><EnableEncoding>False</EnableEncoding><AutomaticProperties>True</AutomaticProperties><GenerateShouldSerialize>False</GenerateShouldSerialize><DisableDebug>True</DisableDebug><PropNameSpecified>Default</PropNameSpecified><Encoder>UTF8</Encoder><CustomUsings></CustomUsings><ExcludeIncludedTypes>False</ExcludeIncludedTypes><EnableInitializeFields>True</EnableInitializeFields>
//  </auto-generated>
// ------------------------------------------------------------------------------
namespace TestXMLRead {
    using System;
    using System.Diagnostics;
    using System.Xml.Serialization;
    using System.Collections;
    using System.Xml.Schema;
    using System.ComponentModel;
    using System.IO;
    using System.Text;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    
    
    #region Base entity class
    public partial class EntityBase<T>
     {
        
        private static System.Xml.Serialization.XmlSerializer serializer;
        
        private static System.Xml.Serialization.XmlSerializer Serializer {
            get {
                if ((serializer == null)) {
                    serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                }
                return serializer;
            }
        }
        
        #region Serialize/Deserialize
        /// <summary>
        /// Serializes current EntityBase object into an XML document
        /// </summary>
        /// <returns>string XML value</returns>
        public virtual string Serialize() {
            System.IO.StreamReader streamReader = null;
            System.IO.MemoryStream memoryStream = null;
            try {
                memoryStream = new System.IO.MemoryStream();
                Serializer.Serialize(memoryStream, this);
                memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
                streamReader = new System.IO.StreamReader(memoryStream);
                return streamReader.ReadToEnd();
            }
            finally {
                if ((streamReader != null)) {
                    streamReader.Dispose();
                }
                if ((memoryStream != null)) {
                    memoryStream.Dispose();
                }
            }
        }
        
        /// <summary>
        /// Deserializes workflow markup into an EntityBase object
        /// </summary>
        /// <param name="xml">string workflow markup to deserialize</param>
        /// <param name="obj">Output EntityBase object</param>
        /// <param name="exception">output Exception value if deserialize failed</param>
        /// <returns>true if this XmlSerializer can deserialize the object; otherwise, false</returns>
        public static bool Deserialize(string xml, out T obj, out System.Exception exception) {
            exception = null;
            obj = default(T);
            try {
                obj = Deserialize(xml);
                return true;
            }
            catch (System.Exception ex) {
                exception = ex;
                return false;
            }
        }
        
        public static bool Deserialize(string xml, out T obj) {
            System.Exception exception = null;
            return Deserialize(xml, out obj, out exception);
        }
        
        public static T Deserialize(string xml) {
            System.IO.StringReader stringReader = null;
            try {
                stringReader = new System.IO.StringReader(xml);
                return ((T)(Serializer.Deserialize(System.Xml.XmlReader.Create(stringReader))));
            }
            finally {
                if ((stringReader != null)) {
                    stringReader.Dispose();
                }
            }
        }
        
        /// <summary>
        /// Serializes current EntityBase object into file
        /// </summary>
        /// <param name="fileName">full path of outupt xml file</param>
        /// <param name="exception">output Exception value if failed</param>
        /// <returns>true if can serialize and save into file; otherwise, false</returns>
        public virtual bool SaveToFile(string fileName, out System.Exception exception) {
            exception = null;
            try {
                SaveToFile(fileName);
                return true;
            }
            catch (System.Exception e) {
                exception = e;
                return false;
            }
        }
        
        public virtual void SaveToFile(string fileName) {
            System.IO.StreamWriter streamWriter = null;
            try {
                string xmlString = Serialize();
                System.IO.FileInfo xmlFile = new System.IO.FileInfo(fileName);
                streamWriter = xmlFile.CreateText();
                streamWriter.WriteLine(xmlString);
                streamWriter.Close();
            }
            finally {
                if ((streamWriter != null)) {
                    streamWriter.Dispose();
                }
            }
        }
        
        /// <summary>
        /// Deserializes xml markup from file into an EntityBase object
        /// </summary>
        /// <param name="fileName">string xml file to load and deserialize</param>
        /// <param name="obj">Output EntityBase object</param>
        /// <param name="exception">output Exception value if deserialize failed</param>
        /// <returns>true if this XmlSerializer can deserialize the object; otherwise, false</returns>
        public static bool LoadFromFile(string fileName, out T obj, out System.Exception exception) {
            exception = null;
            obj = default(T);
            try {
                obj = LoadFromFile(fileName);
                return true;
            }
            catch (System.Exception ex) {
                exception = ex;
                return false;
            }
        }
        
        public static bool LoadFromFile(string fileName, out T obj) {
            System.Exception exception = null;
            return LoadFromFile(fileName, out obj, out exception);
        }
        
        public static T LoadFromFile(string fileName) {
            System.IO.FileStream file = null;
            System.IO.StreamReader sr = null;
            try {
                file = new System.IO.FileStream(fileName, FileMode.Open, FileAccess.Read);
                sr = new System.IO.StreamReader(file);
                string xmlString = sr.ReadToEnd();
                sr.Close();
                file.Close();
                return Deserialize(xmlString);
            }
            finally {
                if ((file != null)) {
                    file.Dispose();
                }
                if ((sr != null)) {
                    sr.Dispose();
                }
            }
        }
        #endregion
        
        #region Clone method
        /// <summary>
        /// Create a clone of this T object
        /// </summary>
        public virtual T Clone() {
            return ((T)(this.MemberwiseClone()));
        }
        #endregion
    }
    #endregion
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="csProjectData")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="csProjectData", IsNullable=false)]
    [System.Runtime.Serialization.DataContractAttribute(Name="ProjectData", Namespace="csProjectData")]
    public partial class ProjectData : EntityBase<ProjectData> {
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        private ProjectDataProjectInformation projectInformationField;
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        private List<ProjectDataCDPackageData> cDPackageField;
        
    [System.Xml.Serialization.XmlElementAttribute(Order=0)]
    [System.Runtime.Serialization.DataMemberAttribute()]
    public ProjectDataProjectInformation ProjectInformation {get; set;}

        
        /// <summary>
        /// ProjectData class constructor
        /// </summary>
        public ProjectData() {
            this.cDPackageField = new List<ProjectDataCDPackageData>();
            this.projectInformationField = new ProjectDataProjectInformation();
        }
        
        [System.Xml.Serialization.XmlArrayAttribute(Order=1)]
        [System.Xml.Serialization.XmlArrayItemAttribute("CDPackageData", IsNullable=false)]
        [System.Runtime.Serialization.DataMemberAttribute()]
        public List<ProjectDataCDPackageData> CDPackage {
            get {
                return this.cDPackageField;
            }
            set {
                this.cDPackageField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="csProjectData")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ProjectDataProjectInformation", Namespace="csProjectData")]
    public partial class ProjectDataProjectInformation : EntityBase<ProjectDataProjectInformation> {
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        private IDInfo projectField;
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        private string rootPathField;
        
    [System.Xml.Serialization.XmlElementAttribute(Order=0)]
    [System.Runtime.Serialization.DataMemberAttribute()]
    public IDInfo Project {get; set;}

    [System.Xml.Serialization.XmlElementAttribute(Order=1)]
    [System.Runtime.Serialization.DataMemberAttribute()]
    public string RootPath {get; set;}

        
        /// <summary>
        /// ProjectDataProjectInformation class constructor
        /// </summary>
        public ProjectDataProjectInformation() {
            this.projectField = new IDInfo();
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="csProjectData")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="csProjectData", IsNullable=true)]
    [System.Runtime.Serialization.DataContractAttribute(Name="IDInfo", Namespace="csProjectData")]
    public partial class IDInfo : EntityBase<IDInfo> {
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        private string descriptionField;
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        private string idField;
        
    [System.Xml.Serialization.XmlElementAttribute(Order=0)]
    [System.Runtime.Serialization.DataMemberAttribute()]
    public string Description {get; set;}

    [System.Xml.Serialization.XmlAttributeAttribute()]
    [System.Runtime.Serialization.DataMemberAttribute()]
    public string ID {get; set;}

    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="csProjectData")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ProjectDataCDPackageData", Namespace="csProjectData")]
    public partial class ProjectDataCDPackageData : EntityBase<ProjectDataCDPackageData> {
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        private ProjectDataCDPackageDataIdentifier identifierField;
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        private List<string> usersField;
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        private ProjectDataCDPackageDataLocationAutoCAD locationAutoCADField;
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        private ProjectDataCDPackageDataLocationRevit locationRevitField;
        
    [System.Xml.Serialization.XmlElementAttribute(Order=0)]
    [System.Runtime.Serialization.DataMemberAttribute()]
    public ProjectDataCDPackageDataIdentifier Identifier {get; set;}

    [System.Xml.Serialization.XmlElementAttribute(Order=2)]
    [System.Runtime.Serialization.DataMemberAttribute()]
    public ProjectDataCDPackageDataLocationAutoCAD LocationAutoCAD {get; set;}

    [System.Xml.Serialization.XmlElementAttribute(Order=3)]
    [System.Runtime.Serialization.DataMemberAttribute()]
    public ProjectDataCDPackageDataLocationRevit LocationRevit {get; set;}

        
        /// <summary>
        /// ProjectDataCDPackageData class constructor
        /// </summary>
        public ProjectDataCDPackageData() {
            this.locationRevitField = new ProjectDataCDPackageDataLocationRevit();
            this.locationAutoCADField = new ProjectDataCDPackageDataLocationAutoCAD();
            this.usersField = new List<string>();
            this.identifierField = new ProjectDataCDPackageDataIdentifier();
        }
        
        [System.Xml.Serialization.XmlArrayAttribute(Order=1)]
        [System.Xml.Serialization.XmlArrayItemAttribute("UserID", IsNullable=false)]
        [System.Runtime.Serialization.DataMemberAttribute()]
        public List<string> Users {
            get {
                return this.usersField;
            }
            set {
                this.usersField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="csProjectData")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ProjectDataCDPackageDataIdentifier", Namespace="csProjectData")]
    public partial class ProjectDataCDPackageDataIdentifier : EntityBase<ProjectDataCDPackageDataIdentifier> {
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        private IDInfo taskField;
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        private IDInfo phaseField;
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        private IDInfo buildingField;
        
    [System.Xml.Serialization.XmlElementAttribute(Order=0)]
    [System.Runtime.Serialization.DataMemberAttribute()]
    public IDInfo Task {get; set;}

    [System.Xml.Serialization.XmlElementAttribute(Order=1)]
    [System.Runtime.Serialization.DataMemberAttribute()]
    public IDInfo Phase {get; set;}

    [System.Xml.Serialization.XmlElementAttribute(Order=2)]
    [System.Runtime.Serialization.DataMemberAttribute()]
    public IDInfo Building {get; set;}

        
        /// <summary>
        /// ProjectDataCDPackageDataIdentifier class constructor
        /// </summary>
        public ProjectDataCDPackageDataIdentifier() {
            this.buildingField = new IDInfo();
            this.phaseField = new IDInfo();
            this.taskField = new IDInfo();
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="csProjectData")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ProjectDataCDPackageDataLocationAutoCAD", Namespace="csProjectData")]
    public partial class ProjectDataCDPackageDataLocationAutoCAD : EntityBase<ProjectDataCDPackageDataLocationAutoCAD> {
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        private string sheetFilePathField;
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        private string xrefPathField;
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        private string detailPathField;
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        private string borderFileField;
        
    [System.Xml.Serialization.XmlElementAttribute(Order=0)]
    [System.Runtime.Serialization.DataMemberAttribute()]
    public string SheetFilePath {get; set;}

    [System.Xml.Serialization.XmlElementAttribute(Order=1)]
    [System.Runtime.Serialization.DataMemberAttribute()]
    public string XrefPath {get; set;}

    [System.Xml.Serialization.XmlElementAttribute(Order=2)]
    [System.Runtime.Serialization.DataMemberAttribute()]
    public string DetailPath {get; set;}

    [System.Xml.Serialization.XmlElementAttribute(Order=3)]
    [System.Runtime.Serialization.DataMemberAttribute()]
    public string BorderFile {get; set;}

    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="csProjectData")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ProjectDataCDPackageDataLocationRevit", Namespace="csProjectData")]
    public partial class ProjectDataCDPackageDataLocationRevit : EntityBase<ProjectDataCDPackageDataLocationRevit> {
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        private string modelFileField;
        
    [System.Xml.Serialization.XmlElementAttribute(Order=0)]
    [System.Runtime.Serialization.DataMemberAttribute()]
    public string ModelFile {get; set;}

    }
}
