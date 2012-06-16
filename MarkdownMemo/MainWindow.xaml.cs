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
using System.Linq;
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

    private void textBox1_previewDragOver(object sender, DragEventArgs e)
    {
      
      var fileNames = e.Data.GetData(DataFormats.FileDrop) as string[];
      var name = fileNames.FirstOrDefault();
      if (!File.Exists(name))
      { 
        e.Effects = DragDropEffects.None;
        return;
      }
      e.Effects = DragDropEffects.Copy;
      e.Handled = true;
    }

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

    private void window_Closed(object sender, EventArgs e)
    {
      var viewModel = this.DataContext as ITerminatable;
      if (viewModel != null)
      {
        viewModel.Treminate();
      }
    }

    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {

    }
  }
}
