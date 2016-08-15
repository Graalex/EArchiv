using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
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
		private IContractFinderService _serv;

		/// <summary>
		/// 
		/// </summary>
		public ContractFinderViewModel(IEventAggregator aggregator)
		{
			if (aggregator == null) throw new ArgumentNullException("aggregator");

			_aggr = aggregator;	
			this.Contragents = new ObservableCollection<Contragent>();
			_org = ConfigurationManager.AppSettings["org"];
			_serv = ServiceLocator.Current.GetInstance<IContractFinderService>();
		}

		#region Properties

		private ICollectionView _contracts;
		/// <summary>
		/// Договора выбранного контрагента
		/// </summary>
		public ICollectionView Contracts
		{
			get {
				_initializeContracts();
				return _contracts;
			}
		}

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

		private string _ccode;
		/// <summary>
		/// ЕДРПОУ код контрагента
		/// </summary>
		public string ContragentCode
		{
			get { return _ccode; }
			set {
				if(_ccode != value) {
					_ccode = value;
					RaisePropertyChanged(() => ContragentCode);
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
					RaisePropertyChanged(() => Contracts);
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
				if (this.ContragentCode != null && this.ContragentCode != "") {
					var contrs = _serv.FindContragentsCode(_org, this.ContragentCode);

					if (contrs != null) {
						foreach (var item in contrs) {
							this.Contragents.Add(item);
						}
					}
					else {
						MessageBox.Show(
							"Контрагент не найден.",
							"Сообщение",
							MessageBoxButton.OK,
							MessageBoxImage.Information
						);
					}
				}

				else if (this.ContragentName != null && this.ContragentName != "") {
					var contrs = _serv.FindContragents(_org, this.ContragentName);

					if (contrs != null) {
						// контрагенты найдены, заполняем список
						foreach (var item in contrs) {
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
			}

			catch (Exception e) {
				MessageBox.Show(
					e.Message,
					"Ошибка",
					MessageBoxButton.OK,
					MessageBoxImage.Error
				);
			}

			finally {
				this.ContragentName = null;
				this.ContragentCode = null;
				Mouse.OverrideCursor = null;
			}

		}

		private bool canFindContragents()
		{
			return (this.ContragentName != null && this.ContragentName.Trim() != "") ||
							(this.ContragentCode != null && this.ContragentCode.Trim() != "");
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

		#region Helper Methods

		private void _initializeContracts()
		{
			if (_curContragent != null) {
				_contracts = CollectionViewSource.GetDefaultView(this.CurrentContragent.Contracts);
				//_contracts.GroupDescriptions.Clear();
				//_contracts.GroupDescriptions.Add(new PropertyGroupDescription("Parent"));
				// TODO: Настроить сортировку, группировку и фильтрацию

			}
		}

		#endregion
	}
}
