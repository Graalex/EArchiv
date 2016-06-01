using System.Windows;

namespace EContract
{
	/// <summary>
	/// Interaction logic for Shell.xaml
	/// </summary>
	public partial class Shell : Window
	{
		/// <summary>
		/// Конструктор по умолчанию
		/// </summary>
		public Shell()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="dataContext">Контекст данніх</param>
		public Shell(ShellViewModel dataContext) :this()
		{
			this.DataContext = dataContext;
		}
	}
}
