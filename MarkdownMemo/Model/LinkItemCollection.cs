using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

namespace MarkdownMemo.Model
{
  public class LinkItemCollection : ObservableCollection<LinkItem>
  {

    public static LinkItemCollection FromXml(string fileName)
    {
      if (!File.Exists(fileName))
      { return new LinkItemCollection(); }

      using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
      {
        var serializer = new XmlSerializer(typeof(LinkItemCollection));
        return (LinkItemCollection)serializer.Deserialize(stream);
      }
    }

    public void ToXml(string fileName)
    {
      using (var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
      {
        var serializer = new XmlSerializer(typeof(LinkItemCollection));
        serializer.Serialize(stream, this);
      }
    }
  }
}
