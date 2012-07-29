﻿using System;
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
  /// ファイル保存ダイアログを表示するViewAction
  /// </summary>
  public class SaveFileDialogAction : IViewAction
  {
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public SaveFileDialogAction()
    {}

    /// <summary>
    /// メッセンジャーへ自身を登録する
    /// </summary>
    /// <param name="recipient">メッセージ受信先オブジェクト</param>
    /// <param name="messenger">メッセンジャー</param>
    public void Register(FrameworkElement recipient, Messenger messenger)
    {
      messenger.Register<SaveFileDialogMessage>(recipient, ShowSaveFileDialog);
    }

    /// <summary>
    /// ファイル保存ダイアログの表示
    /// </summary>
    /// <param name="message">メッセージオブジェクト</param>
    private void ShowSaveFileDialog(SaveFileDialogMessage message)
    {
      var dialog = new SaveFileDialog();
      dialog.Filter = message.Filter;
      dialog.FilterIndex = message.FilterIndex;
      dialog.InitialDirectory = message.InitialDirectory;
      dialog.Title = message.Title;
      
      message.Result = dialog.ShowDialog();
      message.FileName = dialog.FileName;
      message.FileNames = dialog.FileNames;

    }
  }

}
