using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Mariupolgaz.EArchiv.Common;
using Mariupolgaz.EArchiv.Common.Events;
using Mariupolgaz.EArchiv.Common.Models;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace EContract
{
	/// <summary>
	/// Модель вида для оболочки
	/// </summary>
	public class ShellViewModel: BaseViewModel
	{
		private readonly IRegionManager _manager;
		private readonly IEventAggregator _aggregator;
		private readonly IUnityContainer _container;

		/// <summary>
		/// Создает  экземпляр <see cref="ShellViewModel"/>
		/// </summary>
		/// <param name="manager">Экземпляр <see cref="IRegionManager"/></param>
		/// <param name="aggregator">Экземпляр <see cref="IEventAggregator"/></param>
		/// <param name="container">Экземпляр <see cref="IUnityContainer"/></param>
		public ShellViewModel(IRegionManager manager, IEventAggregator aggregator, IUnityContainer container)
		{
			if (manager == null) throw new ArgumentNullException("manager");
			if (aggregator == null) throw new ArgumentNullException("aggregator");
			if (container == null) throw new ArgumentNullException("container");

			_manager = manager;
			_aggregator = aggregator;
			_container = container;

			_aggregator.GetEvent<AuthenticateEvent>().Subscribe(authenticated);
		}

		#region Properties

		private string _user;
		/// <summary>
		/// Пользователь
		/// </summary>
		public string User
		{
			get {
				if (_user != null && _user != "")
					return "Работает: " + _user;
				else
					return "";
			}
			set {
				if(_user != value) {
					_user = value;
					RaisePropertyChanged(() => User);
				}
			}
		}

		#endregion

		#region Helpers

		private void authenticated(AuthenticateMessage msg)
		{
			if(msg.IsAuthenticate) {
				this.User = msg.LoginName;
				_manager.RequestNavigate(RegionNames.ContentRegion, new Uri(ViewNames.ContractFinder, UriKind.Relative));
				_manager.RequestNavigate(RegionNames.LeftRegion, new Uri(ViewNames.ModernFinder, UriKind.Relative));
			}

		}

		#endregion
	}
}
