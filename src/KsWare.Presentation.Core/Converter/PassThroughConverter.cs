﻿// ***********************************************************************
// Assembly         : KsWare.Presentation.Core
// Author           : SchreinerK
// Created          : 2018-03-17
//
// Last Modified By : SchreinerK
// Last Modified On : 2018-03-17
// ***********************************************************************
// <copyright file="PassThroughConverter.cs" company="KsWare">
//     Copyright © 2018 by KsWare
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Globalization;
using System.Windows.Data;

namespace KsWare.Presentation {

	/// <summary>
	/// Class PassThroughConverter.
	/// </summary>
	/// <seealso cref="System.Windows.Data.IValueConverter" />
	/// <autogeneratedoc />
	internal class PassThroughConverter : IValueConverter {

		private static readonly Lazy<PassThroughConverter> LazyDefault =
			new Lazy<PassThroughConverter>(() => new PassThroughConverter());

		/// <summary>
		/// Gets the default PassThroughConverter.
		/// </summary>
		/// <value>The default PassThroughConverter.</value>
		public static PassThroughConverter Default => LazyDefault.Value;

		/// <inheritdoc />
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) { return value; }

		/// <inheritdoc />
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { return value; }
	}

}