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

namespace Mariupolgaz.EArchiv.DocsAbon.ViewModels
{
	/// <summary>
	/// 
	/// </summary>
	public class DocumentViewModel: BaseViewModel
	{
		private readonly IEventAggregator _aggregator;
		/// <summary>
		/// 
		/// </summary>
		public DocumentViewModel(IEventAggregator aggregator)
		{
			if (aggregator == null) throw new ArgumentNullException("aggregator");

			_aggregator = aggregator;
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
			
		}

		#endregion

		#region Events

		private void lsSelectedEventHandler(int ls)
		{
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
	}
}
