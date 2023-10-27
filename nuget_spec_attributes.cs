using System;

namespace Colt.Nuget.Utilities {
    [AttributeUsage(AttributeTargets.Assembly)]
    public class ProjectUrlAttribute : Attribute {
        public ProjectUrlAttribute() : this(string.Empty) { }
        public ProjectUrlAttribute(string url) {
            projectUrl = url;
        }

        public string projectUrl { get; set; }
    }

    [AttributeUsage(AttributeTargets.Assembly)]
    public class LicenseUrlAttribute : Attribute {
        public LicenseUrlAttribute() : this(string.Empty) { }
        public LicenseUrlAttribute(string url) {
            licenseUrl = url;
        }

        public string licenseUrl { get; set; }
    }

    [AttributeUsage(AttributeTargets.Assembly)]
    public class AuthorsAttribute : Attribute {
        #region ctors
        public AuthorsAttribute() : this(string.Empty) { }
        public AuthorsAttribute(string authorsValue) {
            authors = authorsValue;
        }
        #endregion

        #region properties
        public string authors { get; set; }
        #endregion
    }

    [AttributeUsage(AttributeTargets.Assembly)]
    public class OwnersAttribute : Attribute {
        #region ctors
        public OwnersAttribute() : this(string.Empty) { }
        public OwnersAttribute(string ownersValue) {
            owners = ownersValue;
        }
        #endregion

        #region properties
        public string owners { get; set; }
        #endregion
    }
    [AttributeUsage(AttributeTargets.Assembly)]
    public class ReleaseNotesAttribute : Attribute {
        #region ctors
        public ReleaseNotesAttribute() : this(string.Empty) { }
        public ReleaseNotesAttribute(string relNotesValue) {
            releaseNotes = relNotesValue;
        }
        #endregion

        #region properties
        public string releaseNotes { get; set; }
        #endregion
    }
}