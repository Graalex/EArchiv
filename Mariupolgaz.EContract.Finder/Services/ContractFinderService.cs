using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Mariupolgaz.EArchiv.Common.Events;
using Mariupolgaz.EArchiv.Common.Models;
using Mariupolgaz.EArchiv.Common.Servises;
using Microsoft.Practices.Prism.Events;
using VanessaSharp.Data;

namespace Mariupolgaz.EContract.Finder.Services
{
	/// <summary>
	/// Сервис поиска контрагентов и договоров в базе данных 1С
	/// </summary>
	public class ContractFinderService : IContractFinderService, IDisposable
	{
		private OneSConnection _con;
    private IEventAggregator _aggr;

		/// <summary>
		/// 
		/// </summary>
		public ContractFinderService(IEventAggregator aggregator)
		{
			if (aggregator == null) throw new ArgumentNullException("aggregator");
			
			string conn = ConfigurationManager.ConnectionStrings["1C-Mariupolgaz"].ConnectionString;
			_aggr = aggregator;
			_con = new OneSConnection(conn);

			_aggr.GetEvent<ServerConectEvent>().Publish("ПАО Мариупольгаз");
			_con.Open();
			if (_con.State == ConnectionState.Open) _aggr.GetEvent<ServerConectedEvent>().Publish(true);

		}

		/// <summary>
		/// Получить список контрагентов по строке поиска
		/// </summary>
		/// <param name="orgKey">Ключ к параметрам подключения информационной базы 1С</param>
		/// <param name="contragentName">Имя или часть имени контрагента</param>
		/// <returns>Список найденных контрагентов</returns>
		public IList<Contragent> FindContragents(string orgKey, string contragentName)
		{
			IList<Contragent> rslt = new List<Contragent>();

			string qrContragent = "ВЫБРАТЬ " +
													"Контрагенты.Код КАК Код," +
													"Контрагенты.Наименование," +
													"Контрагенты.НаименованиеПолное," +
													"Контрагенты.ИНН," +
													"Контрагенты.КодПоЕДРПОУ," +
													"Контрагенты.НомерСвидетельства " +
											  "ИЗ Справочник.Контрагенты КАК Контрагенты " +
											  "ГДЕ Контрагенты.Наименование ПОДОБНО &Наименование И Контрагенты.ЭтоГруппа = ЛОЖЬ " +
											  "УПОРЯДОЧИТЬ ПО Код";
			string qrConcract = "ВЫБРАТЬ " +
														"ДоговорыКонтрагентов.Код," +
														"ДоговорыКонтрагентов.Наименование," +
														"ДоговорыКонтрагентов.Номер," +
														"ДоговорыКонтрагентов.Родитель.Наименование КАК Родитель, " +
														"ДоговорыКонтрагентов.Дата КАК Дата," +
														"ДоговорыКонтрагентов.СрокДействия," +
														"ДоговорыКонтрагентов.Подписан," +
														"ДоговорыКонтрагентов.ЗакрытДосрочно," +
														"ДоговорыКонтрагентов.ЗаявлениеПрисоединения," +
														"ДоговорыКонтрагентов.ДатаЗаявленияПрисоединения," +
														"ДоговорыКонтрагентов.ДатаЗакрытДосрочно, " +
														"ДоговорыКонтрагентов.Объект.Наименование КАК Объект " +
													"ИЗ Справочник.ДоговорыКонтрагентов КАК ДоговорыКонтрагентов " +
													"ГДЕ ДоговорыКонтрагентов.Владелец.Код = &Код И ДоговорыКонтрагентов.ЭтоГруппа = ЛОЖЬ " +
													"УПОРЯДОЧИТЬ ПО Дата УБЫВ";

			using (var cmd = new OneSCommand(_con)) {
				cmd.CommandText = qrContragent;
				cmd.Parameters.Clear();
				if (contragentName == null || contragentName == "")
					cmd.Parameters.Add(new OneSParameter("Наименование", "%"));
				else
					cmd.Parameters.AddNew(new OneSParameter("Наименование", "%" + contragentName.Trim() + "%"));


				using (var reader = cmd.ExecuteReader()) {

					if (reader.HasRows) {
						IList<Contract> contracts = new List<Contract>();
						var cmdContract = new OneSCommand(_con);
						cmdContract.CommandText = qrConcract;

						while (reader.Read()) {
							string code = reader.GetString(0).Trim();
							contracts.Clear();

							cmdContract.Parameters.Clear();
							cmdContract.Parameters.AddNew(new OneSParameter("Код", code));

							var readerContracts = cmdContract.ExecuteReader();
							if(readerContracts.HasRows) {
								while(readerContracts.Read()) {
									contracts.Add(new Contract(
										readerContracts.GetString(0),
										readerContracts.GetString(1),
										readerContracts.GetString(2),
										readerContracts.GetString(3),
										readerContracts.GetDateTime(4),
										readerContracts.GetDateTime(5),
										readerContracts.GetBoolean(6),
										readerContracts.GetBoolean(7),
										readerContracts.GetBoolean(8),
										readerContracts.GetDateTime(9),
										readerContracts.GetDateTime(10),
										readerContracts.GetString(11)
									));
								}
							}
							readerContracts.Close();
								

              rslt.Add(new Contragent(
								code,
								reader.GetString(1).Trim(),
								contracts,
								reader.GetString(2).Trim(),
								reader.GetString(4),
								reader.GetString(3),
								reader.GetString(5)
              ));
								
						}
					}				
				}
			}

			
			

			return rslt;
		}

		/// <summary>
		/// Получить список контрагентов по коду ЕДРПОУ
		/// </summary>
		/// <param name="orgKey">Ключ к параметрам подключения информационной базы 1С</param>
		/// <param name="contragentCode">ЕДРПОУ контрагента</param>
		/// <returns></returns>
		public IList<Contragent> FindContragentsCode(string orgKey, string contragentCode)
		{
			IList<Contragent> rslt = new List<Contragent>();

			string qrContragent = "ВЫБРАТЬ " +
													"Контрагенты.Код КАК Код," +
													"Контрагенты.Наименование," +
													"Контрагенты.НаименованиеПолное," +
													"Контрагенты.ИНН," +
													"Контрагенты.КодПоЕДРПОУ," +
													"Контрагенты.НомерСвидетельства " +
												"ИЗ Справочник.Контрагенты КАК Контрагенты " +
												"ГДЕ Контрагенты.КодПоЕДРПОУ = &ЕДРПОУ И Контрагенты.ЭтоГруппа = ЛОЖЬ " +
												"УПОРЯДОЧИТЬ ПО Код";
			string qrConcract = "ВЫБРАТЬ " +
														"ДоговорыКонтрагентов.Код," +
														"ДоговорыКонтрагентов.Наименование," +
														"ДоговорыКонтрагентов.Номер," +
														"ДоговорыКонтрагентов.Родитель.Наименование КАК Родитель, " +
														"ДоговорыКонтрагентов.Дата КАК Дата," +
														"ДоговорыКонтрагентов.СрокДействия," +
														"ДоговорыКонтрагентов.Подписан," +
														"ДоговорыКонтрагентов.ЗакрытДосрочно," +
														"ДоговорыКонтрагентов.ЗаявлениеПрисоединения," +
														"ДоговорыКонтрагентов.ДатаЗаявленияПрисоединения," +
														"ДоговорыКонтрагентов.ДатаЗакрытДосрочно, " +
														"ДоговорыКонтрагентов.Объект.Наименование КАК Объект " +
													"ИЗ Справочник.ДоговорыКонтрагентов КАК ДоговорыКонтрагентов " +
													"ГДЕ ДоговорыКонтрагентов.Владелец.Код = &Код И ДоговорыКонтрагентов.ЭтоГруппа = ЛОЖЬ " +
													"УПОРЯДОЧИТЬ ПО Дата УБЫВ";

			using (var cmd = new OneSCommand(_con)) {
				cmd.CommandText = qrContragent;
				cmd.Parameters.Clear();
				cmd.Parameters.Add(new OneSParameter("ЕДРПОУ", contragentCode));

				using (var reader = cmd.ExecuteReader()) {

					if (reader.HasRows) {
						IList<Contract> contracts = new List<Contract>();
						var cmdContract = new OneSCommand(_con);
						cmdContract.CommandText = qrConcract;

						while (reader.Read()) {
							string code = reader.GetString(0).Trim();
							contracts.Clear();

							cmdContract.Parameters.Clear();
							cmdContract.Parameters.AddNew(new OneSParameter("Код", code));

							var readerContracts = cmdContract.ExecuteReader();
							if (readerContracts.HasRows) {
								while (readerContracts.Read()) {
									contracts.Add(new Contract(
										readerContracts.GetString(0),
										readerContracts.GetString(1),
										readerContracts.GetString(2),
										readerContracts.GetString(3),
										readerContracts.GetDateTime(4),
										readerContracts.GetDateTime(5),
										readerContracts.GetBoolean(6),
										readerContracts.GetBoolean(7),
										readerContracts.GetBoolean(8),
										readerContracts.GetDateTime(9),
										readerContracts.GetDateTime(10),
										readerContracts.GetString(11)
									));
								}
							}
							readerContracts.Close();


							rslt.Add(new Contragent(
								code,
								reader.GetString(1).Trim(),
								contracts,
								reader.GetString(2).Trim(),
								reader.GetString(4),
								reader.GetString(3),
								reader.GetString(5)
							));

						}
					}
				}
			}

			return rslt;
		}

		/// <summary>
		/// 
		/// </summary>
		public void Dispose()
		{
			_con.Close();
		}
	}
}
