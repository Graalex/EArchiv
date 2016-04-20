using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mariupolgaz.EArchiv.Common.Servises
{
	public interface IRegisteredService
	{
		//bool CreateUser()
		bool RegisteredUser(string loginName, string password, bool isActivity = false);
	}
}
