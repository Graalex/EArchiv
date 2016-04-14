using System.Collections.ObjectModel;
using Mariupolgaz.EArchiv.Common.Models;

namespace Mariupolgaz.EArchiv.Document.ViewModels
{
	/// <summary>
	/// 
	/// </summary>
	public class KindListViewModel: NotificationObject
	{
		/// <summary>
		/// 
		/// </summary>
		public KindListViewModel()
		{
			this.Kinds = new ObservableCollection<DocumentKind>();
		}

		/// <summary>
		/// 
		/// </summary>
		public ObservableCollection<DocumentKind> Kinds { get; private set; }

		private DocumentKind _currentKind;
		/// <summary>
		/// 
		/// </summary>
		public DocumentKind CurrentKind {
			get { return _currentKind; }
			set {
				if(_currentKind != value) {
					_currentKind = value;
					RaisePropertyChanged(() => CurrentKind);
				}
			}
		}
	}
}
