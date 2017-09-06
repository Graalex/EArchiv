using System;
using System.Collections.Generic;
using System.Configuration;
using FirebirdSql.Data.FirebirdClient;
using Mariupolgaz.EArchiv.Common.Models;
using Mariupolgaz.EArchiv.Common.Servises;

namespace Mariupolgaz.EArchiv.Finder
{
	/// <summary>
	/// Реализует интерфейс <see cref="IFinderService"/> поиска абонентов.
	/// </summary>
	public class FinderService : IFinderService
	{
		private string _csgl;
		private string _csgz;

		/// <summary>
		/// Создает экземпляр <see cref="FinderService"/>.
		/// </summary>
		public FinderService()
		{
            _csgz = ConfigurationManager.ConnectionStrings["Gasolina"].ConnectionString;
        }

		/// <summary>
		/// Находит абонента по лицевому счету
		/// </summary>
		/// <param name="ls">Номер лицевого счета</param>
		/// <returns>Найденый экземпляр <see cref="Abonent"/> или null.</returns>
		public Abonent FindAbonent(int ls)
		{
      using (FbConnection con = new FbConnection(_csgz)) {
				string cmdText = "select a.peracc as LS, a.name as FAMILY, a.firstname as NAME," +
					"a.patronymic as PATRONYMIC, rd.name as DISTRICT, inh.name as CITYKIND," +
					"c.name as CITY, m.name as AREA, st.name as STRTKIND, s.name as STREET, a.buildnum as HOME," +
					"a.buildlitter as LITERA, a.apartmentnum as APART, a.apartmentlitter as APTLIT " +
          "from abon a " +
          "left join street s on s.streetkey = a.streetr " +
          "left join microrajon m on m.microrajonkey = a.microrajonsr " +
          "left join streettype st on st.streettypekey = s.streettyper " +
					"left join city c on c.citykey = s.cityr " +
					"left join inhabitedlocalitytype inh on inh.inhabitedlocalitytypekey = c.inhabitedlocalitytyper " +
					"left join regdivision rd on rd.regdivisionkey = c.regdivisionr " +
					"where a.isclosed = 0 and a.peracc = '" + ls.ToString() + "'";

				FbCommand cmd = new FbCommand(cmdText, con);
				con.Open();
        FbDataReader reader = cmd.ExecuteReader();

        if (!reader.HasRows) return null;

        reader.Read();
				return new Abonent() {
					Family = (Convert.ToString(reader["FAMILY"])).Trim(),
					FirstName = Convert.IsDBNull(reader["NAME"]) ? String.Empty : (Convert.ToString(reader["NAME"])).Trim(),
					LastName = Convert.IsDBNull(reader["PATRONYMIC"]) ? String.Empty : (Convert.ToString(reader["PATRONYMIC"])).Trim(),
					LS = Convert.ToInt32(reader["LS"]),
					Address = new Address(
						-1, (Convert.ToString(reader["DISTRICT"])).Trim(), (Convert.ToString(reader["CITYKIND"])).Trim(), (Convert.ToString(reader["CITY"])).Trim(),
						(Convert.ToString(reader["AREA"])).Trim(), (Convert.ToString(reader["STRTKIND"])).Trim(), (Convert.ToString(reader["STREET"])).Trim(), 
						(Convert.ToString(reader["HOME"])).Trim(),
						Convert.IsDBNull(reader["LITERA"]) ? String.Empty : (Convert.ToString(reader["LITERA"])).Trim(),
						Convert.IsDBNull(reader["APART"]) ? 0 : Convert.ToInt32(reader["APART"])
					)
				};
			}
		}

		/// <summary>
		/// Находит абонентов по фамилии
		/// </summary>
		/// <param name="family">Фамилия абонета или ее часть.</param>
		/// <returns>Коллекция экземпляров <see cref="Abonent"/> или null</returns>
		public IList<Abonent> FindAbonents(string family)
		{
			IList<Abonent> rslt = new List<Abonent>();

			using (FbConnection con = new FbConnection(_csgz))
			{
				string cmdText = "select a.peracc as LS, a.name as FAMILY, a.firstname as NAME," +
					"a.patronymic as PATRONYMIC, rd.name as DISTRICT, inh.name as CITYKIND," +
					"c.name as CITY, m.name as AREA, st.name as STRTKIND, s.name as STREET, a.buildnum as HOME," +
					"a.buildlitter as LITERA, a.apartmentnum as APART, a.apartmentlitter as APTLIT " +
					"from abon a " +
					"left join street s on s.streetkey = a.streetr " +
					"left join microrajon m on m.microrajonkey = a.microrajonsr " +
					"left join streettype st on st.streettypekey = s.streettyper " +
					"left join city c on c.citykey = s.cityr " +
					"left join inhabitedlocalitytype inh on inh.inhabitedlocalitytypekey = c.inhabitedlocalitytyper " +
					"left join regdivision rd on rd.regdivisionkey = c.regdivisionr " +
					"where a.isclosed = 0 and a.name like '" + family + "%'";

				FbCommand cmd = new FbCommand(cmdText, con);
				con.Open();
				FbDataReader reader = cmd.ExecuteReader();

				if (reader.HasRows)
				{
					while (reader.Read())
					{
						rslt.Add(new Abonent() {
							Family = (Convert.ToString(reader["FAMILY"])).Trim(),
							FirstName = Convert.IsDBNull(reader["NAME"]) ? String.Empty : (Convert.ToString(reader["NAME"])).Trim(),
							LastName = Convert.IsDBNull(reader["PATRONYMIC"]) ? String.Empty : (Convert.ToString(reader["PATRONYMIC"])).Trim(),
							LS = Convert.ToInt32(reader["LS"]),
							Address = new Address(
								-1, (Convert.ToString(reader["DISTRICT"])).Trim(), (Convert.ToString(reader["CITYKIND"])).Trim(), (Convert.ToString(reader["CITY"])).Trim(),
								(Convert.ToString(reader["AREA"])).Trim(), (Convert.ToString(reader["STRTKIND"])).Trim(), (Convert.ToString(reader["STREET"])).Trim(),
								(Convert.ToString(reader["HOME"])).Trim(),
								Convert.IsDBNull(reader["LITERA"]) ? String.Empty : (Convert.ToString(reader["LITERA"])).Trim(),
								Convert.IsDBNull(reader["APART"]) ? 0 : Convert.ToInt32(reader["APART"])
							)
						});
					}
				}
			}

			return rslt;
		}

		/// <param name="family">Часть фамилии абонента</param>
		/// <param name="settlement">Название нас. п.</param>
		/// <param name="street">Название улицы</param>
		/// <param name="house">Номер дома</param>
		/// <param name="appartment">Номер квартиры</param>
		/// <returns>Список экземпляров Abonent</returns>
		public IList<Abonent> FindAbonents(string family, string settlement = null, string street = null, string house = null, int? appartment = null)
		{
			IList<Abonent> rslt = new List<Abonent>();

			using (FbConnection con = new FbConnection(_csgz))
			{
				string cityCond = settlement != null ? " and c.name = '" + settlement + "'" : null;
				string streetCond = street != null ? " and s.name like '" + street + "%'" : null;
				string houseCond = house != null ? " and a.buildnum like '" + house + "%'" : null;

				string cmdText = @"select a.peracc as LS, a.name as FAMILY, a.firstname as NAME,
					a.patronymic as PATRONYMIC, rd.name as DISTRICT, inh.name as CITYKIND,
					c.name as CITY, m.name as AREA, st.name as STRTKIND, s.name as STREET, a.buildnum as HOME,
					a.buildlitter as LITERA, a.apartmentnum as APART, a.apartmentlitter as APTLIT
					from abon a
					left join street s on s.streetkey = a.streetr
					left join microrajon m on m.microrajonkey = a.microrajonsr
					left join streettype st on st.streettypekey = s.streettyper
					left join city c on c.citykey = s.cityr
					left join inhabitedlocalitytype inh on inh.inhabitedlocalitytypekey = c.inhabitedlocalitytyper
					left join regdivision rd on rd.regdivisionkey = c.regdivisionr
					where a.isclosed = 0 and a.name like '" + family + "%'" + cityCond + streetCond + houseCond;
				FbCommand cmd = new FbCommand(cmdText, con);
				con.Open();
				FbDataReader reader = cmd.ExecuteReader();

				if (reader.HasRows)
					while (reader.Read())
						rslt.Add(new Abonent() {
							Family = (Convert.ToString(reader["FAMILY"])).Trim(),
							FirstName = Convert.IsDBNull(reader["NAME"]) ? String.Empty : (Convert.ToString(reader["NAME"])).Trim(),
							LastName = Convert.IsDBNull(reader["PATRONYMIC"]) ? String.Empty : (Convert.ToString(reader["PATRONYMIC"])).Trim(),
							LS = Convert.ToInt32(reader["LS"]),
							Address = new Address(
								-1, (Convert.ToString(reader["DISTRICT"])).Trim(), (Convert.ToString(reader["CITYKIND"])).Trim(), (Convert.ToString(reader["CITY"])).Trim(),
								(Convert.ToString(reader["AREA"])).Trim(), (Convert.ToString(reader["STRTKIND"])).Trim(), (Convert.ToString(reader["STREET"])).Trim(),
								(Convert.ToString(reader["HOME"])).Trim(),
								Convert.IsDBNull(reader["LITERA"]) ? String.Empty : (Convert.ToString(reader["LITERA"])).Trim(),
								Convert.IsDBNull(reader["APART"]) ? 0 : Convert.ToInt32(reader["APART"])
							)
						});
			}

			return rslt;
		}

		/// <summary>
		/// Получает список населенных пунктов
		/// </summary>
		/// <returns>Список населенных пунктов или null</returns>
		public IList<Settlement> GetSettlementList()
		{
			IList<Settlement> rslt = new List<Settlement>();

			using (FbConnection con = new FbConnection(_csgz))
			{
				string cmdText = @"select c.citykey as ID, inh.name as CITY_KIND, c.name as NAME
					from CITY c
					left join inhabitedlocalitytype inh on inh.inhabitedlocalitytypekey = c.inhabitedlocalitytyper
					order by c.name";

				FbCommand cmd = new FbCommand(cmdText, con);
				con.Open();
				FbDataReader reader = cmd.ExecuteReader();

				if (reader.HasRows)
				{
					while (reader.Read())
					{
						rslt.Add(new Settlement(
							Convert.ToInt32(reader["ID"]),
							Convert.ToString(reader["CITY_KIND"]).Trim().ToLower(),
							Convert.ToString(reader["NAME"]).Trim()
						));
					}
				}
			}

			return rslt;
		}

		/// <summary>
		/// Возвращает список объектов типа <see cref="Street"/> для насаленного пункта.
		/// </summary>
		/// <param name="settlementID">Идентификатор нас. пункта</param>
		/// <returns>Список объектов <see cref="Street"/> или null</returns>
		public IList<Street> GetStreetsList(int settlementID)
		{
			throw new NotImplementedException("Метод не реализован");
		}
	}
}
