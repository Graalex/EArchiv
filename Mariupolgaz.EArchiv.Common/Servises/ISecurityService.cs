using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace Mariupolgaz.EArchiv.Common.Servises
{
	/// <summary>
	/// Представляет сервис авторизации, аутентификации и аудита.
	/// </summary>
	public interface ISecurityService
	{
		//IPrincipal CurrentPrincipal();

		/// <summary>
		/// Регистрация пользователя в системе
		/// </summary>
		/// <param name="loginName">Логин пользователя</param>
		/// <param name="password">Пароль пользователя</param>
		/// <returns>true если регистрация прошла успешно</returns>
		bool Login(string loginName, string password);

		/// <summary>
		/// Выход пользователя из системы
		/// </summary>
		void Logout();
		/*
		bool CheckPassword();
		byte[] HashPassword(string password);
		bool Can();
		string GenerateRandomString();
		bool Allow();
		bool Deny();
		bool Assign();
		*/
		/// <summary>
		/// Получить список пользователей
		/// </summary>
		/// <returns></returns>
		IList<string> GetIdentities();

		/// <summary>
		/// Получить список пользователей, имеющих право работать с приложением.
		/// </summary>
		/// <param name="appName">Имя приложения</param>
		/// <returns></returns>
		IList<string> GetIdentities(string appName);
	}
}
