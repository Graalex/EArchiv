using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Mariupolgaz.EArchiv.Common.Models;

namespace Mariupolgaz.EContract.DocsContract.ViewModel
{
	/// <summary>
	/// Представляет модель вида для отображения электронных копий
	/// </summary>
	public class DocsContarctViewModel : BaseViewModel
	{
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
				}
			}
		}

		private bool _isDirty;
		/// <summary>
		/// Признак вносились ли изменения в коллекцию документов
		/// </summary>
		public bool IsDirty
		{
			get { return _isDirty; } 
			set {
				if(_isDirty != value) {
					_isDirty = value;
					RaisePropertyChanged(() => IsDirty);
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

		}

		private bool canEditDocument()
		{
			return this.SelectedDocument != null;
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

		}

		private bool canDeleteDocument()
		{
			return this.SelectedDocument != null;
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

		}

		private bool canSaveChange()
		{
			return this.IsDirty;
		}

		#endregion
	}
}
