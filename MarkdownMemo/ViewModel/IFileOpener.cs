
namespace MarkdownMemo.ViewModel
{
  /// <summary>ファイルを開く機能を提供するインターフェイス</summary>
  public interface IFileOpener
  {
    /// <summary>
    /// ファイルを開く
    /// </summary>
    /// <param name="fileName">ファイル名</param>
    void Open(string fileName);
  }
}
