namespace Mariupolgaz.EArchiv.Common.Servises
{
	/// <summary>
	/// 
	/// </summary>
	public interface IRegisteredService
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="loginName"></param>
		/// <param name="password"></param>
		/// <param name="isActivity"></param>
		/// <returns></returns>
		bool RegisteredUser(string loginName, string password, bool isActivity = false);
	}
}
