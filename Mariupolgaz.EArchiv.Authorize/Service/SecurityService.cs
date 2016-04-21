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
			IIdentity idnt = getIdentity(loginName, password);
			// Создать пользователя и определить его права
			throw new NotImplementedException();
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
				byte[] b = Encoding.Unicode.GetBytes(password);
				byte[] pwd = sha512.ComputeHash(b);
				
				bool r = hp.SequenceEqual(pwd);
				if(!r) {
					con.Close();
					throw new Exception("Имя пользователя или пароль не найдены!");
				}

				con.Close();

				GenericIdentity idnt = new GenericIdentity(login);
				return idnt;
			}
			
		}
	}
}
