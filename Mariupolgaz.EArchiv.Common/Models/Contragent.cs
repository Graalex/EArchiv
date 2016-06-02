using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Mariupolgaz.EArchiv.Common.Models
{
	/// <summary>
	/// Контрагент
	/// </summary>
	public class Contragent
	{
		/// <summary>
		/// Создает экземпляр <see cref="Contragent"/>
		/// </summary>
		/// <param name="code">Код в 1С</param>
		/// <param name="name">Наименование</param>
		/// <param name="contracts">Список договоров</param>
		/// <param name="fullName">Наименование полное</param>
		/// <param name="edrpou">ЕДРПОУ</param>
		/// <param name="inn">ИНН</param>
		/// <param name="nomer">Номер свидетельства</param>
		public Contragent(
			string code, 
			string name,
			IList<Contract> contracts, 
			string fullName = "", 
			string edrpou = "", 
			string inn = "", 
			string nomer = ""
		)
		{
			this.Code = code;
			this.Name = name;
			this.Contracts = new ReadOnlyObservableCollection<Contract>(new ObservableCollection<Contract>(contracts));
			this.FullName = fullName;
			this.EDRPOU = edrpou;
			this.INN = inn;
			this.Nomer = nomer;
		}

		/// <summary>
		/// Код в 1С
		/// </summary>
		public string Code { get; private set; }

		/// <summary>
		/// Наименование
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Договора
		/// </summary>
		public ReadOnlyObservableCollection<Contract> Contracts { get; private set; }

		/// <summary>
		/// Наименование полное
		/// </summary>
		public string FullName { get; private set; }

		/// <summary>
		/// ЕДРПОУ
		/// </summary>
		public string EDRPOU { get; private set; }

		/// <summary>
		/// ИНН
		/// </summary>
		public string INN { get; private set; }

		/// <summary>
		/// Номер свидетельства
		/// </summary>
		public string Nomer { get; private set; }
	}
}
