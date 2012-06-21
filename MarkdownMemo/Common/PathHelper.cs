using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MarkdownMemo.Common
{
  /// <summary>
  /// ファイル、ディレクトリ名の操作を補助するクラス
  /// </summary>
  public static class PathHelper
  {
    /// <summary>
    /// "%ApplicationData%\(拡張子を省いた実行ファイル名)" のフォルダを作成する
    /// </summary>
    /// <returns>作成したフォルダのパス</returns>
    public static string CreateAppDataDirectory()
    {
      var dir = Path.Combine(
              Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
              Path.GetFileNameWithoutExtension(Environment.GetCommandLineArgs()[0]));
      Directory.CreateDirectory(dir);
      return dir;
    }

  }
}
