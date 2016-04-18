namespace Mariupolgaz.EArchiv.Common.Models
{
	/// <summary>
	/// Определяет объект сообщение при собитии добавления документа
	/// </summary>
	public class AddDocMessage
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="file"></param>
		/// <param name="kind"></param>
		public AddDocMessage(string file, DocumentKind kind)
		{
			this.File = file;
			this.Kind = kind;
		}

		/// <summary>
		/// 
		/// </summary>
		public string File { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public DocumentKind Kind { get; private set; }
	}
}
