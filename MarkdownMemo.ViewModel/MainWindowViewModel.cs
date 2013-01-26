using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MarkdownMemo.Model;
using My.Mvvm;
using System.Reactive;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using My.Common;

namespace MarkdownMemo.ViewModel
{
  /// <summary>
  /// メイン画面用ViewModelクラス
  /// </summary>
  public class MainwindowViewModel : ViewModelBase,
    IFileCommands, ITerminatable,IDisposable
  {
    #region Fields
    private string _title;
    private MarkdownText _markdownText;
    private ICommand _exit;
    private ICommand _saveHtml;
    private int _caretIndex;
    private Subject<string> _subject;
    private IDisposable _disposable;
    private LinkItemViewModel _linkItemViewModel;
    #endregion

    #region Properties
    /// <summary>HTMLに変換するテキスト</summary>
    public string Text
    {
      get { return _markdownText.Text; }
      set
      {
        _markdownText.Text = value;
        // 以下はMarkdownText.OnTextChanged()経由で呼ばれる
        //_subject.OnNext(value);
        //OnPropertyChanged("Text");
        //SetTitle();
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

    /// <summary>リンク要素を管理するViewModel</summary>
    public LinkItemViewModel LinkItemViewModel { get { return _linkItemViewModel; } }

    /// <summary>カレット位置</summary>
    public int CaretIndex
    {
      get { return _caretIndex; }
      set
      {
        _caretIndex = value;
        OnPropertyChanged("CaretIndex");
      }
    }


    #region commands
    /// <summary>終了</summary>
    public ICommand ExitCommand
    {
      get
      {
        if (_exit == null)
          _exit = new DelegateCommand(_ => this.RequestClose());
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
    #endregion
    #endregion

    #region Constructor
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public MainwindowViewModel()
    {
      this.CaretIndex = 0;
            
      var userDir = PathHelper.CreateAppDataDirectory();
      System.IO.Directory.CreateDirectory(System.IO.Path.Combine(userDir, "image"));
      
      this._linkItemViewModel = new LinkItemViewModel(userDir, Path.Combine(userDir, "LinkItems.xml"));
      this._linkItemViewModel.InsertItem += s => {
        this.Text = this.Text.Insert(CaretIndex < 0 ? 0 : CaretIndex, s);
      };
      
      var processId = System.Diagnostics.Process.GetCurrentProcess().Id.ToString();
      var previewPath = System.IO.Path.Combine(userDir, processId + "_Preview.html");
      _markdownText = new MarkdownText(previewPath, "Markdown.css", 
        text =>
      {
        //ModelのTextプロパティ変更イベントのハンドラ
        OnPropertyChanged("Text");　//ViewModelのPropertyChanged()を呼ぶ 
        _subject.OnNext(text);      //HtmlPreviewイベントの発火
        SetTitle();                 //タイトルの編集マーク＊設定←→解除
      });


      _subject = new Subject<string>();
      var connectable = _subject.Throttle(TimeSpan.FromMilliseconds(500)).Publish();
      connectable.Subscribe(_ =>
        {
          _markdownText.SavePreviewHtml(_linkItemViewModel.ReferenceStringsForPreview);
          var message = new RequestPreviewMessage(new Uri(_markdownText.PreviewPath));
          Messenger.Default.Send(this, message);

        }, e => Trace.TraceError("{0},[StackTrace: {1}]", e.Message, e.StackTrace));
      _disposable = connectable.Connect();

      _markdownText.OpenNew();
      SetTitle();
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// リソースの解放
    /// </summary>
    public void Dispose()
    {
      _disposable.Dispose();
    }
    #endregion

    #region Expricit Interface Implementation
    /// <summary>新規作成(N)</summary>
    void IFileCommands.New(object parameter)
    {
      if (ConfirmSaveFile())
      {
        _markdownText.OpenNew();
        SetTitle();
      }
    }

    /// <summary>上書き保存(N)</summary>
    void IFileCommands.Save(object parameter)
    {
      if (!_markdownText.Save())
      {
        (this as IFileCommands).SaveAs(parameter);
      }
      else { SetTitle(); }
    }

    /// <summary>名前を付けて保存(N)</summary>
    void IFileCommands.SaveAs(object parameter)
    {
      var message = new SaveFileDialogMessage();
      message.DefaultExt = ".md";
      message.Filter = "MarkDownファイル(*.md;*.markdown)|*.md;*.markdown"
                    + "|テキストファイル(*.txt)|*.txt";
      Messenger.Default.Send(this, message);
      if (message.Result == true)
      {
        _markdownText.SaveTo(message.FileName);
        SetTitle();
      }
    }

    /// <summary>開く(O)</summary>
    void IFileCommands.Open(object parameter)
    {
      if (ConfirmSaveFile())
      {
        var message = new OpenFileDialogMessage();
        message.DefaultExt = ".md";
        message.Filter = "MarkDownファイル(*.md;*.markdown)|*.md;*.markdown"
                      + "|テキストファイル(*.txt)|*.txt"
                      + "|全てのファイル(*.*)|*.*";
        Messenger.Default.Send(this, message);
        if (message.Result == true)
        {
          _markdownText.OpenFrom(message.FileName);
          SetTitle();
        }
      }
    }

    /// <summary>新規作成 実行可否</summary>
    bool IFileCommands.CanNew(object parameter)
    { return true; }

    /// <summary>開く 実行可否</summary>
    bool IFileCommands.CanOpen(object parameter)
    { return true; }

    /// <summary>保存 実行可否</summary>
    bool IFileCommands.CanSave(object parameter)
    { return true; }

    /// <summary>名前を付けて保存 実行可否</summary>
    bool IFileCommands.CanSaveAs(object parameter)
    { return true; }

    /// <summary>受け取ったファイルを開く</summary>
    /// <param name="fileName">ファイル名</param>
    void IFileCommands.Receive(string fileName)
    {
      if (!File.Exists(fileName))
      { return; }
      _markdownText.OpenFrom(fileName);
    }

    /// <summary>後始末</summary>
    void ITerminatable.Terminate()
    {
      if (!ConfirmSaveFile())
      { return; }
      _linkItemViewModel.Save();
      File.Delete(_markdownText.PreviewPath);
    }
    #endregion
    
    #region private methods
    #region Command Handlers
    /// <summary>HTML形式で保存</summary>
    private void SaveHtml()
    {
      var message = new SaveFileDialogMessage();
      message.DefaultExt = ".html";
      message.Filter = "Webページ(*.html;*.htm)|*.html;*.htm";
      Messenger.Default.Send(this, message);
      if (message.Result == true)
      {
        _markdownText.SaveAsHtml(message.FileName,
          Path.GetFileNameWithoutExtension(message.FileName),
          _linkItemViewModel.ReferenceStringsForSave);
      }
    }

    /// <summary>Viewを閉じる</summary>
    private void RequestClose()
    {
      Messenger.Default.Send(this, new RequestCloseMessage());
    }
    #endregion

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
      Messenger.Default.Send(this, message);

      if (message.Result == MessageBoxResult.Cancel)
      {
        return false;
      }
      else if (message.Result == MessageBoxResult.Yes)
      {
        (this as IFileCommands).Save(null);
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
