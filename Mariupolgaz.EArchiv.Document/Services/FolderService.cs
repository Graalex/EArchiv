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

		private SqlConnection _con;
		private SqlCommand _cmd;

		/// <summary>
		/// 
		/// </summary>
		public FolderService()
		{
			_con = new SqlConnection(ConfigurationManager.ConnectionStrings["Archiv"].ConnectionString);
			_cmd = new SqlCommand();
			_cmd.Connection = _con;
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
			using (_con) {
				_cmd.CommandType = CommandType.StoredProcedure;
				_cmd.CommandText = "FolderAdd";
				_cmd.Parameters.Clear();
				_cmd.Parameters.AddWithValue("@Name", folder.Name);
				_cmd.Parameters.AddWithValue("@Barcode", null);
				_cmd.Parameters.AddWithValue("@UsrName", "EArchiv");
				int key = -1;
				SqlParameter param = new SqlParameter("@Key", key);
				param.Direction = ParameterDirection.Output;
				_cmd.Parameters.Add(param);

				_con.Open();
				_cmd.ExecuteScalar();

				folder.setID(key);
				return true;
			}
		}
	}
}
