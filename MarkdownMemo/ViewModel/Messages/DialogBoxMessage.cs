using System.Windows;

namespace MarkdownMemo
{
  
  /// <summary>
  /// メッセージボックスメッセージオブジェクト
  /// </summary>
  public class DialogBoxMessage
  {
    #region Properties
    /// <summary>ダイアログタイトル</summary>
    public string Caption { get; set; }
    
    /// <summary>テキスト</summary>
    public string Text { get; set; }
    
    /// <summary>ボタン</summary>
    public MessageBoxButton Button { get; set; }
    
    /// <summary>アイコンイメージ</summary>
    public MessageBoxImage Image { get; set; }
    
    /// <summary>ダイアログの戻り値</summary>
    public MessageBoxResult Result { get; set; }
    #endregion

    #region Constructors
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="text">メッセージ文字列</param>
    public DialogBoxMessage(string text)
      : this(text, string.Empty, MessageBoxButton.OK, MessageBoxImage.None)
    { }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="text">メッセージ文字列</param>
    /// <param name="caption">ダイアログタイトル</param>
    public DialogBoxMessage(string text, string caption)
      : this(text, caption, MessageBoxButton.OK, MessageBoxImage.None)
    { }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="text">メッセージ文字列</param>
    /// <param name="caption">ダイアログタイトル</param>
    /// <param name="button">ボタン</param>
    public DialogBoxMessage(string text, string caption, MessageBoxButton button)
      : this(text, caption, button, MessageBoxImage.None)
    { }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="text">メッセージ文字列</param>
    /// <param name="caption">ダイアログタイトル</param>
    /// <param name="button">ボタン</param>
    /// <param name="image">アイコンイメージ</param>
    public DialogBoxMessage(string text, string caption, MessageBoxButton button, MessageBoxImage image)
    {
      this.Caption = caption;
      this.Text = text;
      this.Button = button;
      this.Image = image;
      this.Result = MessageBoxResult.None;
    }
    #endregion
  }

}
