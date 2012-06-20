using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace MarkdownMemo
{
  public class TextBoxHelper
  {

    public static readonly DependencyProperty CaretPositionProperty
      = DependencyProperty.RegisterAttached("CaretPosition", typeof(int?), typeof(TextBoxHelper),
      new FrameworkPropertyMetadata(null, CaretPositionChanged));

    public static int? GetCaretPosition(DependencyObject obj)
    {
      return (int?)obj.GetValue(CaretPositionProperty);
    }

    public static void SetCaretPosition(DependencyObject obj, int? value)
    {
      obj.SetValue(CaretPositionProperty, value);
    }

    public static void CaretPositionChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
      var textBox = obj as TextBox;
      if (textBox == null)
      { return; }

      var oldValue = e.OldValue as int?;
      var newValue = e.NewValue as int?;
      if (oldValue == null && newValue != null)
      {

        textBox.SelectionChanged += textBox_selectionChanged;
      }

      if ((int)e.NewValue != textBox.CaretIndex)
      {
        textBox.CaretIndex = (int)e.NewValue;
      }

    }

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
