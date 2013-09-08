using System;
namespace MediocreWeb.things
{
    public class ThingMetadata
    {
        public string Key { get; set; }
        public string Title { get; set; }
        public DateTime? TimeStamp { get; set; }

        private string _thingUri;
        public string ThingUri {
            get { return _thingUri ?? Key; }
            set { _thingUri = value; }
        }
    }
}