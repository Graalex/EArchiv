using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mariupolgaz.EArchiv.Common.Models;
using Mariupolgaz.EArchiv.Common.Servises;

namespace Mariupolgaz.EContract.Finder.Services
{
	/// <summary>
	/// Сервис поиска контрагентов и договоров в базе данных 1С
	/// </summary>
	public class ContractFinderService : IContractFinderService
	{
		/// <summary>
		/// Получить список контрагентов по строке поиска
		/// </summary>
		/// <param name="orgKey">Ключ к параметрам подключения информационной базы 1С</param>
		/// <param name="contragentName">Имя или часть имени контрагента</param>
		/// <returns>Список найденных контрагентов</returns>
		public IList<Contragent> FindContragents(string orgKey, string contragentName)
		{
			IList<Contragent> rslt = null;

			//TODO: Только в целях тестирования
			rslt = new List<Contragent>();
			IList<Contract> contracts = new List<Contract>();

			contracts.Add(new Contract("0001", "Contract 422", "422"));
			contracts.Add(new Contract("0002", "Contract 42", "42"));
			contracts.Add(new Contract("0003", "Contract 40", "40/117"));
			rslt.Add(new Contragent("0001", "Рога и копыта", contracts));
			contracts.Clear();

			contracts.Add(new Contract("0004", "Contract 422", "422"));
			contracts.Add(new Contract("0005", "Contract 42", "42"));
			contracts.Add(new Contract("0006", "Contract 40", "40/117"));
			rslt.Add(new Contragent("0002", "Рога и копыта", contracts));
			contracts.Clear();

			contracts.Add(new Contract("0007", "Contract 422", "422"));
			contracts.Add(new Contract("0008", "Contract 42", "42"));
			contracts.Add(new Contract("0009", "Contract 40", "40/117"));
			rslt.Add(new Contragent("0003", "Рога и копыта", contracts));
			contracts.Clear();

			return rslt;
		}
	}
}
