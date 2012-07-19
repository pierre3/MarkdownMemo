using System.Windows;

namespace MarkdownMemo
{

  public static class MessengerBehavior
  {

    public static readonly DependencyProperty ActionsProperty =
        DependencyProperty.RegisterAttached("Actions", typeof(ActionCollection), typeof(MessengerBehavior),
        new UIPropertyMetadata(null, ActionsChanged));

    public static ActionCollection GetActions(DependencyObject target)
    {
      return (ActionCollection)target.GetValue(ActionsProperty);
    }

    public static void SetActions(DependencyObject target, ActionCollection value)
    {
      target.SetValue(ActionsProperty, value);
    }

    private static void ActionsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
      var target = sender as FrameworkElement;
      var newValue = e.NewValue as ActionCollection;
      if (target == null || newValue == null)
      { return; }

      target.Loaded -= frameworkElement_loaded;
      target.Loaded += frameworkElement_loaded;
    }

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
