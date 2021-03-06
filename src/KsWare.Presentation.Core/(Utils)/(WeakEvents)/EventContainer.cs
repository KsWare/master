using System;

namespace KsWare.Presentation {

	/// <summary> Internal container to hold the event handler or a weak reference to the event handler
	/// </summary>
	internal sealed class EventContainer {

		private EventHandle _eventHandle;
		private WeakReference _weakEventHandleWeakRef;

		/// <summary> Initializes a new instance of the <see cref="EventContainer"/> class.
		/// </summary>
		/// <param name="eventHandle">The weak event handle.</param>
		/// <param name="keepAlive">if set to <c>true</c> the <see cref="EventHandle"/> is kept alive.</param>
		public EventContainer(EventHandle eventHandle, bool keepAlive) {
			if (keepAlive) {
				_eventHandle = eventHandle;
			} else {
				_weakEventHandleWeakRef = new WeakReference(eventHandle);
			}
			KeepAlive = keepAlive;
		}

		/// <summary> Gets the weak event handle.
		/// </summary>
		/// <value>a reference to <see cref="EventHandle"/> or <c>null</c> if the <see cref="EventHandle"/> has been garbage collected or container disposed.</value>
		public EventHandle EventHandle => (_eventHandle ?? (_weakEventHandleWeakRef !=null ? (EventHandle) _weakEventHandleWeakRef.Target : null ) );

		/// <summary> Gets a value indicating whether the <see cref="EventHandle"/> is kept alive.
		/// </summary>
		/// <value><c>true</c> if a reference to <see cref="EventHandle"/> is holded by this instance; otherwise, <c>false</c> if a <see cref="WeakReference"/> to <see cref="EventHandle"/> ist used.</value>
		public bool KeepAlive { get; private set; }

		/// <summary> Disposes this container.
		/// </summary>
		/// <remarks>Does not dispose the stored <see cref="EventHandle"/>. Only the reference to it is removed.</remarks>
		public void Dispose() {
			_eventHandle = null;
			_weakEventHandleWeakRef = null;
		}

	}

}