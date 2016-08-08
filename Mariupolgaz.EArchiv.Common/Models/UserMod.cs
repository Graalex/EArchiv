using System;

namespace Mariupolgaz.EArchiv.Common.Models
{
	/// <summary>
	/// Пользователь изменивший документ
	/// </summary>
	public class UserMod
	{
		/// <summary>
		/// Создает экземпляр <see cref="UserMod"/>
		/// </summary>
		/// <param name="name">Имя пользователя</param>
		/// <param name="modifityAt">Время изменения документа</param>
		public UserMod(string name, DateTime? modifityAt)
		{
			this.Name = name;
			this.ModifityAt = modifityAt;
		}

		/// <summary>
		/// Имя пользователя
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Время изменения документа
		/// </summary>
		public DateTime? ModifityAt { get; set; }
	}
}
