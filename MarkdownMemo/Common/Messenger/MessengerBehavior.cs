using System.Windows;

namespace MarkdownMemo.Common
{
  /// <summary>
  /// メッセンジャービヘイビア
  /// </summary>
  public static class MessengerBehavior
  {
    /// <summary>アクションコレクション　添付プロパティ</summary>
    public static readonly DependencyProperty ActionsProperty =
        DependencyProperty.RegisterAttached("Actions", typeof(ActionCollection), typeof(MessengerBehavior),
        new UIPropertyMetadata(null, ActionsChanged));
    
    /// <summary>
    /// ActionsプロパティGetter
    /// </summary>
    public static ActionCollection GetActions(DependencyObject target)
    {
      return (ActionCollection)target.GetValue(ActionsProperty);
    }

    /// <summary>
    /// ActionsプロパティSetter 
    /// </summary>
    public static void SetActions(DependencyObject target, ActionCollection value)
    {
      target.SetValue(ActionsProperty, value);
    }

    /// <summary>
    /// Actionsプロパティ変更コールバック
    /// </summary>
    private static void ActionsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
      var target = sender as FrameworkElement;
      var newValue = e.NewValue as ActionCollection;
      if (target == null || newValue == null)
      { return; }

      target.Loaded -= frameworkElement_loaded;
      target.Loaded += frameworkElement_loaded;
    }

    /// <summary>
    /// ターゲットオブジェクトloadedイベントハンドラ
    /// </summary>
    private static void frameworkElement_loaded(object sender, RoutedEventArgs e)
    {
      var target = sender as FrameworkElement;
      if (target == null)
      { return; }

      var actions = GetActions(target);
      actions.RegisterAll(target);
    }


  }
}
