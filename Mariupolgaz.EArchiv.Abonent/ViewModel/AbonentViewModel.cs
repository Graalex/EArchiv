using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Mariupolgaz.EArchiv.Common.Models;
using Mariupolgaz.EArchiv.Common.Servises;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Prism.Events;
using Mariupolgaz.EArchiv.Common.Events;

namespace Mariupolgaz.EArchiv.Abonent.ViewModel
{
	public class AbonentViewModel: BaseViewModel
	{
		private readonly IEventAggregator _aggregator;
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="aggregator"></param>
		public AbonentViewModel(IEventAggregator aggregator )
		{
			if (aggregator == null) throw new ArgumentNullException("aggregator");

			_aggregator = aggregator;
			this.Abonents = new ObservableCollection<Common.Models.Abonent>();
		}

		#region Properties

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

		private string _family;
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

		private Visibility _resultVisible = Visibility.Visible;
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

		/// <summary>
		/// Список найденных абонентов
		/// </summary>
		public ObservableCollection<Common.Models.Abonent> Abonents { get; private set; }

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

		private Visibility _abnVisible = Visibility.Visible;
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

		#region Commands

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

			var finder = ServiceLocator.Current.GetInstance<IFinderService>();

			try {
				if (this.LS != null) {
					// find for abonent
					var rslt = finder.FindAbonent(this.LS.GetValueOrDefault(0));
					if (rslt != null) {
						clearCurrentAbonent();
						this.Abonents.Add(rslt);
					} else { 
						MessageBox.Show("Не найден абонент с таким номером лицевого счета.", "Сообщение");
					}
				}
				else {
					var rslts = finder.FindAbonents(this.Family);
					if(rslts != null) {
						clearCurrentAbonent();
						foreach (var item in rslts) {
							this.Abonents.Add(item);
						}
					} else {
						MessageBox.Show("Не найдены абоненты с такими фамилиями.", "Сообщение");
					}					
				}
			}

			catch (Exception e) {
				MessageBox.Show(e.Message + "\n" + e.InnerException.Message, "Ошибка"); ;
			}

			finally {
				Mouse.OverrideCursor = null;
			}
		}

		private bool canFindAbonents()
		{
			if((this.LS != null && this.LS > 0) || this.Family != null && this.Family != String.Empty) {
				return true;
			} else {
				return false;
			}
		}

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

		#region Helpers

		private void clearCurrentAbonent()
		{
			this.SelectedAbonent = null;
			this.LS = null;
			this.Family = null;
			this.Abonents.Clear();
		}

		#endregion

	}
}
