﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace KsWare.Presentation {

	// Lazy values support
	partial class BackingFieldsStore {

		//TODO EXPERIMANTAL Lazy Fields

		/// <summary>
		/// [EXPERIMENTAL] Adds a lazy field.
		/// </summary>
		/// <typeparam name="TValue">The type of the field.</typeparam>
		/// <param name="fieldName">Name of the field.</param>
		/// <param name="lazy">The lazy value.</param>
		/// <autogeneratedoc />
		public void AddLazy<TValue>(string fieldName, Lazy<TValue> lazy) {
			AddCore(fieldName,lazy);
		}

//		public void AddLazy<T>(Expression<Func<object, T>> propertyExpression, Lazy<object> lazy) {
//			var name = MemberNameUtil.GetPropertyName(propertyExpression);
//			AddCore(name,lazy);
//		}
//
//		public void AddLazy<T>(Expression<Func<T>> propertyExpression, Lazy<object> lazy) {
//			var name = MemberNameUtil.GetPropertyName(propertyExpression);
//			AddCore(name,lazy);
//		}

		/// <summary>
		/// [EXPERIMENTAL] Gets the lazy value.
		/// </summary>
		/// <typeparam name="TValue">The type field.</typeparam>
		/// <param name="fieldName">Name of the field.</param>
		/// <returns>The value.</returns>
		public TValue GetLazy<TValue>(string fieldName) {
			return (TValue)GetInternal<Lazy<TValue>>(fieldName,default(Lazy<TValue>)).Value;
		}

//		public TValue GetLazy<TValue>(Expression<Func<object,TValue>> propertyExpression) {
//			var name = MemberNameUtil.GetPropertyName(propertyExpression);
//			return (TValue)GetInternal<Lazy<object>>(name,default(TValue)).Value;
//		}
//
//		public TValue GetLazy<TValue>(Expression<Func<TValue>> propertyExpression) {
//			var name = MemberNameUtil.GetPropertyName(propertyExpression);
//			return (TValue)GetInternal<Lazy<object>>(name).Value;
//		}

		/// <summary> [EXPERIMENTAL] Gets the specified property. When lazy initialization occurs, the specified initialization function is used.
		/// </summary>
		/// <typeparam name="TRet">The type of the return value.</typeparam>
		/// <param name="propertyExpression">The property expression.</param>
		/// <param name="valueFactory">The delegate that is invoked to produce the lazily initialized value when it is needed.</param>
		/// <returns>The value</returns>
		/// <exception cref="System.Collections.Generic.KeyNotFoundException"></exception>
		public TRet GetValue<TRet>([NotNull]Expression<Func<object,TRet>> propertyExpression, [NotNull]Func<TRet> valueFactory) {
			if (valueFactory == null) throw new ArgumentNullException(nameof(valueFactory));
			var name = MemberNameUtil.GetPropertyName(propertyExpression);
			if (!_fields.ContainsKey(name)) {
				if (_readOnlyCollection) throw new KeyNotFoundException();
				var value = valueFactory();
				AddCore(name,value);
				if(Equals(value,default(TRet))) OnPropertyChanged(name,default(TRet),value);
			}
			return (TRet)_fields[name].Value;	
		}

		/// <summary> [EXPERIMENTAL] Gets the specified property. When lazy initialization occurs, the specified initialization function is used.
		/// </summary>
		/// <typeparam name="TRet">The type of the return value.</typeparam>
		/// <param name="propertyExpression">The property expression.</param>
		/// <param name="valueFactory">The delegate that is invoked to produce the lazily initialized value when it is needed.</param>
		/// <returns>The value</returns>
		/// <exception cref="System.Collections.Generic.KeyNotFoundException"></exception>
		public TRet GetValue<TRet>([NotNull]Expression<Func<TRet>> propertyExpression, [NotNull]Func<TRet> valueFactory) {
			if (valueFactory == null) throw new ArgumentNullException(nameof(valueFactory));
			var name = MemberNameUtil.GetPropertyName(propertyExpression);
			if (!_fields.ContainsKey(name)) {
				if (_readOnlyCollection) throw new KeyNotFoundException();
				var value = valueFactory();
				AddCore(name,value);
				if(Equals(value,default(TRet))) OnPropertyChanged(name,default(TRet),value);
			}
			return (TRet)_fields[name].Value;	
		}

		private LazyAttribute GetLazyAttribute(PropertyInfo propertyInfo) {
			return propertyInfo.GetCustomAttributes(typeof (LazyAttribute), false).Cast<LazyAttribute>().FirstOrDefault();
		}
	}
}
