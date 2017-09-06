using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Mariupolgaz.EArchiv.Common.Events;
using Mariupolgaz.EArchiv.Common.Models;
using Mariupolgaz.EArchiv.Common.Servises;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;


namespace Mariupolgaz.EArchiv.Abonent.ViewModel
{
	public class AbonentViewModel: BaseViewModel
	{
		private readonly IEventAggregator _aggregator;
		private readonly IFinderService _finder;

		#region Конструктор

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="aggregator"></param>
		public AbonentViewModel(IEventAggregator aggregator )
		{
			_aggregator = aggregator ?? throw new ArgumentNullException("aggregator");
			_finder = ServiceLocator.Current.GetInstance<IFinderService>();

			this.Abonents = new ObservableCollection<Common.Models.Abonent>();
			this.Settlements = new ObservableCollection<Settlement>(_finder.GetSettlementList());
		}

		#endregion

		#region Properties

		#region LS
		private int? _ls;
		/// <summary>
		/// Лицевой счет
		/// </summary>
		public int? LS
		{ 
			get {
				return _ls;
			} 
			set {
				if(_ls != value) {
					_ls = value;
					RaisePropertyChanged(() => LS);
				}
			}
		}

		#endregion

		#region Family
		private string _family = String.Empty;
		/// <summary>
		/// Фамилия (или ее часть) абонента
		/// </summary>
		public string Family 
		{ 
			get {
				return _family;
			} 
			set {
				if(_family != value) {
					_family = value;
					RaisePropertyChanged(() => Family);
				}
			}
		}
		#endregion

		#region Settlements
		/// <summary>
		/// Список населленных пунктов
		/// </summary>
		public ObservableCollection<Settlement> Settlements { get; private set; }
		#endregion

		#region CurrentSettlement
		private Settlement _settl;
		/// <summary>
		/// 
		/// </summary>
		public Settlement CurrentSettlement
		{
			get { return _settl; }
			set {
				if (_settl != value) {
					_settl = value;
					RaisePropertyChanged(() => CurrentSettlement);
        }
			}
		}
		#endregion


		#region CurrentStreet
		private string _street;
		/// <summary>
		/// Выбранная улица
		/// </summary>
		public string CurrentStreet
		{
			get { return _street; }
			set {
				if(_street != value) {
					_street = value;
					RaisePropertyChanged(() => CurrentStreet);
				}
			}
		}
		#endregion

		#region CurrentHouse
		private string _house;
		/// <summary>
		/// Текущий номер дома
		/// </summary>
		public string CurrentHouse
		{
			get { return _house; }
			set {
				if(_house != value) {
					_house = value;
					RaisePropertyChanged(() => CurrentHouse);
				}
			}
		}
		#endregion

		#region CurrentAppartment
		private int? _appart;
		/// <summary>
		/// Номер квартиры
		/// </summary>
		public int? CurrentAppartment
		{
			get { return _appart; }
			set {
				if(_appart != value) {
					_appart = value;
					RaisePropertyChanged(() => CurrentAppartment);
				}
			}
		}
		#endregion

		#region ResultVisible
		private Visibility _resultVisible = Visibility.Collapsed;
		/// <summary>
		/// Определяет видимость панели результатов поиска абонентов
		/// </summary>
		public Visibility ResultVisible
		{
			get {
				return _resultVisible;
			}
			set {
				if(_resultVisible != value) {
					_resultVisible = value;
					RaisePropertyChanged(() => ResultVisible);
				}
			}
		}
		#endregion

		#region Abonents
		/// <summary>
		/// Список найденных абонентов
		/// </summary>
		public ObservableCollection<Common.Models.Abonent> Abonents { get; private set; }
		#endregion

		#region SelectedAbonent
		private Common.Models.Abonent _selAbonent;
		/// <summary>
		/// Выбранный абонент
		/// </summary>
		public Common.Models.Abonent SelectedAbonent
		{
			get { return _selAbonent; }
			set {
				if(_selAbonent != value) {
					_selAbonent = value;
					RaisePropertyChanged(() => SelectedAbonent);
				}
			}
		}
		#endregion

		#region AbonentVisible
		private Visibility _abnVisible = Visibility.Collapsed;
		/// <summary>
		/// Определяет видимость пенели с информацией об выбранном абоненте
		/// </summary>
		public Visibility AbonentVisible
		{
			get { return _abnVisible; }
			set {
				if(_abnVisible != value) {
					_abnVisible = value;
					RaisePropertyChanged(() => AbonentVisible);
				}
			}
		}
		#endregion

		#endregion

		#region Commands

		#region FindAbonents
		/// <summary>
		/// Поиск абонентов
		/// </summary>
		public ICommand FindAbonents
		{
			get { return new DelegateCommand(onFindAbonents, canFindAbonents); }
		}

		private void onFindAbonents()
		{
			Mouse.OverrideCursor = Cursors.Wait;

			//var finder = ServiceLocator.Current.GetInstance<IFinderService>();

			
			try {

				if (this.LS != null) {
					// find for abonent
					var rslt = _finder.FindAbonent(this.LS.GetValueOrDefault(0));
					if (rslt != null) {
						clearCurrentAbonent();
						this.Abonents.Add(rslt);
						this.ResultVisible = Visibility.Visible;
						this.AbonentVisible = Visibility.Visible;
					}
					else {
						MessageBox.Show(
							"Не найден абонент с таким номером лицевого счета.", 
							"Сообщение", 
							MessageBoxButton.OK, 
							MessageBoxImage.Information
						);
					}
				}
				else if (this.Family != null && this.Family != "") {
					var rslts = _finder.FindAbonents(this.Family);
					if (rslts != null) {
						clearCurrentAbonent();
						foreach (var item in rslts) {
							this.Abonents.Add(item);
						}
						this.ResultVisible = Visibility.Visible;
						this.AbonentVisible = Visibility.Visible;
					}
					else {
						MessageBox.Show(
							"Не найдены абоненты с такими фамилиями.", 
							"Сообщение", 
							MessageBoxButton.OK, 
							MessageBoxImage.Information
						);
					}
				}
				else if (
					this.CurrentSettlement != null ||
					this.CurrentHouse != null ||
					this.CurrentHouse != "" ||
					this.CurrentStreet != null ||
					this.CurrentStreet != ""
				) {
					string settl = null;
					if (this.CurrentSettlement != null)
					{
						settl = this.CurrentSettlement.Name;
					}

					var rslt = _finder.FindAbonents(
						this.Family, settl, 
						this.CurrentStreet, 
						this.CurrentHouse, 
						this.CurrentAppartment
					);
					
					if (rslt != null) {
						clearCurrentAbonent();
						foreach (var item in rslt) {
							this.Abonents.Add(item);
						}
						this.ResultVisible = Visibility.Visible;
						this.AbonentVisible = Visibility.Visible;
					}
					else {
						MessageBox.Show(
							"Не найдены абоненты с такими условиями поиска.", 
							"Сообщение", 
							MessageBoxButton.OK, 
							MessageBoxImage.Information
						);
					}
				}
			 
			}

			catch (Exception e) {
				MessageBox.Show(
					e.Message + "\n" + e.InnerException.Message, 
					"Ошибка", 
					MessageBoxButton.OK, 
					MessageBoxImage.Information
				); ;
			}

			finally {
				Mouse.OverrideCursor = null;
			}
		}

		private bool canFindAbonents()
		{
			if(
				(this.LS != null && this.LS > 0) || 
				this.Family != null && this.Family != "" || 
				this.CurrentSettlement != null || 
				this.CurrentStreet != null || 
				this.CurrentHouse != null
			) {
				return true;
			} else {
				return false;
			}
		}
		#endregion

		#region GetDocuments
		/// <summary>
		/// Работа с документами абонента
		/// </summary>
		public ICommand GetDocuments
		{
			get { return new DelegateCommand(onGetDocuments, canGetDocuments); }
		}

		private void onGetDocuments()
		{
			_aggregator
				.GetEvent<LsSelectedEvent>()
				.Publish(this.SelectedAbonent.LS);
		}

		private bool canGetDocuments()
		{
			return this.SelectedAbonent != null ? true : false;
		}
		#endregion

		#endregion

		#region Helpers

		private void clearCurrentAbonent()
		{
			this.SelectedAbonent = null;
			this.LS = null;
			this.Family = null;
			this.CurrentSettlement = null;
			this.CurrentStreet = null;
			this.CurrentHouse = null;
			this.CurrentAppartment = null;
			this.Abonents.Clear();
		}

		#endregion
	}
}
