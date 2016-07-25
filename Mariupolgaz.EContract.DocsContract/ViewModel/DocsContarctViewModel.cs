using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Mariupolgaz.EArchiv.Common.Events;
using Mariupolgaz.EArchiv.Common.Models;
using Mariupolgaz.EArchiv.Common.Servises;
using Mariupolgaz.EContract.DocsContract.View;
using Mariupolgaz.EContract.DocsContract.Views;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;

namespace Mariupolgaz.EContract.DocsContract.ViewModel
{
	/// <summary>
	/// Представляет модель вида для отображения электронных копий
	/// </summary>
	public class DocsContarctViewModel : BaseViewModel
	{
		private readonly IEventAggregator _aggr;
		private readonly IDocumentService _docsrv;

		/// <summary>
		/// <see cref="DocsContarctViewModel"/>
		/// </summary>
		/// <param name="aggregator"></param>
		public DocsContarctViewModel(IEventAggregator aggregator)
		{
			if (aggregator == null) throw new ArgumentNullException("aggregator");

			_aggr = aggregator;
			_docsrv = ServiceLocator.Current.GetInstance<IDocumentService>();

			_aggr.GetEvent<ContractSelectedEvent>().Subscribe(updateContract);
			this.Documents = new ObservableCollection<Document>();
		}

		#region Properties

		/// <summary>
		/// Список отсканированных документов для договора
		/// </summary>
		public ObservableCollection<Document> Documents { get; private set; }

		private Document _seldoc;
		/// <summary>
		/// Текущий документа
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


		private Contract _curcontro;
		/// <summary>
		/// Текущий договор
		/// </summary>
		public Contract CurrentContract
		{
			get { return _curcontro; }
			set {
				if(_curcontro != value) {
					_curcontro = value;
					RaisePropertyChanged(() => CurrentContract);
					RaisePropertyChanged(() => ContractText);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string ContractText
		{
			get { return "Договор № " + _curcontro.Nomer + " " + _curcontro.Name; }	
		}

		private double _img_width = Double.NaN;
		/// <summary>
		/// Ширина изображения
		/// </summary>
		public double ImageWidth
		{
			get { return _img_width; }
			set
			{
				if (_img_width != value) {
					_img_width = value;
					RaisePropertyChanged(() => ImageWidth);
				}
			}
		}

		#endregion

		#region Commands

		/// <summary>
		/// Добавляем новый документ к договору
		/// </summary>
		public ICommand AddDocument
		{
			get { return new DelegateCommand(onAddDocument); }
		}

		private void onAddDocument()
		{
			FileStream fs = null;

			try {
				//TODO: Очень не элегантно (костыль) но нет времени переделаю потом
				// надо будет сообщения и модальные окна вывести в отдельный модуль сервис. 
				var dlg = ServiceLocator.Current.GetInstance<ContractDocAddDialog>();
				bool? rslt = dlg.ShowDialog();
				if (rslt.GetValueOrDefault()) {
					Mouse.OverrideCursor = Cursors.Wait;

					var data = (dlg.DataContext as ContractDocAddViewModel);
					Document doc = new Document(generateDocName(data.SelectedKind), data.SelectedKind);

					BitmapImage bi = new BitmapImage();
					fs = new FileStream(data.File, FileMode.Open);

					bi.BeginInit();
					bi.CacheOption = BitmapCacheOption.OnLoad;
					bi.StreamSource = fs;
					bi.EndInit();

					doc.Source = bi;
					this.Documents.Add(doc);
					this.SelectedDocument = doc;
				}
			}

			catch (Exception e) {
				MessageBox.Show(
					"Произошла ошибка при добавлении документа.\n" + e.Message,
					"Ошибка",
					MessageBoxButton.OK,
					MessageBoxImage.Error
				);
			}

			finally {
				if (fs != null) {
					fs.Close();
				}
				Mouse.OverrideCursor = null;
			}
		}

		/// <summary>
		/// Редектирование выбранного документа
		/// </summary>
		public ICommand EditDocument
		{
			get { return new DelegateCommand(onEditDocument, canEditDocument); }
		}

		private void onEditDocument()
		{
			FileStream fs = null;

			try {
				var dlg = ServiceLocator.Current.GetInstance<ContractDocEditDialog>();
				var data = (dlg.DataContext as ContractDocEditViewModel);
				bool? rslt = dlg.ShowDialog();
				if (rslt.GetValueOrDefault()) {
					Mouse.OverrideCursor = Cursors.Wait;

					if (this.SelectedDocument.Kind != data.SelectedKind) {
						this.SelectedDocument.Kind = data.SelectedKind;
						this.SelectedDocument.Name = generateDocName(data.SelectedKind);
					}

					if (data.File != null && data.File != String.Empty) {
						BitmapImage bi = new BitmapImage();

						bi.BeginInit();
						bi.CacheOption = BitmapCacheOption.OnLoad;
						fs = new FileStream(data.File, FileMode.Open);
						bi.StreamSource = fs;
						bi.EndInit();

						this.SelectedDocument.Source = bi;
					}
				}
			}

			catch (Exception e) {
				MessageBox.Show(
					"Произошла ошибка при изменении документа.\n" + e.Message,
					"Ошибка",
					MessageBoxButton.OK,
					MessageBoxImage.Error
				);
			}

			finally {
				if (fs != null) {
					fs.Close();
				}

				Mouse.OverrideCursor = null;
			}
		}

		private bool canEditDocument()
		{
			return this.SelectedDocument != null ? true : false; 
		}

		/// <summary>
		/// Удаление текущего документа
		/// </summary>
		public ICommand DeleteDocument
		{
			get { return new DelegateCommand(onDeleteDocument, canDeleteDocument); }
		}

		private void onDeleteDocument()
		{
			if (MessageBox.Show(
				"Вы уверены, что хотите удалить документ?",
				"Предупреждение",
				MessageBoxButton.YesNo,
				MessageBoxImage.Warning
			) == MessageBoxResult.Yes) {
				if (this.SelectedDocument.ID != -1) {
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
		/// Сохраняет внесенные изменения
		/// </summary>
		public ICommand SaveChange
		{
			get { return new DelegateCommand(onSaveChange, canSaveChange); }
		}

		private void onSaveChange()
		{
			Mouse.OverrideCursor = Cursors.Wait;

			try {
				foreach (var doc in this.Documents) {
					if (doc.IsDirty) {
						if (doc.ID == -1) {
							_docsrv.SaveDocument(doc, "03361135", this.CurrentContract.Code);
						}
						else {
							_docsrv.SaveDocumentSource(doc);
						}
						doc.IsDirty = false;
					}
				}
			}

			catch (Exception e) {
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

		private bool canSaveChange()
		{
			return checkDirty();
		}

		#endregion

		private void updateContract(Contract contract)
		{
			this.CurrentContract = contract;
			this.Documents.Clear();
			var docs = _docsrv.GetDocuments("03361135", contract.Code);
			foreach(var item in docs) {
				this.Documents.Add(item);
			}
		}

		private string generateDocName(DocumentKind kind)
		{
			string rslt;

			switch (kind.ID) {
				case 1012:
					rslt = "ДГ";
					break;

				case 1013:
					rslt = "ДС";
					break;

				case 1014:
					rslt = "ТС";
					break;

				case 1015:
					rslt = "ЗП";
					break;

				default:
					rslt = "ПР";
					break;
			};

			rslt += ("-" + Convert.ToInt32(this.CurrentContract.Code) + "-" + String.Format("{0:yyyyMdHms}", DateTime.Now));

			return rslt;
		}

		private bool checkDirty()
		{
			bool rslt = false;
			if (this.Documents.Count > 0) {
				rslt = this.Documents.Any(doc => doc.IsDirty);
			}

			return rslt;
		}
	}
}
