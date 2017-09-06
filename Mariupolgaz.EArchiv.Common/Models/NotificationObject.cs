using System;
using System.ComponentModel;
using System.Linq.Expressions;

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

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="action"></param>
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
