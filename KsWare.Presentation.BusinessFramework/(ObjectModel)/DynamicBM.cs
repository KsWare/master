﻿using System;
using KsWare.Presentation.Documentation;

namespace KsWare.Presentation.BusinessFramework {
	
	/// <summary> [EXPERIMENTAL] Provides a dynamic typed business model.
	/// </summary>
	/// <remarks> The type of value is dynamic assigned at runtime. (<see cref="ValueType"/>).
	/// To compare other similar models see <see cref="docːObjectBM.dataˑmanagementˑofˑbasicˑbusinessˑmodels"/>
	/// </remarks>
	public class DynamicBM : ValueBM<object> {

		public DynamicBM() {
			
		}

		public Type ValueType { get { return Fields.Get<Type>("ValueType"); } set { Fields.Set("ValueType", value); } }

	}

}
