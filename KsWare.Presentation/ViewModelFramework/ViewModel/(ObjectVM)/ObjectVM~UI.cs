﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using JetBrains.Annotations;
using KsWare.Presentation.ViewModelFramework.UIProperties;

namespace KsWare.Presentation.ViewModelFramework {

	public partial interface IObjectVM {

		UIPropertiesRoot UI { get; }

	}

	public partial class ObjectVM {

		private Lazy<UIPropertiesRoot> m_UI;

		private void InitUIProperties() {
			m_UI = new Lazy<UIPropertiesRoot>(() => new UIPropertiesRoot(this));
		}

		/// <summary> Provides additional strong typed, bindable properties
		/// </summary>
		/// <remarks>
		/// All properties are implemented as "only property" w/o further logic.
		/// <code>public object Icon {get { return GetValue&lt;object>("Icon"); }set { SetValue("Icon",value);}}</code>
		/// </remarks>
		public UIPropertiesRoot UI { get { return m_UI.Value; } }
	}
}

namespace KsWare.Presentation.ViewModelFramework.UIProperties {

	public class UIPropertiesBase:INotifyPropertyChanged {
		
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")] 
		private readonly IObjectVM m_Owner;
		private readonly BackingFieldsStore m_Fields;

		/// <summary> Initializes a new instance of the <see cref="UIPropertiesBase" /> class.
		/// </summary>
		/// <param name="owner">The owner.</param>
		public UIPropertiesBase(IObjectVM owner) {
			m_Owner = owner;
			m_Fields=new BackingFieldsStore(this,OnPropertyChanged);
		}

		public BackingFieldsStore Fields{get { return m_Fields; }}

		/// <summary> Occurs when a property changed.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected void OnPropertyChanged(string propertyName) {
			EventUtil.Raise(PropertyChanged,this,new PropertyChangedEventArgs(propertyName),"{D8D0E6E6-4ADA-4828-BF1A-80E538170B48}");
		}

		protected class LazyObjectFactory:BackingFieldsStore.IValueFactory {
			public object CreateInstance(Type type, BackingFieldsStore store) {
				return Activator.CreateInstance(type,store.Owner);
			}
		}
	}

	/// <summary> <see cref="UIElement"/>/<see cref="FrameworkElement"/>
	/// </summary>
	public sealed class UIPropertiesRoot:UIPropertiesBase {
		private readonly Lazy<UIEventConnector> m_EventConnector;
		private readonly Lazy<UIEvents> m_Events        ;
		private readonly Lazy<InputGestureCollection> m_InputGestures ;

		/// <summary> Initializes a new instance of the <see cref="UIPropertiesRoot" /> class.
		/// </summary>
		/// <param name="owner">The owner.</param>
		public UIPropertiesRoot(IObjectVM owner):base(owner) {
			Fields.AddLazy(_=>Button         , new Lazy<object>(()=> new Button(owner)));
			Fields.AddLazy(_=>TreeViewItem   , new Lazy<object>(()=> new TreeViewItem(owner)));
			Fields.AddLazy(_=>MenuItem       , new Lazy<object>(()=> new MenuItem(owner)));
			Fields.AddLazy(_=>Selector       , new Lazy<object>(()=> new Selector(owner)));
			Fields.AddLazy(_=>Item           , new Lazy<object>(()=> new ItemContainer(owner)));
			Fields.AddLazy(_=>ComboBox       , new Lazy<object>(()=> new ComboBox(owner)));
			m_EventConnector = new Lazy<UIEventConnector        >(()=> new UIEventConnector(owner));
			m_Events         = new Lazy<UIEvents                >(()=> new UIEvents(owner));
			m_InputGestures  = new Lazy<InputGestureCollection  >(()=> new InputGestureCollection());
		}


		/// <summary> Gets the button properties.
		/// </summary>
		/// <value>
		/// The button properties.
		/// </value>
		[BackingFieldsStore.Lazy(typeof(LazyObjectFactory))]
		public Button Button {get { return Fields.GetLazy(_ => Button); }}

		/// <summary> Gets the TreeViewItem properties.
		/// </summary>
		/// <value>
		/// The TreeViewItem properties.
		/// </value>
		[BackingFieldsStore.Lazy(typeof(LazyObjectFactory))]
		public TreeViewItem TreeViewItem {get { return Fields.GetLazy(_ => TreeViewItem); }}

		/// <summary> Gets the MenuItem properties.
		/// </summary>
		/// <value>
		/// The MenuItem properties.
		/// </value>
		[BackingFieldsStore.Lazy(typeof(LazyObjectFactory))]
		public MenuItem MenuItem {get { return Fields.GetLazy(_ => MenuItem); }}

		public UIEventConnector EventConnector {get { return m_EventConnector.Value; }}

		public UIEvents Events {get { return m_Events.Value; }}

		/// <summary> System.Windows.Controls.Primitives.Selector
		/// </summary>
		[BackingFieldsStore.Lazy(typeof(LazyObjectFactory))]
		public Selector Selector {get { return Fields.GetLazy(_ => Selector); }}

		/// <summary> System.Windows.Controls.Primitives.Selector
		/// </summary>
		[BackingFieldsStore.Lazy(typeof(LazyObjectFactory))]
		public ItemContainer Item {get { return Fields.GetLazy(_ => Item); }}
		
		public InputGestureCollection InputGestures {get { return m_InputGestures.Value; }}

		/// <summary> System.Windows.Controls.ComboBox
		/// </summary>
		[BackingFieldsStore.Lazy(typeof(LazyObjectFactory))]
		public ComboBox ComboBox  {get { return Fields.GetLazy(_ => ComboBox); }}


		// ###

		/// <summary> see <see cref="FrameworkElement.ToolTip"/>
		/// </summary>
		public object ToolTip { get { return Fields.Get(() => ToolTip); } set { Fields.Set(() => ToolTip, value); } }

		/// <summary> see <see cref="UIElement.Visibility"/>
		/// </summary>
		public Visibility Visibility { get { return Fields.Get(() => Visibility); } set { Fields.Set(() => Visibility, value); } }
		
		/// <summary> see <see cref="FrameworkElement.Tag"/>
		/// </summary>
		public object Tag { get { return Fields.Get(() => Tag); } set { Fields.Set(() => Tag, value); } }


	}

	public class UIEvents {

		private readonly IObjectVM m_Owner;

		public UIEvents(IObjectVM owner) {
			m_Owner = owner;
		}

		public event EventHandler<KeyEventArgs> PreviewKeyDown {
			add    { m_Owner.UI.EventConnector.PreviewKeyDownEventHandler+=value; }
			remove { m_Owner.UI.EventConnector.PreviewKeyDownEventHandler-=value; }
		}

		public event EventHandler<ValueChangedEventArgs> DataContextChanged {
			add    { m_Owner.UI.EventConnector.DataContextChangedEventHandler+=value; }
			remove { m_Owner.UI.EventConnector.DataContextChangedEventHandler-=value; }
		}

	}

	public class UIEventConnector {

		private readonly object m_Owner;

		public UIEventConnector(object owner) {
			m_Owner = owner;
		}
		#region PreviewKeyDown

		public void PreviewKeyDown(object sender, KeyEventArgs e) {EventUtil.Raise(PreviewKeyDownEventHandler,m_Owner,e,"{B78F3AB0-F36E-4255-AAF0-9EB05EBE3C76}");}
		internal event EventHandler<KeyEventArgs> PreviewKeyDownEventHandler;

		#endregion

		#region DataContextChanged

		/// <summary> Called by <see cref="FrameworkElement"/> when the data context changes.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The event data.</param>
		/// <seealso cref="System.Windows.FrameworkElement.DataContextChanged"/>
		public void DataContextChanged(object sender, DependencyPropertyChangedEventArgs e) { EventUtil.Raise(DataContextChangedEventHandler, m_Owner, new ValueChangedEventArgs(e), "{402B2416-5C5C-4D81-B63F-005270F52137}"); }
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="dataContext"></param>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <example><code>this.DataContextChanged += (s, e) => UIEventConnector.DataContextChanged(this.DataContext, s, e);</code>Where <c>this</c> is a <see cref="FrameworkElement"/></example>
		public static void DataContextChanged(object dataContext, object sender, DependencyPropertyChangedEventArgs e) {
			var vm = (IObjectVM) dataContext; if(vm==null) return;
			var ec = vm.UI.EventConnector;
			EventUtil.Raise(ec.DataContextChangedEventHandler, ec.m_Owner, new ValueChangedEventArgs(e), "{402B2416-5C5C-4D81-B63F-005270F52137}");
		}		
		internal event EventHandler<ValueChangedEventArgs> DataContextChangedEventHandler;

		#endregion
	}

}

namespace KsWare.Presentation.ViewModelFramework.UIProperties {

	/// <summary> Button
	/// </summary>
	public sealed class Button:UIPropertiesBase {

		/// <summary> Initializes a new instance of the <see cref="Button" /> class.
		/// </summary>
		/// <param name="owner">The owner.</param>
		public Button(IObjectVM owner):base(owner) {}

		/// <summary> see <see cref="System.Windows.Controls.Button.IsDefault"/>
		/// </summary>
		public bool IsDefault { get { return Fields.Get(() => IsDefault); } set { Fields.Set(() => IsDefault, value); } }

		/// <summary> see <see cref="ToggleButton.IsChecked"/>
		/// </summary>
		public bool IsChecked { get { return Fields.Get(() => IsChecked); } set { Fields.Set(() => IsChecked, value); } }

		//### Additional Properties ###

		public bool IsHighlighted { get { return Fields.Get(() => IsHighlighted); } set { Fields.Set(() => IsHighlighted, value); } }
	}

	/// <summary> TreeViewItem </summary>
	public sealed class TreeViewItem:UIPropertiesBase {

		//
		// this implements the TreeViewItem part of ObjectVM
		// 
		// IsExpanded 
		// IsSelected (implemented on other part)
		// 
		// NOT IMPLEMENTED: IsSelectionActive/SetKeyboardFocus

		/// <summary> Initializes a new instance of the <see cref="TreeViewItem" /> class.
		/// </summary>
		/// <param name="owner">The owner.</param>
		internal TreeViewItem(IObjectVM owner) :base(owner) {}

		/// <summary> Gets or sets a value indicating whether this object is expanded.
		/// </summary>
		/// <value><c>true</c> if this object is expanded; otherwise, <c>false</c>.</value>
		/// <remarks></remarks>

		public bool IsExpanded {
			get { return Fields.Get(() => IsExpanded); } 
			set { Fields.SetAndRaise(() => IsExpanded, value, _=> EventUtil.Raise(IsExpandedChanged,this,EventArgs.Empty,"{355AFA11-F463-429A-AB9F-42F51626DAC8}")); }
		}	
		
		/// <summary> Occurs when <see cref="IsExpanded"/> changed.
		/// </summary>
		public event EventHandler IsExpandedChanged ;

		/// <summary>  Gets or sets a value indicating whether the TreeViewItem has no header.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance has no header; otherwise, <c>false</c>.
		/// </value>
		public bool HasNoHeader { get { return Fields.Get(() => HasNoHeader); } set { Fields.Set(() => HasNoHeader, value); } }

		/// <summary> Gets or sets a value indicating whether the TreeViewItem has no expander.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance has no expander; otherwise, <c>false</c>.
		/// </value>
		public bool HasNoExpander { get { return Fields.Get(() => HasNoExpander); } set { Fields.Set(() => HasNoExpander, value); } }

		/// <summary> Gets or sets a value indicating whether the TreeViewItem is the first node.
		/// </summary>
		/// <value>
		///   <c>true</c> if the TreeViewItem is last node; otherwise, <c>false</c>.
		/// </value>
		public bool IsFirst { get { return Fields.Get(() => IsFirst); } set { Fields.Set(() => IsFirst, value); } }

		/// <summary> Gets or sets a value indicating whether the TreeViewItem is the last node.
		/// </summary>
		/// <value>
		///   <c>true</c> if the TreeViewItem is last node; otherwise, <c>false</c>.
		/// </value>
		public bool IsLast { get { return Fields.Get(() => IsLast); } set { Fields.Set(() => IsLast, value); } }

		public IList List { get { return Fields.Get(() => List); } set { Fields.Set(() => List, value); } }

		public bool HasItems { get { return Fields.Get(() => HasItems); } set { Fields.Set(() => HasItems, value); } }

		public bool IsTopLevel { get { return Fields.Get(() => IsTopLevel); } set { Fields.Set(() => IsTopLevel, value); } }
	}

	/// <summary> MenuItem </summary>
	public sealed class MenuItem:UIPropertiesBase {

		/// <summary> Initializes a new instance of the <see cref="TreeViewItem" /> class.
		/// </summary>
		/// <param name="owner">The owner.</param>
		internal MenuItem(IObjectVM owner) :base(owner) {}

		/// <summary>
		/// <see cref="System.Windows.Controls.MenuItem.Icon"/>
		/// </summary>
		public object Icon { get { return Fields.Get(() => Icon); } set { Fields.Set(() => Icon, value); } }

		/// <summary>
		/// <see cref="HeaderedItemsControl.Header"/>
		/// </summary>
		public object Header { get { return Fields.Get(() => Header); } set { Fields.Set(() => Header, value); } }

		/// <summary>
		/// <see cref="System.Windows.Controls.MenuItem.IsCheckable"/>
		/// </summary>
		public bool IsCheckable { get { return Fields.Get(() => IsCheckable); } set { Fields.Set(() => IsCheckable, value); } }

		/// <summary>
		/// <see cref="System.Windows.Controls.MenuItem.IsChecked"/>
		/// </summary>
		public bool IsChecked { get { return Fields.Get(() => IsChecked); } set { Fields.Set(() => IsChecked, value); } }
	}

	/// <summary>
	/// System.Windows.Controls.Primitives.Selector
	/// </summary>
	public sealed class Selector : UIPropertiesBase {

		internal Selector(IObjectVM owner) :base(owner) {}

		/// <summary> Gets or sets the first item in the current selection or returns null if the selection is empty 
		/// </summary>
		/// <value>The first item in the current selection or null if the selection is empty.</value>
		/// <seealso cref="System.Windows.Controls.Primitives.Selector.SelectedItem"/>
		public object SelectedItem { get { return Fields.Get(() => SelectedItem); } set { Fields.Set(() => SelectedItem, value); } }

		/// <summary> Gets or sets the index of the first item in the current selection or returns negative one (-1) if the selection is empty. 
		/// </summary>
		/// <value>The index of first item in the current selection. The default value is negative one (-1).</value>
		/// <seealso cref="System.Windows.Controls.Primitives.Selector.SelectedIndex"/>
		public int SelectedIndex { get { return Fields.Get(() => SelectedIndex); } set { Fields.Set(() => SelectedIndex, value); } }
	}

	public sealed class ComboBox:UIPropertiesBase {

		/// <summary> Initializes a new instance of the <see cref="ComboBox" /> class.
		/// </summary>
		/// <param name="owner">The owner.</param>
		internal ComboBox(IObjectVM owner) :base(owner) {}

		/// <summary> Gets or sets a value that enables or disables editing of the text in text box of the System.Windows.Controls.ComboBox.
		/// </summary>
		/// <value><c>true</c> if the <see cref="System.Windows.Controls.ComboBox"/> can be edited; otherwise <c>false</c>. The default is false.</value>
		/// <seealso cref="System.Windows.Controls.ComboBox.IsEditable"/>
		public bool IsEditable { get { return Fields.Get(() => IsEditable); } set { Fields.Set(() => IsEditable, value); } }

		/// <summary> Gets or sets a value that indicates whether the drop-down for a combo box is currently open.
		/// </summary>
		/// <value>true if the drop-down is open; otherwise, false. The default is false.</value>
		/// <seealso cref="System.Windows.Controls.ComboBox.IsDropDownOpen"/>
		public bool IsDropDownOpen { get { return Fields.Get(() => IsDropDownOpen); } set { Fields.Set(() => IsDropDownOpen, value); } }

	}

	public sealed class ItemContainer : UIPropertiesBase {

		internal ItemContainer(IObjectVM owner) :base(owner) {}

		/// <summary> Gets or sets a value that indicates whether an item is selected.
		/// </summary>
		/// <value><c>true</c> if this item is selected; otherwise, <c>false</c>.</value>
		/// <seealso cref="System.Windows.Controls.Primitives.Selector.IsSelectedProperty"/>
		public bool IsSelected { get { return Fields.Get(() => IsSelected); } set { Fields.Set(() => IsSelected, value); } }


		/// <summary> Gets a value that indicates whether the keyboard focus is within the value returned by a Selector.
		/// </summary>
		/// <value><c>true</c> if the keyboard focus is within the Selector control; otherwise, <c>false</c>.</value>
		/// <seealso cref="System.Windows.Controls.Primitives.Selector.IsSelectionActiveProperty"/>
		public int IsSelectionActive { get { return Fields.Get(() => IsSelectionActive); } set { Fields.Set(() => IsSelectionActive, value); } }

	}
}