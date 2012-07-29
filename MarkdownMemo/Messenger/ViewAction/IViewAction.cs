using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using MarkdownMemo.ViewModel;

namespace MarkdownMemo
{

  /// <summary>
  /// メッセージを受信したViewで実行されるアクションを提供するインターフェース
  /// </summary>
  public interface IViewAction
  {
    /// <summary>
    /// メッセンジャーに自身を登録します
    /// </summary>
    /// <param name="recipient">メッセージ受信先オブジェクト</param>
    /// <param name="messenger">メッセンジャー</param>
    void Register(FrameworkElement recipient, Messenger messenger);
  }

}
