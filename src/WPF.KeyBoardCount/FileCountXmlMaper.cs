using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace WPF.KeyBoardCount
{
    public class FileCountXmlMaper
    {
        public string _savePath = Environment.CurrentDirectory;
        private string _fileName = "rowdata.xml";
        private readonly string _filePath = "";
        private XmlDocument _doc = new XmlDocument();
        private XmlNode _curNode;
        public FileCountXmlMaper()
        {
            if (!Directory.Exists(_savePath))
            {
                Directory.CreateDirectory(_savePath);
            }
            string _filePath = Path.Combine(_savePath, _fileName);
            if (!File.Exists(_filePath))
            {
                FileStream fs = File.Create(_filePath);
                string xml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n\r<rowdata>\n\r<username>\n\r</username>\n\r<guid>\n\r</guid>\n\r<macaddr>\n\r</macaddr>\n\r<timestamp>\n\r</timestamp>\n\r<directis>\n\r</directis>\n\r</rowdata>";
                byte[] bts = System.Text.Encoding.Default.GetBytes(xml);
                fs.Write(bts, 0, bts.Length);
                fs.Close();
            }
            LoadXML();
        }

        public XmlDocument LoadXML()
        {
            _doc.RemoveAll();
            _doc.LoadXml(File.ReadAllText(this._filePath));
            if (_doc == null)
            {
                string xml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n\r<rowdata>\n\r<username>\n\r</username>\n\r<guid>\n\r</guid>\n\r<macaddr>\n\r</macaddr>\n\r<timestamp>\n\r</timestamp>\n\r<directis>\n\r</directis>\n\r</rowdata>";
                _doc.LoadXml(xml);
            }
            return _doc;
        }

        public void SearchDirectory(string path) {
            _curNode = _doc.SelectSingleNode("/rowdata/directis/directoryinfo[@path='" + path + "']");
            if (_curNode == null)
            {
                _curNode = _doc.CreateElement("directoryinfo");

                XmlAttribute workidAttr = _doc.CreateAttribute("guid");
                workidAttr.Value = App.GetGuid();
                XmlAttribute pathAttr = _doc.CreateAttribute("path");
                pathAttr.Value = path;
                XmlAttribute timesatmpAttr = _doc.CreateAttribute("timesatmp");
                timesatmpAttr.Value = DateTime.Now.ToString();
                _curNode.Attributes.Append(workidAttr);
                _curNode.Attributes.Append(pathAttr);
                _curNode.Attributes.Append(timesatmpAttr);
                //node = doc.CreateComment(CreateNode(App.GetGuid(), App.GetCurKeyCount().ToString()));
                _doc.SelectSingleNode("/rowdata/directis/").AppendChild(_curNode);
                _doc.Save(_filePath);
            }
        }

    }

    public class filecount
    {
        public string username { get; set; }
        public string guid { get; set; }
        public string macaddr { get; set; }
        public DateTime timesatmp { get; set; }
        public List<directoryinfo> directis { get; set; }
    }

    public class directoryinfo
    {
        public string guid { get; set; }
        public string path { get; set; }
        public DateTime timesatmp { get; set; }
        public List<countdata> countdatas { get; set; }
    }

    public class countdata
    {
        public string guid { get; set; }
        public int num { get; set; }
        public string name { get; set; }
        public int filecount { get; set; }
        public int rowcount { get; set; }
        public DateTime timesatmp { get; set; }
    }
}
