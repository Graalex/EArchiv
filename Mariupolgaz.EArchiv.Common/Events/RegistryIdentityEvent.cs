﻿using Microsoft.Practices.Prism.Events;

namespace Mariupolgaz.EArchiv.Common.Events
{
	/// <summary>
	/// Собітие регистрации нового пользователя
	/// </summary>
	public class RegistryIdentityEvent: CompositePresentationEvent<IdentityMessage>
	{
	}
}
