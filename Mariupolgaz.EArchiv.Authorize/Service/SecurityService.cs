using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using Mariupolgaz.EArchiv.Common.Servises;

namespace Mariupolgaz.EArchiv.Security.Service
{
	/// <summary>
	/// Реализация интерфейса <see cref="ISecurityService"/>
	/// </summary>
	public class SecurityService : ISecurityService
	{
		private readonly string _conString = 
			ConfigurationManager
			.ConnectionStrings["Security"]
			.ConnectionString;

		public bool Login(string loginName, string password)
		{
			// Получить Identity проверить
			IIdentity identity = getIdentity(loginName, password);

			return true;
		}

		public void Logout()
		{
			throw new NotImplementedException();
		}

		private IIdentity getIdentity(string login, string password) 
		{
			using(SqlConnection con = new SqlConnection(_conString)) {
				string cmdText = "SELECT * FROM [Identities] WHERE LoginName = @Login AND IsActivity = 1";
				SqlCommand cmd = new SqlCommand(cmdText, con);
				cmd.Parameters.Clear();
				cmd.Parameters.AddWithValue("@Login", login.Trim());

				con.Open();
				SqlDataReader reader = cmd.ExecuteReader();
				
				if (!reader.HasRows) {
					con.Close();
					throw new Exception("Имя пользователя или пароль не найдены!");
				}

				reader.Read();
				byte[] hp = (byte[])reader["HashPwd"];
				SHA512Managed sha512 = new SHA512Managed();
				byte[] pwd = sha512.ComputeHash(Encoding.Unicode.GetBytes(password));
				
				if(!hp.SequenceEqual(pwd)) {
					con.Close();
					throw new Exception("Имя пользователя или пароль не найдены!");
				}

				con.Close();

				GenericIdentity idnt = new GenericIdentity(login);
				return idnt;
			}
			
		}

		/// <summary>
		/// Получить список пользователей
		/// </summary>
		/// <returns></returns>
		public IList<string> GetIdentities()
		{
			IList<string> rslt = new List<string>();
			using(SqlConnection con = new SqlConnection(_conString)) {
				SqlCommand cmd = new SqlCommand("SELECT * FROM Identities", con);
				con.Open();
				SqlDataReader reader = cmd.ExecuteReader();
				if(reader.HasRows) {
					while(reader.Read()) {
						rslt.Add(Convert.ToString(reader["LoginName"]));
					}
				}
				return rslt;
			}
		}

		/// <summary>
		/// Получить список пользователей, имеющих право работать с приложением.
		/// </summary>
		/// <param name="appName">Имя приложения</param>
		/// <returns></returns>
		public IList<string> GetIdentities(string appName)
		{
			IList<string> rslt = new List<string>();
			using (SqlConnection con = new SqlConnection(_conString)) {
				SqlCommand cmd = new SqlCommand("SELECT * FROM Identities", con);
				con.Open();
				SqlDataReader reader = cmd.ExecuteReader();
				if (reader.HasRows) {
					while (reader.Read()) {
						rslt.Add(Convert.ToString(reader["LoginName"]));
					}
				}
				return rslt;
			}
		}
	}
}
