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
  /// DialogBoxAction
  /// </summary>
  public class DialogBoxAction : IViewAction
  {
    public DialogBoxAction()
    {}

    public void Register(FrameworkElement recipient, Messenger messenger)
    {
      messenger.Register<DialogBoxMessage>(recipient, ShawMessageBox);
    }

    private void ShawMessageBox(DialogBoxMessage message)
    {
      message.Result = MessageBox.Show(message.Text, message.Caption, message.Button, message.Image);
    }
  }

}
