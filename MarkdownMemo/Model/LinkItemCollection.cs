using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;

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
