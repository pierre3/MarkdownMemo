using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using My.Mvvm;
using MarkdownMemo.Model;
using System.Windows.Input;
using System.IO;

namespace MarkdownMemo.ViewModel
{
  /// <summary>
  /// リンク要素を管理するViewModel
  /// </summary>
  public class LinkItemViewModel : ViewModelBase
  {
    #region Fields
    private ICommand _addLink;
    private ICommand _openLink;
    private ICommand _deleteLink;
    private ICommand _insertLink;

    private string _linkItemsFileName;
    private string _linkName;
    private string _linkPath;
    private LinkItemCollection _linkItems;
    private LinkItem _selectedLinkItem;
    private string _referenceFileDirectory;
    #endregion

    #region Properties
    /// <summary>Preview HTMLのリンクとして挿入する文字列のコレクション</summary>
    public IEnumerable<string> ReferenceStringsForPreview
    {
      get
      {
        return _linkItems.Select(item => string.Format("[{0}]: {1}", item.ID, item.Path));
      }
    }

    /// <summary>保存用HTMLのリンクとして挿入する文字列のコレクション</summary>
    public IEnumerable<string> ReferenceStringsForSave
    {
      get
      {
        return _linkItems.Select(item => item.ToString());
      }
    }

    /// <summary>添付ファイルのコレクション</summary>
    public LinkItemCollection LinkItems
    {
      get { return _linkItems; }
      set
      {
        _linkItems = value;
      }
    }

    /// <summary>追加用参照ファイル名</summary>
    public string LinkName
    {
      get { return _linkName; }
      set
      {
        _linkName = value;
        OnPropertyChanged("LinkName");
      }
    }

    /// <summary>追加用参照ファイルPath</summary>
    public string LinkPath
    {
      get { return _linkPath; }
      set
      {
        _linkPath = value;
        OnPropertyChanged("LinkPath");
      }
    }

    /// <summary>選択中の参照ファイル</summary>
    public LinkItem SelectedLinkItem
    {
      get { return _selectedLinkItem; }
      set
      {
        _selectedLinkItem = value;
        OnPropertyChanged("SelectedLinkItem");
      }
    }

    #region Commands
    /// <summary>参照ファイルの追加</summary>
    public ICommand AddLinkCommand
    {
      get
      {
        if (_addLink == null)
          _addLink = new DelegateCommand(_ => AddLinkItem(), _ => CanAddLinkItem());
        return _addLink;
      }
    }

    /// <summary>参照ファイルを開く</summary>
    public ICommand OpenLinkCommand
    {
      get
      {
        if (_openLink == null)
          _openLink = new DelegateCommand(_ => OpenLinkItem());
        return _openLink;
      }
    }

    /// <summary>参照ファイルの削除</summary>
    public ICommand DeleteLinkCommand
    {
      get
      {
        if (_deleteLink == null)
        {
          _deleteLink = new DelegateCommand(_ => DeleteLinkItem(),
            _ => CanDeleteLinkItem());
        }
        return _deleteLink;
      }
    }

    /// <summary>テキストに貼り付け</summary>
    public ICommand InsertLinkCommand
    {
      get
      {
        if (_insertLink == null)
        {
          _insertLink = new DelegateCommand(_ => InsertLinkItem(),
            _ => CanInsertLinkItem());
        }
        return _insertLink;
      }
    }
    #endregion
    #endregion

    #region Events
    /// <summary>アイテムの挿入イベント</summary>
    public event Action<string> InsertItem;
    private void OnInsertItem(string insertText)
    {
      var handler = InsertItem;
      if (handler != null)
      {
        handler(insertText);
      }
    }
    #endregion

    #region Constructors
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="referenceFileDirectory">参照ファイルの保存先ディレクトリ</param>
    /// <param name="itemsFileName">参照リストの保存先ファイルの名前</param>
    public LinkItemViewModel(string referenceFileDirectory, string itemsFileName)
    {

      this._referenceFileDirectory = referenceFileDirectory;
      this._linkItemsFileName = itemsFileName;
      this._linkItems = LinkItemCollection.FromXml(itemsFileName);
    }
    #endregion

    #region Public Methods
    /// <summary>アイテムをファイルに保存</summary>
    public void Save()
    {
      this.LinkItems.ToXml(_linkItemsFileName);
    }
    #endregion

    #region Private Methods
    /// <summary>参照ファイルの追加</summary>
    private void AddLinkItem()
    {
      string path;
      if (File.Exists(LinkPath))
      {
        path = Path.Combine("image", Path.GetFileName(LinkPath));
        var dest = Path.Combine(_referenceFileDirectory, path);
        try
        {
          File.Copy(LinkPath, dest);
        }
        catch (Exception e)
        {
          Messenger.Default.Send(this, new DialogBoxMessage(e.Message));
          return;
        }
      }
      else
      {
        path = LinkPath;
      }
      LinkItems.Add(new LinkItem(LinkName, path.Replace('\\', '/')));

      LinkName = string.Empty;
      LinkPath = string.Empty;
    }

    /// <summary>参照ファイルの追加 実行可否</summary>
    /// <returns>追加可能な場合True</returns>
    private bool CanAddLinkItem()
    {
      return !string.IsNullOrEmpty(LinkPath)
        && !string.IsNullOrEmpty(LinkName)
        && !LinkItems.Any(item => item.ID == LinkName);
    }

    /// <summary>参照ファイルを開く</summary>
    private void OpenLinkItem()
    {
      var message = new OpenFileDialogMessage();

      message.Filter = "画像ファイル(*.png;*.gif;*.jpg;*.jpeg)|*.png;*.gif;*.jpg;*.jpeg"
                    + "|Webドキュメント(*.htm*)|*.htm*";
      Messenger.Default.Send(this,message);

      if (message.Result == true)
      {
        if (File.Exists(message.FileName))
        {
          this.LinkPath = message.FileName;
          if (string.IsNullOrEmpty(this.LinkName))
          {
            LinkName = Path.GetFileNameWithoutExtension(LinkPath);
          }
        }
      }
    }

    /// <summary>参照ファイルを開く 実行可否</summary>
    /// <returns>実行可能な場合True</returns>
    private bool CanInsertLinkItem()
    {
      return SelectedLinkItem != null;
    }

    /// <summary>ファイル参照用テキストの挿入</summary>
    private void InsertLinkItem()
    {
      if (SelectedLinkItem == null)
      { return; }

      var path = Path.Combine(_referenceFileDirectory, SelectedLinkItem.Path);
      var substring = default(string);
      if (File.Exists(path))
      {
        substring = string.Format("![Alt?][{0}]", SelectedLinkItem.ID);
      }
      else
      {
        substring = string.Format("[Link Text?][{0}]", SelectedLinkItem.ID);
      }
      OnInsertItem(substring);
    }

    /// <summary>参照ファイル削除 実行可否</summary>
    /// <returns>実行可能な場合True</returns>
    private bool CanDeleteLinkItem()
    {
      return SelectedLinkItem != null;
    }

    /// <summary>参照ファイルの削除</summary>
    private void DeleteLinkItem()
    {
      var path = Path.Combine(_referenceFileDirectory, SelectedLinkItem.Path);
      if (File.Exists(path))
      {
        try
        {
          File.Delete(path);
        }
        catch (Exception e)
        {
          Messenger.Default.Send(this,new DialogBoxMessage(e.Message));
          return;
        }
      }
      LinkItems.Remove(SelectedLinkItem);
    }
    #endregion
  }
}
