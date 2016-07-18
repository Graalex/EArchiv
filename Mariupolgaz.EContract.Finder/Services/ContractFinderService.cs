﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mariupolgaz.EArchiv.Common.Models;
using Mariupolgaz.EArchiv.Common.Servises;
using VanessaSharp.Data;

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
			IList<Contragent> rslt = new List<Contragent>();

			var conBuilder = new OneSConnectionStringBuilder
			{
				ServerName = "serv",
				ServerDatabaseName = "base_upp",
				User = "Григорчук А М",
				Password = "ufhujpfdh"
			};


			using (var con = new OneSConnection(conBuilder.ConnectionString)) {
				string qrContragent = "ВЫБРАТЬ " +
														"Контрагенты.Код КАК Код," +
														"Контрагенты.Наименование," +
														"Контрагенты.НаименованиеПолное," +
														"Контрагенты.ИНН," +
														"Контрагенты.КодПоЕДРПОУ," +
														"Контрагенты.НомерСвидетельства " +
											   "ИЗ Справочник.Контрагенты КАК Контрагенты " +
											   "ГДЕ Контрагенты.Наименование ПОДОБНО &Наименование " +
											   "УПОРЯДОЧИТЬ ПО Код";
				string qrConcract = "ВЫБРАТЬ " +
															"ДоговорыКонтрагентов.Код," +
															"ДоговорыКонтрагентов.Наименование," +
															"ДоговорыКонтрагентов.Номер," +
															"ДоговорыКонтрагентов.Дата КАК Дата," +
															"ДоговорыКонтрагентов.СрокДействия," +
															"ДоговорыКонтрагентов.Подписан," +
															"ДоговорыКонтрагентов.ЗакрытДосрочно," +
															"ДоговорыКонтрагентов.ЗаявлениеПрисоединения," +
															"ДоговорыКонтрагентов.ДатаЗаявленияПрисоединения," +
															"ДоговорыКонтрагентов.ДатаЗакрытДосрочно " +
														"ИЗ Справочник.ДоговорыКонтрагентов КАК ДоговорыКонтрагентов " +
														"ГДЕ ДоговорыКонтрагентов.Владелец.Код = &Код " +
														"УПОРЯДОЧИТЬ ПО Дата УБЫВ";

				using (var cmd = new OneSCommand(con)) {
					cmd.CommandText = qrContragent;
					cmd.Parameters.Clear();
					cmd.Parameters.AddNew(new OneSParameter("Наименование", "%" + contragentName + "%"));

					con.Open();

					using (var reader = cmd.ExecuteReader()) {

						if (reader.HasRows) {
							IList<Contract> contracts = new List<Contract>();
							var cmdContract = new OneSCommand(con);
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
											readerContracts.GetDateTime(3),
											readerContracts.GetDateTime(4),
											readerContracts.GetBoolean(5),
											readerContracts.GetBoolean(6),
											readerContracts.GetBoolean(7),
											readerContracts.GetDateTime(8),
											readerContracts.GetDateTime(9)
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

						reader.Close();
					}
				}
			}

			
			

			return rslt;
		}
	}
}