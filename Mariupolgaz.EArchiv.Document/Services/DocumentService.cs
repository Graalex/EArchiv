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
		private string _con;
		private IEnumerable<DocumentKind> _kinds;
		
		/// <summary>
		/// 
		/// </summary>
		public DocumentService()
		{
			_con = ConfigurationManager.ConnectionStrings["Archiv"].ConnectionString;
			//TODO: Костыль позже переделать
			_kinds = this.GetKindsByClass(2);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="kind"></param>
		/// <returns></returns>
		Common.Models.Document IDocumentService.CreateDocument(string name, DocumentKind kind)
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

						int id = Convert.ToInt32(reader["Kind"]);
						var kind = _kinds.First(item => item.ID == id);

						list.Add(new Common.Models.Document(Convert.ToInt32(reader["ID"]), kind, Convert.ToString(reader["Name"]), (byte[])reader["Hash"], thrumb,
							Convert.ToDateTime(reader["CreateAt"]), Convert.ToDateTime(reader["ModifyAt"]), Convert.ToBoolean(reader["IsMarkDelete"]), src));
					}
				}

				return list;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ls"></param>
		/// <param name="kindID"></param>
		/// <returns></returns>
		IList<Common.Models.Document> IDocumentService.GetDocuments(int ls, int kindID)
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

						int id = Convert.ToInt32(reader["Kind"]);
						var kind = _kinds.First(item => item.ID == id);

						list.Add(new Common.Models.Document(Convert.ToInt32(reader["ID"]), kind, Convert.ToString(reader["Name"]), (byte[])reader["Hash"], thrumb,
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
			using(SqlConnection con = new SqlConnection(_con)) {
				SqlCommand cmd = new SqlCommand();
				cmd.Connection = con;
				cmd.CommandType = CommandType.StoredProcedure;

				con.Open();
				SqlTransaction tranc = con.BeginTransaction(IsolationLevel.RepeatableRead);

				try {
					//первая часть сохраняем сам документ
					cmd.CommandText = "DocAdd";

					cmd.Parameters.Clear();
					cmd.Parameters.AddWithValue("@Name", doc.Name);
					cmd.Parameters.AddWithValue("@Kind", doc.Kind.ID);
					cmd.Parameters.AddWithValue("@CreateAt", doc.CreateAt);
					cmd.Parameters.AddWithValue("@ModifyAt", doc.ModifyAt);
					cmd.Parameters.AddWithValue("@UsrName", "EArchiv");
					cmd.Parameters.AddWithValue("@Hash", doc.ConvertHash());

					long len = doc.Thumbnails.StreamSource.Length;
					byte[] buf = new byte[len];
					doc.Thumbnails.StreamSource.Read(buf, 0, (int)len);
					cmd.Parameters.AddWithValue("@Thumbnails", buf);

					len = doc.Source.StreamSource.Length;
					buf = new byte[len];
					doc.Source.StreamSource.Read(buf, 0, (int)len);
					cmd.Parameters.AddWithValue("@Raw", buf);

					int key = -1;
					SqlParameter parametr = new SqlParameter("@Key", key);
					parametr.Direction = ParameterDirection.Output;
					cmd.Parameters.Add(parametr);

					cmd.ExecuteNonQuery();
					doc.setID(key);

					//вторая часть добавляем документ в папку
					cmd.CommandText = "DocFolderAdd";

					cmd.Parameters.Clear();
					cmd.Parameters.AddWithValue("@DocID", key);
					cmd.Parameters.AddWithValue("FolderID", folder.ID);
					cmd.Parameters.AddWithValue("UsrName", "EArchiv");

					parametr = new SqlParameter("@Key", key);
					parametr.Direction = ParameterDirection.Output;
					cmd.Parameters.Add(parametr);

					cmd.ExecuteNonQuery();

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
		public IList<DocumentKind> GetKindsByClass(int classID)
		{
			using (SqlConnection con = new SqlConnection(_con)) {
				SqlCommand cmd = new SqlCommand();
				cmd.Connection = con;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "DocKindsGetForClass";
				cmd.Parameters.Clear();
				cmd.Parameters.AddWithValue("@Class", classID);
				IList<DocumentKind> list = null;

				con.Open();
				SqlDataReader reader = cmd.ExecuteReader();

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
			using(SqlConnection con = new SqlConnection(_con)) {
				SqlCommand cmd = new SqlCommand();
				cmd.Connection = con;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "DocClassGetAll";
				IList<DocumentClass> list = null;

				con.Open();
				SqlDataReader reader = cmd.ExecuteReader();

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
		/// <param name="doc"></param>
		public void SaveDocumentAttributes(Common.Models.Document doc)
		{
			using(SqlConnection con = new SqlConnection(_con)) {
				SqlCommand cmd = new SqlCommand();
				cmd.Connection = con;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "DocAttrSet";
				cmd.Parameters.Clear();
				cmd.Parameters.AddWithValue("@Key", doc.ID);
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
		public void SaveDocumentSource(Common.Models.Document doc)
		{
			using(SqlConnection con = new SqlConnection(_con)) {
				SqlCommand cmd = new SqlCommand();
				cmd.Connection = con;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "DocSet";
				cmd.Parameters.Clear();
				cmd.Parameters.AddWithValue("@Key", doc.ID);
				cmd.Parameters.AddWithValue("@Kind", doc.Kind.ID);
				cmd.Parameters.AddWithValue("@ModifyAt", doc.ModifyAt);
				cmd.Parameters.AddWithValue("@UsrName", "EArchiv");
				cmd.Parameters.AddWithValue("@Hash", doc.ConvertHash());

				long len = doc.Thumbnails.StreamSource.Length;
				byte[] buf = new byte[len];
				doc.Thumbnails.StreamSource.Read(buf, 0, (int)len);
				cmd.Parameters.AddWithValue("Thumbnails", buf);

				len = doc.Source.StreamSource.Length;
				buf = new byte[len];
				doc.Source.StreamSource.Read(buf, 0, (int)len);
				cmd.Parameters.AddWithValue("@Raw", buf);
				cmd.Parameters.AddWithValue("Raw", buf);

				con.Open();
				cmd.ExecuteNonQuery();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="newFolder"></param>
		public void ChangeDocumentFolder(int key, Folder newFolder)
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
