using System;
using System.IO;
using System.Linq;
using System.Windows;
using My.Mvvm;

namespace MarkdownMemo
{
  /// <summary>
  /// MainWindow.xaml の相互作用ロジック
  /// </summary>
  public partial class MainWindow : FileEditorWindowView
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
      this.ReceiveFile(name);
      e.Handled = true;
    }

  }
}
