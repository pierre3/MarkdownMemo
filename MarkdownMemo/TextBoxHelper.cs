using System.Windows;
using System.Windows.Controls;

namespace MarkdownMemo
{
  /// <summary>
  /// TextBoxの添付プロパティを定義するクラス
  /// </summary>
  public class TextBoxHelper
  {
    /// <summary>
    /// キャレット位置を表す依存関係プロパティ
    /// </summary>
    public static readonly DependencyProperty CaretPositionProperty
      = DependencyProperty.RegisterAttached("CaretPosition", typeof(int), typeof(TextBoxHelper),
      new FrameworkPropertyMetadata(-1, CaretPositionChanged));

    /// <summary>
    /// キャレット位置プロパティGetter
    /// </summary>
    /// <param name="obj">依存関係オブジェクト</param>
    /// <returns>キャレット位置</returns>
    public static int GetCaretPosition(DependencyObject obj)
    {
      return (int)obj.GetValue(CaretPositionProperty);
    }

    /// <summary>
    /// キャレット位置プロパティSetter
    /// </summary>
    /// <param name="obj">依存関係オブジェクト</param>
    /// <param name="value">キャレット位置</param>
    public static void SetCaretPosition(DependencyObject obj, int value)
    {
      obj.SetValue(CaretPositionProperty, value);
    }

    /// <summary>
    /// キャレット位置プロパティ変更イベントハンドラ
    /// </summary>
    /// <param name="obj">依存関係オブジェクト</param>
    /// <param name="e">イベント引数</param>
    public static void CaretPositionChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
      var textBox = obj as TextBox;
      if (textBox == null)
      { return; }

      var oldValue = (int)e.OldValue;
      var newValue = (int)e.NewValue;
      if (oldValue<0  && newValue >= 0)
      {
        textBox.SelectionChanged += textBox_selectionChanged;
      }
      else if (oldValue >= 0 && newValue < 0)
      {
        textBox.SelectionChanged -= textBox_selectionChanged;
      }

      if (newValue != textBox.CaretIndex)
      {
        textBox.CaretIndex = newValue;
      }

    }

    /// <summary>
    /// テキストボックスSelectionChangedイベントハンドラ
    /// </summary>
    /// <param name="o">イベント発生元</param>
    /// <param name="e">イベント引数</param>
    private static void textBox_selectionChanged(object o, RoutedEventArgs e)
    {
      var sender = o as TextBox;
      if (sender != null)
      {
        SetCaretPosition(sender, sender.CaretIndex);
      }
    
    }

 

  }
}
