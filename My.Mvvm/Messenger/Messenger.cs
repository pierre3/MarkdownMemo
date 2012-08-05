using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;


namespace My.Mvvm
{

  /// <summary>
  /// メッセンジャー
  /// </summary>
  public class Messenger
  {
    #region Fields
    private List<MessageParameter> _parmeterList = new List<MessageParameter>();
    #endregion

    #region Methods
    /// <summary>
    /// アクションの登録
    /// </summary>
    /// <typeparam name="TMessage">メッセージオブジェクトの型</typeparam>
    /// <param name="recipient">メッセージ受け取り先オブジェクト</param>
    /// <param name="action">メッセージアクション</param>
    public void Register<TMessage>(FrameworkElement recipient, Action<TMessage> action)
    {
      var parameter = new MessageParameter(typeof(TMessage),
        recipient.DataContext as ViewModelBase, action);

      if (!_parmeterList.Contains(parameter))
      {
        _parmeterList.Add(parameter);
      }
    }

    /// <summary>
    /// メッセージ
    /// </summary>
    /// <typeparam name="TMessage">メッセージオブジェクトの型</typeparam>
    /// <param name="sender">メッセージ送信元</param>
    /// <param name="message">メッセージオブジェクト</param>
    public void Send<TMessage>(ViewModelBase sender, TMessage message)
    {
      var actions = _parmeterList.Where(o => o.Sender == sender && o.MessageType == typeof(TMessage))
        .Select(o => o.Action as Action<TMessage>);
      foreach (var act in actions)
      {
        act(message);
      }
    }
    #endregion

    #region Nested Classes
    /// <summary>
    /// メッセンジャーパラメータ
    /// </summary>
    public struct MessageParameter : IEquatable<MessageParameter>
    {
      #region Fields
      private readonly Type _messageType;
      private readonly ViewModelBase _sender;
      private readonly Delegate _action;
      #endregion

      #region Properties
      /// <summary>メッセージオブジェクトの型</summary>
      public Type MessageType { get { return _messageType; } }
      /// <summary>メッセージ送信元</summary>
      public ViewModelBase Sender { get { return _sender; } }
      /// <summary>メッセージアクション</summary>
      public Delegate Action { get { return _action; } }
      #endregion

      #region Constructors
      /// <summary>
      /// コンストラクタ
      /// </summary>
      /// <param name="messageType">メッセージタイプ</param>
      /// <param name="sender">メッセージ送信元</param>
      /// <param name="action">メッセージアクション</param>
      public MessageParameter(Type messageType, ViewModelBase sender, Delegate action)
      {
        _messageType = messageType;
        _sender = sender;
        _action = action;
      }
      #endregion

      #region Infrastructure
      /// <summary>
      /// <see cref="System.IEquatable{T}.Equals"/>の実装
      /// </summary>
      public bool Equals(MessageParameter other)
      {
        return this.MessageType == other.MessageType
          && this.Sender == other.Sender
          && this.Action == other.Action;
      }

      /// <summary>
      /// <see cref="System.Object.Equals(Object)"/>のオーバーライド
      /// </summary>
      public override bool Equals(object obj)
      {

        if (obj == null || GetType() != obj.GetType())
        {
          return false;
        }

        return this.Equals((MessageParameter)obj);
      }

      /// <summary>
      /// <see cref="System.Object.GetHashCode"/>のオーバーライド
      /// </summary>
      public override int GetHashCode()
      {
        return MessageType.GetHashCode() ^ Sender.GetHashCode()
          ^ Action.GetHashCode();
      }

      /// <summary>
      /// 演算子 == のオーバーロード
      /// </summary>
      public static bool operator ==(MessageParameter left, MessageParameter right)
      {
        return left.Equals(right);
      }

      /// <summary>
      /// 演算子 != のオーバーロード
      /// </summary>
      public static bool operator !=(MessageParameter left, MessageParameter right)
      {
        return !(left == right);
      }
      #endregion
    }
    #endregion

  }

}
