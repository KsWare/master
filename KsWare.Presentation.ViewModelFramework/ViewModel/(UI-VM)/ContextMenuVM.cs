﻿using System.Windows.Controls;

namespace KsWare.Presentation.ViewModelFramework {

	/// <summary> Provides a view model for <see cref="ContextMenu"/>
	/// </summary>
	public class ContextMenuVM:ObjectVM {

		public ContextMenuVM() {
			RegisterChildren(()=>this);
		}

		public StringVM Header { get; private set; }

		public ListVM<ObjectVM> Items { get; private set; }

		public bool IsPopup {get { return Fields.Get<bool>("IsPopup"); } set { Fields.Set("IsPopup", value); }}

	}
}
