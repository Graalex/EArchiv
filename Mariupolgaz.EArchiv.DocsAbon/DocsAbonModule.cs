using System;
using Mariupolgaz.EArchiv.Common;
using Mariupolgaz.EArchiv.DocsAbon.Views;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace Mariupolgaz.EArchiv.DocsAbon
{
	/// <summary>
	/// 
	/// </summary>
	public class DocsAbonModule : IModule
	{
		private readonly IUnityContainer _container;
		private readonly IRegionManager _manager;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="container"></param>
		/// <param name="manager"></param>
		public DocsAbonModule(IUnityContainer container, IRegionManager manager)
		{
			if (container == null) throw new ArgumentNullException("container");
			if (manager == null) throw new ArgumentNullException("manager");

			_container = container;
			_manager = manager;
		}

		/// <summary>
		/// Иницилизация модуля.
		/// </summary>
		public void Initialize()
		{
			_manager.RegisterViewWithRegion(
				RegionNames.ClassDocListRegion,
				() => _container.Resolve<ClassDocView>()
			);
			_manager.RegisterViewWithRegion(
				RegionNames.KindsListRegion, 
				() => _container.Resolve<KindDocView>()
			);
			_manager.RegisterViewWithRegion(
				RegionNames.DocumentRegion,
				() => _container.Resolve<DocumentView>()
			);
		}
	}
}
