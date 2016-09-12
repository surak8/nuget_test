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
}