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
    /// コンストラクタ
    /// </summary>
    public MainWindow()
    {
      InitializeComponent();
      
      //プレビューファイル保存先
      var previewPath = System.IO.Path.Combine(IOHelper.CreateAppDataDirectory(), "Preview.html");
      //HTMLプレビュー更新のトリガイベント
      var updateTrigger = Observable.FromEvent<TextChangedEventHandler, TextChangedEventArgs>(
        h => (sender, args) => h(args),
        h => textBox1.TextChanged += h,
        h => textBox1.TextChanged -= h).Throttle(TimeSpan.FromMilliseconds(500));
      //HTMLプレビュー更新依頼 コールバック
      Action<string> requestPreview = path =>
        this.Dispatcher.BeginInvoke(new Action(() =>
          this.prevewBrowser.Navigate(new Uri(path))));
      
      //ViewModelインスタンス生成
      var viewModel = new MainwindowViewModel(previewPath, "style.css",
        updateTrigger, requestPreview);

      //ウインドウ終了依頼　コールバック
      EventHandler handler = null;
      handler = (_, __) =>
        {
          viewModel.RequestClose -= handler;
          this.Close();
        };
      viewModel.RequestClose += handler;

      this.DataContext = viewModel;

    }


  }
}
