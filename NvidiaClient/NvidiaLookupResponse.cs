using System.Xml.Serialization;

namespace NvidiaDriverUpdater.NvidiaClient
{
    [XmlRoot("LookupValueSearch")]
    public class NvidiaLookupResponse
    {
        [XmlElement("LookupValues")]
        public LookupValues LookupValues { get; set; }
    }

    public class LookupValues
    {
        [XmlElement("LookupValue")]
        public LookupValue[] Values { get; set; }
    }

    public class LookupValue
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Value")]
        public string Value { get; set; }
    }
}