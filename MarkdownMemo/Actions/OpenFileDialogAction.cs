using System.Windows;
using MarkdownMemo.ViewModel;
using Microsoft.Win32;
using My.Mvvm;

namespace MarkdownMemo
{

  /// <summary>
  /// ファイルを開くダイアログViewアクション
  /// </summary>
  public class OpenFileDialogAction : IViewAction
  {
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public OpenFileDialogAction()
    {}

    /// <summary>
    /// メッセンジャーへ自身を登録する
    /// </summary>
    /// <param name="recipient">メッセージ受信先オブジェクト</param>
    /// <param name="messenger">メッセンジャー</param>
    public void Register(FrameworkElement recipient, Messenger messenger)
    {
      messenger.Register<OpenFileDialogMessage>(recipient, ShowFileOpenDialog);
    }

    /// <summary>
    /// ファイルを開くダイアログを表示
    /// </summary>
    /// <param name="message">メッセージオブジェクト</param>
    private void ShowFileOpenDialog(OpenFileDialogMessage message)
    {
      var dialog = new OpenFileDialog();
      dialog.Filter = message.Filter;
      dialog.FilterIndex = message.FilterIndex;
      dialog.InitialDirectory = message.InitialDirectory;
      dialog.Title = message.Title;
      dialog.Multiselect = message.Multiselect;

      message.Result = dialog.ShowDialog();
      message.FileName = dialog.FileName;
      message.FileNames = dialog.FileNames;

    }
  }

}
