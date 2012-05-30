using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;

namespace MarkdownMemo
{
  public class MainwindowViewModel : INotifyPropertyChanged
  {
    #region Fields
    private string _title;
    private MarkdownText _markdownText;
    private ICommand _new;
    private ICommand _open;
    private ICommand _save;
    private ICommand _saveAs;
    private ICommand _exit;
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

    /// <summary>
    /// プロパティ変更通知イベント
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// プロパティ変更通知イベントを発生させます
    /// </summary>
    /// <param name="name">プロパティ名</param>
    private void OnPropertyChanged(string name)
    {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null)
        handler(this, new PropertyChangedEventArgs(name));
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
    public MainwindowViewModel(string previewPath, string cssName,
      IObservable<EventArgs> updatePreviewTrigger, Action<string> requestPreview)
    {
      _markdownText = new MarkdownText(previewPath, cssName);
      _markdownText.TextChanged += () => OnPropertyChanged("Text");

      updatePreviewTrigger.Subscribe(_ =>
        {
          _markdownText.SavePreviewHtml();
          requestPreview(_markdownText.PreviewPath);
        }, e => Trace.TraceError("{0},[StackTrace: {1}]", e.Message, e.StackTrace));

      _markdownText.OpenNew();
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
      var dialog = new SaveFileDialog();
      dialog.DefaultExt = ".txt";
      dialog.Filter = "テキストファイル(*.txt)|*.txt";
      if (dialog.ShowDialog() == true)
      {
        _markdownText.SaveTo(dialog.FileName);
        SetTitle();
      }
    }

    /// <summary>開く(O)</summary>
    private void Open()
    {
      if (ConfirmSaveFile())
      {
        var dialog = new OpenFileDialog();
        dialog.DefaultExt = ".txt";
        dialog.Filter = "テキストファイル(*.txt)|*.txt|全てのファイル(*.*)|*.*";
        dialog.Multiselect = false;
        if (dialog.ShowDialog() == true)
        {
          _markdownText.OpenFrom(dialog.FileName);
          SetTitle();
        }
      }
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

      var result = MessageBox.Show("編集中のテキストが保存されていません。上書き保存しますか？", "Markdown Memo",
          MessageBoxButton.YesNoCancel);
      if (result == MessageBoxResult.Cancel)
      {
        return false;
      }
      else if (result == MessageBoxResult.Yes)
      {
        Save();
      }
      return true;
    }

    private void SetTitle()
    {
      var name = string.IsNullOrEmpty(_markdownText.SourcePath) ?
        "無題" : Path.GetFileName(_markdownText.SourcePath);
      this.Title = name + (_markdownText.IsTextChanged ? " *" : "") + " - Markdown Memo";
    }
    #endregion
  }
}
