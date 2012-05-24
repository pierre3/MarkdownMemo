using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Windows.Input;
using System.IO;
using System.ComponentModel;
using MarkdownSharp;

namespace MarkdownMemo
{
  public class MarkdownText : INotifyPropertyChanged
  {
    #region Fields
    /// <summary>プロパティのバッキングストア</summary>
    private string _text;
    #endregion

    #region Properties
    /// <summary>
    /// HTMLに変換するテキスト
    /// </summary>
    public string Text
    {
      get { return _text; }
      set
      {
        this._text = value;
        this.IsTextChanged = true;
        OnPropertyChanged("Text");
      }
    }
    
    /// <summary>
    /// 現在編集中のテキストファイルのパス
    /// </summary>
    public string SourcePath { get; protected set; }

    /// <summary>
    /// 開いたテキストファイルのエンコーディング
    /// </summary>
    public Encoding Encoding { get; protected set; }

    /// <summary>
    /// HTMLプレビューファイル名(フルパス)
    /// </summary>
    public string PreviewPath { get; set; }
    
    /// <summary>
    /// スタイルシート(CSS)のファイル名
    /// (HTMLプレビューファイル保存先フォルダからの相対パス)
    /// </summary>
    public string CssName { get; set; }

    /// <summary>
    /// テキストが変更されたらTrue。OpenNewメソッドでFalseに初期化されます。
    /// </summary>
    public bool IsTextChanged { get; protected set; }
    #endregion

    #region Events
    /// <summary>
    /// プロパティ変更通知イベント
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;
    #endregion

    #region Protected Methods
    /// <summary>
    /// プロパティ変更通知イベントを発生させます
    /// </summary>
    /// <param name="name">プロパティ名</param>
    protected void OnPropertyChanged(string name)
    {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null)
        handler(this, new PropertyChangedEventArgs(name));
    }
    #endregion

    #region Constructor
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="previewPath">
    /// HTMLプレビューファイル名をフルパスで指定します
    /// </param>
    /// <param name="cssName">
    /// CSSファイル名をHTMLプレビューファイル保存先からの相対パスで指定します
    /// </param>
    public MarkdownText(string previewPath,string cssName)
    {
      OpenNew();
      this.PreviewPath = previewPath;
      this.CssName = cssName;
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// 新規作成
    /// </summary>
    public void OpenNew()
    {
      this.Text = string.Empty;
      this.IsTextChanged = false;
      this.SourcePath = string.Empty;
      this.Encoding = Encoding.UTF8;
    }

    /// <summary>
    /// エンコーディングを判別してテキストファイルを読み込みます。
    /// </summary>
    /// <param name="fileName">ファイル名</param>
    public void OpenFrom(string fileName)
    {
      if(!File.Exists(fileName))
        throw new FileNotFoundException();

      byte[] bytes;
      using(var stream = new FileStream(fileName,FileMode.Open,FileAccess.Read))
      {
        bytes = stream.GetBytes();
      }
      var encoding = bytes.GetCode();
        if(encoding == null)
          throw new NotSupportedException();

      this.Text = bytes.ToDecodedString(encoding);
      this.SourcePath = fileName;
      this.Encoding = encoding;
      this.IsTextChanged = false;
    }

    /// <summary>
    /// 指定したエンコーディングでテキストファイルを読み込みます
    /// </summary>
    /// <param name="fileName">ファイル名</param>
    /// <param name="encoding">エンコーディング</param>
    public void OpenFrom(string fileName, Encoding encoding)
    {
      if(!File.Exists(fileName))
        throw new FileNotFoundException();
      if (encoding == null)
        throw new ArgumentNullException();

      using (var stream = new FileStream(fileName,FileMode.Open,FileAccess.Read))
      {
        this.Text = stream.GetBytes().ToDecodedString(encoding);
      }

      this.SourcePath = fileName;
      this.Encoding = encoding;
      this.IsTextChanged = false;
    }

    /// <summary>
    /// 編集中のテキストを上書き保存する
    /// </summary>
    public bool Save()
    {
      if(!File.Exists(this.SourcePath) || this.Encoding == null)
      {
        return false;
      }
      SaveTo(this.SourcePath, this.Encoding);
      this.IsTextChanged = false;
      return true;
    }

    /// <summary>
    /// 編集中のMarkdownテキストをEncodingプロパティのエンコーディングでファイルに保存します
    /// </summary>
    /// <param name="fileName">ファイル名</param>
    public void SaveTo(string fileName)
    {
      SaveTo(fileName, this.Encoding);
    }

    /// <summary>
    /// 編集中のMarkdownテキストを指定したエンコーディングでファイルに保存します
    /// </summary>
    /// <param name="fileName">ファイル名</param>
    /// <param name="encoding">エンコーディング</param>
    public void SaveTo(string fileName, Encoding encoding)
    {
      if (encoding == null)
        throw new ArgumentNullException("encoding");

      var buf = encoding.GetBytes(this.Text);
      using (var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
      {
        stream.Write(buf, 0, buf.Length);
      }
      this.SourcePath = fileName;
      this.IsTextChanged = false;
    }
    
    /// <summary>
    /// 編集中のMarkdownテキストをHTMLに変換します
    /// </summary>
    /// <param name="title">HTMLタイトル</param>
    /// <param name="cssName">CSSファイル名</param>
    /// <returns>XHTMLドキュメント</returns>
    public XhtmlDocument ToXhtml(string title,string cssName)
    {
      var mdString = new Markdown().Transform(this.Text).Replace("\n",Environment.NewLine);
      return new XhtmlDocument(title,cssName,mdString);
    }

    /// <summary>
    /// 編集中のMarkdownテキストをHTMLに変換し、保存します
    /// </summary>
    /// <param name="fileName">ファイル名</param>
    /// <param name="tilte">タイトル</param>
    public void SaveAsHtml(string fileName,string tilte)
    {
      
    }

    /// <summary>
    ///編集中のMarkdownテキストをHTMLプレビューファイルに変換し保存します
    /// </summary>
    public void SavePreviewHtml()
    {
      this.ToXhtml("Markdown Memo Preview",this.CssName).Save(this.PreviewPath, 
        SaveOptions.DisableFormatting | SaveOptions.OmitDuplicateNamespaces );
    }
    #endregion
  }
}
