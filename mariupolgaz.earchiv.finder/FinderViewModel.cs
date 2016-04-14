using System.Windows.Input;
using Mariupolgaz.EArchiv.Common.Models;

namespace Mariupolgaz.EArchiv.Finder
{
	/// <summary>
	/// Модель представления панели поиска абонента
	/// </summary>
	public class FinderViewModel : BaseViewModel
	{
		/// <summary>
		/// Лицевой счет
		/// </summary>
		public int? LS { get; set; }

		/// <summary>
		/// Фамилия искомого абонента
		/// </summary>
		public string Family {get; set;}

		/// <summary>
		/// Поиск абонента по критериям
		/// </summary>
		public ICommand FindAbonents
		{
			get { return new DelegateCommand(onFindAbonents, canFindAbonents); }
		}

		private void onFindAbonents()
		{

		}

		private bool canFindAbonents()
		{
			return true;
		}
	}
}
