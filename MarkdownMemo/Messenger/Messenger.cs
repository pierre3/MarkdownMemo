using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using MarkdownMemo.ViewModel;

namespace MarkdownMemo
{

  /// <summary>
  /// Messenger
  /// </summary>
  public class Messenger
  {
    private static Messenger _instance = new Messenger();

    private List<MessageParameter> _parmeterList = new List<MessageParameter>();
    public static Messenger Default { get { return _instance; } }

    public void Register<TMessage>(FrameworkElement recipient, Action<TMessage> action)
    {
      var parameter = new MessageParameter()
        {
          MessageType = typeof(TMessage),
          Sender = recipient.DataContext as ViewModelBase,
          Action = action
        };

      if (!_parmeterList.Contains(parameter))
      {
        _parmeterList.Add(parameter);
      }
    }

    public void Send<TMessage>(ViewModelBase sender, TMessage message)
    {
      var actions = _parmeterList.Where(o => o.Sender == sender && o.MessageType == typeof(TMessage))
        .Select(o => o.Action as Action<TMessage>);
      foreach (var act in actions)
      {
        act(message);
      }
    }

    public struct MessageParameter : IEquatable<MessageParameter>
    {
      public Type MessageType { set; get; }
      public ViewModelBase Sender { set; get; }
      public Delegate Action { set; get; }

      public bool Equals(MessageParameter other)
      {
        return this.MessageType == other.MessageType
          && this.Sender == other.Sender
          && this.Action == other.Action;
      }

      public override bool Equals(object obj)
      {

        if (obj == null || GetType() != obj.GetType())
        {
          return false;
        }

        return this.Equals((MessageParameter)obj);
      }

      public override int GetHashCode()
      {
        return MessageType.GetHashCode() ^ Sender.GetHashCode()
          ^ Action.GetHashCode();
      }

      public static bool operator ==(MessageParameter left, MessageParameter right)
      {
        return left.Equals(right);
      }

      public static bool operator !=(MessageParameter left, MessageParameter right)
      {
        return !(left == right);
      }
    }
  }

}
