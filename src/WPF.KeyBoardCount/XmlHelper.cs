using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace WPF.KeyBoardCount
{
    class XmlHelper
    {
        private readonly string _fileName = "tdata.td";

        private readonly string _filePath = "";

        private XmlDocument doc = new XmlDocument();

        public XmlHelper(string dir)
        {
            //
            this._filePath = Path.Combine(dir, _fileName);
            if (!CreateFile(this._filePath))
            {
                this._filePath = Path.Combine(this.getSysBaseDir(), _fileName);
                if (!CreateFile(this._filePath))
                {
                    Console.WriteLine("创建数据文件失败");
                    return;
                }
            }
            LoadXML();
        }

        private bool CreateFile(string filepath)
        {
            try
            {
                if (File.Exists(filepath))
                    return true;
                FileStream fs = File.Create(filepath);
                string xml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n\r<tdata>\n\r<days>\n\r</days>\n\r</tdata>";
                byte[] bts = System.Text.Encoding.Default.GetBytes(xml);
                fs.Write(bts, 0, bts.Length);
                fs.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private string getSysBaseDir()
        {
            return System.Environment.SystemDirectory;
        }

        public XmlDocument LoadXML()
        {
            doc.RemoveAll();
            doc.LoadXml(File.ReadAllText(this._filePath));
            if (doc == null)
            {
                //
                doc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n\r<tdata>\n\r<days>\n\r</days>\n\r</tdata>");
            }
            return doc;
        }

        public bool WriteData()
        {
            XmlNode node = doc.SelectSingleNode("/tdata/days/onedaydata[@day='" + DateTime.Today.ToString("yyyy-MM-dd") + "']");
            if (node != null)
            {
                //增加数量
                long num = 0;
                if (!long.TryParse(node.InnerText, out num))
                {
                    Console.WriteLine("数据文件中的数据错误！");
                    return false;
                }
                num += App.GetCurKeyCount();
                node.InnerText = num.ToString();
            }
            else
            {
                node = doc.CreateElement("onedaydata");
                XmlAttribute workidAttr = doc.CreateAttribute("workid");
                workidAttr.Value = App.GetGuid();
                XmlAttribute macaddrAttr = doc.CreateAttribute("macaddr");
                macaddrAttr.Value = App.mac;
                XmlAttribute dayAttr = doc.CreateAttribute("day");
                dayAttr.Value = DateTime.Now.ToString("yyyy-MM-dd");
                XmlAttribute timesatmpAttr = doc.CreateAttribute("timesatmp");
                timesatmpAttr.Value = DateTime.Now.ToString();
                XmlText txtNode = doc.CreateTextNode(App.GetCurKeyCount().ToString());
                node.AppendChild(txtNode);
                node.Attributes.Append(workidAttr);
                node.Attributes.Append(macaddrAttr);
                node.Attributes.Append(dayAttr);
                node.Attributes.Append(timesatmpAttr);
                //node = doc.CreateComment(CreateNode(App.GetGuid(), App.GetCurKeyCount().ToString()));
                doc.SelectSingleNode("tdata/days").AppendChild(node);
                //node = doc.CreateElement("");
                //创建新节点
            }
            doc.Save(_filePath);
            App.ClearCurKeyCount();
            return true;
        }

        public bool Run()
        {
            while (true)
            {
                //开发过程为不影响以前版本使用先关闭写数据功能
                //WriteData();
                Thread.Sleep(600000);
            }
        }
    }
}
