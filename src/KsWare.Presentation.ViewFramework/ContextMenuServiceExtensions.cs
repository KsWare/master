﻿using System.Windows;

namespace KsWare.Presentation.ViewFramework
{

	public static class ContextMenuServiceExtensions
	{

		public static readonly DependencyProperty DataContextProperty =
			DependencyProperty.RegisterAttached("DataContext",
					                            typeof(object), typeof(ContextMenuServiceExtensions),
					                            new UIPropertyMetadata(DataContextChanged));

		public static object GetDataContext(FrameworkElement obj) {
			return obj.GetValue(DataContextProperty);
		}

		public static void SetDataContext(FrameworkElement obj, object value) {
			obj.SetValue(DataContextProperty, value);
		}

		private static void DataContextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			var frameworkElement = d as FrameworkElement;
			if(frameworkElement==null) return;

			if(frameworkElement.ContextMenu!=null)
				frameworkElement.ContextMenu.DataContext = GetDataContext(frameworkElement);
		}

	}

}