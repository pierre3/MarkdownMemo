using System;
using System.Windows;
using System.Windows.Controls;
using System.Security;
using System.Runtime.InteropServices;

namespace MarkdownMemo
{
  /// <summary>
  /// PasswordBoxの添付プロパティを定義するクラス
  /// </summary>
  public class PasswordBoxHelper
  {
    /// <summary>
    /// バインド可能なセキュアパスワード依存関係プロパティ
    /// </summary>
    public static readonly DependencyProperty BindableSecurePasswordProperty
      = DependencyProperty.RegisterAttached("BindableSecurePassword", typeof(SecureString), typeof(PasswordBoxHelper),
      new FrameworkPropertyMetadata(null, BindableSecurePasswordChanged));

    /// <summary>
    /// バインド可能なセキュアパスワードプロパティGetter
    /// </summary>
    /// <param name="obj">依存関係オブジェクト</param>
    /// <returns>セキュアパスワード</returns>
    public static SecureString GetBindableSecurePassword(DependencyObject obj)
    {
      return (SecureString)obj.GetValue(BindableSecurePasswordProperty);
    }

    /// <summary>
    /// バインド可能なセキュアパスワードプロパティSetter
    /// </summary>
    /// <param name="obj">依存関係オブジェクト</param>
    /// <param name="value">セキュアパスワード位置</param>
    public static void SetBindableSecurePassword(DependencyObject obj, SecureString value)
    {
      obj.SetValue(BindableSecurePasswordProperty, value);
    }

    /// <summary>
    /// バインド可能なセキュアパスワード変更イベントハンドラ
    /// </summary>
    /// <param name="obj">依存関係オブジェクト</param>
    /// <param name="e">イベント引数</param>
    public static void BindableSecurePasswordChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
      var passBox = obj as PasswordBox;
      if (passBox == null)
      { return; }

      var oldValue = (SecureString)e.OldValue;
      var newValue = (SecureString)e.NewValue;
      if (oldValue == null && newValue != null)
      {
        passBox.PasswordChanged += passwordBox_passwordChanged; 
      }
      else if (oldValue != null && newValue == null)
      {
        passBox.PasswordChanged -= passwordBox_passwordChanged; 
      }
      
      var passPtr = Marshal.SecureStringToBSTR(newValue);
      try
      {
        var nextPass = Marshal.PtrToStringBSTR(passPtr);
        if(nextPass != passBox.Password)
        {
          passBox.Password = nextPass; 
        }
      }finally
      {
        Marshal.ZeroFreeBSTR(passPtr);
      }
    }

    /// <summary>
    /// PasswordBox passwordChangedイベントハンドラ
    /// </summary>
    /// <param name="o">イベント発生元</param>
    /// <param name="e">イベント引数</param>
    private static void passwordBox_passwordChanged(object o, RoutedEventArgs e)
    {
      var sender = o as PasswordBox;
      if (sender != null)
      {
        SetBindableSecurePassword(sender, sender.SecurePassword);
      }
    }

 

  }
}
