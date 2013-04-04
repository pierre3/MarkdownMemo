using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using MarkdownMemo.ViewModel;
using My.Common;
using My.Mvvm;
using System.Diagnostics;

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

      //ログファイルの登録
      var listener = new TextWriterTraceListener(
        System.IO.Path.Combine(PathHelper.CreateAppDataDirectory(), "Trace.log"));
      Trace.Listeners.Clear();
      Trace.Listeners.Add(listener);
      
      //コマンドライン引数で渡されたファイルを受け取る
      var startupFile = e.Args.FirstOrDefault();
      if (startupFile != default(string) 
          && System.IO.File.Exists(startupFile))
      {
          (this.MainWindow as MainWindow).ReceiveFile(startupFile);
      }

      //未処理のエラーをLogに記録
      this.DispatcherUnhandledException += (sender, args) =>
        {
          
          Trace.TraceError("Error:[{0}]" +Environment.NewLine + "{1}",
            DateTime.Now.ToString("g"), args.Exception.ToString());
          Trace.Flush();
          args.Handled = false;
        };

      AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
        {
          Trace.TraceError("Error:[{0}]" + Environment.NewLine + "{1}", 
            DateTime.Now.ToString("g"), args.ExceptionObject.ToString());
          Trace.Flush();
        };
    }
  }
}
