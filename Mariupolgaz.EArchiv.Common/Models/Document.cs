using System;
using System.Security.Cryptography;
using System.Windows.Media.Imaging;

namespace Mariupolgaz.EArchiv.Common.Models
{
	/// <summary>
	/// Представляет электронный документ
	/// </summary>
	public class Document: NotificationObject
	{
		#region Constructors

		/// <summary>
		/// Создает экземпляр <see cref="Document"/>
		/// </summary>
		/// <remarks>
		/// Используется при создании нового документа
		/// </remarks>
		/// <param name="name">Название документа</param>
		/// <param name="kind">ID типа документа</param>
		public Document(string name, DocumentKind kind)
		{
			this._id = -1;
			this._kind = kind;
			this._createat = DateTime.Now;
			this.Name = name;
			this.IsDirty = true;
		}

		/// <summary>
		/// Создает экземпляр <see cref="Document"/>
		/// </summary>
		/// <remarks>
		/// Используется при создании документа из хранилища
		/// </remarks>
		/// <param name="id">ID документа</param>
		/// <param name="kind">ID типа документа</param>
		/// <param name="name">Имя документа</param>
		/// <param name="hash">Хеш документа</param>
		/// <param name="thumbnails">Миниатюра документа</param>
		/// <param name="createAt">Дата создания</param>
		/// <param name="modifyAt">Дата и время модификации</param>
		/// <param name="isMarkDel">Пометка на удаление</param>
		/// <param name="source">Скан копия документа</param>
		public Document(int id, DocumentKind kind, string name, byte[] hash, BitmapImage thumbnails, DateTime createAt, DateTime modifyAt,
										bool isMarkDel, BitmapImage source)
		{
			this._id = id;
			this._kind = kind;
			this._name = name;
			this._hash = hash;
			this._thumbnails = thumbnails;
			this._createat = createAt;
			this._modifyat = modifyAt;
			this._mark_del = isMarkDel;
			this._source = source;
			this.IsDirty = false;
		}

		#endregion  

		#region Properties

		private int _id = -1;
		/// <summary>
		/// ID документа
		/// </summary>
		public int ID {
			get { return _id; }
		}

		private DocumentKind _kind;
		/// <summary>
		/// Tип документа
		/// </summary>
		public DocumentKind Kind {
			get { return _kind; }
			set {
				if(_kind != value) {
					_kind = value;
					this.IsDirty = true;
					RaisePropertyChanged(() => Kind);
				}
			}
		}

		private string _name;
		/// <summary>
		/// Название документа
		/// </summary>
		public string Name {
			get { return _name; }
			set {
				if(_name != value) {
					_name = value;
					_modifyat = DateTime.Now;
					this.IsDirty = true;
					RaisePropertyChanged(() => Name);
					RaisePropertyChanged(() => ModifyAt);
				}
			}
		}

		private byte[] _hash;
		/// <summary>
		/// Хеш документа расчитанный по алгоритму SHA-256
		/// </summary>
		public string Hash {
			get {
				string rslt = String.Empty;
        for (int i=0; i<_hash.Length; i++) {
					rslt += String.Format("0:X2", _hash[i]);
				}
				return rslt;
			}
		}

		/// <summary>
		/// Размер скан копии документа
		/// </summary>
		public long Size {
			get { return _source.StreamSource.Length; }
		}

		private BitmapImage _thumbnails;
		/// <summary>
		/// Миниатюра документа
		/// </summary>
		public BitmapImage Thumbnails {
			get { return _thumbnails; }
		}

		private DateTime _createat;
		/// <summary>
		/// Дата и время создания документа
		/// </summary>
		public DateTime CreateAt {
			get { return _createat; }
		}

		private DateTime _modifyat;
		/// <summary>
		/// Дата и время изменения документа
		/// </summary>
		public DateTime ModifyAt {
			get { return _modifyat; }
		}

		private bool _mark_del;
		/// <summary>
		/// Пометка на удаление
		/// </summary>
		public bool IsMarkDelete {
			get { return _mark_del; }
		}

		private BitmapImage _source;
		/// <summary>
		/// Скан копия документа
		/// </summary>
		public BitmapImage Source {
			get { return _source; }
			set {
				_source = value;
				_modifyat = DateTime.Now;
				_thumbnails = buildThumbnails();
				_hash = getHash();
				this.IsDirty = true;
				RaisePropertyChanged(() => Source);
				RaisePropertyChanged(() => ModifyAt);
				RaisePropertyChanged(() => Thumbnails);
				RaisePropertyChanged(() => Hash);
			}
		}

		private bool _dirty;
		/// <summary>
		/// Признак модификации документа
		/// </summary>
		public bool IsDirty
		{
			get { return _dirty; }
			private set {
				if(_dirty != value) {
					_dirty = value;
					RaisePropertyChanged(() => IsDirty);
				}
			}
		}

		// TODO: Надо будет добавить создание штрих кода документа (причем разных).

		#endregion

		/// <summary>
		/// Изменяет ID документа
		/// </summary>
		/// <param name="id">новое ID</param>
		public void setID(int id)
		{
			if (id <= 0) throw new ArgumentException("ID должен быть числом больше нуля");

			_id = id;
			RaisePropertyChanged(() => ID);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public byte[] ConvertHash()
		{
			return _hash;
		}

		#region Helpers

		// Создает миниатюру скана документа
		private BitmapImage buildThumbnails()
		{
			//throw new NotImplementedException();
			return null;
		}

		// Вычисляет хэш скана документа по алгоритму SHA-256 
		private byte[] getHash()
		{
			SHA256 sha = SHA256Managed.Create();
			long len = _source.StreamSource.Length;
			byte[] buf = new byte[len];

			_source.StreamSource.Read(buf, 0, (int)len);
			return sha.ComputeHash(buf);
		}



		#endregion
	}
}
