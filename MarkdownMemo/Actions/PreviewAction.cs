using System;
using System.Windows;
using MarkdownMemo.ViewModel;
using My.Mvvm;

namespace MarkdownMemo
{

  /// <summary>
  /// プレビューを更新するビューアクション
  /// </summary>
  public class PreviewAction : IViewAction
  {

    private FrameworkElement _recipient;
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PreviewAction()
    {}

    /// <summary>
    /// メッセンジャーに自身を登録する
    /// </summary>
    /// <param name="recipient">メッセージ受信先オブジェクト</param>
    /// <param name="messenger">メッセンジャー</param>
    public void Register(FrameworkElement recipient, Messenger messenger)
    {
      this._recipient = recipient;
      messenger.Register<RequestPreviewMessage>(recipient, navigatePreview);
    }

    /// <summary>
    /// プレビュー表示の内容を更新する
    /// </summary>
    /// <param name="message">メッセージオブジェクト</param>
    private void navigatePreview(RequestPreviewMessage message)
    {
      var mainWin = _recipient as MainWindow;
      if (mainWin == null)
      { return; }
      mainWin.Dispatcher.BeginInvoke(
        new Action(()=>mainWin.prevewBrowser.Navigate(message.Uri)));
    }
  }

}
