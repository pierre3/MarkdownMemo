using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using MarkdownMemo.Common;
using MarkdownMemo.ViewModel;

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

      //HTMLプレビュー更新のトリガイベント
      var updateTrigger = Observable.FromEvent<TextChangedEventHandler, TextChangedEventArgs>(
        h => (sender, args) => h(args),
        h => mainWindow.textBox1.TextChanged += h,
        h => mainWindow.textBox1.TextChanged -= h).Throttle(TimeSpan.FromMilliseconds(500));

      //HTMLプレビュー更新依頼 コールバック
      Action<string> requestPreview = path =>
        this.Dispatcher.BeginInvoke(new Action(() =>
          mainWindow.prevewBrowser.Navigate(new Uri(path))));

      //ViewModelインスタンス生成
      var viewModel = new MainwindowViewModel(previewPath, "style.css",
        updateTrigger, requestPreview, e.Args.FirstOrDefault());

      //ウインドウ終了依頼　コールバック
      EventHandler handler = null;
      handler = (_, __) =>
      {
        viewModel.RequestClose -= handler;
        mainWindow.Close();
      };
      viewModel.RequestClose += handler;

      mainWindow.DataContext = viewModel;
      //Messenger.Default.Register<DialogMessage>(viewModel, MarkdownMemo.MainWindow.ShowMessageBox);
      mainWindow.Show();
    }
  }
}
