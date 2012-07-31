using System.Windows;
using MarkdownMemo.Common;

namespace MarkdownMemo
{

  /// <summary>
  /// メッセージBOｘを表示するViewアクション
  /// </summary>
  public class DialogBoxAction : IViewAction
  {
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public DialogBoxAction()
    {}

    /// <summary>
    /// メッセンジャーに自身を登録する
    /// </summary>
    /// <param name="recipient">メッセージ受信先オブジェクト</param>
    /// <param name="messenger">メッセンジャー</param>
    public void Register(FrameworkElement recipient, Messenger messenger)
    {
      messenger.Register<DialogBoxMessage>(recipient, ShawMessageBox);
    }

    /// <summary>
    /// メッセージBoxの表示
    /// </summary>
    /// <param name="message">メッセージオブジェクト</param>
    private void ShawMessageBox(DialogBoxMessage message)
    {
      message.Result = MessageBox.Show(message.Text, message.Caption, message.Button, message.Image);
    }
  }

}
