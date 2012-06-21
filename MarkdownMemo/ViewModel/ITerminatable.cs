
namespace MarkdownMemo.ViewModel
{
  /// <summary>
  /// 終了時に行う処理を提供するインターフェイス
  /// </summary>
  public interface ITerminatable
  {
    /// <summary>
    /// 後始末
    /// </summary>
    void Treminate();
  }
}
