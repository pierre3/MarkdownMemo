using System;
using System.Windows;
using System.Windows.Input;
namespace My.Mvvm
{
  /// <summary>
  /// ファイル編集機能を備えたViewの基本型
  /// </summary>
  /// <remarks>
  /// このWindowは、IFileCommandsインターフェースを実装するViewModelを要求します
  /// </remarks>
  public abstract class FileEditorWindowView : Window, IFileCommandPerformer
  {
    /// <summary>IFileCommandsを実装するオブジェクト(ViewModel)</summary>
    public IFileCommands FileCommands { 
      get { return this.DataContext as IFileCommands; } 
    }
    /// <summary>
    /// Newコマンドのハンドラ
    /// </summary>
    public virtual void FileNewExecuted(object sender, ExecutedRoutedEventArgs e)
    {
      if (FileCommands != null)
      { 
        FileCommands.New(e.Parameter); 
      }
    }

    /// <summary>
    /// Openコマンドのハンドラ
    /// </summary>
    public virtual void FileOpenExecuted(object sender, ExecutedRoutedEventArgs e)
    {
      if (FileCommands != null)
      {
        FileCommands.Open(e.Parameter);
      }
    }

    /// <summary>
    /// Saveコマンドのハンドラ
    /// </summary>
    public virtual void FileSaveExecuted(object sender, ExecutedRoutedEventArgs e)
    {
      if (FileCommands != null)
      {
        FileCommands.Save(e.Parameter);
      }
    }

    /// <summary>
    /// SaveAsコマンドのハンドラ
    /// </summary>
    public virtual void FileSaveAsExecuted(object sender, ExecutedRoutedEventArgs e)
    {
      if (FileCommands != null)
      {
        FileCommands.SaveAs(e.Parameter);
      }
    }

    /// <summary>
    /// NewコマンドCanExecuteのハンドラ
    /// </summary>
    public virtual void FileNewCanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
      if (FileCommands != null)
      {
        e.CanExecute = FileCommands.CanNew(e.Parameter);
      }
    }

    /// <summary>
    /// OpenコマンドCanExecuteのハンドラ
    /// </summary>
    public virtual void FileOpenCanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
      if (FileCommands != null)
      {
        e.CanExecute = FileCommands.CanOpen(e.Parameter);
      }
    }

    /// <summary>
    /// SaveコマンドCanExecuteのハンドラ
    /// </summary>
    public virtual void FileSaveCanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
      if (FileCommands != null)
      {
        e.CanExecute = FileCommands.CanSave(e.Parameter);
      }
    }

    /// <summary>
    /// SaveAsコマンドCanExecuteのハンドラ
    /// </summary>
    public virtual void FileSaveAsCanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
      if (FileCommands != null)
      {
        e.CanExecute = FileCommands.CanSaveAs(e.Parameter);
      }
    }

    /// <summary>
    /// 受け取ったファイルを開く
    /// </summary>
    /// <param name="fileName">ファイル名</param>
    /// <remarks>
    /// ViewModelがIFileOpenerインターフェイスを実装していれば
    /// IFileOpener.Open(string)を実行する。
    /// </remarks>
    public virtual void ReceiveFile(string fileName)
    {
      var fileEditor = this.DataContext as IFileCommands;
      if (fileEditor == null)
      { return; }
      fileEditor.Receive(fileName);
    }

    /// <summary>
    /// ViewModelの終了処理
    /// </summary>
    /// <remarks>
    /// ViewModelがITerminatableインターフェイスを実装していれば
    /// ITerminatable.Terminate()を実行する。
    /// </remarks>
    protected virtual void TerminateViewModel()
    {
      var terminatable = this.DataContext as ITerminatable;
      if (terminatable == null)
      { return; }
      terminatable.Terminate();
    }

    /// <summary>
    /// ViewModelの解放 
    /// </summary>
    protected virtual void DisposeViewModel()
    {
      var disposable = this.DataContext as System.IDisposable;
      if (disposable == null)
      { return; }
      disposable.Dispose();
    }

    /// <summary>
    /// OnClosedのオーバーライド
    /// </summary>
    /// <remarks>
    /// ViewModelの終了処理と解放処理を実行します。
    /// </remarks>
    protected override void OnClosed(EventArgs e)
    {
      TerminateViewModel();
      DisposeViewModel();
      base.OnClosed(e);

    }
  }
}
