using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Mariupolgaz.EArchiv.Common.Models;
using Mariupolgaz.EArchiv.Common.Servises;

namespace Mariupolgaz.EArchiv.Finder
{
	/// <summary>
	/// Реализует интерфейс <see cref="IFinderService"/> поиска абонентов.
	/// </summary>
	public class FinderService : IFinderService
	{
		private string _cons;

		/// <summary>
		/// Создает экземпляр <see cref="FinderService"/>.
		/// </summary>
		public FinderService()
		{
			_cons = ConfigurationManager.ConnectionStrings["Globus"].ConnectionString;
		}

		/// <summary>
		/// Находит абонента по лицевому счету
		/// </summary>
		/// <param name="ls">Номер лицевого счета</param>
		/// <returns>Найденый экземпляр <see cref="Abonent"/> или null.</returns>
		public Abonent FindAbonent(int ls)
		{
			using (SqlConnection con = new SqlConnection(_cons)) {
				SqlCommand cmd = new SqlCommand();
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Connection = con;
				cmd.CommandText = "NASTInfoByLSNomer";
				cmd.Parameters.AddWithValue("@LSNOMER", ls);

				con.Open();
				SqlDataReader reader = cmd.ExecuteReader();
				Address addr;
				Abonent abn = null;

				if (reader.HasRows) {
					reader.Read();
					addr = new Address(0, Convert.ToString(reader["NP_TYPE"]), Convert.ToString(reader["NP_NAME"]), Convert.ToString(reader["UTYP"]),
						Convert.ToString(reader["UNAM"]), Convert.ToString(reader["DOM"]), Convert.ToString(reader["LIT"]), Convert.ToInt32(reader["NKV"]));
					abn = new Abonent(Convert.ToInt32(reader["NOMER"]), Convert.ToString(reader["NAME"]), addr);
				}

				return abn;
			}
		}

		/// <summary>
		/// Находит абонентов по фамилии
		/// </summary>
		/// <param name="family">Фамилия абонета или ее часть.</param>
		/// <returns>Коллекция экземпляров <see cref="Abonent"/> или null</returns>
		public IList<Abonent> FindAbonents(string family)
		{
			using (SqlConnection con = new SqlConnection(_cons)) {
				SqlCommand cmd = new SqlCommand();
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Connection = con;
				cmd.CommandText = "NAST_InfoByFamily";
				cmd.Parameters.AddWithValue("@Family", family.Trim());

				con.Open();
				SqlDataReader reader = cmd.ExecuteReader();
				IList<Abonent> abonents = null;

				if (reader.HasRows) {
					abonents = new List<Abonent>();
					while (reader.Read()) {
						Address addr = new Address(0, Convert.ToString(reader["NP_TYPE"]), Convert.ToString(reader["NP_NAME"]), Convert.ToString(reader["UTYP"]),
							Convert.ToString(reader["UNAM"]), Convert.ToString(reader["DOM"]), Convert.ToString(reader["LIT"]), Convert.ToInt32(reader["NKV"]));
						Abonent abn = new Abonent(Convert.ToInt32(reader["NOMER"]), Convert.ToString(reader["NAME"]), addr);
						abonents.Add(abn);
					}
				}

				return abonents;
			}
		}

		/// <summary>
		/// Ищет абонентов по адресу проживания
		/// </summary>
		/// <param name="settlementName">Название нас. пункта или часть названия</param>
		/// <param name="streetName">Название улицы или часть названия</param>
		/// <returns>Коллекция экземпляров <see cref="Abonent"/> или null</returns>
		public IList<Abonent> FindAbonents(string settlementName, string streetName = null)
		{
			using (SqlConnection con = new SqlConnection(_cons)) {
				SqlCommand cmd = new SqlCommand();
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Connection = con;
				cmd.CommandText = "NAST_InfoByAddress";
				cmd.Parameters.AddWithValue("@Settlement", settlementName.Trim());
				cmd.Parameters.AddWithValue("@Street", streetName.Trim());

				con.Open();
				SqlDataReader reader = cmd.ExecuteReader();
				IList<Abonent> abonents = null;

				if (reader.HasRows) {
					abonents = new List<Abonent>();
					while (reader.Read()) {
						Address addr = new Address(0, Convert.ToString(reader["NP_TYPE"]), Convert.ToString(reader["NP_NAME"]), Convert.ToString(reader["UTYP"]),
							Convert.ToString(reader["UNAM"]), Convert.ToString(reader["DOM"]), Convert.ToString(reader["LIT"]), Convert.ToInt32(reader["NKV"]));
						Abonent abn = new Abonent(Convert.ToInt32(reader["NOMER"]), Convert.ToString(reader["NAME"]), addr);
						abonents.Add(abn);
					}
				}

				return abonents;
			}
		}
	}
}
