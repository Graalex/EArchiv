using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Mariupolgaz.EArchiv.Common.Events;
using Mariupolgaz.EArchiv.Common.Models;
using Microsoft.Practices.Prism.Events;

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



		#endregion

		#region Events

		private void lsSelectedEventHandler(int ls)
		{
			
		}

		#endregion
	}
}
