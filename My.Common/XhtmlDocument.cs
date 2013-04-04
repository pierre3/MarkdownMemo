using System.Xml.Linq;

namespace My.Common
{
  /// <summary>
  /// XHTMLドキュメントを表します
  /// </summary>
  public class XhtmlDocument : XDocument
  {
    /// <summary>XMLネームスペース</summary>
    public static readonly XNamespace Xmlns = "http://www.w3.org/1999/xhtml";
    
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="title">HTMLタイトル</param>
    /// <param name="styleSheet">cssスタイルシート名</param>
    /// <param name="body">bodyに表示する内容</param>
    public XhtmlDocument(string title, string styleSheet, string body)
      : base(new XDeclaration("1.0", "utf-8", "no"),
        new XDocumentType("html", "-//W3C//DTD XHTML 1.0 Transitional//EN",
          "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd", null))
    {
      //<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="ja" lang="ja">
      //  <head>
      //    <meta http-equiv="Content-Type" content="application/xhtml+xml; charset=UTF-8"/>
      //    <title> ${title} </title>
      //    <link rel="stylesheet" type="text/css" href=${styleSheet}/>
      //    
      //  </head>
      //  <body>${body}</body>
      //</html>
      XElement bodyContents;
      try
      {
        bodyContents = XElement.Parse(
          string.Format(@"<body xmlns=""{0}"">{1}</body>", Xmlns, body));
      }
      catch
      {
        bodyContents = new XElement(Xmlns+"body","変換できない文字列が含まれています。");
      }
      this.Add(
          new XElement(Xmlns + "html",
            new XAttribute("xmlns", Xmlns),
            new XAttribute(XNamespace.Xml + "lang", "ja"),
            new XAttribute("lang", "ja"),
            new XElement(Xmlns + "head",
              new XElement(Xmlns + "meta",
                new XAttribute("http-equiv", "Content-Type"),
                new XAttribute("content", "application/xhtml+xml; charset=UTF-8")),
              new XElement(Xmlns + "title", title),
              new XElement(Xmlns + "link",
                new XAttribute("rel", "stylesheet"),
                new XAttribute("type", "text/css"),
                new XAttribute("href", styleSheet))),
            bodyContents));
    }

  }
}
