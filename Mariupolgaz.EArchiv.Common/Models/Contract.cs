using System;

namespace Mariupolgaz.EArchiv.Common.Models
{
	/// <summary>
	/// 
	/// </summary>
	public class Contract
	{
		/// <summary>
		/// Создает экземпляр <see cref="Contract"/>
		/// </summary>
		/// <param name="code">Код в 1С</param>
		/// <param name="name">Наименование</param>
		/// <param name="nomer">Номер договора</param>
		/// <param name="parent">Название головного договора</param>
		/// <param name="date">Дата заключения</param>
		/// <param name="expiry">Дата окончания</param>
		/// <param name="isSigned">Подписан</param>
		/// <param name="isEarly">Досрочно закрыт</param>
		/// <param name="isAsseccion">Заявление о присоединении</param>
		/// <param name="closedDate">Дата закрытия</param>
		/// <param name="asseccionDate">Дата заявления о присоединении</param>
		public Contract(
			string code,
			string name,
			string nomer,
			string parent,
			DateTime? date = null,
			DateTime? expiry = null,
			bool? isSigned = null,
			bool isEarly = false,
			bool isAsseccion = false,
			DateTime? closedDate = null,
			DateTime? asseccionDate = null
		)
		{
			this.Code = code;
			this.Name = name;
			this.Nomer = nomer;
			this.Date = date;
			this.Parent = parent;
			this.Expiry = expiry;
			this.IsSigned = isSigned;
			this.IsEarly = isEarly;
			this.IsAsseccion = isAsseccion;
			this.ClosedDate = closedDate;
			this.AsseccionDate = asseccionDate;
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
		/// Номер договора
		/// </summary>
		public string Nomer { get; private set; }

		/// <summary>
		/// Дата заключения
		/// </summary>
		public DateTime? Date { get; private set; }

		/// <summary>
		/// Дата окончания
		/// </summary>
		public DateTime? Expiry { get; private set; }

		/// <summary>
		/// Подписан
		/// </summary>
		public bool? IsSigned { get; private set; }

		/// <summary>
		/// Досрочно закрыт
		/// </summary>
		public bool IsEarly { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public bool IsAsseccion { get; private set; }

		/// <summary>
		/// Заявление о присоединении
		/// </summary>
		public DateTime? ClosedDate { get; private set; }

		/// <summary>
		/// Дата заявления о присоединении
		/// </summary>
		public DateTime? AsseccionDate { get; private set; }

		/// <summary>
		/// Головной договор
		/// </summary>
		public string Parent { get; private set; }
	}
}
