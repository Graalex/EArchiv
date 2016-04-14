using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Mariupolgaz.EArchiv.Common.Models
{
	/// <summary>
	/// Представляет базовую функциональность уведомления при изменении свойства
	/// </summary>
	public abstract class NotificationObject : INotifyPropertyChanged
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		public NotificationObject()
		{
		}

		/// <summary>
		/// Происходит при изменении значения свойства
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged<T>(Expression<Func<T>> action)
		{
			var propertyName = GetPropertyName(action);
			RaisePropertyChanged(propertyName);
		}

		private static string GetPropertyName<T>(Expression<Func<T>> action)
		{
			var expression = (MemberExpression)action.Body;
			var propertyName = expression.Member.Name;
			return propertyName;
		}

		private void RaisePropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
				handler(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
