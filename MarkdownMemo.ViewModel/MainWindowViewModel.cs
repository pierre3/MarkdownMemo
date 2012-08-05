using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MarkdownMemo.Model;
using My.Mvvm;

namespace MarkdownMemo.ViewModel
{
  /// <summary>
  /// メイン画面用ViewModelクラス
  /// </summary>
  public class MainwindowViewModel : ViewModelBase, 
    IFileOpener, ITerminatable
  {
    #region Fields
    private string _title;
    private MarkdownText _markdownText;
    private ICommand _new;
    private ICommand _open;
    private ICommand _save;
    private ICommand _saveAs;
    private ICommand _exit;
    private ICommand _addLink;
    private ICommand _openLink;
    private ICommand _deleteLink;
    private ICommand _insertLink;
    private ICommand _saveHtml;
    private string _linkItemsFileName;
    private string _linkName;
    private string _linkPath;
    private LinkItemCollection _linkItems;
    private LinkItem _selectedLinkItem;
    private int? _caretIndex;
    #endregion

    #region Properties
    /// <summary>HTMLに変換するテキスト</summary>
    public string Text
    {
      get { return _markdownText.Text; }
      set
      {
        _markdownText.Text = value;
        //OnPropertyChanged("Text") はMarkdown.Textプロパティで呼ばれる
        SetTitle();
      }
    }

    /// <summary>タイトルとして表示する文字列</summary>
    public string Title
    {
      get { return _title; }
      set
      {
        _title = value;
        OnPropertyChanged("Title");
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

    /// <summary>追加用添付ファイル名</summary>
    public string LinkName
    {
      get { return _linkName; }
      set
      {
        _linkName = value;
        OnPropertyChanged("LinkName");
      }
    }

    /// <summary>追加用添付ファイルPath</summary>
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

    /// <summary>カレット位置</summary>
    public int? CaretIndex
    {
      get { return _caretIndex; }
      set
      {
        _caretIndex = value;
        OnPropertyChanged("CaretIndex");
      }
    }

    /// <summary>メッセンジャー</summary>
    public Messenger Messenger { set; get; }

    #region commands
    /// <summary>新規作成</summary>
    public ICommand NewCommand
    {
      get
      {
        if (_new == null)
          _new = new DelegateCommand(_ => this.New());
        return _new;
      }
    }

    /// <summary>開く</summary>
    public ICommand OpenCommand
    {
      get
      {
        if (_open == null)
          _open = new DelegateCommand(_ => this.Open());
        return _open;
      }
    }

    /// <summary>上書き保存</summary>
    public ICommand SaveCommand
    {
      get
      {
        if (_save == null)
          _save = new DelegateCommand(_ => this.Save());
        return _save;
      }
    }

    /// <summary>名前を付けて保存</summary>
    public ICommand SaveAsCommand
    {
      get
      {
        if (_saveAs == null)
          _saveAs = new DelegateCommand(_ => this.SaveAs());
        return _saveAs;
      }
    }

    /// <summary>終了</summary>
    public ICommand ExitCommand
    {
      get
      {
        if (_exit == null)
          _exit = new DelegateCommand(_ => OnRequestClose());
        return _exit;
      }
    }

    /// <summary>Html形式で保存</summary>
    public ICommand SaveHtmlCommand
    {
      get
      {
        if (_saveHtml == null)
          _saveHtml = new DelegateCommand(_ => SaveHtml());
        return _saveHtml;
      }
    }

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

    /// <summary>参照ファイルの追加</summary>
    public ICommand OpenLinkCommand
    {
      get
      {
        if (_openLink == null)
          _openLink = new DelegateCommand(_ => OpenLinkItem());
        return _openLink;
      }
    }

    /// <summary>参照ファイルの追加</summary>
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

    /// <summary>参照ファイルの追加</summary>
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
    /// <summary>Viewの終了要求</summary>
    public EventHandler RequestClose;
    private void OnRequestClose()
    {
      var handler = RequestClose;
      if (handler != null)
        handler(this, new EventArgs());
    }
    #endregion

    #region Constructor
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="previewPath">
    /// HTMLプレビューファイル名をフルパスで指定します
    /// </param>
    /// <param name="cssName">
    /// CSSファイル名をHTMLプレビューファイル保存先からの相対パスで指定します
    /// </param>
    /// <param name="requestPreview">Viewによるプレビュー表示の更新を記述するデリゲート</param>
    /// <param name="startupFile">起動時に読み込むファイルの名前</param>
    /// <param name="updatePreviewTrigger">プレビュー表示の更新のトリガとなるイベントのIObservable</param>
    public MainwindowViewModel(string previewPath, string cssName,
      IObservable<EventArgs> updatePreviewTrigger, Action<string> requestPreview, string startupFile)
    {
      this.Messenger = new Messenger();
      this.CaretIndex = 0;
      this._linkItemsFileName = Path.Combine(
        Path.GetDirectoryName(previewPath), "LinkItems.xml");
      LinkItems = LinkItemCollection.FromXml(_linkItemsFileName);

      _markdownText = new MarkdownText(previewPath, cssName);
      _markdownText.TextChanged += () => OnPropertyChanged("Text");

      updatePreviewTrigger.Subscribe(_ =>
        {
          _markdownText.SavePreviewHtml(
            LinkItems.Select(item => string.Format("[{0}]: {1}", item.ID, item.Path)));
          var message = new RequestPreviewMessage(new Uri(_markdownText.PreviewPath));
          this.Messenger.Send(this, message);
          //requestPreview(_markdownText.PreviewPath);
        }, e => Trace.TraceError("{0},[StackTrace: {1}]", e.Message, e.StackTrace));

      if (File.Exists(startupFile))
      {
        _markdownText.OpenFrom(startupFile);
      }
      else
      {
        _markdownText.OpenNew();
      }

      SetTitle();
    }
    #endregion

    #region Command Handlers
    /// <summary>新規作成(N)</summary>
    private void New()
    {
      if (ConfirmSaveFile())
      {
        _markdownText.OpenNew();
        SetTitle();
      }
    }

    /// <summary>上書き保存(N)</summary>
    private void Save()
    {
      if (!_markdownText.Save())
      {
        SaveAs();
      }
      else { SetTitle(); }
    }

    /// <summary>名前を付けて保存(N)</summary>
    private void SaveAs()
    {
      var message = new SaveFileDialogMessage();
      message.DefaultExt = ".md";
      message.Filter = "MarkDownファイル(*.md;*.markdown)|*.md;*.markdown"
                    + "|テキストファイル(*.txt)|*.txt";
      if (message.Result == true)
      {
        _markdownText.SaveTo(message.FileName);
        SetTitle();
      }
    }

    /// <summary>HTML形式で保存</summary>
    private void SaveHtml()
    {
      var message = new SaveFileDialogMessage();
      message.DefaultExt = ".html";
      message.Filter = "Webページ(*.html;*.htm)|*.html;*.htm";
      this.Messenger.Send(this, message);
      if (message.Result == true)
      {
        _markdownText.SaveAsHtml(message.FileName,
          Path.GetFileNameWithoutExtension(message.FileName),
          LinkItems.Select(item => item.ToString()));
      }
    }

    /// <summary>開く(O)</summary>
    private void Open()
    {
      if (ConfirmSaveFile())
      {
        var message = new OpenFileDialogMessage();
        message.DefaultExt = ".md";
        message.Filter = "MarkDownファイル(*.md;*.markdown)|*.md;*.markdown"
                      + "|テキストファイル(*.txt)|*.txt"
                      + "|全てのファイル(*.*)|*.*";
        this.Messenger.Send(this,message);
        if (message.Result == true)
        {
          _markdownText.OpenFrom(message.FileName);
          SetTitle();
        }
      }
    }

    /// <summary>参照ファイルの追加</summary>
    private void AddLinkItem()
    {
      string path;
      if (File.Exists(LinkPath))
      {
        path = Path.Combine("image", Path.GetFileName(LinkPath));
        var dest = Path.Combine(Path.GetDirectoryName(_markdownText.PreviewPath), path);
        try
        {
          File.Copy(LinkPath, dest);
        }
        catch (Exception e)
        {
          MessageBox.Show(e.Message);
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

    /// <summary>
    /// 参照ファイルの追加 実行可否
    /// </summary>
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
      this.Messenger.Send(this, message);
      
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

    /// <summary>
    /// 参照ファイルを開く 実行可否
    /// </summary>
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
      var path = Path.Combine(Path.GetDirectoryName(_markdownText.PreviewPath), SelectedLinkItem.Path);
      var substring = default(string);
      if (File.Exists(path))
      {
        substring = string.Format("![Alt?][{0}]", SelectedLinkItem.ID);
      }
      else
      {
        substring = string.Format("[Link Text?][{0}]", SelectedLinkItem.ID);
      }
      this.Text = this.Text.Insert(CaretIndex ?? 0, substring);
    }

    /// <summary>
    /// 参照ファイル削除 実行可否
    /// </summary>
    /// <returns>実行可能な場合True</returns>
    private bool CanDeleteLinkItem()
    {
      return SelectedLinkItem != null;
    }

    /// <summary>参照ファイルの削除</summary>
    private void DeleteLinkItem()
    {
      var path = Path.Combine(Path.GetDirectoryName(_markdownText.PreviewPath), SelectedLinkItem.Path);
      if (File.Exists(path))
      {
        try
        {
          File.Delete(path);
        }
        catch (Exception e)
        {
          MessageBox.Show(e.Message);
          return;
        }
      }
      LinkItems.Remove(SelectedLinkItem);
    }

    #endregion

    #region Expricit Interface Implementation
    /// <summary>
    /// 指定したファイルを開く
    /// </summary>
    /// <param name="fileName"></param>
    void IFileOpener.Open(string fileName)
    {
      if (!File.Exists(fileName))
      { return; }
      _markdownText.OpenFrom(fileName);
    }

    /// <summary>
    /// 後始末
    /// </summary>
    void ITerminatable.Terminate()
    {
      if (!ConfirmSaveFile())
      { return; }
      this.LinkItems.ToXml(_linkItemsFileName);
      File.Delete(_markdownText.PreviewPath);
    }
    #endregion

    #region private methods
    /// <summary>
    /// 未保存のテキストを上書き保存するダイアログを表示します
    /// </summary>
    /// <returns>
    /// ユーザがMessageBoxResult.Cancelを応答した場合Falseを返します
    /// </returns>
    private bool ConfirmSaveFile()
    {
      if (!_markdownText.IsTextChanged)
      {
        return true;
      }

      var message = new DialogBoxMessage("編集中のテキストが保存されていません。上書き保存しますか？", "Markdown Memo",
          MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
      this.Messenger.Send(this, message);
      
      if (message.Result == MessageBoxResult.Cancel)
      {
        return false;
      }
      else if (message.Result == MessageBoxResult.Yes)
      {
        Save();
      }
      return true;
    }

    /// <summary>画面タイトルの更新</summary>
    private void SetTitle()
    {
      var name = string.IsNullOrEmpty(_markdownText.SourcePath) ?
        "無題" : Path.GetFileName(_markdownText.SourcePath);
      this.Title = name + (_markdownText.IsTextChanged ? " *" : "") + " - Markdown Memo";
    }
    #endregion
  }
}
