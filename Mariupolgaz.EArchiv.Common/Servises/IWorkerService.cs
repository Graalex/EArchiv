using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mariupolgaz.EArchiv.Common.Models;

namespace Mariupolgaz.EArchiv.Common.Servises
{
	/// <summary>
	/// 
	/// </summary>
	public interface IWorkerService
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="worker"></param>
		void RegisterIdentity(WorkerIdentity worker);

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		WorkerIdentity GetCurrentIdentity();
	}
}
