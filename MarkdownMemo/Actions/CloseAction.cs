using System.Windows;
using MarkdownMemo.ViewModel;
using My.Mvvm;

namespace MarkdownMemo
{

  /// <summary>
  /// メッセージBOｘを表示するViewアクション
  /// </summary>
  public class CloseAction : IViewAction
  {
    private Window _recipient;
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public CloseAction()
    {}

    /// <summary>
    /// メッセンジャーに自身を登録する
    /// </summary>
    /// <param name="recipient">メッセージ受信先オブジェクト</param>
    /// <param name="messenger">メッセンジャー</param>
    public void Register(FrameworkElement recipient, Messenger messenger)
    {
      this._recipient = recipient as Window;
      
      if (this._recipient == null)
      { return; }

      messenger.Register<RequestCloseMessage>(recipient, WindowClose);
    }

    /// <summary>
    /// メッセージBoxの表示
    /// </summary>
    /// <param name="message">メッセージオブジェクト</param>
    private void WindowClose(RequestCloseMessage message)
    {
      _recipient.Close();
    }
  }

}
