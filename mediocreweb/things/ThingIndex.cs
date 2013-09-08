using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;

namespace MediocreWeb.things
{
    public class ThingIndex
    {

        public static string ThingsDirectoryPath {
            get {
                return HostingEnvironment.MapPath("~/things/");
            }
        }

        public static DirectoryInfo ThingsDirectory {
            get { return new DirectoryInfo(ThingsDirectoryPath); }
        }

        public static IEnumerable<ThingMetadata> GetAllThings() {
            var thingsDirectory = ThingsDirectory;

            var validSubDirectories = thingsDirectory
                .GetDirectories()
                .Where(d => d.GetFiles("default.cshtml").Any())
                .Select(ThingMetadata.Create);

            var validRootThings = thingsDirectory
                .GetFiles("*.cshtml")
                .Select(ThingMetadata.Create);

            return validSubDirectories
                .Concat(validRootThings)
                .OrderByDescending(t => t.DateTime)
                .ThenBy(t => t.Title);
        }

        public ThingIndex() {
            AllThings = GetAllThings().ToArray();
        }

        public ThingMetadata[] AllThings { get; private set; }

    }
}