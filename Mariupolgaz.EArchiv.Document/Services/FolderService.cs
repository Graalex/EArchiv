using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Mariupolgaz.EArchiv.Common.Models;
using Mariupolgaz.EArchiv.Common.Servises;

namespace Mariupolgaz.EArchiv.Document.Services
{
	/// <summary>
	/// 
	/// </summary>
	public class FolderService : IFolderService
	{

		private string _con;

		/// <summary>
		/// 
		/// </summary>
		public FolderService()
		{
			_con = ConfigurationManager.ConnectionStrings["Archiv"].ConnectionString;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public Folder CreateFolder(string name)
		{
			return new Folder(name);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ownID"></param>
		/// <returns></returns>
		public IList<Folder> GetFolders(int ownID)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Folder LoadFolder(int id)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="folder"></param>
		/// <returns></returns>
		public bool MarkDeleteFolder(Folder folder)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="folder"></param>
		/// <returns></returns>
		public bool SaveFolder(Folder folder)
		{
			using (SqlConnection con = new SqlConnection(_con)) {
				SqlCommand cmd = new SqlCommand("FolderAdd", con);
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.Clear();
				cmd.Parameters.AddWithValue("@Name", folder.Name);
				cmd.Parameters.AddWithValue("@Barcode", null);
				cmd.Parameters.AddWithValue("@UsrName", "EArchiv");
				int key = -1;
				SqlParameter param = new SqlParameter("@Key", key);
				param.Direction = ParameterDirection.Output;
				cmd.Parameters.Add(param);

				con.Open();
				cmd.ExecuteScalar();

				folder.setID(key);
				return true;
			}
		}
	}
}
