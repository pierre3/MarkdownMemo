using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;

namespace MarkdownMemo.Model
{
  [Serializable]
  public class LinkItem : INotifyPropertyChanged
  {
    #region Fields
    private string _id;
    private string _path;
    #endregion

    #region Properties
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
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
      var handler = PropertyChanged;
      if (handler != null)
        handler(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion

    #region Constructors
    public LinkItem()
    {
      this.ID = string.Empty;
      this.Path = string.Empty;
    }
    public LinkItem(string id, string path)
    {
      this.ID = id;
      this.Path = path;
    }
    #endregion

    #region Methods
    public override string ToString()
    {
      return string.Format("[{0}]: {1}", this.ID, System.IO.Path.GetFileName(this.Path));
    }
    #endregion
  }
}
