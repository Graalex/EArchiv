using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Mariupolgaz.EArchiv.Common.Models;
using Mariupolgaz.EArchiv.Common.Servises;
using Microsoft.Practices.ServiceLocation;

namespace Mariupolgaz.EContract.Finder.ViewModels
{
	/// <summary>
	/// Модель вида для поиска договоров
	/// </summary>
	public class ContractFinderViewModel: BaseViewModel
	{
		private string _org;


		public ContractFinderViewModel()
		{
			// первоначальная иницилизация
			this.ContractVisibility = Visibility.Collapsed;
			this.ContragentVisibility = Visibility.Collapsed;
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
				}
			}
		}

		private Visibility _contragentVisibility;
		/// <summary>
		/// Определяет видимость элемента управления со списком контрагентов
		/// </summary>
		public Visibility ContragentVisibility
		{
			get { return _contragentVisibility; }
			set
			{
				if (_contragentVisibility != value) {
					_contragentVisibility = value;
					RaisePropertyChanged(() => ContragentVisibility);
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

		private Visibility _contractVisibility;
		/// <summary>
		/// Определяет видимость элемента управления со списком договоров контрагента
		/// </summary>
		public Visibility ContractVisibility
		{ 
			get { return _contractVisibility; }
			set {
				if(_contractVisibility != value) {
					RaisePropertyChanged(() => ContractVisibility);
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
			get { return new DelegateCommand(onFindContragents); }
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

					// отображаем элементы управления
					this.ContragentVisibility = Visibility.Visible;
					this.ContractVisibility = Visibility.Visible;
				}
				else {
					this.ContragentVisibility = Visibility.Collapsed;
					this.ContractVisibility = Visibility.Collapsed;
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

		}

		private bool canGetDocuments()
		{
			return this.CurrentContract != null ? true : false;
		}

		#endregion

		#endregion
	}
}
