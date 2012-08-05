using System;
using System.Windows.Input;

namespace My.Mvvm
{
  /// <summary>
  /// デリゲートコマンド
  /// </summary>
  public class DelegateCommand : ICommand
  {
    private Action<object> _execute;
    private Predicate<object> _canExecute;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="execute">コマンド実行処理</param>
    /// <param name="canExecute">コマンド実行可否判定処理</param>
    public DelegateCommand(Action<object> execute, Predicate<object> canExecute)
    {
      this._execute = execute;
      this._canExecute = canExecute;
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="execute">コマンド実行処理</param>
    public DelegateCommand(Action<object> execute)
      : this(execute, null)
    { }

    /// <summary>
    /// コマンド実行可否判定
    /// </summary>
    /// <param name="parameter">パラメータ</param>
    /// <returns>コマンド実行可能の場合True</returns>
    public bool CanExecute(object parameter)
    {
      if (_canExecute == null)
      {
        return true;
      }
      else
      {
        return _canExecute(parameter);
      }
    }

    /// <summary>
    /// コマンド実行可否変更イベント
    /// </summary>
    public event EventHandler CanExecuteChanged
    {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }

    /// <summary>
    /// コマンド実行
    /// </summary>
    /// <param name="parameter">パラメータ</param>
    public void Execute(object parameter)
    {
      if (_execute != null)
        _execute(parameter);
    }
  }
}
