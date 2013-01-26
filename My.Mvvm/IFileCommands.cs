
namespace My.Mvvm
{
  /// <summary>
  /// ファイルを操作する機能を提供するインターフェイス
  /// </summary>
  public interface IFileCommands
  {
    /// <summary>新規作成</summary>
    void New(object parameter);

    /// <summary>開く</summary>
    void Open(object parameter);
    
    /// <summary>保存</summary>
    void Save(object parameter);

    /// <summary>名前を付けて保存</summary>
    void SaveAs(object parameter);

    /// <summary>新規作成 実行可否</summary>
    bool CanNew(object parameter);

    /// <summary>開く 実行可否</summary>
    bool CanOpen(object parameter);

    /// <summary>保存 実行可否</summary>
    bool CanSave(object parameter);

    /// <summary>名前を付けて保存 実行可否</summary>
    bool CanSaveAs(object parameter);

    /// <summary>
    /// ファイルを受け取って開く
    /// </summary>
    /// <remarks>
    /// たとえば、コマンドライン引数でファイル名を受け取った場合、
    /// ドラッグアンドドロップでファイルを渡された場合など
    /// </remarks>
    /// <param name="fileName">ファイル名</param>
    void Receive(string fileName);
  }
}
