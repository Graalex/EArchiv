using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Mariupolgaz.EArchiv.Common.Events;
using Mariupolgaz.EArchiv.Common.Models;
using Mariupolgaz.EArchiv.Common.Servises;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;

namespace Mariupolgaz.EContract.Finder.ViewModels
{
	/// <summary>
	/// Модель вида для поиска договоров
	/// </summary>
	public class ContractFinderViewModel: BaseViewModel
	{
		private string _org;
		private readonly IEventAggregator _aggr;

		/// <summary>
		/// 
		/// </summary>
		public ContractFinderViewModel(IEventAggregator aggregator)
		{
			if (aggregator == null) throw new ArgumentNullException("aggregator");

			_aggr = aggregator;
			this.Contragents = new ObservableCollection<Contragent>();
			_org = ConfigurationManager.AppSettings["org"];
    }
		
		#region Properties

		private string _cname;
		/// <summary>
		/// Строка поиска по имени контрагента
		/// </summary>
		public string ContragentName
		{
			get { return _cname; }
			set {
				if(_cname != value) {
					_cname = value;
					RaisePropertyChanged(() => ContragentName);
				}
			}
		}

		/// <summary>
		/// Список найденных котрагентов
		/// </summary>
		public ObservableCollection<Contragent> Contragents { get; private set; }

		private Contragent _curContragent;
		/// <summary>
		/// Выбранный контрагент из списка
		/// </summary>
		public Contragent CurrentContragent
		{
			get { return _curContragent; }
			set {
				if(_curContragent != value) {
					_curContragent = value;
					RaisePropertyChanged(() => CurrentContragent);
					this.ContractIndex = 0;
				}
			}
		}

		private Contract _curContract;
		/// <summary>
		/// Выбранный договор контрагента
		/// </summary>
		public Contract CurrentContract
		{
			get { return _curContract; }
			set {
				if(_curContract != value) {
					_curContract = value;
					RaisePropertyChanged(() => CurrentContract);
				}
			}
		}

		private int _contrIdx = 0;
		/// <summary>
		/// 
		/// </summary>
		public int ContragentIndex
		{ 
			get { return _contrIdx; }
			set {
				if(_contrIdx != value) {
					_contrIdx = value;
					//_conIdx = 0;
					RaisePropertyChanged(() => ContragentIndex);
					//RaisePropertyChanged(() => ContractIndex);
				}
			}
		}

		private int _conIdx = 0;
		/// <summary>
		/// 
		/// </summary>
		public int ContractIndex
		{
			get { return _conIdx; }
			set
			{
				if (_conIdx != value) {
					_conIdx = value;
					RaisePropertyChanged(() => ContractIndex);
				}
			}
		}

		#endregion

		#region Commands

		#region FindContragents

		/// <summary>
		/// Получает список абонентов по строке поиска
		/// </summary>
		public ICommand FindContragents
		{
			get { return new DelegateCommand(onFindContragents, canFindContragents); }
		}

		private void onFindContragents()
		{
			Mouse.OverrideCursor = Cursors.Wait;

			try {
				this.Contragents.Clear();
				// ищем сервис поиска договоров в 1С и получаеи список контрагентов
				var contr = (ServiceLocator.Current.GetInstance<IContractFinderService>()).FindContragents(_org, this.ContragentName);
				this.ContragentName = null;

				if (contr != null) {
					// контрагенты найдены, заполняем список
					foreach (var item in contr) {
						this.Contragents.Add(item);
					}
					this.ContragentIndex = 0;
					this.ContractIndex = 0;
				}
				else {
					MessageBox.Show(
						"Контрагенты не найдены.",
						"Сообщение",
						MessageBoxButton.OK,
						MessageBoxImage.Information
					);
				}
			}

			catch(Exception e) {
				MessageBox.Show(
					e.Message,
					"Ошибка",
					MessageBoxButton.OK,
					MessageBoxImage.Error
				);
			}

			finally {
				Mouse.OverrideCursor = null;
			}

		}

		private bool canFindContragents()
		{
			return this.ContragentName != null && this.ContragentName.Trim() != "";
		}

		#endregion

		#region GetDocuments

		/// <summary>
		/// Получает список электронных копий связанных с договором
		/// </summary>
		public ICommand GetDocuments
		{
			get { return new DelegateCommand(onGetDocuments, canGetDocuments); }
		}

		private void onGetDocuments()
		{
			_aggr.GetEvent<ContractSelectEvent>().Publish(true);
			_aggr.GetEvent<ContractSelectedEvent>().Publish(this.CurrentContract);

		}

		private bool canGetDocuments()
		{
			return this.CurrentContract != null ? true : false;
		}

		#endregion

		#endregion
	}
}
