using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mariupolgaz.EArchiv.Common.Models
{
	/// <summary>
	/// 
	/// </summary>
	public class WorkerIdentity
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="loginName"></param>
		public WorkerIdentity(string loginName)
		{
			this.LoginName = loginName;
		}

		/// <summary>
		/// 
		/// </summary>
		public string LoginName { get; private set; }
	}
}
