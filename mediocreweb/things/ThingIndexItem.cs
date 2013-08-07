using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Web;

namespace MediocreWeb.things
{
    public class ThingIndexItem
    {

        private class ItemMetaData
        {
            public string Title { get; set; }
            public DateTime? Date { get; set; }
        }

        public ThingIndexItem() { }

        public ThingIndexItem(DirectoryInfo directoryInfo) {
            Contract.Requires(directoryInfo != null);
            Id = directoryInfo.Name;
            Title = Id;

            var metaDataFile = directoryInfo.EnumerateFiles("metadata.json").FirstOrDefault();
            if (metaDataFile != null)
                LoadAttributes(metaDataFile);

        }

        private bool LoadAttributes(FileInfo metaDataFile) {
            var serializer = new ServiceStack.Text.JsonSerializer<ItemMetaData>();
            ItemMetaData metaData;
            using (var metaDataFileStream = File.OpenRead(metaDataFile.FullName))
            using (var metaDataTextStream = new StreamReader(metaDataFileStream))
                metaData = serializer.DeserializeFromReader(metaDataTextStream);

            if (metaData == null)
                return false;

            DateTime = metaData.Date;
            if (!String.IsNullOrWhiteSpace(metaData.Title)) {
                Title = metaData.Title;
            }

            return true;
        }

        public string Id { get; set; }

        public string Title { get; set; }

        public DateTime? DateTime { get; set; }

    }
}