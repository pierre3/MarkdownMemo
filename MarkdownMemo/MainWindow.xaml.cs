using System;
using System.IO;
using System.Linq;
using System.Windows;
using MarkdownMemo.ViewModel;

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
    }

    /// <summary>
    /// テキストボックスにファイルをドラッグした際の処理
    /// </summary>
    private void textBox1_previewDragOver(object sender, DragEventArgs e)
    {

      var fileNames = e.Data.GetData(DataFormats.FileDrop) as string[];
      if (fileNames == null)
      { return; }

      var name = fileNames.FirstOrDefault();
      if (!File.Exists(name))
      {
        e.Effects = DragDropEffects.None;
        return;
      }
      e.Effects = DragDropEffects.Copy;
      e.Handled = true;
    }

    /// <summary>
    /// テキストボックスにファイルをドロップした際の処理
    /// </summary>
    private void textBox1_previewDrop(object sender, DragEventArgs e)
    {

      var fileNames = e.Data.GetData(DataFormats.FileDrop) as string[];
      if (fileNames == null)
      { return; }

      var name = fileNames.FirstOrDefault();
      var fileOpener = this.DataContext as IFileOpener;
      if (fileOpener != null)
        fileOpener.Open(name);
      e.Handled = true;
    }

    /// <summary>
    /// ウィンドウを閉じる際の処理
    /// </summary>
    private void window_Closed(object sender, EventArgs e)
    {
      var viewModel = this.DataContext as ITerminatable;
      if (viewModel != null)
      {
        viewModel.Treminate();
      }
    }
  }
}
