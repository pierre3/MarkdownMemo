using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MarkdownMemo.Common;
using MarkdownMemo.Model;
using Microsoft.Win32;

namespace MarkdownMemo.ViewModel
{
  /// <summary>
  /// ViewModel基底クラス
  /// </summary>
  public class ViewModelBase : INotifyPropertyChanged
  {
    /// <summary>
    /// プロパティ変更通知イベント
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;
    
    /// <summary>
    /// プロパティ変更通知イベント発生
    /// </summary>
    /// <param name="name">プロパティ名</param>
    protected void OnPropertyChanged(string name)
    {
      var handler = PropertyChanged;
      if (handler != null)
      {
        handler(this, new PropertyChangedEventArgs(name));
      }
    }
  }
}
