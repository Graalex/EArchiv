using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Interactivity;

namespace Mariupolgaz.EArchiv.Security.Views
{
	public class PropertyBehavior : Behavior<DependencyObject>
	{
		private PropertyInfo _propertyInfo;
		private EventInfo _eventInfo;
		private Delegate _handler;
		
		public string Property { get; set; }
		public string UpdateEvent { get; set; }

		public static readonly DependencyProperty BindingProperty =
			DependencyProperty.RegisterAttached(
				"Binding",
				typeof(object),
        typeof(PropertyBehavior),
				new FrameworkPropertyMetadata { BindsTwoWayByDefault = true }
			);
		public object Binding
		{
			get { return GetValue(BindingProperty); }
			set { SetValue(BindingProperty, value); }
		}

		protected override void OnAttached()
		{
			Type elementType = AssociatedObject.GetType();

			if(Property == null) {
				PresentationTraceSources.DependencyPropertySource.TraceData(
					TraceEventType.Error,
					1,
					"Целевое свойство не должно быть null."
				);
			}

			_propertyInfo = elementType.GetProperty(Property, BindingFlags.Instance | BindingFlags.Public);
			if(_propertyInfo == null) {
				PresentationTraceSources.DependencyPropertySource.TraceData(
					TraceEventType.Error,
					2,
					String.Format("Свойство \"{0}\" не найдено.", Property)
				);
			}

			if(UpdateEvent == null) {
				PresentationTraceSources.DependencyPropertySource.TraceData(
					TraceEventType.Error,
					3,
					"Событие обновления должно быть null."
				);
			}

			_eventInfo = elementType.GetEvent(UpdateEvent);
			if(_eventInfo == null) {
				PresentationTraceSources.MarkupSource.TraceData(
					TraceEventType.Error,
					4,
					String.Format("Событие \"{0}\" не найдено.", UpdateEvent)
				);
			}

			_handler = compileDelegate(_eventInfo, onEvent);
			_eventInfo.AddEventHandler(AssociatedObject, _handler);
		}

		protected override void OnDetaching()
		{
			if (_eventInfo == null) return;
			if (_handler == null) return;

			_eventInfo.RemoveEventHandler(AssociatedObject, _handler);
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			if (e.Property.Name != "Binding") return;
			if (AssociatedObject == null) return;
			if (_propertyInfo == null) return;

			object oldValue = _propertyInfo.GetValue(AssociatedObject, null);
			if (oldValue.Equals(e.NewValue)) return;

			if(_propertyInfo.CanWrite) {
				_propertyInfo.SetValue(AssociatedObject, e.NewValue, null);
			}

			base.OnPropertyChanged(e);
		}

		private static Delegate compileDelegate(EventInfo eventInfo, Action action)
		{
			ParameterExpression[] parameters =
			eventInfo
				.EventHandlerType
				.GetMethod("Invoke")
				.GetParameters()
				.Select(p => System.Linq.Expressions.Expression.Parameter(p.ParameterType))
				.ToArray();

			return System.Linq.Expressions.Expression.Lambda(
				eventInfo.EventHandlerType,
				System.Linq.Expressions.Expression.Call(
					System.Linq.Expressions.Expression.Constant(action),
					"Invoke",
					Type.EmptyTypes
				),
				parameters
			)
			.Compile();
    }

		private void onEvent()
		{
			if (AssociatedObject == null) return;
			if (_propertyInfo == null) return;

			Binding = _propertyInfo.GetValue(AssociatedObject, null);
		}
	}
}
