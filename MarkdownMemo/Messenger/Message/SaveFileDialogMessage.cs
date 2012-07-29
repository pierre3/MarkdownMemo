using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using MarkdownMemo.ViewModel;
using Microsoft.Win32;

namespace MarkdownMemo
{
  /// <summary>
  ///  ファイル保存ダイアログメッセージオブジェクト
  /// </summary>
  public class SaveFileDialogMessage
  {
    /// <summary>タイトル</summary>
    public string Title { set; get; }
    /// <summary>初期ディレクトリ</summary>
    public string InitialDirectory { set; get; }
    /// <summary>フィルタ</summary>
    public string Filter { set; get; }
    /// <summary>フィルタインデックス</summary>
    public int FilterIndex { set; get; }
    /// <summary>既定の拡張子</summary>
    public string DefaultExt { set; get; }
    /// <summary>選択ファイル名</summary>
    public string FileName { set; get; }
    /// <summary>選択ファイル名のコレクション</summary>
    public string[] FileNames { set; get; }
    /// <summary>ダイアログの戻り値</summary>
    public bool? Result { set; get; }
    
    /// <summary>コンストラクタ</summary>
    public SaveFileDialogMessage()
    {
      this.Title = string.Empty;
      this.InitialDirectory = string.Empty;
      this.Filter = string.Empty;
      this.FilterIndex = 0;
      this.DefaultExt = string.Empty;
      this.FileName = string.Empty;
      this.FileNames = new String[0];
      this.Result = null;
    }

  }

}
