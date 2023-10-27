using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

// http://docs.nuget.org/docs/reference/command-line-reference

namespace Colt.Nuget.Utilities {
    public class nuspec_generator {

        #region main-line
        [STAThread]
        public static void Main(string[] args) {
            XmlWriterSettings xws;
            const string MY_NAME = "aname";
            int exitCode = 0;
            string productName, filename;

            TextWriterTraceListener twtl = new TextWriterTraceListener(Console.Out, MY_NAME);
            Trace.Listeners.Add(twtl);
            productName = AsmAttributeExtractor.productFrom(Assembly.GetEntryAssembly());
            xws = new XmlWriterSettings();
            xws.Indent = true;
            xws.IndentChars = "\t";

            filename = productName + ".nuspec";
            using (XmlWriter xw = XmlWriter.Create(filename, xws)) {
                generatePackage(new MyPackage(), xw);
            }
            Console.WriteLine("wrote: " + filename);
            Trace.Listeners.Remove(MY_NAME);
            Environment.Exit(exitCode);
        }

        static void generatePackage(MyPackage myPackage, XmlWriter xw) {
            XmlSerializer xs;

            try {
                xs = new XmlSerializer(myPackage.GetType());
                xs.UnknownAttribute += unknownAttributeFound;
                xs.UnknownElement += unknownElementFound;
                xs.UnknownNode += unknownNodeFound;
                xs.UnreferencedObject += unreferencedNodeFound;
                xs.Serialize(xw, myPackage);
                xs.UnknownAttribute -= unknownAttributeFound;
                xs.UnknownElement -= unknownElementFound;
                xs.UnknownNode -= unknownNodeFound;
                xs.UnreferencedObject -= unreferencedNodeFound;
                xs = null;
            } catch (Exception ex) {
                Trace.WriteLine(ex.GetType().FullName + ":" + ex.Message);
            }
        }

        static void unreferencedNodeFound(object sender, UnreferencedObjectEventArgs e) {
            traceMethod(MethodBase.GetCurrentMethod());
        }

        static void unknownNodeFound(object sender, XmlNodeEventArgs e) {
            traceMethod(MethodBase.GetCurrentMethod());
        }

        static void unknownElementFound(object sender, XmlElementEventArgs e) {
            traceMethod(MethodBase.GetCurrentMethod());
        }

        static void unknownAttributeFound(object sender, XmlAttributeEventArgs e) {
            traceMethod(MethodBase.GetCurrentMethod());
        }

        static void traceMethod(MethodBase mb) {
            Trace.WriteLine(mb.ReflectedType.Name + "." + mb.Name);
        }
        #endregion

        static void generateFile(XmlWriter xw) {
            Assembly a = Assembly.GetEntryAssembly();
            AssemblyName an = a.GetName();

            xw.WriteProcessingInstruction("version", "1.0");
            xw.WriteStartElement("package");
            xw.WriteStartElement("metadata");
            xw.WriteElementString("id", AsmAttributeExtractor.productFrom(a));
            xw.WriteElementString("version", an.Version.ToString());
            xw.WriteElementString("title", AsmAttributeExtractor.titleFrom(a));
            xw.WriteElementString("authors", AsmAttributeExtractor.authorsFrom(a));
            xw.WriteElementString("owners", AsmAttributeExtractor.releaseNotesFrom(a));
            xw.WriteElementString("requireLicenseAcceptance", "false");
            xw.WriteElementString("releaseNotes", "some release notes");
            xw.WriteElementString("tags", "some tags");
            xw.WriteElementString("licenseUrl", AsmAttributeExtractor.licenseUrlFrom(a));

            xw.WriteElementString("projectUrl", AsmAttributeExtractor.projectUrlFrom(a));
            xw.WriteElementString("description", AsmAttributeExtractor.descriptionFrom(a));
            xw.WriteElementString("copyright", AsmAttributeExtractor.copyrightFrom(a));

            xw.WriteEndElement();
            xw.WriteEndElement();
            xw.WriteEndDocument();
        }
    }

    #region delegates
    delegate string AttrHandler(Attribute attr);
    #endregion

    static class AsmAttributeExtractor {
        #region constants
        public const string ERROR = "error";
        public const string ERROR_URL = "http://error";
        #endregion
        static string readAttributeValue<T>(Assembly a, AttrHandler attrHandler) {
            object[] attrs;
            string ret = null;

            if ((attrs = a.GetCustomAttributes(typeof(T), false)) != null && attrs.Length > 0) {
                if (attrHandler != null)
                    ret = attrHandler(attrs[0] as Attribute);
            }
            return ret;
        }

        #region attribute-handler routines
        static string readTitle(Attribute attr) {
            AssemblyTitleAttribute ata;

            if ((ata = attr as AssemblyTitleAttribute) != null)
                return ata.Title;
            return ERROR; ;
        }
        static string readDescription(Attribute attr) {
            AssemblyDescriptionAttribute ata;

            if ((ata = attr as AssemblyDescriptionAttribute) != null)
                return ata.Description;
            return ERROR;
        }
        static string readCopyright(Attribute attr) {
            AssemblyCopyrightAttribute ata;

            if ((ata = attr as AssemblyCopyrightAttribute) != null)
                return ata.Copyright;
            return ERROR;
        }
        static string readLicense(Attribute attr) {
            LicenseUrlAttribute ata;

            if ((ata = attr as LicenseUrlAttribute) != null)
                return ata.licenseUrl;
            return string.Empty;
        }
        static string readProject(Attribute attr) {
            ProjectUrlAttribute ata;

            if ((ata = attr as ProjectUrlAttribute) != null)
                return ata.projectUrl;
            return string.Empty;
        }
        static string readProduct(Attribute attr) {
            AssemblyProductAttribute ata;

            if ((ata = attr as AssemblyProductAttribute) != null)
                return ata.Product;
            return ERROR;
        }
        static string readAuthors(Attribute attr) {
            AuthorsAttribute ata;

            if ((ata = attr as AuthorsAttribute) != null)
                return ata.authors;
            return ERROR;

        }
        static string readOwners(Attribute attr) {
            OwnersAttribute ata;

            if ((ata = attr as OwnersAttribute) != null)
                return ata.owners;
            return ERROR;

        }
        static string readReleaseNotes(Attribute attr) {
            ReleaseNotesAttribute ata;

            if ((ata = attr as ReleaseNotesAttribute) != null)
                return ata.releaseNotes;
            return ERROR;

        }


        internal static string productFrom(Assembly a) {
            return readAttributeValue<AssemblyProductAttribute>(a, new AttrHandler(readProduct));
        }

        internal static string titleFrom(Assembly a) {
            return readAttributeValue<AssemblyTitleAttribute>(a, new AttrHandler(readTitle));
        }

        internal static string licenseUrlFrom(Assembly a) {
            return readAttributeValue<LicenseUrlAttribute>(a, new AttrHandler(readLicense));
        }

        internal static string projectUrlFrom(Assembly a) {
            return readAttributeValue<ProjectUrlAttribute>(a, new AttrHandler(readProject));
        }

        internal static string descriptionFrom(Assembly a) {
            return readAttributeValue<AssemblyDescriptionAttribute>(a, new AttrHandler(readDescription));
        }

        internal static string copyrightFrom(Assembly a) {
            return readAttributeValue<AssemblyCopyrightAttribute>(a, new AttrHandler(readCopyright));
        }

        internal static string authorsFrom(Assembly a) {
            return readAttributeValue<AuthorsAttribute>(a, new AttrHandler(readAuthors));
        }
        internal static string ownersFrom(Assembly a) {
            return readAttributeValue<OwnersAttribute>(a, new AttrHandler(readOwners));
        }


        internal static string releaseNotesFrom(Assembly a) {
            return readAttributeValue<ReleaseNotesAttribute>(a, new AttrHandler(readReleaseNotes));
        }

        #endregion
    }

    [XmlRoot("package")]
    public class MyPackage : IXmlSerializable {
        public MyMetadata metadata;

        public MyPackage() {
            metadata = new MyMetadata();
        }
        public XmlSchema GetSchema() {
            throw new NotImplementedException();
        }

        public void ReadXml(XmlReader reader) {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer) {
            writer.WriteStartElement("metadata");
            metadata.WriteXml(writer);
            writer.WriteEndElement();
        }

        internal void addDependency(string v1, string v2) {
            metadata.addDependency(v1, v2);
        }
    }

    public class PkgDependency : IXmlSerializable {

        #region ctors
        public PkgDependency() : this(null, null) { }
        public PkgDependency(string adepName, string adepVersion) {
            this.dependencyName = adepName;
            this.dependencyVersion = adepVersion;
        }
        #endregion

        #region properties
        public string dependencyName { get; set; }
        public string dependencyVersion { get; set; }

        #endregion

        #region IXmlSerializable implementation
        public XmlSchema GetSchema() {
            throw new NotImplementedException();
        }

        public void ReadXml(XmlReader reader) {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer) {
            //            writer.WriteStartElement("dependenct")
            writer.WriteStartElement("dependency");
            writer.WriteAttributeString("id", dependencyName);
            writer.WriteAttributeString("version", dependencyVersion);
            writer.WriteEndElement();
        }
        #endregion
    }

    [XmlType("typename")]
    public class MyMetadata : IXmlSerializable {
        #region fields
        string id;
        Version version;
        string title;
        string authors;
        string owners;
        string licenseUrl;
        string projectUrl;
        string iconUrl;
        bool requireLicense;
        string description;
        string releaseNotes;
        string copyright;
        string tags;
        List<PkgDependency> _deps;
        #endregion

        #region ctor
        public MyMetadata() {
            Assembly a = Assembly.GetEntryAssembly();
            AssemblyName an = a.GetName();

            _deps = new List<PkgDependency>();
            // required entries
            authors = owners = iconUrl = releaseNotes = tags = string.Empty;
            id = AsmAttributeExtractor.productFrom(a);
            version = an.Version;
            authors = "authors";
            description = AsmAttributeExtractor.descriptionFrom(a);

            // optional entries here
            //            iconUrl = "http://iconurl_here";
            title = AsmAttributeExtractor.titleFrom(a);
            requireLicense = false;
            licenseUrl = AsmAttributeExtractor.licenseUrlFrom(a);
            projectUrl = AsmAttributeExtractor.projectUrlFrom(a);
            copyright = AsmAttributeExtractor.copyrightFrom(a);
            authors = AsmAttributeExtractor.authorsFrom(a);
            owners = AsmAttributeExtractor.ownersFrom(a);
            releaseNotes = AsmAttributeExtractor.releaseNotesFrom(a);
        }
        #endregion

        #region IXmlSerializable implementation
        public XmlSchema GetSchema() {
            throw new NotImplementedException();
        }

        public void ReadXml(XmlReader reader) {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer) {
            if (!string.IsNullOrEmpty(id)) writer.WriteElementString("id", id);
            if (version != null) writer.WriteElementString("version", version.ToString());
            if (!string.IsNullOrEmpty(authors)) writer.WriteElementString("authors", authors);
            if (!string.IsNullOrEmpty(owners)) writer.WriteElementString("owners", owners);
            if (!string.IsNullOrEmpty(licenseUrl)) writer.WriteElementString("licenseUrl", licenseUrl);
            if (!string.IsNullOrEmpty(projectUrl)) writer.WriteElementString("projectUrl", projectUrl);
            if (!string.IsNullOrEmpty(iconUrl)) writer.WriteElementString("iconUrl", iconUrl);
            writer.WriteElementString("requireLicenseAcceptance", requireLicense.ToString().ToLower());
            if (!string.IsNullOrEmpty(description)) writer.WriteElementString("description", description);
            if (!string.IsNullOrEmpty(releaseNotes)) writer.WriteElementString("releaseNotes", releaseNotes);
            if (!string.IsNullOrEmpty(copyright)) writer.WriteElementString("copyright", copyright);
            if (!string.IsNullOrEmpty(tags)) writer.WriteElementString("tags", tags);
            if (_deps != null && _deps.Count > 0) {
                writer.WriteStartElement("dependencies");
                foreach (PkgDependency dep in _deps)
                    dep.WriteXml(writer);
                writer.WriteEndElement();
            }
        }

        internal void addDependency(string v1, string v2) {
            _deps.Add(new PkgDependency(v1, v2));
        }
        #endregion
    }
}