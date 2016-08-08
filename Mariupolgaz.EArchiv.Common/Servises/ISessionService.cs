namespace Mariupolgaz.EArchiv.Common.Servises
{
	/// <summary>
	/// Определяет сервис управления пользовательскими сесиями.
	/// </summary>
	public interface ISessionService
	{
		/// <summary>
		/// Начинает новоую сессию
		/// </summary>
		/// <param name="identKey">Идентификатор пользователя</param>
		/// <param name="appKey">Идентификатор приложения</param>
		/// <returns>Идентификатор новой сессии</returns>
		int StartSession(int identKey, int appKey);

		/// <summary>
		/// Закрывает сессию
		/// </summary>
		/// <param name="sesKey">Идентификатор сессии</param>
		void StopSession(int sesKey);
	}
}
