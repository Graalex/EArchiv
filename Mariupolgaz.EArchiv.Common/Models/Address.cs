using System;

namespace Mariupolgaz.EArchiv.Common.Models
{
	/// <summary>
	/// Представляет объект модели "Адрес" в базе NAST.
	/// </summary>
	public class Address
	{
		/// <summary>
		/// Создает экземпляр Address
		/// </summary>
		/// <param name="id">Код адреса из базы Globus</param>
		/// <param name="disrict">Областной район</param>
		/// <param name="settlementType">Тип населенного пункта</param>
		/// <param name="settlementName">Название нас. п.</param>
		/// <param name="area">Городской район</param>
		/// <param name="streetType">Тип улицы</param>
		/// <param name="streetName">Название улицы</param>
		/// <param name="houseNumb">Номер дома</param>
		/// <param name="litera">Литера</param>
		/// <param name="apartment">Номер квартиры</param>
		public Address(int id, string disrict, string settlementType, string settlementName, string area, string streetType, 
			string streetName, string houseNumb, string litera, int apartment)
		{
			this.ID = id;
			this.SettlementType = settlementType;
			this.SettlementName = settlementName;
			this.StreetType = streetType;
			this.StreetName = streetName;
			this.HouseNumb = houseNumb;
			this.Litera = litera;
			this.Apartment = apartment;
			this.District = disrict;
			this.Area = area;
		}

		/// <summary>
		/// Код адреса из базы Globus
		/// </summary>
		public int ID { get; private set; }

		/// <summary>
		/// Тип нас. п.
		/// </summary>
		public string SettlementType { get; private set; }

		/// <summary>
		/// Название нас. п.
		/// </summary>
		public string SettlementName { get; private set; }

		/// <summary>
		/// Тип улицы
		/// </summary>
		public string StreetType { get; private set; }

		/// <summary>
		/// Название улицы
		/// </summary>
		public string StreetName { get; private set; }

		/// <summary>
		/// Номер дома
		/// </summary>
		public string HouseNumb { get; private set; }

		/// <summary>
		/// Литера
		/// </summary>
		public string Litera { get; private set; }

		/// <summary>
		/// Номер квартиры
		/// </summary>
		public int Apartment { get; private set; }

		/// <summary>
		/// Областной район
		/// </summary>
		public string District { get; private set; }

		/// <summary>
		/// Городской район
		/// </summary>
		public string Area { get; private set; }

		/// <summary>
		///  Строковое представление адреса
		/// </summary>
		/// <returns>Строковое представление адреса</returns>
		public override string ToString()
		{
			string rslt = String.Empty;
			if(this.District != "Мариуполь") {
				rslt = this.District + ", ";
			}

			rslt += this.SettlementType + " " + this.SettlementName + ", ";

			if(this.Area != null && this.District == "Мариуполь") {
				rslt += this.Area + " р-н, ";
			}
			rslt += this.StreetType.ToLower() + " " + this.StreetName + ", ";
			rslt += this.HouseNumb;
			if(this.Litera != null && this.Litera != String.Empty) {
				rslt += this.Litera;
			}
			if(this.Apartment != 0) {
				rslt += " кв. " + this.Apartment.ToString();
			}
			return rslt;
		}
	}
}
