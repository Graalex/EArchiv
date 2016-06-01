using System.Collections.Generic;
using Mariupolgaz.EArchiv.Common.Models;

namespace Mariupolgaz.EArchiv.Common.Servises
{
	/// <summary>
	/// Интерфейс сервиса поиска Абонентов
	/// </summary>
	public interface IFinderService
	{
		/// <summary>
		/// Ищет абонента по лицевому счету 
		/// </summary>
		/// <param name="ls">Лицевой счет</param>
		/// <returns>Найденный экземпляр Abonent или null</returns>
		Abonent FindAbonent(int ls);

		/// <summary>
		/// Ищет абонентов по части фамилии
		/// </summary>
		/// <param name="family">Фамилия</param>
		/// <returns>Список экземпляров Abonent</returns>
		IList<Abonent> FindAbonents(string family);

		/// <summary>
		/// Ищет абонентов по части адреса
		/// </summary>
		/// <param name="family">Часть фамилии абонента</param>
		/// <param name="settlement">Название нас. п.</param>
		/// <param name="street">Название улицы</param>
		/// <param name="house">Номер дома</param>
		/// <param name="appartment">Номер квартиры</param>
		/// <returns>Список экземпляров Abonent</returns>
		IList<Abonent> FindAbonents(string family, string settlement = null, string street = null, string house = null, int? appartment = null);

		/// <summary>
		/// Получает список населенных пунктов
		/// </summary>
		/// <returns>Список населенных пунктов или null</returns>
		IList<Settlement> GetSettlementList();

		/// <summary>
		/// Возвращает список объектов типа <see cref="Street"/> для насаленного пункта.
		/// </summary>
		/// <param name="settlementID">Идентификатор нас. пункта</param>
		/// <returns>Список объектов <see cref="Street"/> или null</returns>
		IList<Street> GetStreetsList(int settlementID);

	}
}
