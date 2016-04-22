using System;

namespace Mariupolgaz.EArchiv.Common.Events
{
	/// <summary>
	/// Сообщение при срабатывании события входа пользователя
	/// </summary>
	public class AuthenticateMessage
	{
		/// <summary>
		/// Создает єкземпляр <see cref="AuthenticateMessage"/>
		/// </summary>
		/// <param name="loginName">Имя входа</param>
		/// <param name="loginAt">Дата и время входа</param>
		/// <param name="isAuthenticate">Успешность регистрации</param>
		public AuthenticateMessage(string loginName, DateTime loginAt, bool isAuthenticate)
		{
			this.LoginName = loginName;
			this.LoginAt = loginAt;
			this.IsAuthenticate = isAuthenticate;
		}

		/// <summary>
		/// Имя входа
		/// </summary>
		public string LoginName { get; private set; }

		/// <summary>
		/// Дата и время входа
		/// </summary>
		public DateTime LoginAt { get; private set; }

		/// <summary>
		/// Результат регистрации
		/// </summary>
		public bool IsAuthenticate { get; private set; }
	}
}
