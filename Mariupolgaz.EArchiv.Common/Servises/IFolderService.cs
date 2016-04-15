using System.Collections.Generic;
using Mariupolgaz.EArchiv.Common.Models;

namespace Mariupolgaz.EArchiv.Common.Servises
{
	/// <summary>
	/// 
	/// </summary>
	public interface IFolderService
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="ownID"></param>
		/// <returns></returns>
		IList<Folder> GetFolders(int ownID);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <remarks></remarks>
		Folder CreateFolder(string name);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="folder"></param>
		/// <returns></returns>
		bool SaveFolder(Folder folder);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Folder LoadFolder(int id);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="folder"></param>
		/// <returns></returns>
		bool MarkDeleteFolder(Folder folder);
	}
}
