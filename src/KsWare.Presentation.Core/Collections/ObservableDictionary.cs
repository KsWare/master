﻿// ***********************************************************************
// Assembly         : KsWare.Presentation.Core
// Author           : SchreinerK
// Created          : 2018-03-18
//
// Last Modified By : SchreinerK
// Last Modified On : 2018-03-18
// ***********************************************************************
// <copyright file="ObservableDictionary.cs" company="KsWare">
//     Copyright © 2002-2018 by KsWare. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace KsWare.Presentation.Collections {

	/// <summary>
	/// [EXPERIMENTAL] Class ObservableDictionary.
	/// </summary>
	/// <typeparam name="TKey">The type of the t key.</typeparam>
	/// <typeparam name="TValue">The type of the t value.</typeparam>
	/// <seealso cref="System.Collections.Generic.IDictionary{TKey, TValue}" />
	/// <seealso cref="System.Collections.Specialized.INotifyCollectionChanged" />
	/// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
	/// <autogeneratedoc />
	[PublicAPI]
	[SuppressMessage("ReSharper", "ClassWithVirtualMembersNeverInherited.Global")]
	public class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, INotifyCollectionChanged,
		INotifyPropertyChanged {
		private const string CountString = "Count";
		private const string IndexerName = "Item[]";
		private const string KeysName = "Keys";
		private const string ValuesName = "Values";

		private IDictionary<TKey, TValue> _dictionary;

		/// <summary>
		/// Gets the dictionary.
		/// </summary>
		/// <value>The dictionary.</value>
		/// <autogeneratedoc />
		protected IDictionary<TKey, TValue> Dictionary => _dictionary;

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ObservableDictionary{TKey, TValue}"/> class.
		/// </summary>
		/// <autogeneratedoc />
		public ObservableDictionary() { _dictionary = new Dictionary<TKey, TValue>(); }

		/// <summary>
		/// Initializes a new instance of the <see cref="ObservableDictionary{TKey, TValue}"/> class.
		/// </summary>
		/// <param name="dictionary">The dictionary.</param>
		/// <autogeneratedoc />
		public ObservableDictionary(IDictionary<TKey, TValue> dictionary) {
			_dictionary = new Dictionary<TKey, TValue>(dictionary);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ObservableDictionary{TKey, TValue}"/> class.
		/// </summary>
		/// <param name="comparer">The comparer.</param>
		/// <autogeneratedoc />
		public ObservableDictionary(IEqualityComparer<TKey> comparer) {
			_dictionary = new Dictionary<TKey, TValue>(comparer);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ObservableDictionary{TKey, TValue}"/> class.
		/// </summary>
		/// <param name="capacity">The capacity.</param>
		/// <autogeneratedoc />
		public ObservableDictionary(int capacity) { _dictionary = new Dictionary<TKey, TValue>(capacity); }

		/// <summary>
		/// Initializes a new instance of the <see cref="ObservableDictionary{TKey, TValue}"/> class.
		/// </summary>
		/// <param name="dictionary">The dictionary.</param>
		/// <param name="comparer">The comparer.</param>
		/// <autogeneratedoc />
		public ObservableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) {
			_dictionary = new Dictionary<TKey, TValue>(dictionary, comparer);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ObservableDictionary{TKey, TValue}"/> class.
		/// </summary>
		/// <param name="capacity">The capacity.</param>
		/// <param name="comparer">The comparer.</param>
		/// <autogeneratedoc />
		public ObservableDictionary(int capacity, IEqualityComparer<TKey> comparer) {
			_dictionary = new Dictionary<TKey, TValue>(capacity, comparer);
		}

		#endregion

		#region IDictionary<TKey,TValue> Members

		/// <summary>
		/// Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2" />.
		/// </summary>
		/// <param name="key">The object to use as the key of the element to add.</param>
		/// <param name="value">The object to use as the value of the element to add.</param>
		/// <autogeneratedoc />
		public void Add(TKey key, TValue value) { Insert(key, value, true); }

		/// <summary>
		/// Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the specified key.
		/// </summary>
		/// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2" />.</param>
		/// <returns><see langword="true" /> if the <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the key; otherwise, <see langword="false" />.</returns>
		/// <autogeneratedoc />
		public bool ContainsKey(TKey key) { return Dictionary.ContainsKey(key); }

		/// <summary>
		/// Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the keys of the <see cref="T:System.Collections.Generic.IDictionary`2" />.
		/// </summary>
		/// <value>The keys.</value>
		/// <autogeneratedoc />
		public ICollection<TKey> Keys => Dictionary.Keys;

		/// <summary>
		/// Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2" />.
		/// </summary>
		/// <param name="key">The key of the element to remove.</param>
		/// <returns><see langword="true" /> if the element is successfully removed; otherwise, <see langword="false" />.  This method also returns <see langword="false" /> if <paramref name="key" /> was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2" />.</returns>
		/// <exception cref="System.ArgumentNullException">key</exception>
		/// <autogeneratedoc />
		public bool Remove(TKey key) {
			if (key == null) throw new ArgumentNullException(nameof(key));

			Dictionary.TryGetValue(key, out var value);
			var removed = Dictionary.Remove(key);
			if (removed)
				OnCollectionChanged(NotifyCollectionChangedAction.Remove, new KeyValuePair<TKey, TValue>(key, value));
				//OnCollectionChanged();

			return removed;
		}


		/// <summary>
		/// Gets the value associated with the specified key.
		/// </summary>
		/// <param name="key">The key whose value to get.</param>
		/// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value" /> parameter. This parameter is passed uninitialized.</param>
		/// <returns><see langword="true" /> if the object that implements <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the specified key; otherwise, <see langword="false" />.</returns>
		/// <autogeneratedoc />
		public bool TryGetValue(TKey key, out TValue value) { return Dictionary.TryGetValue(key, out value); }


		/// <summary>
		/// Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2" />.
		/// </summary>
		/// <value>The values.</value>
		/// <autogeneratedoc />
		public ICollection<TValue> Values => Dictionary.Values;


		/// <summary>
		/// Gets or sets the <see cref="TValue"/> with the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>TValue.</returns>
		/// <autogeneratedoc />
		public TValue this[TKey key] { get => Dictionary[key]; set => Insert(key, value, false); }

		#endregion


		#region ICollection<KeyValuePair<TKey,TValue>> Members

		/// <summary>
		/// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
		/// </summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
		/// <autogeneratedoc />
		public void Add(KeyValuePair<TKey, TValue> item) { Insert(item.Key, item.Value, true); }


		/// <summary>
		/// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
		/// </summary>
		/// <autogeneratedoc />
		public void Clear() {
			if (Dictionary.Count > 0) {
				Dictionary.Clear();
				OnCollectionChanged();
			}
		}


		/// <summary>
		/// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
		/// <returns><see langword="true" /> if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, <see langword="false" />.</returns>
		/// <autogeneratedoc />
		public bool Contains(KeyValuePair<TKey, TValue> item) { return Dictionary.Contains(item); }


		/// <summary>
		/// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
		/// <autogeneratedoc />
		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) { Dictionary.CopyTo(array, arrayIndex); }


		/// <summary>
		/// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
		/// </summary>
		/// <value>The count.</value>
		/// <autogeneratedoc />
		public int Count => Dictionary.Count;


		/// <summary>
		/// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
		/// </summary>
		/// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
		/// <autogeneratedoc />
		public bool IsReadOnly => Dictionary.IsReadOnly;


		/// <summary>
		/// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.
		/// </summary>
		/// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
		/// <returns><see langword="true" /> if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, <see langword="false" />. This method also returns <see langword="false" /> if <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.</returns>
		/// <autogeneratedoc />
		public bool Remove(KeyValuePair<TKey, TValue> item) { return Remove(item.Key); }

		#endregion


		#region IEnumerable<KeyValuePair<TKey,TValue>> Members

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>An enumerator that can be used to iterate through the collection.</returns>
		/// <autogeneratedoc />
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() { return Dictionary.GetEnumerator(); }

		#endregion


		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator() { return ((IEnumerable) Dictionary).GetEnumerator(); }

		#endregion


		#region INotifyCollectionChanged Members

		/// <summary>
		/// Occurs when the collection changes.
		/// </summary>
		/// <autogeneratedoc />
		public event NotifyCollectionChangedEventHandler CollectionChanged;

		#endregion


		#region INotifyPropertyChanged Members

		/// <summary>
		/// Occurs when a property value changes.
		/// </summary>
		/// <autogeneratedoc />
		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		/// <summary>
		/// Adds the range.
		/// </summary>
		/// <param name="items">The items.</param>
		/// <exception cref="System.ArgumentNullException">items</exception>
		/// <exception cref="System.ArgumentException">An item with the same key has already been added.</exception>
		/// <autogeneratedoc />
		public void AddRange(IDictionary<TKey, TValue> items) {
			if (items == null) throw new ArgumentNullException(nameof(items));


			if (items.Count > 0) {
				if (Dictionary.Count > 0) {
					if (items.Keys.Any((k) => Dictionary.ContainsKey(k)))
						throw new ArgumentException("An item with the same key has already been added.");
					else
						foreach (var item in items)
							Dictionary.Add(item);
				}
				else _dictionary = new Dictionary<TKey, TValue>(items);


				OnCollectionChanged(NotifyCollectionChangedAction.Add, items.ToArray());
			}
		}

		private void Insert(TKey key, TValue value, bool add) {
			if (key == null) throw new ArgumentNullException(nameof(key));


			if (Dictionary.TryGetValue(key, out var item)) {
				if (add) throw new ArgumentException("An item with the same key has already been added.");
				if (Equals(item, value)) return;
				Dictionary[key] = value;


				OnCollectionChanged(NotifyCollectionChangedAction.Replace, new KeyValuePair<TKey, TValue>(key, value),
					new KeyValuePair<TKey, TValue>(key,                                                        item));
			}
			else {
				Dictionary[key] = value;

				OnCollectionChanged(NotifyCollectionChangedAction.Add, new KeyValuePair<TKey, TValue>(key, value));
			}
		}

		private void OnPropertyChanged() {
			OnPropertyChanged(CountString);
			OnPropertyChanged(IndexerName);
			OnPropertyChanged(KeysName);
			OnPropertyChanged(ValuesName);
		}

		/// <summary>
		/// Called when [property changed].
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		/// <autogeneratedoc />
		protected virtual void OnPropertyChanged(string propertyName) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private void OnCollectionChanged() {
			OnPropertyChanged();
			CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		private void OnCollectionChanged(NotifyCollectionChangedAction action, KeyValuePair<TKey, TValue> changedItem) {
			OnPropertyChanged();
			CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, changedItem));
		}

		private void OnCollectionChanged(NotifyCollectionChangedAction action,
			KeyValuePair<TKey, TValue> newItem,
			KeyValuePair<TKey, TValue> oldItem) {
			OnPropertyChanged();
			CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, newItem, oldItem));
		}

		private void OnCollectionChanged(NotifyCollectionChangedAction action, IList newItems) {
			OnPropertyChanged();
			CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, newItems));
		}
	}

}