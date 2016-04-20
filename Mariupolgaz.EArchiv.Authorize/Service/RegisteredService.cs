using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Mariupolgaz.EArchiv.Common.Servises;

namespace Mariupolgaz.EArchiv.Security.Service
{
	public class RegisteredService : IRegisteredService
	{
		private readonly string _conString =
			ConfigurationManager
			.ConnectionStrings["Security"]
			.ConnectionString;

		public bool RegisteredUser(string loginName, string password, bool isActivity = false)
		{
			bool rslt = false;
			string l = loginName.Trim();
			if (l.Length > 25) throw new Exception("Длина имени для входа не должна быть больше 25 символов");
			byte[] hp = (new SHA512Managed()).ComputeHash(
				(new StreamReader(password)).BaseStream
			);

			using(SqlConnection con = new SqlConnection(_conString)) {
				string cmdText = "INSERT Identities (LoginName, HashPwd, IsActivity) VALUES(@Login, @Pwd, @Activity)";
				SqlCommand cmd = new SqlCommand(cmdText, con);
				cmd.Parameters.Clear();
				cmd.Parameters.AddWithValue("@Login", l);
				cmd.Parameters.AddWithValue("@Pwd", hp);
				cmd.Parameters.AddWithValue("@Activity", isActivity);

				con.Open();
				rslt = (cmd.ExecuteNonQuery() > 0) ? true : false;
				con.Close();
			}

			return rslt;
		}
	}
}
