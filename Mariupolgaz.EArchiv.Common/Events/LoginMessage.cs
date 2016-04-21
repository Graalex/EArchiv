using System;

namespace Mariupolgaz.EArchiv.Common.Events
{
	/// <summary>
	/// Сообщение при срабатывании события входа пользователя
	/// </summary>
	public class LoginMessage
	{
		/// <summary>
		/// Создает єкземпляр <see cref="LoginMessage"/>
		/// </summary>
		/// <param name="loginName">Имя входа</param>
		/// <param name="loginAt">Дата и время входа</param>
		public LoginMessage(string loginName, DateTime loginAt)
		{
			this.LoginName = loginName;
			this.LoginAt = loginAt;
		}

		/// <summary>
		/// Имя входа
		/// </summary>
		public string LoginName { get; private set; }

		/// <summary>
		/// Дата и время входа
		/// </summary>
		public DateTime LoginAt { get; private set; }
	}
}
