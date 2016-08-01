using System;
using System.Windows.Media.Imaging;

namespace Mariupolgaz.EArchiv.Common.Models
{
	/// <summary>
	/// 
	/// </summary>
	public  class ContractDocument : Document
	{
		#region Constructors
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="kind"></param>
		/// <param name="docDate"></param>
		/// <param name="docNumb"></param>
		public ContractDocument(string name, DocumentKind kind, DateTime docDate, string docNumb) : base(name, kind)
		{
			this.DocumentDate = docDate;
			this.DocumentNumber = docNumb;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="kind"></param>
		/// <param name="name"></param>
		/// <param name="hash"></param>
		/// <param name="thumbnails"></param>
		/// <param name="createAt"></param>
		/// <param name="modifyAt"></param>
		/// <param name="isMarkDel"></param>
		/// <param name="source"></param>
		/// <param name="docDate"></param>
		/// <param name="docNumb"></param>
		public ContractDocument(
			int id, DocumentKind kind, string name, byte[] hash,
			BitmapImage thumbnails, DateTime createAt, DateTime modifyAt,
			bool isMarkDel, BitmapImage source,
			DateTime docDate, string docNumb
		) : base(id, kind, name, hash, thumbnails, createAt, modifyAt, isMarkDel, source)
		{
			this.DocumentDate = docDate;
			this.DocumentNumber = docNumb;
		}

		#endregion

		#region Properties

		/// <summary>
		/// 
		/// </summary>
		public DateTime DocumentDate { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string DocumentNumber { get; set; }

		#endregion
	}
}
