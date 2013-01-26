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
      //var mainWindow = new MainWindow();

      var startupFile = e.Args.FirstOrDefault();
      if (startupFile != default(string) 
          && System.IO.File.Exists(startupFile))
      {
          (this.MainWindow as MainWindow).ReceiveFile(startupFile);
      }

      //mainWindow.Show();

    }
  }
}
