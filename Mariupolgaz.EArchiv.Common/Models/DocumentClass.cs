using System;

namespace Mariupolgaz.EArchiv.Common.Models
{
	/// <summary>
	/// Представляет клас документа
	/// </summary>
	public class DocumentClass : NotificationObject
	{
		/// <summary>
		/// Создает экземпляр <see cref="DocumentClass"/>
		/// </summary>
		/// <param name="name">Название класа документа</param>
		public DocumentClass(string name)
		{
			_id = -1;
			_name = name;
			_mark_del = false;
		}

		/// <summary>
		/// Создает экземпляр <see cref="DocumentClass"/>
		/// </summary>
		/// <param name="id">ID класа</param>
		/// <param name="name">Название класа</param>
		/// <param name="isMarkDel">Признак удаления</param>
		public DocumentClass(int id, string name, bool isMarkDel)
		{
			_id = id;
			_name = name;
			_mark_del = isMarkDel;
		}

		private int _id;
		/// <summary>
		/// ID класа
		/// </summary>
		public int ID
		{
			get { return _id; }
		}

		private string _name;
		/// <summary>
		/// Название класа
		/// </summary>
		public string Name
		{
			get { return _name; }
			set
			{
				if (_name != value) {
					_name = value;
					RaisePropertyChanged(() => Name);
				}
			}
		}

		private bool _mark_del;
		/// <summary>
		/// Признак удаления
		/// </summary>
		public bool IsMarkDelete
		{
			get { return _mark_del; }
			set
			{
				if (_mark_del != value) {
					_mark_del = value;
					RaisePropertyChanged(() => IsMarkDelete);
				}
			}
		}

		/// <summary>
		/// Устанавливает класу документ ID.
		/// </summary>
		/// <param name="id">Новы ID</param>
		protected void setID(int id)
		{
			if (id <= 0) throw new ArgumentException("", "id");
			_id = id;
			RaisePropertyChanged(() => ID);
		}
	}
}
