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

        public ThingIndexItem() { }

        public ThingIndexItem(DirectoryInfo directoryInfo) {
            Contract.Requires(directoryInfo != null);
            Id = directoryInfo.Name;
            Title = Id;
        }

        public string Id { get; set; }

        public string Title { get; set; }

    }
}