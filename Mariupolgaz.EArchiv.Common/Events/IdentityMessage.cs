using System;

namespace Mariupolgaz.EArchiv.Common.Events
{
	/// <summary>
	/// Информация о зарегестрированном пользователе
	/// </summary>
	public class IdentityMessage
	{
		/// <summary>
		/// Создает экземпляр <see cref="IdentityMessage"/>
		/// </summary>
		/// <param name="loginName">Имя входа</param>
		/// <param name="isActivated">Признак активации</param>
		/// <param name="registeredAt">Дата и время регистрации</param>
		public IdentityMessage(string loginName, bool isActivated, DateTime registeredAt)
		{
			this.LoginName = loginName;
			this.IsActivated = isActivated;
			this.RegisteredAt = registeredAt;
		}

		/// <summary>
		/// Имя входа
		/// </summary>
		public string LoginName { get; private set; }

		/// <summary>
		/// Признак активации пользователя
		/// </summary>
		public bool IsActivated { get; private set; }

		/// <summary>
		/// Дата и время регистрации
		/// </summary>
		public DateTime RegisteredAt { get; private set; }
	}
}
