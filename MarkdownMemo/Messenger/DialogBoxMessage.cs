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
  /// DialogMessage
  /// </summary>
  public class DialogBoxMessage : IMessage
  {
    #region Properties
    public ViewModelBase Sender { get; protected set; }
    public string Caption { get; set; }
    public string Text { get; set; }
    public MessageBoxButton Button { get; set; }
    public MessageBoxImage Image { get; set; }
    public MessageBoxResult Result { get; set; }
    #endregion

    #region Constructors
    public DialogBoxMessage(ViewModelBase sender, string text)
      : this(sender, text, string.Empty, MessageBoxButton.OK, MessageBoxImage.None)
    { }
    public DialogBoxMessage(ViewModelBase sender, string text, string caption)
      : this(sender, text, caption, MessageBoxButton.OK, MessageBoxImage.None)
    { }
    public DialogBoxMessage(ViewModelBase sender, string text, string caption, MessageBoxButton button)
      : this(sender, text, caption, button, MessageBoxImage.None)
    { }

    public DialogBoxMessage(ViewModelBase sender, string text, string caption, MessageBoxButton button, MessageBoxImage image)
    {
      this.Sender = sender;
      this.Caption = caption;
      this.Text = text;
      this.Button = button;
      this.Image = image;
      this.Result = MessageBoxResult.None;
    }
    #endregion
  }

}
