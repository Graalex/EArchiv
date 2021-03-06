﻿using System.Collections.ObjectModel;
using Mariupolgaz.EArchiv.Common.Models;
using Microsoft.Practices.ServiceLocation;
using Mariupolgaz.EArchiv.Common.Servises;

namespace Mariupolgaz.EArchiv.DocsAbon.ViewModels
{
	/// <summary>
	/// 
	/// </summary>
	public class KindDocViewModel: BaseViewModel
	{
		//TODO: Сейчас жестко забит класс документов, позже его нужно брать 
		// из конфигурационного файла.
		private const int _DOC_CLASS = 2;

		/// <summary>
		/// 
		/// </summary>
		public KindDocViewModel()
		{
			var doc = ServiceLocator.Current.GetInstance<IDocumentService>();
			/*
			if(doc != null) {
				this.DocumentKinds = new ObservableCollection<DocumentKind>(
					doc.GetKindsByClass(_DOC_CLASS)
				);
			} else {
				this.DocumentKinds = new ObservableCollection<DocumentKind>();
			}
			 * */

			this.DocumentKinds = new ObservableCollection<DocumentKind>();
			this.DocumentKinds.Add(new DocumentKind(1, "Паспорт", false));
			this.DocumentKinds.Add(new DocumentKind(2, "ИНН", false));
			this.DocumentKinds.Add(new DocumentKind(3, "Право на земельный участок", false));
		}

		/// <summary>
		/// 
		/// </summary>
		public ObservableCollection<DocumentKind> DocumentKinds { get; private set; }
	}
}
