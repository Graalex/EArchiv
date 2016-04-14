﻿using System.Collections.Generic;
using Mariupolgaz.EArchiv.Common.Models;

namespace Mariupolgaz.EArchiv.Common.Servises
{
	/// <summary>
	/// Определения сервиса обработки электронных документов.
	/// </summary>
	public interface IDocumentService
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="classID"></param>
		/// <returns></returns>
		IList<DocumentKind> GetKindsByClass(int classID);

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		IList<DocumentClass> GetClasses();
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="ls"></param>
		/// <returns></returns>
		IList<Document> GetDocuments(int ls);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ls"></param>
		/// <param name="kindID"></param>
		/// <returns></returns>
		IList<Document> GetDocuments(int ls, int kindID);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="docID"></param>
		/// <returns></returns>
		DocumentKind GetDocumentKind(int docID);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="docID"></param>
		/// <returns></returns>
		DocumentClass GetDocumentClass(int docID);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="docID"></param>
		/// <returns></returns>
		int GetDocumentLS(int docID);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="doc"></param>
		/// <param name="folder"></param>
		/// <param name="ls"></param>
		/// <returns></returns>
		void SaveDocument(Document doc, Folder folder, int ls);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="doc"></param>
		void SaveDocumentAttributes(Document doc);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="doc"></param>
		void SaveDocumentSource(Document doc);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="newFolder"></param>
		void ChangeDocumentFolder(int key, Folder newFolder);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="docID"></param>
		/// <returns></returns>
		Document LoadDocument(int docID);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="kind"></param>
		/// <returns></returns>
		Document CreateDocument(string name, int kind);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="doc"></param>
		/// <returns></returns>
		bool MarkDeleteDocument(Document doc);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		Folder CreateFolder(string name);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="folder"></param>
		/// <returns></returns>
		void SaveFolder(Folder folder);
	
	}
}