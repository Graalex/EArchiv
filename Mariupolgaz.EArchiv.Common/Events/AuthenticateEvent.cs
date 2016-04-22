using Microsoft.Practices.Prism.Events;

namespace Mariupolgaz.EArchiv.Common.Events
{
	/// <summary>
	/// Событие входа пользователя
	/// </summary>
	public class AuthenticateEvent: CompositePresentationEvent<AuthenticateMessage>
	{
	}
}
