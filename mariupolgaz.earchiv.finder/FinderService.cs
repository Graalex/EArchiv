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
				cmd.CommandText = "GZL_GetAbonentByLS";
				cmd.Parameters.AddWithValue("@Ls", ls);

				con.Open();
				SqlDataReader reader = cmd.ExecuteReader();
				Address addr;
				Abonent abn = null;

				if (reader.HasRows) {
					reader.Read();
					string city = Convert.ToString(reader["CITY"]).Trim();
					/*
					int idx = city.LastIndexOf(' ');
					string np = city.Substring(0, idx);
					string npt = city.Substring(idx+1).ToLower();
					*/

					string ul = Convert.ToString(reader["UL"]).Trim();
					/*
					idx = ul.LastIndexOf(' ');
					string uln = ul.Substring(0, idx);
					string ult = ul.Substring(idx+1).ToLower();
					*/

					addr = new Address(0, null /*npt*/, city /*np*/, null /*ult*/, ul /*uln*/,	Convert.ToString(reader["NOM"]).Trim(), 
						Convert.ToString(reader["LIT"]), Convert.ToInt32(reader["NKV"]));
					abn = new Abonent(Convert.ToInt32(reader["NOMER"]), Convert.ToString(reader["NAME"]).Trim(), addr);
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
				cmd.CommandText = "GZL_GetAbonentsByFamily";
				cmd.Parameters.AddWithValue("@Family", family.Trim());

				con.Open();
				SqlDataReader reader = cmd.ExecuteReader();
				IList<Abonent> abonents = null;

				if (reader.HasRows) {
					abonents = new List<Abonent>();
					while (reader.Read()) {
						string city = Convert.ToString(reader["CITY"]).Trim();
						/*
						int idx = city.LastIndexOf(' ');
						string np = city.Substring(0, idx);
						string npt = city.Substring(idx + 1).ToLower();
						*/

						string ul = Convert.ToString(reader["UL"]).Trim();
						/*
						idx = ul.LastIndexOf(' ');
						string uln = ul.Substring(0, idx);
						string ult = ul.Substring(idx + 1).ToLower();
						*/

						Address addr = new Address(0, null /*npt*/, city /*np*/, null /*ult*/, ul /*uln*/, Convert.ToString(reader["NOM"]).Trim(),
							Convert.ToString(reader["LIT"]), Convert.ToInt32(reader["NKV"]));
						Abonent abn = new Abonent(Convert.ToInt32(reader["NOMER"]), Convert.ToString(reader["NAME"]).Trim(), addr);
						abonents.Add(abn);
					}
				}
				
				return abonents;
			}
		}

		/// <param name="family">Часть фамилии абонента</param>
		/// <param name="settlement">Название нас. п.</param>
		/// <param name="street">Название улицы</param>
		/// <param name="house">Номер дома</param>
		/// <param name="appartment">Номер квартиры</param>
		/// <returns>Список экземпляров Abonent</returns>
		public IList<Abonent> FindAbonents(
			string family, 
			string settlement = null, 
			string street = null, 
			string house = null, 
			int? appartment = null
		)
		{
			using (SqlConnection con = new SqlConnection(_cons)) {
				SqlCommand cmd = new SqlCommand();
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Connection = con;
				cmd.CommandText = "GZL_GetAbonentsByAddress";

				SqlParameter param = new SqlParameter();
				param.IsNullable = true;
				param.ParameterName = "@Family";
				param.Value = family ?? (object)DBNull.Value;
				cmd.Parameters.Add(param);

				param = new SqlParameter();
				param.IsNullable = true;
				param.ParameterName = "@Settlement";
				param.Value = settlement ?? (object)DBNull.Value;
				cmd.Parameters.Add(param);

				param = new SqlParameter();
				param.IsNullable = true;
				param.ParameterName = "@Street";
				param.Value = street ?? (object)DBNull.Value;
				cmd.Parameters.Add(param);

				param = new SqlParameter();
				param.IsNullable = true;
				param.ParameterName = "@House";
				param.Value = house ?? (object)DBNull.Value;
				cmd.Parameters.Add(param);

				param = new SqlParameter();
				param.IsNullable = true;
				param.ParameterName = "@Appartment";
				param.Value = appartment ?? (object)DBNull.Value;
				cmd.Parameters.Add(param);

				con.Open();
				SqlDataReader reader = cmd.ExecuteReader();
				IList<Abonent> abonents = null;

				if (reader.HasRows) {
					abonents = new List<Abonent>();
					while (reader.Read()) {
						string city = Convert.ToString(reader["CITY"]).Trim();
						/*
						int idx = city.LastIndexOf(' ');
						string np = city.Substring(0, idx);
						string npt = city.Substring(idx + 1).ToLower();
						*/

						string ul = Convert.ToString(reader["UL"]).Trim();
						/*
						idx = ul.LastIndexOf(' ');
						string uln = ul.Substring(0, idx);
						string ult = ul.Substring(idx + 1).ToLower();
						*/

						Address addr = new Address(0, null /*npt*/, city /*np*/, null /*ult*/, ul /*uln*/, Convert.ToString(reader["NOM"]).Trim(),
							Convert.ToString(reader["LIT"]), Convert.ToInt32(reader["NKV"]));
						Abonent abn = new Abonent(Convert.ToInt32(reader["NOMER"]), Convert.ToString(reader["NAME"]).Trim(), addr);
						abonents.Add(abn);
					}
				}
				
				return abonents;
			}
		}

		/// <summary>
		/// Получает список населенных пунктов
		/// </summary>
		/// <returns>Список населенных пунктов или null</returns>
		public IList<Settlement> GetSettlementList()
		{
			IList<Settlement> rslt = null;

			using(SqlConnection con = new SqlConnection(_cons)) {
				string cmdText = "SELECT * FROM GZL_SPRNP";
				SqlCommand cmd = new SqlCommand(cmdText, con);
				con.Open();
				SqlDataReader reader = cmd.ExecuteReader();
				if(reader.HasRows) {
					rslt = new List<Settlement>();
					while(reader.Read()) {
						rslt.Add(
							new Settlement(Convert.ToInt32(reader["NOMER"]), Convert.ToString(reader["NAME"]).Trim())
						);
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
			IList<Street> rslt = null;

			using(SqlConnection con = new SqlConnection(_cons)) {
				string cmdText = "SELECT NOMER, UNAM FROM GZL_SPRUL WHERE NP = @NP";
				SqlCommand cmd = new SqlCommand(cmdText, con);
				cmd.Parameters.AddWithValue("@NP", settlementID);

				con.Open();
				SqlDataReader reader = cmd.ExecuteReader();

				if(reader.HasRows) {
					rslt = new List<Street>();
					while(reader.Read()) {
						rslt.Add(
							new Street(
								Convert.ToInt32(reader["NOMER"]),
								Convert.ToString(reader["UNAM"]).Trim()
							)
						);
					}
				}
			}

			return rslt;
		}
	}
}
