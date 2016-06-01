namespace Mariupolgaz.EArchiv.Common.Models
{
	/// <summary>
	/// Представляет улицу в нас. пункте
	/// </summary>
	public class Street
	{
		/// <summary>
		/// Создает экземпляр <see cref="Street"/>
		/// </summary>
		/// <param name="id">Идентификатор объекта</param>
		/// <param name="name">Нименование улицы</param>
		public Street(int id, string name)
		{
			this.ID = id;
			this.Name = name;
		}

		/// <summary>
		/// Идентификатор
		/// </summary>
		public int ID { get; private set; }

		/// <summary>
		/// Наименование улицы
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Возвращает строковое представление объекта
		/// </summary>
		/// <remarks>
		/// Переопределяет базовый метод Object.ToString()
		/// </remarks>
		/// <returns>Строка пердставления (пример ул. Фонтанная)</returns>
		public override string ToString()
		{
			int idx = this.Name.LastIndexOf(' ');
			return this.Name.Substring(idx + 1).ToLower() +
				" " +
				this.Name.Substring(0, idx);
		}
	}
}
