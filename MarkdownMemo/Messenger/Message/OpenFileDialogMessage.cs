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
  /// ファイルを開くダイアログメッセージオブジェクト
  /// </summary>
  public class OpenFileDialogMessage : SaveFileDialogMessage
  {
    /// <summary>複数のファイルを選択可能か否かを設定、取得する</summary>
    public bool Multiselect { set; get; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public OpenFileDialogMessage()
      : base()
    {
      this.Multiselect = false;
    }
  }


}
