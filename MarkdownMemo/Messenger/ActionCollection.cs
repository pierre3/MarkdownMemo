using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using MarkdownMemo.ViewModel;

namespace MarkdownMemo
{

  /// <summary>
  /// Messengerアクションコレクション
  /// </summary>
  public class ActionCollection : Freezable, IList<IViewAction>,IList
  {
    #region Fields
    /// <summary>メッセージ送信元オブジェクト 依存関係プロパティ</summary>
    public static readonly DependencyProperty SourceObjectProperty =
        DependencyProperty.Register("SourceObject", typeof(object), typeof(ActionCollection));

    /// <summary>コレクションのBacking Store</summary>
    private System.Collections.ObjectModel.Collection<IViewAction> _items
      = new System.Collections.ObjectModel.Collection<IViewAction>();
    #endregion

    #region Properties
    /// <summary>
    /// アクション登録先オブジェクト(Messenger)
    /// </summary>
    public object SourceObject
    {
      get { return this.GetValue(SourceObjectProperty); }
      set { this.SetValue(SourceObjectProperty, value); }
    }
    #endregion

    #region Methods
    /// <summary>
    /// SourceObjectプロパティGetter
    /// </summary>
    public static object GetSourceObject(DependencyObject obj)
    {
      return (object)obj.GetValue(SourceObjectProperty);
    }

    /// <summary>
    /// SourceObjectoプロパディSetter
    /// </summary>
    public static void SetSourceObject(DependencyObject obj, object value)
    {
      obj.SetValue(SourceObjectProperty, value);
    }

    /// <summary>
    /// コレクション内のアクションをメッセンジャーに登録する
    /// </summary>
    /// <param name="recipient">メッセージ受信先オブジェクト</param>
    public void RegisterAll(FrameworkElement recipient)
    {
      var messenger = SourceObject as Messenger;
      if (messenger == null)
      { throw new InvalidOperationException("SourceObject Property"); }

      foreach (var action in _items)
      {
        action.Register(recipient, messenger);
      }
    }

    /// <summary>
    /// <see cref="System.Windows.Freezable.CreateInstanceCore"/>のオーバーライド 
    /// </summary>
    protected override Freezable CreateInstanceCore()
    {
      return new ActionCollection();
    }

    #region System.Collections.Generic.ICollection<IViewAction>の実装
    /// <summary>
    /// コレクションの要素数を取得
    /// </summary>
    public int Count
    {
      get { return _items.Count; }
    }

    /// <summary>
    /// コレクションが読み込み専用か否かを取得
    /// <remarks>常にFalseを返す</remarks>
    /// </summary>
    public bool IsReadOnly
    {
      get { return false; }
    }

    /// <summary>
    /// 要素の追加
    /// </summary>
    /// <param name="item">追加する要素</param>
    public void Add(IViewAction item)
    {
      _items.Add(item);
    }

    /// <summary>
    /// コレクション内の要素をすべて削除
    /// </summary>
    public void Clear()
    {
      _items.Clear();
    }

    /// <summary>
    /// 指定した要素がコレクションに含まれるかを調べる
    /// </summary>
    /// <param name="item">要素</param>
    /// <returns>指定した要素が含まれる場合True</returns>
    public bool Contains(IViewAction item)
    {
      return _items.Contains(item);
    }

    /// <summary>
    ///コレクションのコピー 
    /// </summary>
    /// <param name="array">コピー先配列</param>
    /// <param name="arrayIndex">コピーを開始するコピー先配列のIndex</param>
    public void CopyTo(IViewAction[] array, int arrayIndex)
    {
      _items.CopyTo(array, arrayIndex);
    }

    /// <summary>
    /// 指定した要素をコレクションから削除する
    /// </summary>
    /// <param name="item">削除する要素</param>
    /// <returns>正常に削除が行われた場合にTrue</returns>
    public bool Remove(IViewAction item)
    {
      return _items.Remove(item);
    }
    #endregion

    #region System.Collections.Generic.IList<IViewAction>の実装
    /// <summary>
    /// 指定したアイテムのIndexを取得
    /// </summary>
    /// <param name="item">アイテム</param>
    /// <returns>指定したアイテムのIndex。存在しない場合は-1を返す</returns>
    public int IndexOf(IViewAction item)
    {
      return _items.IndexOf(item);
    }

    /// <summary>
    /// アイテムの挿入
    /// </summary>
    /// <param name="index">挿入位置</param>
    /// <param name="item">挿入アイテム</param>
    public void Insert(int index, IViewAction item)
    {
      _items.Insert(index, item);
    }

    /// <summary>
    /// 指定したIndexのアイテムをコレクションから削除する
    /// </summary>
    /// <param name="index">インデックス</param>
    public void RemoveAt(int index)
    {
      _items.RemoveAt(index);
    }

    /// <summary>
    /// インデクサ
    /// </summary>
    public IViewAction this[int index]
    {
      get
      {
        return _items[index];
      }
      set
      {
        _items[index] = value;
      }
    }
    #endregion

    #region System.Collections.ICollection の実装
    /// <summary>
    /// コピー
    /// </summary>
    /// <param name="array">コピー先配列</param>
    /// <param name="index">コピーを開始するコピー先配列のインデックス</param>
    public void CopyTo(Array array, int index)
    {
      ((ICollection)_items).CopyTo(array, index);
    }

    /// <summary>
    /// コレクションへのアクセスが同期されているか否かを示す
    /// </summary>
    public bool IsSynchronized
    {
      get { return ((ICollection)_items).IsSynchronized; }
    }
    
    /// <summary>
    /// コレクションへのアクセスを同期するために使用できるオブジェクトを取得
    /// </summary>
    public object SyncRoot
    {
      get { return ((ICollection)_items).SyncRoot; }
    }
    #endregion

    #region System.Collections.IList の実装
    /// <summary>
    /// 要素の追加
    /// </summary>
    /// <param name="value">追加する要素</param>
    /// <returns>新しい要素が追加された位置。追加できなかった場合は-1を返す</returns>
    public int Add(object value)
    {
      return ((IList)_items).Add(value);
    }

    /// <summary>
    /// 指定したオブジェクトが格納されているかを調べる
    /// </summary>
    /// <param name="value">確認するオブジェクト</param>
    /// <returns>指定したオブジェクトが格納されている場合はTrue</returns>
    public bool Contains(object value)
    {
      return ((IList)_items).Contains(value);
    }

    /// <summary>
    /// 指定した項目のコレクション内でのインデックスを取得する
    /// </summary>
    /// <param name="value">項目</param>
    /// <returns>インデックス</returns>
    public int IndexOf(object value)
    {
      return ((IList)_items).IndexOf(value);
    }

    /// <summary>
    /// 指定した場所にオブジェクトを挿入する
    /// </summary>
    /// <param name="index">挿入位置</param>
    /// <param name="value">挿入するオブジェクト</param>
    public void Insert(int index, object value)
    {
      ((IList)_items).Insert(index, value);
    }

    /// <summary>
    /// コレクションが固定サイズかどうかをしめす
    /// </summary>
    public bool IsFixedSize
    {
      get { return ((IList)_items).IsFixedSize; }
    }

    /// <summary>
    /// 指定したオブジェクトを削除
    /// </summary>
    /// <param name="value">削除するオブジェクト</param>
    public void Remove(object value)
    {
      ((IList)_items).Remove(value);
    }

    /// <summary>
    /// インデクサ
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    object IList.this[int index]
    {
      get
      {
        return ((IList)_items)[index];
      }
      set
      {
        ((IList)_items)[index] = value;
      }
    }
    #endregion

    #region System.Collections.Generic.IEnumerable<IViewAction> の実装
    /// <summary>
    /// コレクションを反復処理する列挙子を返す
    /// </summary>
    /// <returns>コレクション列挙子</returns>
    public IEnumerator<IViewAction> GetEnumerator()
    {
      return _items.GetEnumerator();
    }
    #endregion

    #region System.Collections.IEnumerable の実装
    /// <summary>
    /// コレクションを反復処理する列挙子を返す
    /// </summary>
    /// <returns>コレクション列挙子</returns>
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return (_items as System.Collections.IEnumerable).GetEnumerator();
    }
    #endregion

    #endregion
  }

}
