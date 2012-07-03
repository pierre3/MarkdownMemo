using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace MarkdownMemo.Model
{
  /// <summary>
  /// 参照アイテムオブジェクト
  /// </summary>
  [Serializable]
  public class LinkItem : INotifyPropertyChanged
  {
    #region Fields
    private string _id;
    private string _path;
    #endregion

    #region Properties
    /// <summary>オブジェクトを識別する文字列</summary>
    [XmlAttribute("ID")]
    public string ID
    {
      set
      {
        _id = value;
        OnPropertyChanged("ID");
      }
      get { return _id; }
    }

    /// <summary>参照ファイルのパス</summary>
    [XmlAttribute("Path")]
    public string Path
    {
      set
      {
        _path = value;
        OnPropertyChanged("Path");
      }
      get { return _path; }
    }
    #endregion

    #region events
    /// <summary>プロパティ変更通知イベント</summary>
    public event PropertyChangedEventHandler PropertyChanged;
    /// <summary>
    /// プロパティ変更通知イベントを発生させます
    /// </summary>
    /// <param name="propertyName">プロパティ名</param>
    protected void OnPropertyChanged(string propertyName)
    {
      var handler = PropertyChanged;
      if (handler != null)
        handler(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion

    #region Constructors
    /// <summary>コンストラクタ</summary>
    public LinkItem()
    {
      this.ID = string.Empty;
      this.Path = string.Empty;
    }
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="id">オブジェクトを識別する文字列</param>
    /// <param name="path">参照ファイルのパス</param>
    public LinkItem(string id, string path)
    {
      this.ID = id;
      this.Path = path;
    }
    #endregion

    #region Methods
    /// <summary>
    /// 表示用テキストの取得
    /// </summary>
    /// <returns>文字列</returns>
    public override string ToString()
    {
      return string.Format("[{0}]: {1}", ID, Path);
    }
    #endregion
  }
}
