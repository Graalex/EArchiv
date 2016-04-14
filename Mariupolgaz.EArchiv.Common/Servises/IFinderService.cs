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
		/// <param name="settlementName">Название нас. п.</param>
		/// <param name="streetName">Название улицы</param>
		/// <returns>Список экземпляров Abonent</returns>
		IList<Abonent> FindAbonents(string settlementName, string streetName = null);

	}
}
