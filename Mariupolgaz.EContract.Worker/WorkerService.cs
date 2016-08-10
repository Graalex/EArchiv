using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mariupolgaz.EArchiv.Common.Models;
using Mariupolgaz.EArchiv.Common.Servises;

namespace Mariupolgaz.EContract.Worker
{
	/// <summary>
	/// 
	/// </summary>
	public class WorkerService : IWorkerService
	{
		private WorkerIdentity _ident;

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public WorkerIdentity GetCurrentIdentity()
		{
			return _ident;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="worker"></param>
		public void RegisterIdentity(WorkerIdentity worker)
		{
			_ident = worker;
		}
	}
}
