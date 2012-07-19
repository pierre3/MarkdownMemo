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
  /// ActionCollection
  /// </summary>
  public class ActionCollection : Freezable, IList<IViewAction>,IList
  {
    private System.Collections.ObjectModel.Collection<IViewAction> _items
      = new System.Collections.ObjectModel.Collection<IViewAction>();

    public object SourceObject
    {
      get { return this.GetValue(SourceObjectProperty); }
      set { this.SetValue(SourceObjectProperty, value); }
    }

    public static object GetSourceObject(DependencyObject obj)
    {
      return (object)obj.GetValue(SourceObjectProperty);
    }

    public static void SetSourceObject(DependencyObject obj, object value)
    {
      obj.SetValue(SourceObjectProperty, value);
    }

    public static readonly DependencyProperty SourceObjectProperty =
        DependencyProperty.Register("SourceObject", typeof(object), typeof(ActionCollection));

    
    protected override Freezable CreateInstanceCore()
    {
      return (Freezable)Activator.CreateInstance(base.GetType());
    }

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

    public void Add(IViewAction item)
    {
      _items.Add(item);
    }

    public void Clear()
    {
      _items.Clear();
    }

    public bool Contains(IViewAction item)
    {
      return _items.Contains(item);
    }

    public void CopyTo(IViewAction[] array, int arrayIndex)
    {
      _items.CopyTo(array, arrayIndex);
    }

    public int Count
    {
      get { return _items.Count; }
    }

    public bool IsReadOnly
    {
      get { return false; }
    }

    public bool Remove(IViewAction item)
    {
      return _items.Remove(item);
    }

    public IEnumerator<IViewAction> GetEnumerator()
    {
      return _items.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return (_items as System.Collections.IEnumerable).GetEnumerator();
    }

    public int IndexOf(IViewAction item)
    {
      return _items.IndexOf(item);
    }

    public void Insert(int index, IViewAction item)
    {
      _items.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
      _items.RemoveAt(index);
    }

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

    public void CopyTo(Array array, int index)
    {
      ((ICollection)_items).CopyTo(array, index);
    }

    public bool IsSynchronized
    {
      get { return ((ICollection)_items).IsSynchronized; }
    }

    public object SyncRoot
    {
      get { return ((ICollection)_items).SyncRoot; }
    }

    public int Add(object value)
    {
      return ((IList)_items).Add(value);
    }

    public bool Contains(object value)
    {
      return ((IList)_items).Contains(value);
    }

    public int IndexOf(object value)
    {
      return ((IList)_items).IndexOf(value);
    }

    public void Insert(int index, object value)
    {
      ((IList)_items).Insert(index, value);
    }

    public bool IsFixedSize
    {
      get { return ((IList)_items).IsFixedSize; }
    }

    public void Remove(object value)
    {
      ((IList)_items).Remove(value);
    }

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
  }

}
