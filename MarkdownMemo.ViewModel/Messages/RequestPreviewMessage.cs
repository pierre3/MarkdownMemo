using System;

namespace MarkdownMemo.ViewModel
{
  
  /// <summary>
  /// プレビュー更新依頼メッセージ
  /// </summary>
  public class RequestPreviewMessage
  {
    #region Properties
    /// <summary>更新するドキュメントのURI</summary>
    public Uri Uri{set;get;}
    #endregion

    #region Constructors
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="uri">更新するドキュメントのURI</param>
    public RequestPreviewMessage(Uri uri)
    {
      this.Uri = uri;
    }
    #endregion
  }

}
