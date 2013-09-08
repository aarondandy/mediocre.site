using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using ServiceStack.Text;

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

        public static FileInfo ThingsMetadataFile {
            get { return new FileInfo(Path.Combine(ThingsDirectoryPath, "metadata.json")); }
        }

        public static ThingMetadata[] ReadMetadata() {
            using (var metadataFile = File.OpenRead(ThingsMetadataFile.FullName)) {
                using (JsConfig.BeginScope()) {
                    JsConfig.DateHandler = JsonDateHandler.ISO8601;
                    var serializer = new JsonSerializer<ThingMetadata[]>();
                    using (var reader = new StreamReader(metadataFile)) {
                        return serializer.DeserializeFromReader(reader);
                    }
                }
            }
        }

        public ThingIndex() : this(ReadMetadata()) { }

        public ThingIndex(IEnumerable<ThingMetadata> things) {
            _core = things.ToDictionary(x => x.Key);
        }

        private readonly Dictionary<string, ThingMetadata> _core;

        public IEnumerable<ThingMetadata> AllTheThings {
            get {
                return _core.Values
                .OrderByDescending(x => x.TimeStamp);
            }
        }

        public ThingMetadata GetAThing(string key) {
            ThingMetadata result;
            _core.TryGetValue(key, out result);
            return result;
        }

    }
}