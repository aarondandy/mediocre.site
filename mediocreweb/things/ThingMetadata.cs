using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace MediocreWeb.things
{
    public class ThingMetadata
    {

        /*private class ItemMetaData
        {
            public string Title { get; set; }
            public DateTime? Date { get; set; }
        }*/

        private class MetadataFileModel
        {

            public static MetadataFileModel Read(FileInfo fileInfo) {
                return Read(fileInfo.FullName);
            }

            public static MetadataFileModel Read(string filePath) {
                var serializer = new ServiceStack.Text.JsonSerializer<MetadataFileModel>();
                using (var metaDataFileStream = File.OpenRead(filePath))
                using (var metaDataTextStream = new StreamReader(metaDataFileStream))
                    return serializer.DeserializeFromReader(metaDataTextStream);
            }

            public string Title { get; set; }
            public DateTime? Date { get; set; }
        }

        private static ThingMetadata Create(
            string baseName,
            FileInfo metadataFileInfo
        ) {
            var result = new ThingMetadata();
            if (metadataFileInfo.Exists) {
                var metadata = MetadataFileModel.Read(metadataFileInfo);
                if (metadata != null) {
                    result.Title = metadata.Title;
                    result.DateTime = metadata.Date;
                }
            }

            if (String.IsNullOrWhiteSpace(result.Title))
                result.Title = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(
                    baseName.Replace('-', ' '));

            result.ThingUri = baseName;

            return result;
        }

        public static ThingMetadata Create(DirectoryInfo directoryInfo) {
            return Create(
                directoryInfo.Name,
                new FileInfo(Path.Combine(directoryInfo.FullName, "metadata.json")));
        }

        public static ThingMetadata Create(FileInfo fileInfo) {
            return Create(
                Path.GetFileNameWithoutExtension(fileInfo.Name),
                new FileInfo(Path.ChangeExtension(fileInfo.FullName, ".json")));
        }

        private ThingMetadata() { }

        public string Title { get; set; }

        public DateTime? DateTime { get; set; }

        public string ThingUri { get; set; }

    }
}