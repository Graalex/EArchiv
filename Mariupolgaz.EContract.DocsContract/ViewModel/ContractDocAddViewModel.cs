using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Linq;
using System.Windows.Input;
using Mariupolgaz.EArchiv.Common.Models;
using Mariupolgaz.EArchiv.Common.Servises;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Win32;

namespace Mariupolgaz.EContract.DocsContract.ViewModel
{
	/// <summary>
	/// 
	/// </summary>
	public class ContractDocAddViewModel : BaseViewModel
	{
		private IEventAggregator _evnAggr;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="eventAggr">Агрегатор событий</param>
		public ContractDocAddViewModel(IEventAggregator eventAggr)
		{
			if (eventAggr == null) throw new ArgumentNullException("evenAggr");

			_evnAggr = eventAggr;
			var ds = ServiceLocator.Current.GetInstance<IDocumentService>();
			this.Kinds = new ObservableCollection<DocumentKind>(ds.GetKindsByClass(6));
			this.Files = new List<string>();
      this.DocumentDate = DateTime.Today;
    }

		#region Properties

		/// <summary>
		/// Список полных путей выбранных файлов изображений
		/// </summary>
		public IList<string> Files { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public string Paths
		{
			get {
				string rslt = "";
				foreach(string item in this.Files) {
					rslt += item + ";";
				}

				return rslt;
			}
		}
				
		private DocumentKind _kind;
		/// <summary>
		/// Выбранный тип документа
		/// </summary>
		public DocumentKind SelectedKind
		{
			get { return _kind; }
			set
			{
				if (_kind != value) {
					_kind = value;
					RaisePropertyChanged(() => SelectedKind);
				}
			}
		}

		private DateTime _docDate;
		/// <summary>
		/// Дата документа
		/// </summary>
		public DateTime DocumentDate
		{
			get { return _docDate; }
			set {
				if(_docDate != value) {
					_docDate = value;
					RaisePropertyChanged(() => DocumentDate);
				}
			}
		}

		private string _docNumb;
		/// <summary>
		/// Номер документа
		/// </summary>
		public string DocumentNumber
		{
			get { return _docNumb; }
			set {
				if(_docNumb != value) {
					_docNumb = value;
					RaisePropertyChanged(() => DocumentNumber);
				}
			}
		}

		/// <summary>
		/// Список типов документов
		/// </summary>
		public ObservableCollection<DocumentKind> Kinds { get; private set; }

		#endregion

		/// <summary>
		/// 
		/// </summary>
		public Action Close { get; set; }

		#region Commands

		/// <summary>
		/// Выбор файла с изображением
		/// </summary>
		public ICommand AddFile
		{
			get { return new DelegateCommand(onAddFile); }
		}

		private void onAddFile()
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Multiselect = true;
			dlg.Filter = "Изображения (jpeg)|*.jpg;(tiff)|*.tif;(bmp)|*.bmp";
			bool? rslt = dlg.ShowDialog();
			if (rslt.GetValueOrDefault()) {
				this.Files = dlg.FileNames;
				RaisePropertyChanged(() => Paths);
			}
		}

		/// <summary>
		/// Команда закрітия диалога
		/// </summary>
		public ICommand Ok
		{
			get { return new DelegateCommand(onOk, canOk); }
		}

		private void onOk()
		{
			this.Close();

		}

		private bool canOk()
		{
			return (this.Files.Count > 0  && this.SelectedKind != null);
		}

		#endregion
	}
}
