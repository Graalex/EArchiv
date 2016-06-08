using System.Collections.Generic;
using Mariupolgaz.EArchiv.Common.Models;

namespace Mariupolgaz.EArchiv.Common.Servises
{
	/// <summary>
	/// Интерфейс сервиса поиска контрагентов и договоров в базе данных 1С
	/// </summary>
	public interface IContractFinderService
	{
		/// <summary>
		/// Получить список контрагентов по строке поиска
		/// </summary>
		/// <param name="orgKey">Ключ к параметрам подключения информационной базы 1С</param>
		/// <param name="contragentName">Имя или часть имени контоагента</param>
		/// <returns></returns>
		IList<Contragent> FindContragents(string orgKey, string contragentName);

	}
}
