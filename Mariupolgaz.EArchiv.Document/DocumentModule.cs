using System;
using Mariupolgaz.EArchiv.Common.Servises;
using Mariupolgaz.EArchiv.Document.Services;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace Mariupolgaz.EArchiv.Document
{
	/// <summary>
	/// Представляет модуль работы с документами
	/// </summary>
	public class DocumentModule : IModule
	{
		private readonly IUnityContainer _container;
		
		/// <summary>
		/// <see cref="DocumentModule"/>
		/// </summary>
		/// <param name="container">Экземпляр реализующий интерфейс контейнера зависимостей</param>
		public DocumentModule(IUnityContainer container)
		{
			if (container == null) throw new ArgumentNullException("container");

			_container = container;
		}

		/// <summary>
		/// Инициализирует модуль работы с документами
		/// </summary>
		public void Initialize()
		{
			_container.RegisterType<IDocumentService, DocumentService>(new ContainerControlledLifetimeManager());
		}
	}
}
