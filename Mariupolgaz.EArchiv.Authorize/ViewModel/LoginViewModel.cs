using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Mariupolgaz.EArchiv.Common.Events;
using Mariupolgaz.EArchiv.Common.Models;
using Mariupolgaz.EArchiv.Common.Servises;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;

namespace Mariupolgaz.EArchiv.Security.ViewModel
{
	/// <summary>
	/// Модель для вида входа в систему
	/// </summary>
	public class LoginViewModel: BaseViewModel
	{
		private readonly IEventAggregator _eventAggr;
		private readonly ISecurityService _security;

		#region Конструктор
		/// <summary>
		/// Создает єкземпляр <see cref="LoginViewModel"/>
		/// </summary>
		/// <param name="eventAggregator">Экземпляр агрегатора событий</param>
		public LoginViewModel(IEventAggregator eventAggregator)
		{
			if (eventAggregator == null) throw new ArgumentNullException("eventAggregator");

			_eventAggr = eventAggregator;
			_security = ServiceLocator.Current.GetInstance<ISecurityService>();

			this.Users = new ObservableCollection<string>(_security.GetIdentities());
		}
		#endregion

		#region Свойства

		#region Users
		/// <summary>
		/// Список пользователей для входа
		/// </summary>
		public ObservableCollection<string> Users { get; private set; }
		#endregion

		#region LoginName
		/// <summary>
		/// Имя входа
		/// </summary>
		public string LoginName { get; set; }
		#endregion

		#region Password
		/// <summary>
		/// Пароль
		/// </summary>
		public string Password { get; set; }
		#endregion

		#endregion

		#region Команды

		#region Login
		/// <summary>
		/// Вход в систему
		/// </summary>
		public ICommand Login
		{
			get { return new DelegateCommand(onLogin, canLogin); }
		}

		private void onLogin()
		{
			Mouse.OverrideCursor = Cursors.Wait;
			try {
				
				if (_security.Login(this.LoginName, this.Password)) {
					_eventAggr
						.GetEvent<AuthenticateEvent>()
						.Publish(new AuthenticateMessage(
							this.LoginName,
							DateTime.Now,
							true
					));
				} else {
					_eventAggr
						.GetEvent<AuthenticateEvent>()
						.Publish(new AuthenticateMessage(
							this.LoginName,
							DateTime.Now,
							false
					));
				}
			}

			catch (Exception e) {
				MessageBox.Show(
					"Ошибка при входе пользователя в программу.\n" + e.Message,
					"Ошибка",
					MessageBoxButton.OK,
					MessageBoxImage.Error
				);
			}

			finally {
				Mouse.OverrideCursor = null;
			}
		}

		private bool canLogin()
		{
			return (this.LoginName != null && this.LoginName != String.Empty);
		}
		#endregion

		#endregion

	}
}
