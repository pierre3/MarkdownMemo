using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MarkdownMemo.Common
{
  public static class XEnumerable
  {
    /// <summary>
    /// XElementのシーケンスから、指定した属性を持つ要素を取得
    /// </summary>
    /// <typeparam name="TResult">戻り値として取得する型</typeparam>
    /// <param name="elements">XML要素のシーケンス</param>
    /// <param name="elementName">取得する要素の名前</param>
    /// <param name="attributeName">取得したい要素の持つ属性の名前</param>
    /// <param name="selector">取得したデータを戻り値の形に加工する処理を指定</param>
    /// <returns>結果のシーケンス</returns>
    public static IEnumerable<TResult> WithAttribute<TResult>(this IEnumerable<XElement> elements,
      XName elementName, XName attributeName, Func<XElement, XAttribute, TResult> selector)
    {
      return elements.Where(elem => elem.Name == elementName)
        .Where(elem => elem.HasAttributes && elem.Attribute(attributeName) != null)
        .Select(elem => selector(elem, elem.Attribute(attributeName)));
    }
  }
}
