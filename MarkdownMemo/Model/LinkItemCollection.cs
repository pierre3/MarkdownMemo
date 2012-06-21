using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;

namespace MarkdownMemo.Model
{
  /// <summary>
  /// 参照アイテムオブジェクトのコレクション
  /// </summary>
  public class LinkItemCollection : ObservableCollection<LinkItem>
  {
    /// <summary>
    /// Xmlファイルから生成
    /// </summary>
    /// <param name="fileName">ファイル名</param>
    /// <returns>LinkItemCollectionのインスタンス</returns>
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

    /// <summary>
    /// Xmlファイルへ保存
    /// </summary>
    /// <param name="fileName">ファイル名</param>
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
