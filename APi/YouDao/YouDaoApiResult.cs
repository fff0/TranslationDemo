using System.Collections.Generic;

namespace FutureAuto.Machine.TranslationSoftware.APi.YouDao
{
    internal class YouDaoApiResult
    {
        public List<string> returnPhrase { get; set; } = new List<string>();
        public string query { get; set; }
        public string errorCode { get; set; }
        public string l { get; set; }
        public string tSpeakUrl { get; set; }
        public List<WebData> web { get; set; } = new List<WebData>();
        public string requestId { get; set; }
        public List<string> translation { get; set; } = new List<string>();
        public UrlData mTerminalDict { get; set; } = new UrlData();
        public UrlData dict { get; set; } = new UrlData();
        public UrlData webdict { get; set; } = new UrlData();
        public BasicData basic { get; set; } = new BasicData();
        public string isWord { get; set; }
        public string speakUrl { get; set; }

    }

    public class WebData
    {
        public List<string> value
        {
            get;
            set;
        } = new List<string>();

        public string key { get; set; }
    }

    public class UrlData
    {
        public string url { get; set; }
    }

    public class BasicData
    {
        public string phonetic { get; set; }

        public List<string> explains
        {
            get;
            set;
        } = new List<string>();
    }
}
