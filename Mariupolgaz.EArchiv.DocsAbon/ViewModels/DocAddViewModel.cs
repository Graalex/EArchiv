using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Mariupolgaz.EArchiv.Common.Events;
using Mariupolgaz.EArchiv.Common.Models;
using Mariupolgaz.EArchiv.Common.Servises;
using Mariupolgaz.EArchiv.DocsAbon.Views;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Win32;

namespace Mariupolgaz.EArchiv.DocsAbon.ViewModels
{
	public class DocAddViewModel: BaseViewModel
	{
		private IEventAggregator _evnAggr;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="eventAggr">Агрегатор событий</param>
		public DocAddViewModel(IEventAggregator eventAggr)
		{
			if (eventAggr == null) throw new ArgumentNullException("evenAggr");

			_evnAggr = eventAggr;
			var ds = ServiceLocator.Current.GetInstance<IDocumentService>();
			this.Kinds = new ObservableCollection<DocumentKind>(ds.GetKindsByClass(2));
			
			/*
			this.Kinds = new ObservableCollection<DocumentKind>();
			this.Kinds.Add(new DocumentKind(1, "Паспорт", false));
			this.Kinds.Add(new DocumentKind(2, "ИНН", false));
			this.Kinds.Add(new DocumentKind(3, "Право на земельный участок", false));
			*/
		}

		#region Properties

		private string _file;
		/// <summary>
		/// Полный путь к файлу с изображением
		/// </summary>
		public string File
		{
			get { return _file; }
			set {
				if(_file != value) {
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
			set {
				if(_kind != value) {
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
			if(!System.IO.File.Exists(this.File)) {
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
