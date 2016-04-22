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
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using Mariupolgaz.EArchiv.DocsAbon.Views;
using System.Windows.Media.Imaging;
using System.IO;

namespace Mariupolgaz.EArchiv.DocsAbon.ViewModels
{
	/// <summary>
	/// 
	/// </summary>
	public class DocumentViewModel: BaseViewModel
	{
		private readonly IEventAggregator _aggregator;
		private readonly IDocumentService _docsrv;

		/// <summary>
		/// 
		/// </summary>
		public DocumentViewModel(IEventAggregator aggregator)
		{
			if (aggregator == null) throw new ArgumentNullException("aggregator");

			_aggregator = aggregator;
			_docsrv = ServiceLocator.Current.GetInstance<IDocumentService>();

			this.Documents = new ObservableCollection<Document>();
			_aggregator.GetEvent<LsSelectedEvent>().Subscribe(lsSelectedEventHandler);
		}

		#region Properties

		/// <summary>
		/// Список папок связанных с лицевым счетом
		/// </summary>
		public ObservableCollection<Document> Documents { get; private set; }

		private Document _seldoc;
		/// <summary>
		/// Текущий документ
		/// </summary>
		public Document SelectedDocument
		{
			get { return _seldoc; }
			set {
				if(_seldoc != value) {
					_seldoc = value;
					RaisePropertyChanged(() => SelectedDocument);
				}
			}
		}

		private int? _ls;
		/// <summary>
		/// Лицевой счет
		/// </summary>
		public int? LS
		{
			get { return _ls; }
			set {
				if(_ls != value) {
					_ls = value;
					RaisePropertyChanged(() => LS);
					RaisePropertyChanged(() => Url);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Url
		{
			get
			{
				if (_ls != -1) {
					return @"http://earchiv/documents/ls/" + this.LS.ToString();
				}
				else {
					return null;
				}
			}
		}

		private Visibility _display = Visibility.Collapsed;
		/// <summary>
		/// 
		/// </summary>
		public Visibility DisplayMode
		{
			get { return _display; }
			set
			{
				if (_display != value)
				{
					_display = value;
					RaisePropertyChanged(() => DisplayMode);
				}
			}
		}

		#endregion

		#region Commands

		/// <summary>
		/// Добавляет новый документ
		/// </summary>
		public ICommand AddDocument 
		{
			get { return new DelegateCommand(onAddDocument); }
		}

		private void onAddDocument()
		{
			//TODO: Очень не элегантно (костыль) но нет времени переделаю потом
			// надо будет сообщения и модальные окна вывести в отдельный модуль сервис.

			var dlg = ServiceLocator.Current.GetInstance<DocAddDialog>();
			bool? rslt = dlg.ShowDialog();
			if(rslt.GetValueOrDefault()) {
				var data = (dlg.DataContext as DocAddViewModel);
				Document doc = new Document(generateDocName(data.SelectedKind), data.SelectedKind);
				BitmapImage bi = new BitmapImage();

				bi.BeginInit();
				FileStream fs = new FileStream(data.File, FileMode.Open);

				bi.StreamSource = fs;
				bi.EndInit();
				doc.Source = bi;
				this.Documents.Add(doc);
				this.SelectedDocument = doc;
      }
			
		}

		/// <summary>
		/// 
		/// </summary>
		public ICommand SaveDocuments
		{
			get { return new DelegateCommand(onSaveDocuments, canSaveDocuments); }
		}

		private void onSaveDocuments()
		{
			Mouse.OverrideCursor = Cursors.Wait;

			try {
				foreach (var doc in this.Documents) {
					if (doc.IsDirty) {
						if (doc.ID == -1) {
							_docsrv.SaveDocument(doc, new Folder(this.LS.ToString()), this.LS.GetValueOrDefault());
						} else {
							_docsrv.SaveDocumentSource(doc);
						}
						doc.IsDirty = false;
					}
				}
			}

			catch(Exception e) {
				MessageBox.Show(
					"Произошла ошибка при сохранении документов!\n" + e.Message,
					"Ошибка",
					MessageBoxButton.OK,
					MessageBoxImage.Error
				);
			}

			finally {
				Mouse.OverrideCursor = null;
			}
		}

		private bool canSaveDocuments()
		{
			return checkDirty();
		}

		/// <summary>
		/// 
		/// </summary>
		public ICommand ChangeDocument
		{
			get { return new DelegateCommand(onChangeDocument, canChangeDocument); }
		}

		private void onChangeDocument()
		{
			var dlg = ServiceLocator.Current.GetInstance<ChangeDocView>();
			var data = (dlg.DataContext as ChangeDocViewModel);
			bool? rslt = dlg.ShowDialog();
			if (rslt.GetValueOrDefault()) {
				if (this.SelectedDocument.Kind != data.SelectedKind) {
					this.SelectedDocument.Kind = data.SelectedKind;
					this.SelectedDocument.Name = generateDocName(data.SelectedKind);
				}
				if(data.File != null && data.File != String.Empty) {
					BitmapImage bi = new BitmapImage();

					bi.BeginInit();
					FileStream fs = new FileStream(data.File, FileMode.Open);

					bi.StreamSource = fs;
					bi.EndInit();

					this.SelectedDocument.Source = bi;
				}
			}
		}

		private bool canChangeDocument()
		{
			return (this.Documents.Count > 0 && this.SelectedDocument != null) ? true : false;
		}

		/// <summary>
		/// 
		/// </summary>
		public ICommand DeleteDocument
		{
			get { return new DelegateCommand(onDeleteDocument, canDeleteDocument); }
		}

		private void onDeleteDocument()
		{
			if(MessageBox.Show(
				"Вы уверены, что хотите удалить документ?",
				"Предупреждение",
				MessageBoxButton.YesNo,
				MessageBoxImage.Warning
			) == MessageBoxResult.Yes) {
				if(this.SelectedDocument.ID != -1) {
					_docsrv.MarkDeleteDocument(this.SelectedDocument);
				}
				this.Documents.Remove(this.SelectedDocument);
			}
		}

		private bool canDeleteDocument()
		{
			return this.SelectedDocument != null ? true : false;
		}

		/// <summary>
		/// 
		/// </summary>
		public ICommand CopyUrl
		{
			get { return new DelegateCommand(onCopyUrl); }
		}

		private void onCopyUrl()
		{
			Mouse.OverrideCursor = Cursors.Wait;
			Clipboard.SetData(DataFormats.Text, (object)this.Url);
			Mouse.OverrideCursor = null;
    }

		#endregion

		#region Events

		private void lsSelectedEventHandler(int ls)
		{
			// проверяем были ли какие либо изменения в списке документов
			if(checkDirty()) {
				if (MessageBox.Show(
					"Один или несколько документов были вами изменены.\nСохранить результат изменений?\nПри отказе все ваши изменения будут отменены!", 
					"Внимание", 
					MessageBoxButton.YesNo, 
					MessageBoxImage.Question) == MessageBoxResult.Yes) {
					 onSaveDocuments();
				}
			}

			// очистка предыдущего списка документов
			this.Documents.Clear();
			this.SelectedDocument = null;

			// получение нового списка документов
			this.LS = ls;
			Mouse.OverrideCursor = Cursors.Wait;
			try {
				var doc = ServiceLocator.Current.GetInstance<IDocumentService>();
				
				IList<Document> rslt = doc.GetDocuments(ls);
				if (rslt != null) {
					foreach (var item in rslt) {
						this.Documents.Add(item);
					}
				}
				 
				// отображение панели для работы с документами
				this.DisplayMode = Visibility.Visible;
			}

			catch(Exception e) {
				MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}

			finally {
				Mouse.OverrideCursor = null;
			}
		}

		#endregion

		#region Helpers

		private string generateDocName(DocumentKind kind)
		{
			string rslt;

			switch (kind.ID) {
        case 1:
					rslt = "ПП";
					break;

				case 2:
					rslt = "ИН";
					break;

				case 3:
					rslt = "СЗ";
					break;

				case 4:
					rslt = "СД";
					break;

				case 5:
					rslt = "ДГ";
					break;
				
				case 6:
					rslt = "ПД";
					break;

				default:
					rslt = "ПР";
					break;
			};

			rslt += ("-" + this.LS + "-" + String.Format("{0:yyyyMdHms}", DateTime.Now));

			return rslt;
		}

		private bool checkDirty()
		{
			bool rslt = false;
			if(this.Documents.Count > 0) {
				rslt = this.Documents.Any(doc => doc.IsDirty);
			}

			return rslt;
		}

		#endregion
	}
}
