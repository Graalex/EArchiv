using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Mariupolgaz.EArchiv.Common.Events;
using Mariupolgaz.EArchiv.Common.Models;
using Microsoft.Practices.Prism.Events;

namespace Mariupolgaz.EArchiv
{
	/// <summary>
	/// Определяет модель вида главного окна приложения
	/// </summary>
	public class ShellViewModel: BaseViewModel
	{
		private readonly IEventAggregator _eventAggregator;

		/// <summary>
		/// Создает экземпляр <see cref="ShellViewModel"/>
		/// </summary>
		/// <param name="eventAggregator"></param>
		public ShellViewModel(IEventAggregator eventAggregator )
		{
			if (eventAggregator == null) throw new ArgumentNullException("eventAggregator");

			_eventAggregator = eventAggregator;

			this.AbonentPanelVisible = Visibility.Collapsed;
			this.LoginPanelVisible = Visibility.Visible;
			this.IsAuthenticate = false;

			_eventAggregator.GetEvent<AuthenticateEvent>().Subscribe(authenticate);
		}

		#region Свойства

		#region LoginPanelVisible
		private Visibility _loginVisible;
		/// <summary>
		/// Видимость панели для входа пользователя в систему
		/// </summary>
		public Visibility LoginPanelVisible
		{
			get { return _loginVisible; }
			set {
				if(_loginVisible != value) {
					_loginVisible = value;
					RaisePropertyChanged(() => LoginPanelVisible);
				}
			}
		}
		#endregion

		#region AbonentPanelVisible
		private Visibility _abonentVisible;
		/// <summary>
		/// Видимость для панели абонентов
		/// </summary>
		public Visibility AbonentPanelVisible
		{
			get { return _abonentVisible; }
			set {
				if(_abonentVisible != value) {
					_abonentVisible = value;
					RaisePropertyChanged(() => AbonentPanelVisible);
				}
			}
		}
		#endregion

		#region IsAuthenticate
		private bool _isAuthenticate;
		/// <summary>
		/// Устанавливает прошел ли пользователь аутентификацию
		/// </summary>
		public bool IsAuthenticate
		{
			get { return _isAuthenticate; }
			set {
				if(_isAuthenticate != value) {
					_isAuthenticate = value;
					RaisePropertyChanged(() => IsAuthenticate);
				}
			}
		}
		#endregion

		#endregion

		#region Helpers

		private void authenticate(AuthenticateMessage msg)
		{
			if(msg.IsAuthenticate) {
				this.LoginPanelVisible = Visibility.Collapsed;
				this.AbonentPanelVisible = Visibility.Visible;
				this.IsAuthenticate = true;
			} else {
				this.LoginPanelVisible = Visibility.Visible;
				this.AbonentPanelVisible = Visibility.Collapsed;
				this.IsAuthenticate = false;
			}
		}

		#endregion
	}
}
