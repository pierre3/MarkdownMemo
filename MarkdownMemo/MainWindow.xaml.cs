using System;
using System.Diagnostics;
using System.IO;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using MarkdownSharp;
using System.Windows.Input;
using Microsoft.Win32;

namespace MarkdownMemo
{
  /// <summary>
  /// MainWindow.xaml の相互作用ロジック
  /// </summary>
  public partial class MainWindow : Window
  {
    /// <summary>
    /// 終了(X)コマンド
    /// </summary>
    public static RoutedUICommand WindowCloseCommand = new RoutedUICommand();
    
    /// <summary>
    /// Markdownテキスト 静的クラス
    /// </summary>
    /// <remarks>
    /// 編集するMarkdownテキストを保持やファイルへの読み書きを行います 
    /// </remarks>
    public static MarkdownText MarkdownText;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public MainWindow()
    {
      var previewPath = System.IO.Path.Combine(IOHelper.CreateAppDataDirectory(), "Preview.html");
      MarkdownText = new MarkdownText(previewPath, "style.css");
      MarkdownText.PropertyChanged += (sender, e) => 
      {
        if (e.PropertyName == "Text")
          SetTitle();
      };

      InitializeComponent();
      SetTitle();

      var textChanged = Observable.FromEvent<TextChangedEventHandler, TextChangedEventArgs>(
        h => (sender, args) => h(args),
        h => textBox1.TextChanged += h,
        h => textBox1.TextChanged -= h);

      textChanged.Throttle(TimeSpan.FromMilliseconds(500))
        .Subscribe(_ =>
        {
          MarkdownText.SavePreviewHtml();
          prevewBrowser.Dispatcher.BeginInvoke(
            new Action(() => prevewBrowser.Navigate(new Uri(MarkdownText.PreviewPath))));

        }, e => Trace.TraceError("{0},[StackTrace: {1}]", e.Message, e.StackTrace));
    }

    #region Command Handlers
    /// <summary>新規作成(N)</summary>
    private void FileNew_Execute(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
    {
      if (ConfirmSaveFile())
      {
        MarkdownText.OpenNew();
        SetTitle();
      }
    }
    
    /// <summary>上書き保存(N)</summary>
    private void FileSave_Execute(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
    {
      if (!MarkdownText.Save())
      {
        FileSaveAs_Execute(null, e);
      }
      else { SetTitle(); }
    }

    /// <summary>名前を付けて保存(N)</summary>
    private void FileSaveAs_Execute(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
    {
      var dialog = new SaveFileDialog();
      dialog.DefaultExt = ".txt";
      dialog.Filter = "テキストファイル(*.txt)|*.txt";
      if (dialog.ShowDialog() == true)
      {
        MarkdownText.SaveTo(dialog.FileName);
        SetTitle();
      }
    }

    /// <summary>終了(X)</summary>
    private void WindowClose_Execute(object sender, ExecutedRoutedEventArgs e)
    {
      if(ConfirmSaveFile())
        this.Close();
    }

    /// <summary>開く(O)</summary>
    private void FileOpen_Executed(object sender, ExecutedRoutedEventArgs e)
    {
      if (ConfirmSaveFile())
      {
        var dialog = new OpenFileDialog();
        dialog.DefaultExt = ".txt";
        dialog.Filter = "テキストファイル(*.txt)|*.txt|全てのファイル(*.*)|*.*";
        dialog.Multiselect = false;
        if (dialog.ShowDialog() == true)
        {
          MarkdownText.OpenFrom(dialog.FileName);
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
      if (!MarkdownText.IsTextChanged)
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
        FileSave_Execute(null, null);
      }
      return true;
    }

    private void SetTitle()
    {
      var name = string.IsNullOrEmpty(MarkdownText.SourcePath) ? 
        "無題" : Path.GetFileName(MarkdownText.SourcePath);
      this.Title = name + (MarkdownText.IsTextChanged ? " *" : "") + " - Markdown Memo";
    }
    #endregion

  }
}
