﻿using System;
using System.Windows.Data;

namespace KsWare.Presentation {

	/// <summary>
	/// [EXPERIMENTAL] Class FieldBinding.
	/// </summary>
	/// <autogeneratedoc />
	public class FieldBinding  {

		/// <summary>
		/// Initializes a new instance of the <see cref="FieldBinding"/> class.
		/// </summary>
		/// <autogeneratedoc />
		public FieldBinding() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="FieldBinding"/> class.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="mode">The mode.</param>
		/// <param name="converter">The converter.</param>
		/// <autogeneratedoc />
		public FieldBinding(BackingFieldsStore.IBackingFieldInfo source, BindingMode mode=BindingMode.Default, IValueConverter converter=null) {
			Source    = source;
			Mode      = mode;
			Converter = converter ?? DataTypeConverter.Default;
		}

		/// <summary>
		/// Gets or sets the source field.
		/// </summary>
		/// <value>The source field.</value>
		public BackingFieldsStore.IBackingFieldInfo Source { get; set; }

		/// <summary>
		/// Gets or sets the converter.
		/// </summary>
		/// <value>The converter.</value>
		/// <autogeneratedoc />
		public IValueConverter Converter { get; set; } = DataTypeConverter.Default;

		/// <summary>
		/// Gets or sets the binding mode.
		/// </summary>
		/// <value>The binding mode.</value>
		public BindingMode Mode { get; set; } = BindingMode.Default;

		#region internal

		internal BackingFieldsStore.IBackingFieldInfo Target { get; set; }

		private string SourceChangedEventId { get; set; }
		private string TargetChangedEventId { get; set; }

		internal bool HasEvents => SourceChangedEventId != null || TargetChangedEventId != null;

		internal void Add(EventHandler<ValueChangedEventArgs> sourceChangedEvent,
			EventHandler<ValueChangedEventArgs> targetChangedEvent) {
			if (sourceChangedEvent != null) {
				SourceChangedEventId = Guid.NewGuid().ToString("B");
				Source.ValueChangedEvent.Register(null, SourceChangedEventId, sourceChangedEvent);
			}
			if (targetChangedEvent != null) {
				TargetChangedEventId = Guid.NewGuid().ToString("B");
				Target.ValueChangedEvent.Register(null, TargetChangedEventId, targetChangedEvent);
			}
		}

		internal void Release() {
			if (SourceChangedEventId != null) Source.ValueChangedEvent.Release(null, SourceChangedEventId);
			if (TargetChangedEventId != null) Target.ValueChangedEvent.Release(null, TargetChangedEventId);
		}

		#endregion
	}

}