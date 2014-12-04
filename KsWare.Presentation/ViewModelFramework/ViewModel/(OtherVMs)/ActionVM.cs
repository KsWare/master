﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Input;
using JetBrains.Annotations;
using KsWare.Presentation.BusinessFramework;
using KsWare.Presentation.Core.Providers;
using KsWare.Presentation.ViewModelFramework.Providers;
using KsWare.Presentation.Core;

namespace KsWare.Presentation.ViewModelFramework {

	/// <summary> Provides a viewmodel for an action 
	/// </summary>
	/// <remarks></remarks>
	/// <seealso cref="ActionActiveProgressVM"/>
	/// <remarks>
	/// <para>Naming convention for the property: <c>NameAction</c> (e.g. the action prints everything <c>PrintAction</c>)</para>
	/// </remarks>
	public class ActionVM:ObjectVM,ICommand { //REVISE: rename to CommandVM?
	
		private static Dictionary<string, RoutedCommand> s_RoutedCommands;

		private bool m_IsActive;
		private RoutedCommand m_RoutedCommand;

		/// <summary> Initializes a new instance of the <see cref="ActionVM"/> class.
		/// </summary>
		/// <remarks></remarks>
		public ActionVM() {
			ContextMenu = RegisterChild(()=>ContextMenu, new ContextMenuVM());
		}

		/// <summary> Gets or sets the action metadata.
		/// </summary>
		/// <value>The action metadata.</value>
		/// <remarks></remarks>
		public new ActionMetadata Metadata {get {return (ActionMetadata) base.Metadata;}set{base.Metadata=value;}}

		/// <summary> Creates the default metadata.
		/// </summary>
		/// <returns></returns>
		/// <remarks></remarks>
		protected override ViewModelMetadata CreateDefaultMetadata() {
			return new ActionMetadata{Reflection=this.Reflection};
		}

		/// <summary> Raises the <see cref="ObjectVM.MetadataChanged"/>-event.<br/>
		/// For derived classes: Called when Metadata-Property has been changed.
		/// </summary>
		/// <remarks></remarks>
		protected override void OnMetadataChanged(ValueChangedEventArgs<ViewModelMetadata> e) {
			base.OnMetadataChanged(e);

			EventUtil.ReleaseAll<IActionProvider>(this,"{467DC56E-CF44-48D5-9AE1-38809733F587}",
				obj => obj.CanExecuteChanged-=AtActionProviderCanExecuteChanged);
			
			if(Metadata==null) return;
			Metadata.ActionProviderChanged += AtActionProviderChanged;
			if (Metadata.HasActionProvider) AtActionProviderChanged(Metadata, EventArgs.Empty);
		}

		private void AtActionProviderChanged(object sender, EventArgs e) {
			EventUtil.Register(this,"{467DC56E-CF44-48D5-9AE1-38809733F587}",Metadata.ActionProvider,
				obj => obj.CanExecuteChanged+=AtActionProviderCanExecuteChanged);
			OnActionProviderChanged();
		}

		protected virtual void OnActionProviderChanged() {
			
		}

		/// <summary> Handles the ActionProvider CanExecuteChanged event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="eventArgs">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		/// <remarks></remarks>
		private void AtActionProviderCanExecuteChanged(object sender, EventArgs eventArgs) { 
			OnPropertyChanged("CanExecute");
			EventUtil.Raise(CanExecuteChanged,this,EventArgs.Empty,"{E372D3E9-878A-4F7D-8B17-57F4300E7442}");
			EventUtil.WeakEventManager.Raise<EventHandler>(LazyWeakEventProperties,"CanExecuteChangedEvent",EventArgs.Empty);
		}

		public ContextMenuVM ContextMenu { get; private set; }

		/// <summary> Executes the action.
		/// </summary>
		/// <param name="parameter">The parameter for the action.</param>
		/// <remarks></remarks>
		[UsedImplicitly]
		public void Execute(object parameter) { Metadata.ActionProvider.Execute(parameter); }

		/// <summary> Executes this action.
		/// </summary>
		public void Execute() { Metadata.ActionProvider.Execute(null); }

		bool ICommand.CanExecute(object parameter) {return Metadata.ActionProvider.CanExecute;}

		/// <summary> Occurs when <see cref="CanExecute"/> has been changed.
		/// </summary>
		public event EventHandler CanExecuteChanged;

		/// <summary> Gets a value indicating whether this instance can execute.
		/// </summary>
		/// <remarks></remarks>
		public bool CanExecute {get {return Metadata.ActionProvider.CanExecute;}}

		/// <summary> Gets the event source for the event which occurs when <see cref="CanExecute"/> has been changed.
		/// </summary>
		/// <value>The event source.</value>
		public IWeakEventSource<EventHandler> CanExecuteChangedEvent { get { return WeakEventProperties.Get<EventHandler>("CanExecuteChangedEvent"); }}

		public IObservable<bool> CanExecuteObservable {get {return Metadata.ActionProvider.CanExecuteObservable;}}

			/// <summary> Sets <see cref="CanExecute"/>.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <param name="value">if set to <c>true</c> [value].</param>
		/// <remarks></remarks>
		public void SetCanExecute(object token, bool value) {
			Metadata.ActionProvider.SetCanExecute(token, value);
		}

		/// <summary> Alias to Metadata.ActionProvider.ExecutedCallback </summary>
		public EventHandler<ExecutedEventArgs> MːExecutedCallback{set { Metadata.ActionProvider.ExecutedCallback = value; }}

		/// <summary> Sets the Metadata.ActionProvider.ExecutedCallback. Excptions are forwarded using <see cref="IObjectVM.RequestUserFeedback"/>
		/// </summary>
		/// <value>A <see cref="Action"/></value>
		/// <remarks>
		/// <para>Naming convention for handlers: <c>DoActionName</c> (e.g. <c>StartAction</c> => <c>DoStart</c>)</para>
		/// <para>NOTE: If a debugger is attached, exceptions are not catched and forwarded.</para></remarks>
		public Action MːDoAction {
			set {
				Metadata.ActionProvider.CatchExceptionsAndRequestUserFeedback = true;
				Metadata.ActionProvider.ExecutedCallback = (s, e) => value();
			}
		}

		/// <summary> Sets the Metadata.ActionProvider.ExecutedCallback. Excptions are forwarded using <see cref="IObjectVM.RequestUserFeedback"/>
		/// </summary>
		/// <value>A <see cref="Action{T}"/></value>
		/// <remarks>NOTE: If a debugger is attached, the exceptions are not catched and forwarded.</remarks>
		public Action<object> MːDoActionP {
			set {
				Metadata.ActionProvider.CatchExceptionsAndRequestUserFeedback = true;
				Metadata.ActionProvider.ExecutedCallback = (s, e) => value(e.Parameter);
			}
		}
	
		/// <summary> Supports ToogleButton.IsChecked, MenuEvent.IsChecked
		/// </summary>
		public bool IsActive {
			get { return m_IsActive; }
			set {
				if (Equals(m_IsActive, value)) return;
				m_IsActive = value;
				OnPropertyChanged("IsActive");
			}
		}

		/// <summary> Supports MenuEvent.InputGestureText
		/// </summary>
		public string InputGestureText {get { return Fields.Get(()=>InputGestureText); }set { Fields.Set(()=>InputGestureText, value); }}

		public EventHandler<CanExecuteEventArgs> MːCanExecuteCallback {set { Metadata.ActionProvider.CanExecuteCallback = value; }}

		protected override void OnDataChanged(DataChangedEventArgs e) {
			base.OnDataChanged(e);

			if (e.NewData is ActionBM) {
				BusinessActionProvider p;
				if (!Metadata.HasActionProvider) {
					Metadata.ActionProvider=p=new BusinessActionProvider();
				} else if (Metadata.ActionProvider is BusinessActionProvider) {
					p=(BusinessActionProvider) Metadata.ActionProvider;
				} else {
					throw new InvalidOperationException("The current ActionProvider does not support business action! "+
						"\n\t"+"Parent Type: " + DebugUtil.FormatTypeFullName(Parent) + "."+MemberName +
						"\n\t"+"MemberPath: " + this.MemberPath +
						"\n\t"+"Current Provider: " + DebugUtil.FormatTypeFullName(Metadata.ActionProvider)+
						"\n\t"+"Solution 1: "+ "specify manually => MyAction=RegisterChild(_=>MyAction, new ActionVM {Metadata = {ActionProvider = new BusinessActionProvider()}});"+
						"\n\t"+"Solution 2: "+ "use attribute for the property => [ActionMetadata(ActionProvider = typeof(BusinessActionProvider))]"+
						"\n\t"+"ErrorID: {E0899036-CFFC-4791-A75A-4BF7F7BDA64A}");
				}
				p.BusinessObject = e.NewData;
			}
		}

		#region DRAFT RoutedCommands support 

		public bool HasRoutedCommand{get { return m_RoutedCommand!=null; }}

		public RoutedCommand RoutedCommand {
			get {
				if(m_RoutedCommand==null) {
					var key = Parent.GetType().FullName+"."+MemberName;
					if(s_RoutedCommands==null) {
						s_RoutedCommands = new Dictionary<string, RoutedCommand>();
						if(!s_RoutedCommands.ContainsKey(key)) {
							m_RoutedCommand=new RoutedCommand(MemberName, Parent.GetType());
							s_RoutedCommands.Add(key,m_RoutedCommand);
						} else {
							m_RoutedCommand = s_RoutedCommands[key];
						}
					}
					m_RoutedCommand=new RoutedCommand(MemberName, Parent.GetType());
				}
				return m_RoutedCommand;
			}
		}

		public CommandBinding CreateCommandBinding() {
			var commandBinding = new CommandBinding(RoutedCommand,ExecuteCommand,CheckCommand);
			return commandBinding;
		}

		public void ReleaseCommandBinding(CommandBinding commandBinding) {
			if(commandBinding.Command!=RoutedCommand) {/*uups*/return;}
			commandBinding.Executed  -=ExecuteCommand;
			commandBinding.CanExecute-=CheckCommand;
		}

		public bool IsRoutedCommand { get; set; }

		private void ExecuteCommand(object sender, ExecutedRoutedEventArgs e){
			Execute(e.Parameter);
		}

		private void CheckCommand(object sender, CanExecuteRoutedEventArgs e) {
			e.CanExecute = CanExecute;
		}

		#endregion

		public static ActionVM CreateMenuEntry(string header, string icon, object tag, EventHandler<ExecutedEventArgs> executeHandler) {
			var vm = new ActionVM {UI = {MenuItem = {Header = header,Icon=icon},Tag = tag},MːExecutedCallback = executeHandler};
			return vm;
		}

	}

}