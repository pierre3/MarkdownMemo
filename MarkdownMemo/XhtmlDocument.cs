using System.Xml.Linq;

namespace MarkdownMemo
{
  /// <summary>
  /// XHTMLドキュメントを表します
  /// </summary>
  public class XhtmlDocument : XDocument
  {

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
      //    <title> $(title) </title>
      //    <link rel="stylesheet" type="text/css" href=${styleSheet}/>
      //    <body>$(body)</body>
      //  </head>
      //</html>
      XNamespace xmlns = "http://www.w3.org/1999/xhtml";
      XElement bodyContents;
      try
      {
        bodyContents = XElement.Parse(
          string.Format(@"<body xmlns=""{0}"">{1}</body>", xmlns, body));
      }
      catch
      {
        bodyContents = new XElement(xmlns+"body","変換できない文字列が含まれています。");
      }
      this.Add(
          new XElement(xmlns + "html",
            new XAttribute("xmlns", xmlns),
            new XAttribute(XNamespace.Xml + "lang", "ja"),
            new XAttribute("lang", "ja"),
            new XElement(xmlns + "head",
              new XElement(xmlns + "meta",
                new XAttribute("http-equiv", "Content-Type"),
                new XAttribute("content", "application/xhtml+xml; charset=UTF-8")),
              new XElement(xmlns + "title", title),
              new XElement(xmlns + "link",
                new XAttribute("rel", "stylesheet"),
                new XAttribute("type", "text/css"),
                new XAttribute("href", styleSheet)),
            bodyContents)));
    }


  }
}
