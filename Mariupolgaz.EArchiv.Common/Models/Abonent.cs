using System;
using System.Text.RegularExpressions;

namespace Mariupolgaz.EArchiv.Common.Models
{
	/// <summary>
	/// Представляет объект модели Абонент
	/// </summary>
	public class Abonent: NotificationObject
	{
		/// <summary>
		/// Создает экземпляр <see cref="Abonent"/>
		/// </summary>
		public Abonent()
		{

		}

		/// <summary>
		/// Создает экземпляр <see cref="Abonent"/>
		/// </summary>
		/// <param name="ls">Лицевой счет</param>
		/// <param name="family">Фамилия абонента</param>
		public Abonent(int ls, string family)
		{
			this.LS = ls;
			this.Family = family;
		}

		/// <summary>
		/// Создает экземпляр <see cref="Abonent"/>
		/// </summary>
		/// <param name="ls">Лицевой счет</param>
		/// <param name="family">Фамилия абонента</param>
		/// <param name="address">Почтовый адрес</param>
		public Abonent(int ls, string family, Address address)
		{
			this.LS = ls;
			this.Family = family;
			this.Address = address;
		}


		/// <summary>
		/// Создает экземпляр <see cref="Abonent"/>
		/// </summary>
		/// <param name="ls">Лицевой счет</param>
		/// <param name="family">Фамилия абонента</param>
		/// <param name="firstName">Имя абонента</param>
		/// <param name="lastName">Отчество абонента</param>
		/// <param name="pasport">Серия и номер паспорта</param>
		/// <param name="inn">ИНН</param>
		/// <param name="address">Почтовый адрес</param>
		public Abonent(int ls, string family, string firstName, string lastName, string pasport, int inn, Address address)
		{
			this.LS = ls;
			this.Family = family;
			this.FirstName = firstName;
			this.LastName = lastName;
			this.Pasport = pasport;
			this.INN = inn;
			this.Address = address;
		}

		private int _ls = 0;
		/// <summary>
		/// Лицевой счет
		/// </summary>
		public int LS { 
			get { return _ls; }
			set
			{
				if (_ls != value) {
					_ls = value;
					RaisePropertyChanged(() => LS);
				}
			}
	 }

		private string _fam = string.Empty;
		/// <summary>
		/// Фамилия абонента
		/// </summary>
		public string Family { 
			get { return _fam; } 
			set {
				if (value == null || value == string.Empty) throw new ArgumentException("Недопустимое значение свойства.", "Family");
				if(_fam != value) {
					_fam = value;
					RaisePropertyChanged(() => Family);
					RaisePropertyChanged(() => FullFamily);
				}
			}
		}

		private string _fnam = string.Empty;
		/// <summary>
		/// Имя абонента
		/// </summary>
		public string FirstName {
			get { return _fnam; }
			set {
				if (_fnam != value) {
					_fnam = value;
					RaisePropertyChanged(() => FirstName);
					RaisePropertyChanged(() => FullFamily);
				}
			}
		}

		private string _lnam = string.Empty;
		/// <summary>
		/// Отчество абонента
		/// </summary>
		public string LastName {
			get { return _lnam; }
			set {
				if (_lnam != value) {
					_lnam = value;
					RaisePropertyChanged(() => LastName);
					RaisePropertyChanged(() => FullFamily);
				}
			}
		}

		private string _pasport = string.Empty;
		/// <summary>
		/// Серия и номер паспорта абонента
		/// </summary>
		public string Pasport {
			get { return _pasport; }
			set {
				if (!Regex.IsMatch(value, @"\{2}[А-Я]\s\d{6}")) throw new ArgumentException("Недопустимый формат серии и номера паспорта.", "Pasport");
				if (_pasport != value) {
					_pasport = value;
					RaisePropertyChanged(() => Pasport);
				}
			}
		}


		/// <summary>
		/// ИНН абонента
		/// </summary>
		private int _inn = 0;
		/// <summary>
		/// 
		/// </summary>
		public int INN {
			get { return _inn; }
			set {
				if(_inn != value) {
					_inn = value;
					RaisePropertyChanged(() => INN);
				}
			}
		}

		private Address _addr;
		/// <summary>
		/// Адрес абонента
		/// </summary>
		public Address Address { 
			get { return _addr; } 
			set {
				if(_addr != value) {
					_addr = value;
					RaisePropertyChanged(() => Address);
				}
			} 
		}

		/// <summary>
		/// Преобразует экземпляр в строковое представление
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return this.LS.ToString() + " " + this.Family + " " + this.FirstName + " " + this.LastName;
		}

		/// <summary>
		/// Полное имя абонента
		/// </summary>
		public string FullFamily
		{
			get {
				return this.Family + " " + this.FirstName + " " + this.LastName;
			}
		}
	}
		
}
