using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Mariupolgaz.EArchiv.Common.Models;
using Mariupolgaz.EArchiv.Common.Servises;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Win32;

namespace Mariupolgaz.EArchiv.DocsAbon.ViewModels
{
	/// <summary>
	/// 
	/// </summary>
	public class ChangeDocViewModel: BaseViewModel
	{

		public ChangeDocViewModel()	
		{
			var doc = ServiceLocator.Current.GetInstance<IDocumentService>();
			this.Kinds = new ObservableCollection<DocumentKind>(doc.GetKindsByClass(2));
		}

		private string _file;
		/// <summary>
		/// 
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
		/// 
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
		/// 
		/// </summary>
		public ObservableCollection<DocumentKind> Kinds { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public Action Close { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public ICommand Ok
		{
			get { return new DelegateCommand(onOk, canOk); }
		}

		private void onOk()
		{
			if (!System.IO.File.Exists(this.File)) {
				MessageBox.Show("Файл не существует!", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
				return;
			}

			this.Close();
		}

		private bool canOk()
		{
			return (this.SelectedKind != null || (this.File != null && this.File != String.Empty)) ? true : false;
		}

		/// <summary>
		/// Выбор файла с изображением
		/// </summary>
		public ICommand AddFile
		{
			get { return new DelegateCommand(onAddFile); }
		}

		public int DocumentService { get; private set; }

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
	}
}
