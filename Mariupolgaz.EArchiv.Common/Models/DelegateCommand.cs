using System;
using System.Windows.Input;

namespace Mariupolgaz.EArchiv.Common.Models
{
	/// <summary>
	/// Реализация интерфейса <see cref="ICommand"/>
	/// </summary>
	public class DelegateCommand : ICommand
	{
		private readonly Action _command;
		private readonly Func<bool> _canExecute;

		/// <summary>
		/// Происходит при проверки на возможность выполнения команды
		/// </summary>
		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		/// <summary>
		/// Конструктор экземпляра
		/// </summary> DelegateCommand
		/// <param name="command">Делегат выполняющий команду</param>
		/// <param name="canExecute">Делегат проверяющий возможность исполнения команды</param>
		public DelegateCommand(Action command, Func<bool> canExecute = null)
		{
			if (command == null)
				throw new ArgumentNullException();
			_canExecute = canExecute;
			_command = command;
		}

		/// <summary>
		/// Выполнение команнды
		/// </summary>
		/// <param name="parameter">Параметр команды</param>
		public void Execute(object parameter)
		{
			_command();
		}

		/// <summary>
		/// Проверяет возможность исполнения команды
		/// </summary>
		/// <param name="parameter">Параметр команды</param>
		/// <returns>Истина если команда может быть выполнена</returns>
		public bool CanExecute(object parameter)
		{
			return _canExecute == null || _canExecute();
		}

	}
}
