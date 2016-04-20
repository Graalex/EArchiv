using System;
using System.Windows;
using System.Windows.Input;
using Mariupolgaz.EArchiv.Common.Events;
using Mariupolgaz.EArchiv.Common.Models;
using Mariupolgaz.EArchiv.Common.Servises;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;

namespace Mariupolgaz.EArchiv.Security.ViewModel
{
	/// <summary>
	/// Модель для визуального компонента регистрации пользователей
	/// </summary>
	public class RegisteredViewModel: BaseViewModel
	{
		private readonly IEventAggregator _eventAggr;

		#region Конструктор
		
		/// <summary>
		/// Создает экземпляр <see cref="RegisteredViewModel"/>
		/// </summary>
		/// <param name="eventAggregator">Экземпляр агрегатора событий</param>
		public RegisteredViewModel(IEventAggregator eventAggregator)
		{
			if (eventAggregator == null) throw new ArgumentNullException("eventAggregator");

			_eventAggr = eventAggregator;
		}

		#endregion

		#region Свойства

		#region LoginName
		private string _login;
		/// <summary>
		/// Имя входа
		/// </summary>
		public string LoginName 
		{ 
			get { return _login; } 
			set {
				if(_login != value) {
					_login = value;
					RaisePropertyChanged(() => LoginName);
				}
			}
		}
		#endregion

		#region Password
		private string _pwd;
		/// <summary>
		/// Пароль
		/// </summary>
		public string Password
		{ 
			get { return _pwd; } 
			set {
				if(_pwd != value) {
					_pwd = value;
					RaisePropertyChanged(() => Password);
				}
			}
		}
		#endregion

		#region RepeatPassword
		private string _rpwd;
		/// <summary>
		/// Повтор пароля
		/// </summary>
		public string RepeatPassword
		{ 
			get { return _rpwd; } 
			set {
				if(_rpwd != value) {
					_rpwd = value;
					RaisePropertyChanged(() => RepeatPassword);
				}
			}
		}
		#endregion

		#region IsActivity
		private bool? _activity = false;
		/// <summary>
		/// Флаг активации нового пользователя
		/// </summary>
		public bool? IsActivity
		{
			get { return _activity; }
			set {
				if(_activity != value) {
					_activity = value;
					RaisePropertyChanged(() => IsActivity);
				}
			}
		}
		#endregion

		#region RegisterMessage
		private string _msg;
		/// <summary>
		/// Текст сообщения о результате регистрации или ошибки
		/// </summary>
		public string RegisterMessage
		{
			get { return _msg; }
			set {
				if(_msg != value) {
					_msg = value;
					RaisePropertyChanged(() => RegisterMessage);
				}
			}
		}
		#endregion

		#region InfoPanelVisible
		private Visibility _pvisible = Visibility.Collapsed;
		/// <summary>
		/// Определяет видимость панели с информацией о регистрации
		/// </summary>
		public Visibility InfoPanelVisible
		{
			get { return _pvisible; }
			set {
				if(_pvisible != value) {
					_pvisible = value;
					RaisePropertyChanged(() => InfoPanelVisible);
				}
			}
		}
		#endregion

		#endregion

		#region Команды

		#region RegistryUser
		/// <summary>
		/// Команда регистрации нового пользователя
		/// </summary>
		public ICommand RegistryUser
		{
			get { return new DelegateCommand(onRegister, canRegister); }
		}

		private void onRegister()
		{
			Mouse.OverrideCursor = Cursors.Wait;
			try {
				var srv = ServiceLocator.Current.GetInstance<IRegisteredService>();
				if (srv.RegisteredUser(this.LoginName, this.Password, this.IsActivity.GetValueOrDefault(false))) {
					this._eventAggr
						.GetEvent<RegistryIdentityEvent>()
						.Publish(new IdentityMessage(
							this.LoginName,
							this.IsActivity.GetValueOrDefault(false),
							DateTime.Now
						));
					this.RegisterMessage = "Пользователь " + this.LoginName + "успешно зарегестрирован!";
				} else {
					this.RegisterMessage = "Пользователь " + this.LoginName + "не зарегестрирован!";
				}
				this.InfoPanelVisible = Visibility.Visible;
			}

			catch(Exception e) {
				this.RegisterMessage = 
					"Произошла ошибка при регистрации пользователя "
					+ this.LoginName +
					"!\n"
					+ e.Message;
				this.InfoPanelVisible = Visibility.Visible;
			}

			finally {
				Mouse.OverrideCursor = null;
			}
		}

		private bool canRegister()
		{
			if (
				(this.LoginName != null && this.LoginName != String.Empty) &&
				(this.Password != null && this.Password != String.Empty) &&
				(this.Password == this.RepeatPassword)
			)
				return true;
			else
				return false;
		}
		#endregion

		#region CancelRegistry
		/// <summary>
		/// Команда отмены регистрации
		/// </summary>
		public ICommand CancelRegistry
		{
			get { return new DelegateCommand(onCancel); }
		}

		private void onCancel()
		{
			this.LoginName = null;
			this.Password = null;
			this.RepeatPassword = null;
			this.IsActivity = null;
			this.RegisterMessage = null;
			this.InfoPanelVisible = Visibility.Collapsed;
		}
		#endregion

		#endregion
	}
}
