
namespace MarkdownMemo.ViewModel
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
