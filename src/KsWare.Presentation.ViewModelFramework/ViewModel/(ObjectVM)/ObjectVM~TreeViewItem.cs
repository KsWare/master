﻿using System.Collections;
using System.ComponentModel;
using JetBrains.Annotations;

namespace KsWare.Presentation.ViewModelFramework 
{
	//
	// this file implements the TreeViewItem part of ObjectVM
	// 
	// IsExpanded 
	// IsSelected (implemented on other part)
	// 
	// NOT IMPLEMENTED: IsSelectionActive/SetKeyboardFocus

	partial class ObjectVM
	{
		private ITreeViewItemSupport _treeViewItem;

		/// <summary>
		/// <see cref="System.Windows.Controls.TreeViewItem"/>
		/// </summary>
		public ITreeViewItemSupport TreeViewItem {
			get {
				if (_treeViewItem == null) _treeViewItem = new TreeViewItemSupport(this);
				return _treeViewItem;
			}
			protected set => _treeViewItem = value;
		}

		public interface ITreeViewItemSupport
		{
			bool IsExpanded { get; set; }
			bool HasNoHeader { get; set; }
			bool HasNoExpander { get; set; }
			IList Nodes { get; set; }
			bool IsLast { get; set; }
		}

		public class TreeViewItemSupport:ITreeViewItemSupport,INotifyPropertyChanged{
			private readonly ObjectVM _parent;
			private bool _isExpanded;
			private bool _hasNoHeader;
			private bool _hasNoExpander;
			private bool _isLast;
			private IList _nodes;

			public TreeViewItemSupport(ObjectVM parent) {
				_parent = parent;
			}

			/// <summary> Gets or sets a value indicating whether this object is expanded.
			/// </summary>
			/// <value><c>true</c> if this object is expanded; otherwise, <c>false</c>.</value>
			/// <remarks></remarks>
			public bool IsExpanded {
				get => _isExpanded;
				set {
					if (_isExpanded == value) return;
					_isExpanded = value;
					OnPropertyChanged(nameof(IsExpanded));
				}
			}

			public event PropertyChangedEventHandler PropertyChanged;

			[NotifyPropertyChangedInvocator]
			protected virtual void OnPropertyChanged(string propertyName) {
				var handler = PropertyChanged;
				if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
			}

			public bool HasNoHeader {
				get => _hasNoHeader;
				set { _hasNoHeader = value; OnPropertyChanged(nameof(HasNoHeader));}
			}

			public bool HasNoExpander {
				get => _hasNoExpander;
				set { _hasNoExpander = value; OnPropertyChanged(nameof(HasNoExpander));}
			}

			public bool IsLast {
				get => _isLast;
				set { _isLast = value; OnPropertyChanged(nameof(IsLast));}
			}

			public IList Nodes {
				get => _nodes;
				set => _nodes = value;
			}
		}
	}

}
