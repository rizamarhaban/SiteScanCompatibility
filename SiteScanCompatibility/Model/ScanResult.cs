using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace SiteScanCompatibility.Model
{
    // TODO: Needs to be cleanup

    [DataContract]
    [KnownType(typeof(Url))]
    [KnownType(typeof(Results))]
    public class RootObject
    {
        [JsonProperty("version")]
        public string version { get; set; }

        [JsonProperty("url")]
        public Url url { get; set; }

        [JsonProperty("processTime")]
        public double processTime { get; set; }

        [JsonProperty("results")]
        public Results results { get; set; }
    }

    [DataContract]
    public class Results
    {
        [JsonProperty("imageCompression")]
        public ImageCompression imageCompression { get; set; }

        [JsonProperty("w3c-validator")]
        public W3cValidator w3c_validator { get; set; }

        [JsonProperty("altImg")]
        public AltImg altImg { get; set; }

        [JsonProperty("ariaTags")]
        public AriaTags ariaTags { get; set; }

        [JsonProperty("browserDetection")]
        public BrowserDetection browserDetection { get; set; }

        [JsonProperty("compression")]
        public Compression compression { get; set; }

        [JsonProperty("cssprefixes")]
        public Cssprefixes cssprefixes { get; set; }

        [JsonProperty("ie11tiles")]
        public Ie11tiles ie11tiles { get; set; }

        [JsonProperty("inputTypes")]
        public InputTypes inputTypes { get; set; }

        [JsonProperty("jslibs")]
        public Jslibs jslibs { get; set; }

        [JsonProperty("pagination")]
        public Pagination pagination { get; set; }

        [JsonProperty("pluginfree")]
        public Pluginfree pluginfree { get; set; }

        [JsonProperty("prefetch")]
        public Prefetch prefetch { get; set; }

        [JsonProperty("rendermode")]
        public Rendermode rendermode { get; set; }

        [JsonProperty("responsive")]
        public Responsive responsive { get; set; }

        [JsonProperty("touch")]
        public Touch touch { get; set; }
    }

    [DataContract]
    public class Url
    {
        [JsonProperty("uri")]
        public string uri { get; set; }
    }

    [DataContract]
    public class ImageCompression
    {
        [JsonProperty("testName")]
        public string testName { get; set; }

        [JsonProperty("passed")]
        public bool passed { get; set; }
    }

    public class W3cValidator
    {
        [JsonProperty("testName")]
        public string testName { get; set; }

        [JsonProperty("passed")]
        public bool passed { get; set; }

        [JsonProperty("data")]
        public List<string> data { get; set; }
    }

    public class AltImg
    {
        [JsonProperty("testName")]
        public string testName { get; set; }

        [JsonProperty("passed")]
        public bool passed { get; set; }

        [JsonProperty("data")]
        public List<string> data { get; set; }
    }

    public class AriaTags
    {
        [JsonProperty("testName")]
        public string testName { get; set; }

        [JsonProperty("passed")]
        public bool passed { get; set; }

        [JsonProperty("data")]
        public List<object> data { get; set; }
    }

    public class Datum
    {
        [JsonProperty("passed")]
        public bool passed { get; set; }
        public string pattern { get; set; }
        public int lineNumber { get; set; }
        public string url { get; set; }
    }

    public class Javascript
    {
        [JsonProperty("passed")]
        public bool passed { get; set; }

        [JsonProperty("data")]
        public List<Datum> data { get; set; }
    }

    public class Comments
    {
        [JsonProperty("passed")]
        public bool passed { get; set; }
    }

    public class Data
    {
        [JsonProperty("javascript")]
        public Javascript javascript { get; set; }

        [JsonProperty("comments")]
        public Comments comments { get; set; }
    }

    public class BrowserDetection
    {
        [JsonProperty("testName")]
        public string testName { get; set; }

        [JsonProperty("passed")]
        public bool passed { get; set; }

        [JsonProperty("data")]
        public Data data { get; set; }
    }

    public class Data2
    {
        [JsonProperty("compression")]
        public string compression { get; set; }
    }

    public class Compression
    {
        [JsonProperty("testName")]
        public string testName { get; set; }

        [JsonProperty("passed")]
        public bool passed { get; set; }

        [JsonProperty("data")]
        public Data2 data { get; set; }
    }

    public class Selector
    {

        [JsonProperty("selector")]
        public string selector { get; set; }

        [JsonProperty("lineNumber")]
        public int lineNumber { get; set; }

        [JsonProperty("styles")]
        public List<string> styles { get; set; }
    }

    public class Datum2
    {
        [JsonProperty("cssFile")]
        public string cssFile { get; set; }

        [JsonProperty("selectors")]
        public List<Selector> selectors { get; set; }
    }

    [DataContract]
    public class Cssprefixes
    {
        [JsonProperty("testName")]
        public string testName { get; set; }

        [JsonProperty("passed")]
        public bool passed { get; set; }

        [JsonProperty("data")]
        public List<Datum2> data { get; set; }
    }

    public class Data3
    {
        public bool square70 { get; set; }
        public bool square150 { get; set; }
        public bool wide310 { get; set; }
        public bool square310 { get; set; }
        public bool notifications { get; set; }
    }

    public class Ie11tiles
    {
        [JsonProperty("testName")]
        public string testName { get; set; }

        [JsonProperty("passed")]
        public bool passed { get; set; }

        [JsonProperty("data")]
        public Data3 data { get; set; }
    }

    public class InputTypes
    {
        [JsonProperty("testName")]
        public string testName { get; set; }

        [JsonProperty("passed")]
        public bool passed { get; set; }
    }

    public class Datum3
    {
        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("needsUpdate")]
        public bool needsUpdate { get; set; }

        [JsonProperty("minVersion")]
        public string minVersion { get; set; }

        [JsonProperty("version")]
        public string version { get; set; }

        [JsonProperty("url")]
        public string url { get; set; }

        [JsonProperty("lineNumber")]
        public int lineNumber { get; set; }
    }

    public class Jslibs
    {
        [JsonProperty("testName")]
        public string testName { get; set; }

        [JsonProperty("url")]
        public string url { get; set; }

        [JsonProperty("passed")]
        public bool passed { get; set; }

        [JsonProperty("data")]
        public List<Datum3> data { get; set; }
    }

    public class Data4
    {
    }

    public class Pagination
    {
        [JsonProperty("testName")]
        public string testName { get; set; }

        [JsonProperty("passed")]
        public bool passed { get; set; }

        [JsonProperty("data")]
        public Data4 data { get; set; }
    }

    public class Pluginfree
    {
        [JsonProperty("testName")]
        public string testName { get; set; }

        [JsonProperty("passed")]
        public bool passed { get; set; }

        [JsonProperty("data")]
        public List<object> data { get; set; }
    }

    public class Data5
    {
        public bool prefetch { get; set; }
        public bool dnsprefetch { get; set; }
        public bool prerender { get; set; }
    }

    public class Prefetch
    {
        [JsonProperty("testName")]
        public string testName { get; set; }

        [JsonProperty("passed")]
        public bool passed { get; set; }

        [JsonProperty("data")]
        public Data5 data { get; set; }
    }

    public class Data6
    {
        [JsonProperty("source")]
        public string source { get; set; }

        //[JsonProperty("mode")]
        //public List<string> mode { get; set; }
    }

    public class Rendermode
    {
        [JsonProperty("testName")]
        public string testName { get; set; }

        [JsonProperty("passed")]
        public bool passed { get; set; }

        [JsonProperty("data")]
        public Data6 data { get; set; }
    }

    public class Data7
    {
        public List<object> minBreakPoints { get; set; }
        public List<object> maxBreakPoints { get; set; }
        public List<object> spectrum { get; set; }
    }

    public class Responsive
    {
        [JsonProperty("testName")]
        public string testName { get; set; }

        [JsonProperty("passed")]
        public bool passed { get; set; }

        [JsonProperty("data")]
        public Data7 data { get; set; }
    }

    public class Touch
    {
        [JsonProperty("testName")]
        public string testName { get; set; }

        [JsonProperty("passed")]
        public bool passed { get; set; }
    }


}
