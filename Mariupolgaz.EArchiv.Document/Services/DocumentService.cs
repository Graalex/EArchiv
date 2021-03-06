﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using comm = Mariupolgaz.EArchiv.Common.Models;
using Mariupolgaz.EArchiv.Common.Servises;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Media;

namespace Mariupolgaz.EArchiv.Document.Services
{
	/// <summary>
	/// 
	/// </summary>
	public class DocumentService : IDocumentService
	{
		private string _con;
		private IEnumerable<comm.DocumentKind> _kinds;
		
		/// <summary>
		/// 
		/// </summary>
		public DocumentService()
		{
			_con = ConfigurationManager.ConnectionStrings["Archiv"].ConnectionString;
			//TODO: Костыль позже переделать
			_kinds = this.GetKinds();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="kind"></param>
		/// <returns></returns>
		public comm.Document CreateDocument(string name, comm.DocumentKind kind)
		{
			return new Common.Models.Document(name, kind);
		}

		public comm.DocumentClass GetDocumentClass(int docID)
		{
			throw new NotImplementedException();
		}

		public comm.DocumentKind GetDocumentKind(int docID)
		{
			throw new NotImplementedException();
		}

		public int GetDocumentLS(int docID)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Возвращает список документов по лицевому счету
		/// </summary>
		/// <param name="ls"></param>
		/// <returns></returns>
		public IList<comm.Document> GetDocuments(int ls)
		{
			using (SqlConnection con = new SqlConnection(_con)) {
				IList<Common.Models.Document> list = null;
				SqlCommand cmd = new SqlCommand();
				cmd.Connection = con;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "GetDocByLS";
				cmd.Parameters.Clear();
				cmd.Parameters.AddWithValue("@LS", ls);
				cmd.Parameters.AddWithValue("@Kind", -1);

				con.Open();
				SqlDataReader reader = cmd.ExecuteReader();
				if (reader.HasRows) {
					
					list = new List<Common.Models.Document>();
					while (reader.Read()) {
						/*
						BitmapImage thrumb = new BitmapImage();
						thrumb.BeginInit();
						MemoryStream stream = new MemoryStream((byte[])reader["Thumbnails"]);
						thrumb.StreamSource = stream;
						thrumb.EndInit();
						*/

						BitmapImage src = new BitmapImage();
						src.BeginInit();
						MemoryStream stream = new MemoryStream((byte[])reader["Source"]);
						src.StreamSource = stream;
						src.EndInit();
						
						int id = Convert.ToInt32(reader["Kind"]);
						var kind = _kinds.First(item => item.ID == id);

						list.Add(new Common.Models.Document(Convert.ToInt32(reader["ID"]), kind, Convert.ToString(reader["Name"]), (byte[])reader["Hash"], null /*thrumb*/,
							Convert.ToDateTime(reader["CreateAt"]), Convert.ToDateTime(reader["ModifyAt"]), Convert.ToBoolean(reader["IsMarkDelete"]), src));
					}
				}

				return list;
			}
		}

		/// <summary>
		/// Возвращает список документов по ЕДРПОУ предприятия и коду договора в базе 1С
		/// </summary>
		/// <param name="orgCode"></param>
		/// <param name="contractCode"></param>
		/// <returns></returns>
		public IList<comm.ContractDocument> GetDocuments(string orgCode, string contractCode)
		{
			IList<comm.ContractDocument> rslt = new List<comm.ContractDocument>();

			using(SqlConnection con = new SqlConnection(_con)) {
				using(SqlCommand cmd = new SqlCommand("GetDocByContract", con)) {
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@OrgCode", orgCode.Trim());
					cmd.Parameters.AddWithValue("@ContractCode", contractCode.Trim());
					con.Open();

					using(SqlDataReader reader = cmd.ExecuteReader()) {
						if(reader.HasRows) {
							while(reader.Read()) {
								BitmapImage src = new BitmapImage();
								src.BeginInit();
								MemoryStream stream = new MemoryStream((byte[])reader["Source"]);
								src.StreamSource = stream;
								src.EndInit();

								int id = Convert.ToInt32(reader["Kind"]);
								var kind = _kinds.First(item => item.ID == id);

								rslt.Add(
									new Common.Models.ContractDocument(
										Convert.ToInt32(reader["ID"]), kind, Convert.ToString(reader["Name"]), (byte[])reader["Hash"], null /*thrumb*/,
										Convert.ToDateTime(reader["CreateAt"]), Convert.ToDateTime(reader["ModifyAt"]), Convert.ToBoolean(reader["IsMarkDelete"]),
										src, Convert.ToString(reader["AddUser"]),
										Convert.ToDateTime(reader["DocumentDate"]), Convert.ToString(reader["DocumentNomer"])
								));
							}
						}
					}

					cmd.CommandText = "GetUsrWorks";
					cmd.Parameters.Clear();
					cmd.Parameters.AddWithValue("@TableName", "DocAttr");
					cmd.Parameters.AddWithValue("@Operation", "UPD");
					cmd.Parameters.AddWithValue("@RecID", 0);

					foreach (var item in rslt) {
						cmd.Parameters["@RecID"].Value = item.ID;

						using (SqlDataReader r = cmd.ExecuteReader()) {
							if (r.HasRows) {
								while (r.Read()) {
									item.UsersModifity.Add(
										new comm.UserMod(
											Convert.ToString(r["UsrName"]),
											Convert.ToDateTime(r["EventTime"])
										)
									);
								}
							}
						}
					}
        }
			}

			return rslt;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ls"></param>
		/// <param name="kindID"></param>
		/// <returns></returns>
		public IList<comm.Document> GetDocuments(int ls, int kindID)
		{
			using(SqlConnection con = new SqlConnection(_con)) {
				IList<Common.Models.Document> list = null;
				SqlCommand cmd = new SqlCommand();
				cmd.Connection = con;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "GetDocByLS";
				cmd.Parameters.Clear();
				cmd.Parameters.AddWithValue("@LS", ls);
				cmd.Parameters.AddWithValue("@Kind", kindID);

				con.Open();
				SqlDataReader reader = cmd.ExecuteReader();
				if(reader.HasRows) {
					list = new List<Common.Models.Document>();
					while(reader.Read()) {
						/*
						BitmapImage thrumb = new BitmapImage();
						thrumb.BeginInit();
						MemoryStream stream = new MemoryStream((byte[])reader["Thumbnails"]);
						thrumb.StreamSource = stream;
						thrumb.EndInit();
						*/

						BitmapImage src = new BitmapImage();
						src.BeginInit();
						MemoryStream stream = new MemoryStream((byte[])reader["Source"]);
						src.StreamSource = stream;
						src.EndInit();

						int id = Convert.ToInt32(reader["Kind"]);
						var kind = _kinds.First(item => item.ID == id);

						list.Add(new Common.Models.Document(Convert.ToInt32(reader["ID"]), kind, Convert.ToString(reader["Name"]), (byte[])reader["Hash"], null /*thrumb*/,
							Convert.ToDateTime(reader["CreateAt"]), Convert.ToDateTime(reader["ModifyAt"]), Convert.ToBoolean(reader["IsMarkDelete"]), src));
					}
				}

				return list;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="docID"></param>
		/// <returns></returns>
		public comm.Document LoadDocument(int docID)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="doc"></param>
		/// <returns></returns>
		public bool MarkDeleteDocument(comm.Document doc)
		{
			using (SqlConnection con = new SqlConnection(_con))
			{
				try {
					bool rslt = false;

					SqlCommand cmd = new SqlCommand("DocDelMark", con);
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@Key", doc.ID);

					con.Open();

					cmd.ExecuteNonQuery();
					rslt = true;

					return rslt;
				}

				catch(Exception e) {
					throw new Exception(e.Message);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="doc"></param>
		/// <param name="folder"></param>
		/// <param name="ls"></param>
		public void SaveDocument(comm.Document doc, comm.Folder folder, int ls)
		{
			using(SqlConnection con = new SqlConnection(_con)) {
				SqlCommand cmd = new SqlCommand();
				cmd.Connection = con;
				cmd.CommandType = CommandType.StoredProcedure;

				con.Open();
				SqlTransaction tranc = con.BeginTransaction();
				cmd.Transaction = tranc;

				try {
					//первая часть сохраняем сам документ
					cmd.CommandText = "DocAdd";

					cmd.Parameters.Clear();
					cmd.Parameters.AddWithValue("@Name", doc.Name);
					cmd.Parameters.AddWithValue("@Kind", doc.Kind.ID);
					cmd.Parameters.AddWithValue("@UsrName", "EArchiv");
					cmd.Parameters.AddWithValue("@Hash", doc.ConvertHash());

					//TODO: Костыль позже убрать
					long len;
					byte[] buf;
					if (doc.Thumbnails != null) {
						len = doc.Thumbnails.StreamSource.Length;
						buf = new byte[len];
						doc.Thumbnails.StreamSource.Read(buf, 0, (int)len);
						cmd.Parameters.AddWithValue("@Thumbnails", buf);
					} else {
							buf = new byte[2];
							buf[0] = 1;
							buf[1] = 2;
							cmd.Parameters.AddWithValue("@Thumbnails", buf);
					}

					FileStream s = new FileStream((doc.Source.StreamSource as FileStream).Name, FileMode.Open);
					
					len = s.Length;
					buf = new byte[len];
					s.Position = 0;
          s.Read(buf, 0, (int)len);
					cmd.Parameters.AddWithValue("@Raw", buf);

					int key = -1;
					SqlParameter parametr = new SqlParameter("@Key", key);
					parametr.Direction = ParameterDirection.Output;
					cmd.Parameters.Add(parametr);

					cmd.ExecuteNonQuery();
					doc.setID(Convert.ToInt32(cmd.Parameters["@Key"].Value));

					//вторая часть добавляем документ в папку
					//TODO: Надо подумать как переделать
					/*
					cmd.CommandText = "DocFolderAdd";

					cmd.Parameters.Clear();
					cmd.Parameters.AddWithValue("@DocID", key);
					cmd.Parameters.AddWithValue("FolderID", folder.ID);
					cmd.Parameters.AddWithValue("UsrName", "EArchiv");

					parametr = new SqlParameter("@Key", key);
					parametr.Direction = ParameterDirection.Output;
					cmd.Parameters.Add(parametr);

					cmd.ExecuteNonQuery();
					*/

					// третья часть связываем документ с лицевым счетом
					cmd.CommandText = "DocLsAdd";

					cmd.Parameters.Clear();
					cmd.Parameters.AddWithValue("@Doc", doc.ID);
					cmd.Parameters.AddWithValue("Ls", ls);
					cmd.Parameters.AddWithValue("UsrName", "EArchiv");

					parametr = new SqlParameter("@Key", key);
					parametr.Direction = ParameterDirection.Output;
					cmd.Parameters.Add(parametr);

					cmd.ExecuteNonQuery();

					tranc.Commit();
				}

				catch(Exception e) {
					try {
						tranc.Rollback();
						throw new Exception(e.Message);
					}

					catch(Exception ex) {
						throw new Exception(ex.Message);
					}
				}
			}

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="doc"></param>
		/// <param name="orgCode"></param>
		/// <param name="contractCode"></param>
		/// <param name="user"></param>
		public void SaveDocument(comm.ContractDocument doc, string orgCode, string contractCode, string user)
		{
			using(SqlConnection con = new SqlConnection(_con)) {
				using(SqlCommand cmd = new SqlCommand()) {
					cmd.Connection = con;
					cmd.CommandType = CommandType.StoredProcedure;

					con.Open();
					SqlTransaction tranc = con.BeginTransaction();
					cmd.Transaction = tranc;

					try {
						//первая часть сохраняем сам документ
						cmd.CommandText = "DocAdd";

						cmd.Parameters.Clear();
						cmd.Parameters.AddWithValue("@Name", doc.Name);
						cmd.Parameters.AddWithValue("@Kind", doc.Kind.ID);
						cmd.Parameters.AddWithValue("@UsrName", user.Trim());
						cmd.Parameters.AddWithValue("@Hash", doc.ConvertHash());

						//TODO: Костыль позже убрать
						long len;
						byte[] buf;
						if (doc.Thumbnails != null) {
							len = doc.Thumbnails.StreamSource.Length;
							buf = new byte[len];
							doc.Thumbnails.StreamSource.Read(buf, 0, (int)len);
							cmd.Parameters.AddWithValue("@Thumbnails", buf);
						}
						else {
							buf = new byte[2];
							buf[0] = 1;
							buf[1] = 2;
							cmd.Parameters.AddWithValue("@Thumbnails", buf);
						}

						FileStream s = new FileStream((doc.Source.StreamSource as FileStream).Name, FileMode.Open);

						len = s.Length;
						buf = new byte[len];
						s.Position = 0;
						s.Read(buf, 0, (int)len);
						cmd.Parameters.AddWithValue("@Raw", buf);

						int key = -1;
						SqlParameter parametr = new SqlParameter("@Key", key);
						parametr.Direction = ParameterDirection.Output;
						cmd.Parameters.Add(parametr);

						cmd.ExecuteNonQuery();
						doc.setID(Convert.ToInt32(cmd.Parameters["@Key"].Value));

						// вторая часть связываем документ с договором
						cmd.CommandText = "DocContractAdd";

						cmd.Parameters.Clear();
						cmd.Parameters.AddWithValue("@Doc", doc.ID);
						cmd.Parameters.AddWithValue("@OrgCode", orgCode);
						cmd.Parameters.AddWithValue("@ContractCode", contractCode);
						cmd.Parameters.AddWithValue("@ContractDate", doc.DocumentDate);
						cmd.Parameters.AddWithValue("@ContractNomer", doc.DocumentNumber);
						cmd.Parameters.AddWithValue("UsrName", user.Trim());

						parametr = new SqlParameter("@Key", key);
						parametr.Direction = ParameterDirection.Output;
						cmd.Parameters.Add(parametr);

						cmd.ExecuteNonQuery();

						tranc.Commit();
					}

					catch(Exception e) {
						tranc.Rollback();
						throw new Exception(e.Message);
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="classID"></param>
		/// <returns></returns>
		public IList<comm.DocumentKind> GetKindsByClass(int classID)
		{
			using (SqlConnection con = new SqlConnection(_con)) {
				SqlCommand cmd = new SqlCommand();
				cmd.Connection = con;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "DocKindsGetForClass";
				cmd.Parameters.Clear();
				cmd.Parameters.AddWithValue("@Class", classID);
				IList<comm.DocumentKind> list = null;

				con.Open();
				SqlDataReader reader = cmd.ExecuteReader();

				if (reader.HasRows) {
					list = new List<comm.DocumentKind>();
					while (reader.Read()) {
						list.Add(new comm.DocumentKind(Convert.ToInt32(reader["KEY"]), Convert.ToString(reader["NAME"]), Convert.ToBoolean(reader["ISMARKDEL"])));
					}
				}

				return list;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public IList<comm.DocumentKind> GetKinds()
		{
			IList<comm.DocumentKind> rslt = new List<comm.DocumentKind>();

			using(SqlConnection con = new SqlConnection(_con)) {
				using(SqlCommand cmd = new SqlCommand("DocKindsgetAll", con)) {
					cmd.CommandType = CommandType.StoredProcedure;
					con.Open();

					using(SqlDataReader reader = cmd.ExecuteReader()) {
						if (reader.HasRows) {
							while(reader.Read()) {
								rslt.Add(
									new Common.Models.DocumentKind(Convert.ToInt32(reader["KEY"]), Convert.ToString(reader["KIND_NAME"]), false)
								);
							}
						}
					}
				}
			}

			return rslt;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public IList<comm.DocumentClass> GetClasses()
		{
			using(SqlConnection con = new SqlConnection(_con)) {
				SqlCommand cmd = new SqlCommand();
				cmd.Connection = con;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "DocClassGetAll";
				IList<comm.DocumentClass> list = null;

				con.Open();
				SqlDataReader reader = cmd.ExecuteReader();

				if(reader.HasRows) {
					list = new List<comm.DocumentClass>();
					while(reader.Read()) {
						list.Add(new comm.DocumentClass(Convert.ToInt32(reader["KEY"]), Convert.ToString(reader["NAME"]), Convert.ToBoolean(reader["IS_MARK_DEL"])));
					}
				}

				return list;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="doc"></param>
		public void SaveDocumentAttributes(comm.Document doc)
		{
			using(SqlConnection con = new SqlConnection(_con)) {
				SqlCommand cmd = new SqlCommand();
				cmd.Connection = con;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "DocAttrSet";
				cmd.Parameters.Clear();
				cmd.Parameters.AddWithValue("@Key", doc.ID);
				cmd.Parameters.AddWithValue("@Name", doc.Name);
				cmd.Parameters.AddWithValue("@Kind", doc.Kind.ID);
				cmd.Parameters.AddWithValue("@ModifyAt", doc.ModifyAt);
				cmd.Parameters.AddWithValue("@UsrName", "EArchiv");

				con.Open();
				cmd.ExecuteNonQuery();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="doc"></param>
		public void SaveDocumentSource(comm.Document doc)
		{
			using(SqlConnection con = new SqlConnection(_con)) {
				SqlCommand cmd = new SqlCommand();
				cmd.Connection = con;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "DocSet";
				cmd.Parameters.Clear();
				cmd.Parameters.AddWithValue("@Key", doc.ID);
				cmd.Parameters.AddWithValue("@Name", doc.Name);
				cmd.Parameters.AddWithValue("@Kind", doc.Kind.ID);
				cmd.Parameters.AddWithValue("@ModifyAt", doc.ModifyAt);
				cmd.Parameters.AddWithValue("@UsrName", "EArchiv");
				cmd.Parameters.AddWithValue("@Hash", doc.ConvertHash());

				long len;
				byte[] buf;

				/*
				long len = doc.Thumbnails.StreamSource.Length;
				byte[] buf = new byte[len];
				doc.Thumbnails.StreamSource.Read(buf, 0, (int)len);
				cmd.Parameters.AddWithValue("Thumbnails", buf);
				*/
				buf = new byte[2];
				buf[0] = 1;
				buf[1] = 2;
				cmd.Parameters.AddWithValue("@Thumbnails", buf);

				FileStream s = new FileStream((doc.Source.StreamSource as FileStream).Name, FileMode.Open);
				len = s.Length;
				buf = new byte[len];
				s.Position = 0;
				s.Read(buf, 0, (int)len);
				cmd.Parameters.AddWithValue("@Raw", buf);
				
				con.Open();
				cmd.ExecuteNonQuery();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="doc"></param>
		/// <param name="user"></param>
		public void SaveDocumentSource(comm.ContractDocument doc, string user)
		{
			using (SqlConnection con = new SqlConnection(_con)) {
				SqlCommand cmd = new SqlCommand();
				cmd.Connection = con;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "DocSet";
				cmd.Parameters.Clear();
				cmd.Parameters.AddWithValue("@Key", doc.ID);
				cmd.Parameters.AddWithValue("@Name", doc.Name);
				cmd.Parameters.AddWithValue("@Kind", doc.Kind.ID);
				cmd.Parameters.AddWithValue("@ModifyAt", doc.ModifyAt);
				cmd.Parameters.AddWithValue("@UsrName", user.Trim());
				cmd.Parameters.AddWithValue("@Hash", doc.ConvertHash());

				long len;
				byte[] buf;

				/*
				long len = doc.Thumbnails.StreamSource.Length;
				byte[] buf = new byte[len];
				doc.Thumbnails.StreamSource.Read(buf, 0, (int)len);
				cmd.Parameters.AddWithValue("Thumbnails", buf);
				*/
				buf = new byte[2];
				buf[0] = 1;
				buf[1] = 2;
				cmd.Parameters.AddWithValue("@Thumbnails", buf);

				FileStream s = new FileStream((doc.Source.StreamSource as FileStream).Name, FileMode.Open);
				len = s.Length;
				buf = new byte[len];
				s.Position = 0;
				s.Read(buf, 0, (int)len);
				cmd.Parameters.AddWithValue("@Raw", buf);

				con.Open();
				cmd.ExecuteNonQuery();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="newFolder"></param>
		public void ChangeDocumentFolder(int key, comm.Folder newFolder)
		{
			using(SqlConnection con = new SqlConnection(_con)) {
				SqlCommand cmd = new SqlCommand();
				cmd.Connection = con;
				cmd.CommandText = "DocFolderChange";
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.Clear();
				cmd.Parameters.AddWithValue("@Key", key);
				cmd.Parameters.AddWithValue("@Folder", newFolder.ID);
				cmd.Parameters.AddWithValue("@UsrName", "EArchiv");

				con.Open();
				cmd.ExecuteNonQuery();
      }
		}
	}
}
