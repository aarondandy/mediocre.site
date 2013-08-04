using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace MediocreWeb.things
{
    public class ThingIndex
    {

        public static string ThingsDirectoryPath {
            get { return HostingEnvironment.MapPath("~/things/"); }
        }

        public static DirectoryInfo ThingsDirectory {
            get { return new DirectoryInfo(ThingsDirectoryPath); }
        }

        public static ThingIndexItem[] GetAllThings() {
            return Array.ConvertAll(ThingsDirectory.GetDirectories(), di => new ThingIndexItem(di));
        }

        public ThingIndex() {
            AllThings = GetAllThings().ToArray();
        }

        public ThingIndexItem[] AllThings { get; private set; }

    }
}