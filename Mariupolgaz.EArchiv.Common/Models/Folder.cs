using System;

namespace Mariupolgaz.EArchiv.Common.Models
{
	/// <summary>
	/// Представляет папку документов
	/// </summary>
	public class Folder: NotificationObject
	{
		/// <summary>
		/// Создает экземпляр <see cref="Folder"/>
		/// </summary>
		/// <param name="name">Имя папки</param>
		public Folder(string name)
		{
			this._id = -1;
			this._name = name;
		}

		/// <summary>
		/// Создает экземпляр <see cref="Folder"/>
		/// </summary>
		/// <param name="id">папки</param>
		/// <param name="name">Название папки</param>
		/// <param name="barcode">Штрих-код</param>
		/// <param name="isMarkDelete">Пометка на удаление</param>
		public Folder(int id, string name, string barcode, bool isMarkDelete)
		{
			this._id = id;
			this._name = name;
			this._barcode = barcode;
			this._mark_del = isMarkDelete;
		}

		private int _id;
		/// <summary>
		/// ID папки
		/// </summary>
		public int ID {
			get { return _id; }
		}
		private string _name;
		/// <summary>
		/// Название папки
		/// </summary>
		public string Name {
			get { return _name; }
			set {
				if(_name != value) {
					_name = value;
					RaisePropertyChanged(() => Name);
				}
			}
		}

		private string _barcode;
		/// <summary>
		/// Штрих-код папки
		/// </summary>
		public string BarCode {
			get { return _barcode; }
			set {
				if(_barcode != value) {
					_barcode = value;
					RaisePropertyChanged(() => BarCode);
				}
			}
		}

		private bool _mark_del;
		/// <summary>
		/// Пометка на удаление
		/// </summary>
		public bool IsMarkDelete {
			get { return _mark_del; }
			set {
				if(_mark_del != value) {
					_mark_del = value;
					RaisePropertyChanged(() => IsMarkDelete);
				}
			}
		}

		/// <summary>
		/// Изменяет ID папки
		/// </summary>
		/// <param name="id">новое ID</param>
		public void setID(int id)
		{
			if (id <= 0) throw new ArgumentException("ID должен быть числом больше нуля");

			_id = id;
			RaisePropertyChanged(() => ID);
		}
	}
}
