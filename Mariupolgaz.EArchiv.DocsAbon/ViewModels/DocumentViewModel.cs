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

		private int _ls = -1;
		/// <summary>
		/// Лицевой счет
		/// </summary>
		public int LS
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
					return "http:////data-serv//documents//ls//" + this.LS.ToString();
				}
				else {
					return null;
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

		#endregion

		#region Events

		private void lsSelectedEventHandler(int ls)
		{
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
			}

			catch(Exception e) {
				MessageBox.Show(e.Message, "Ошибка");
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

		#endregion
	}
}
