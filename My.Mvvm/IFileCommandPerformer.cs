using System;
using System.Windows.Input;

namespace My.Mvvm
{
  /// <summary>
  /// ファイル操作関連コマンドの実行者　インターフェース
  /// </summary>
  public interface IFileCommandPerformer
  {
    /// <summary>
    /// コマンド処理の実体を保持するオブジェクト(ViewModel)
    /// </summary>
    IFileCommands FileCommands { get; }

    /// <summary>
    /// Newコマンドの実行
    /// </summary>
    void FileNewExecuted(object sender, ExecutedRoutedEventArgs e);

    /// <summary>
    /// Openコマンドの実行
    /// </summary>
    void FileOpenExecuted(object sender, ExecutedRoutedEventArgs e);

    /// <summary>
    /// Saveコマンドの実行
    /// </summary>
    void FileSaveExecuted(object sender, ExecutedRoutedEventArgs e);

    /// <summary>
    /// SaveAsコマンドの実行
    /// </summary>
    void FileSaveAsExecuted(object sender, ExecutedRoutedEventArgs e);

    /// <summary>
    /// Newコマンドの実行可否
    /// </summary>
    void FileNewCanExecute(object sender, CanExecuteRoutedEventArgs e);

    /// <summary>
    /// Openコマンドの実行可否
    /// </summary>
    void FileOpenCanExecute(object sender, CanExecuteRoutedEventArgs e);

    /// <summary>
    /// Saveコマンドの実行可否
    /// </summary>
    void FileSaveCanExecute(object sender, CanExecuteRoutedEventArgs e);

    /// <summary>
    /// SaveAsコマンドの実行可否
    /// </summary>
    void FileSaveAsCanExecute(object sender, CanExecuteRoutedEventArgs e);


    /// <summary>
    /// ファイルの受け取り
    /// </summary>
    void ReceiveFile(string fileName);

  }
}
