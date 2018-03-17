﻿#if(DYNAMIC_SUPPORT)
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

// http://stackoverflow.com/questions/3142495/deserialize-json-into-c-sharp-dynamic-object

namespace KsWare.JsonFx {

	internal sealed class DynamicJsonConverter : JavaScriptConverter {

		public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer) {
			if (dictionary == null) throw new ArgumentNullException("dictionary");

			return type == typeof (object) ? new DynamicJsonObject(dictionary) : null;
		}

		public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer) { throw new NotImplementedException(); }

		public override IEnumerable<Type> SupportedTypes { get { return new ReadOnlyCollection<Type>(new List<Type>(new[] {typeof (object)})); } }

		#region Nested type: DynamicJsonObject

		private sealed class DynamicJsonObject : DynamicObject {
			private readonly IDictionary<string, object> mDictionary;

			public DynamicJsonObject(IDictionary<string, object> dictionary) {
				if (dictionary == null) throw new ArgumentNullException("dictionary");
				mDictionary = dictionary;
			}

			public override string ToString() {
				var sb = new StringBuilder("{");
				ToString(sb);
				return sb.ToString();
			}

			private void ToString(StringBuilder sb) {
				var firstInDictionary = true;
				foreach (var pair in mDictionary) {
					if (!firstInDictionary)
						sb.Append(",");
					firstInDictionary = false;
					var value = pair.Value;
					var name = pair.Key;
					if (value is string) {
						sb.AppendFormat("{0}:\"{1}\"", name, value);
					} else if (value is IDictionary<string, object>) {
						new DynamicJsonObject((IDictionary<string, object>) value).ToString(sb);
					} else if (value is ArrayList) {
						sb.Append(name + ":[");
						var firstInArray = true;
						foreach (var arrayValue in (ArrayList) value) {
							if (!firstInArray)
								sb.Append(",");
							firstInArray = false;
							if (arrayValue is IDictionary<string, object>)
								new DynamicJsonObject((IDictionary<string, object>) arrayValue).ToString(sb);
							else if (arrayValue is string)
								sb.AppendFormat("\"{0}\"", arrayValue);
							else
								sb.AppendFormat("{0}", arrayValue);

						}
						sb.Append("]");
					} else {
						sb.AppendFormat("{0}:{1}", name, value);
					}
				}
				sb.Append("}");
			}

			public override bool TryGetMember(GetMemberBinder binder, out object result) {
				if (!mDictionary.TryGetValue(binder.Name, out result)) {
					// return null to avoid exception.  caller can check for null this way...
					result = null;
					return true;
				}

				var dictionary = result as IDictionary<string, object>;
				if (dictionary != null) {
					result = new DynamicJsonObject(dictionary);
					return true;
				}

				var arrayList = result as ArrayList;
				if (arrayList != null && arrayList.Count > 0) {
					if (arrayList[0] is IDictionary<string, object>)
						result = new List<object>(arrayList.Cast<IDictionary<string, object>>().Select(x => new DynamicJsonObject(x)));
					else
						result = new List<object>(arrayList.Cast<object>());
				}

				return true;
			}
		}

		#endregion
	}
}
#endif