using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using MarkdownMemo.ViewModel;
using My.Common;
using My.Mvvm;

namespace MarkdownMemo
{
  /// <summary>
  /// App.xaml の相互作用ロジック
  /// </summary>
  public partial class App : Application
  {
    /// <summary>
    /// OnStartup
    /// </summary>
    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);
      var mainWindow = new MainWindow();

      //プレビューファイル保存先
      //複数プロセスでの起動を考慮して、プレビューファイルに自プロセスのIDを付加する
      var processId = System.Diagnostics.Process.GetCurrentProcess().Id.ToString();
      var userDir = PathHelper.CreateAppDataDirectory();
      
      System.IO.Directory.CreateDirectory(System.IO.Path.Combine(userDir, "image"));
      var previewPath = System.IO.Path.Combine(userDir, processId + "_Preview.html");

      //ViewModelインスタンス生成
      var viewModel = new MainwindowViewModel(previewPath, "style.css", e.Args.FirstOrDefault());

      mainWindow.DataContext = viewModel;
      mainWindow.Show();
    }
  }
}
