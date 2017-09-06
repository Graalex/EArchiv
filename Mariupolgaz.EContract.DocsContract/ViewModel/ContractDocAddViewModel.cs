using System;
using System.Collections.ObjectModel;
using System.Windows;
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
			_evnAggr = eventAggr ?? throw new ArgumentNullException("evenAggr");
			var ds = ServiceLocator.Current.GetInstance<IDocumentService>();
			this.Kinds = new ObservableCollection<DocumentKind>(ds.GetKindsByClass(6));
		}

		#region Properties

		private string _file;
		/// <summary>
		/// Полный путь к файлу с изображением
		/// </summary>
		public string File
		{
			get { return _file; }
			set
			{
				if (_file != value) {
					_file = value;
					RaisePropertyChanged(() => File);
				}
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
			dlg.Multiselect = false;
			dlg.Filter = "Изображения (jpeg)|*.jpg;(tiff)|*.tif;(bmp)|*.bmp";
			bool? rslt = dlg.ShowDialog();
			if (rslt.GetValueOrDefault()) {
				this.File = dlg.FileName;
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
			if (!System.IO.File.Exists(this.File)) {
				MessageBox.Show("Файл не существует!", "Сообщение");
				return;
			}

			this.Close();

		}

		private bool canOk()
		{
			return (this.File != String.Empty && this.SelectedKind != null);
		}

		#endregion
	}
}
