﻿using System.Windows;
using KsWare.Presentation.Core;
using KsWare.Presentation.Core.Providers;

namespace KsWare.Presentation.ViewModelFramework {

	public class ExceptionFeedbackVM : MessageFeedbackVM<ExceptionFeedbackEventArgs> {

		public ExceptionFeedbackVM() {
			RegisterChildren(_=>this);
			CopyDetailsToClipboardAction.MːDoAction = DoCopyDetailsToClipboard;
			CopyAllToClipboardAction.MːDoAction = DoCopyAllToClipboard;
		}

		public void DoCopyDetailsToClipboard() {
			Clipboard.SetText(DetailMessage);
		}
		public void DoCopyAllToClipboard() {
			Clipboard.SetText(Caption+"\r\n\r\nMessage:\r\n"+(MessageBoxText??"")+"\r\n\r\nDetails:\r\n"+(DetailMessage??""));
		}

		public string DetailMessage { get { return Fields.Get(() => DetailMessage); } set { Fields.Set(() => DetailMessage, value); } }
		public ActionVM CopyDetailsToClipboardAction { get; private set; }
		public ActionVM CopyAllToClipboardAction { get; private set; }

		protected override void OnDataChanged(DataChangedEventArgs e) {
			base.OnDataChanged(e);if(Data==null) return;
			DetailMessage  = Data.DetailMessage;
		}		
	}
}