using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using Mariupolgaz.EArchiv.Common.Models;
using Mariupolgaz.EArchiv.Common.Servises;
using System.Windows.Media.Imaging;
using System.IO;

namespace Mariupolgaz.EArchiv.Document.Services
{
	/// <summary>
	/// 
	/// </summary>
	public class DocumentService : IDocumentService
	{
		private SqlConnection _con;
		private SqlCommand _cmd;

		/// <summary>
		/// 
		/// </summary>
		public DocumentService()
		{
			_con = new SqlConnection(ConfigurationManager.ConnectionStrings["Archiv"].ConnectionString);
			_cmd = new SqlCommand();
			_cmd.Connection = _con;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="kind"></param>
		/// <returns></returns>
		Common.Models.Document IDocumentService.CreateDocument(string name, int kind)
		{
			return new Common.Models.Document(name, kind);
		}

		DocumentClass IDocumentService.GetDocumentClass(int docID)
		{
			throw new NotImplementedException();
		}

		DocumentKind IDocumentService.GetDocumentKind(int docID)
		{
			throw new NotImplementedException();
		}

		int IDocumentService.GetDocumentLS(int docID)
		{
			throw new NotImplementedException();
		}

		IList<Common.Models.Document> IDocumentService.GetDocuments(int ls)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ls"></param>
		/// <param name="kindID"></param>
		/// <returns></returns>
		IList<Common.Models.Document> IDocumentService.GetDocuments(int ls, int kindID)
		{
			using(_con) {
				IList<Common.Models.Document> list = null;
				_cmd.CommandType = CommandType.StoredProcedure;
				_cmd.CommandText = "GetDocByLS";
				_cmd.Parameters.Clear();
				_cmd.Parameters.AddWithValue("@LS", ls);
				_cmd.Parameters.AddWithValue("@Kind", kindID);

				_con.Open();
				SqlDataReader reader = _cmd.ExecuteReader();
				if(reader.HasRows) {
					list = new List<Common.Models.Document>();
					while(reader.Read()) {
						BitmapImage thrumb = new BitmapImage();
						thrumb.BeginInit();
						MemoryStream stream = new MemoryStream((byte[])reader["Thumbnails"]);
						thrumb.StreamSource = stream;
						thrumb.EndInit();

						BitmapImage src = new BitmapImage();
						src.BeginInit();
						stream = new MemoryStream((byte[])reader["Source"]);
						src.StreamSource = stream;
						src.EndInit();

						list.Add(new Common.Models.Document(Convert.ToInt32(reader["ID"]), Convert.ToInt32(reader["Kind"]), Convert.ToString(reader["Name"]), (byte[])reader["Hash"], thrumb,
							Convert.ToDateTime(reader["CreateAt"]), Convert.ToDateTime(reader["ModifyAt"]), Convert.ToBoolean(reader["IsMarkDelete"]), src));
					}
				}

				return list;
			}
		}

		Common.Models.Document IDocumentService.LoadDocument(int docID)
		{
			throw new NotImplementedException();
		}

		bool IDocumentService.MarkDeleteDocument(Common.Models.Document doc)
		{
			throw new NotImplementedException();
		}

		void IDocumentService.SaveDocument(Common.Models.Document doc, Folder folder, int ls)
		{
			using(_con) {
				_cmd.CommandType = CommandType.StoredProcedure;

				_con.Open();
				SqlTransaction tranc = _con.BeginTransaction(IsolationLevel.RepeatableRead);

				try {
					//первая часть сохраняем сам документ
					_cmd.CommandText = "DocAdd";

					_cmd.Parameters.Clear();
					_cmd.Parameters.AddWithValue("@Name", doc.Name);
					_cmd.Parameters.AddWithValue("@Kind", doc.KindID);
					_cmd.Parameters.AddWithValue("@CreateAt", doc.CreateAt);
					_cmd.Parameters.AddWithValue("@ModifyAt", doc.ModifyAt);
					_cmd.Parameters.AddWithValue("@UsrName", "EArchiv");
					_cmd.Parameters.AddWithValue("@Hash", doc.ConvertHash());

					long len = doc.Thumbnails.StreamSource.Length;
					byte[] buf = new byte[len];
					doc.Thumbnails.StreamSource.Read(buf, 0, (int)len);
					_cmd.Parameters.AddWithValue("@Thumbnails", buf);

					len = doc.Source.StreamSource.Length;
					buf = new byte[len];
					doc.Source.StreamSource.Read(buf, 0, (int)len);
					_cmd.Parameters.AddWithValue("@Raw", buf);

					int key = -1;
					SqlParameter parametr = new SqlParameter("@Key", key);
					parametr.Direction = ParameterDirection.Output;
					_cmd.Parameters.Add(parametr);

					_cmd.ExecuteNonQuery();
					doc.setID(key);

					//вторая часть добавляем документ в папку
					_cmd.CommandText = "DocFolderAdd";

					_cmd.Parameters.Clear();
					_cmd.Parameters.AddWithValue("@DocID", key);
					_cmd.Parameters.AddWithValue("FolderID", folder.ID);
					_cmd.Parameters.AddWithValue("UsrName", "EArchiv");

					parametr = new SqlParameter("@Key", key);
					parametr.Direction = ParameterDirection.Output;
					_cmd.Parameters.Add(parametr);

					_cmd.ExecuteNonQuery();

					// третья часть связываем документ с лицевым счетом
					_cmd.CommandText = "DocLsAdd";

					_cmd.Parameters.Clear();
					_cmd.Parameters.AddWithValue("@Doc", doc.ID);
					_cmd.Parameters.AddWithValue("Ls", ls);
					_cmd.Parameters.AddWithValue("UsrName", "EArchiv");

					parametr = new SqlParameter("@Key", key);
					parametr.Direction = ParameterDirection.Output;
					_cmd.Parameters.Add(parametr);

					_cmd.ExecuteNonQuery();

					tranc.Commit();
				}

				catch(Exception e) {
					tranc.Rollback();
					throw new Exception(e.Message);
				}
			}

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="classID"></param>
		/// <returns></returns>
		IList<DocumentKind> IDocumentService.GetKindsByClass(int classID)
		{
			using (_con) {
				_cmd.CommandType = CommandType.StoredProcedure;
				_cmd.CommandText = "DocKindsGetForClass";
				_cmd.Parameters.Clear();
				_cmd.Parameters.AddWithValue("@Class", classID);
				IList<DocumentKind> list = null;

				_con.Open();
				SqlDataReader reader = _cmd.ExecuteReader();

				if (reader.HasRows) {
					list = new List<DocumentKind>();
					while (reader.Read()) {
						list.Add(new DocumentKind(Convert.ToInt32(reader["KEY"]), Convert.ToString(reader["NAME"]), Convert.ToBoolean(reader["ISMARKDEL"])));
					}
				}

				return list;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public IList<DocumentClass> GetClasses()
		{
			using(_con) {
				_cmd.CommandType = CommandType.StoredProcedure;
				_cmd.CommandText = "DocClassGetAll";
				IList<DocumentClass> list = null;

				_con.Open();
				SqlDataReader reader = _cmd.ExecuteReader();

				if(reader.HasRows) {
					list = new List<DocumentClass>();
					while(reader.Read()) {
						list.Add(new DocumentClass(Convert.ToInt32(reader["KEY"]), Convert.ToString(reader["NAME"]), Convert.ToBoolean(reader["IS_MARK_DEL"])));
					}
				}

				return list;
			}
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
		/// <param name="folder"></param>
		public void SaveFolder(Folder folder)
		{
			using(_con) {
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
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="doc"></param>
		public void SaveDocumentAttributes(Common.Models.Document doc)
		{
			using(_con) {
				_cmd.CommandType = CommandType.StoredProcedure;
				_cmd.CommandText = "DocAttrSet";
				_cmd.Parameters.Clear();
				_cmd.Parameters.AddWithValue("@Key", doc.ID);
				_cmd.Parameters.AddWithValue("@Kind", doc.KindID);
				_cmd.Parameters.AddWithValue("@ModifyAt", doc.ModifyAt);
				_cmd.Parameters.AddWithValue("@UsrName", "EArchiv");

				_con.Open();
				_cmd.ExecuteNonQuery();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="doc"></param>
		public void SaveDocumentSource(Common.Models.Document doc)
		{
			using(_con) {
				_cmd.CommandType = CommandType.StoredProcedure;
				_cmd.CommandText = "DocSet";
				_cmd.Parameters.Clear();
				_cmd.Parameters.AddWithValue("@Key", doc.ID);
				_cmd.Parameters.AddWithValue("@Kind", doc.KindID);
				_cmd.Parameters.AddWithValue("@ModifyAt", doc.ModifyAt);
				_cmd.Parameters.AddWithValue("@UsrName", "EArchiv");
				_cmd.Parameters.AddWithValue("@Hash", doc.ConvertHash());

				long len = doc.Thumbnails.StreamSource.Length;
				byte[] buf = new byte[len];
				doc.Thumbnails.StreamSource.Read(buf, 0, (int)len);
				_cmd.Parameters.AddWithValue("Thumbnails", buf);

				len = doc.Source.StreamSource.Length;
				buf = new byte[len];
				doc.Source.StreamSource.Read(buf, 0, (int)len);
				_cmd.Parameters.AddWithValue("@Raw", buf);
				_cmd.Parameters.AddWithValue("Raw", buf);

				_con.Open();
				_cmd.ExecuteNonQuery();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="newFolder"></param>
		public void ChangeDocumentFolder(int key, Folder newFolder)
		{
			using(_con) {
				_cmd.CommandText = "DocFolderChange";
				_cmd.CommandType = CommandType.StoredProcedure;
				_cmd.Parameters.Clear();
				_cmd.Parameters.AddWithValue("@Key", key);
				_cmd.Parameters.AddWithValue("@Folder", newFolder.ID);
				_cmd.Parameters.AddWithValue("@UsrName", "EArchiv");

				_con.Open();
				_cmd.ExecuteNonQuery();
      }
		}
	}
}
