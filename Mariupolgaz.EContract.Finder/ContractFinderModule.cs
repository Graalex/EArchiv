using System;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace Mariupolgaz.EContract.Finder
{
	/// <summary>
	/// Модуль поиска договоров в базе 1С
	/// </summary>  
	public class ContractFinderModule : IModule
	{
		/// <summary>
		/// Создает экземпляр <see cref="ContractFinderModule"/>
		/// </summary>
		/// <param name="container">Экземпляр контейнера зависимостей</param>
		public ContractFinderModule(IUnityContainer container)
		{

		}
		
		/// <summary>
		/// Иницилизация модуля
		/// </summary>
		public void Initialize()
		{
			throw new NotImplementedException();
		}
	}
}
