using System.Windows;

namespace My.Mvvm
{
  /// <summary>
  /// <see cref="System.Windows.Window"/>のView
  /// </summary>
  public abstract class WindowView:Window
  {
    /// <summary>
    /// ファイルを開く
    /// </summary>
    /// <param name="fileName">ファイル名</param>
    /// <remarks>
    /// ViewModelがIFileOpenerインターフェイスを実装していれば
    /// IFileOpener.Open(string)を実行する。
    /// </remarks>
    protected virtual void OpenFile(string fileName)
    {
      var opener = this.DataContext as IFileOpener;
      if(opener==null)
      { return; }
      opener.Open(fileName);
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
  }
}
