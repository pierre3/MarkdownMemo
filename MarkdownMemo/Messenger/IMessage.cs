﻿using System;
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
  /// IMessage
  /// </summary>
  public interface IMessage
  {
    ViewModelBase Sender { get; }
  }

}
