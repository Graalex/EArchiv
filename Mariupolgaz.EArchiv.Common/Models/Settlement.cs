namespace Mariupolgaz.EArchiv.Common.Models
{
	/// <summary>
	/// Представляет населенный пункт
	/// </summary>
	public class Settlement
	{
		/// <summary>
		/// Создает экземпляр <see cref="Settlement"/>
		/// </summary>
		/// <param name="id">ID нас. пункта</param>
		/// <param name="kind">Тип нас. пункта</param>
		/// <param name="name">Название нас. пункта</param>
		public Settlement(int id, string kind, string name)
		{
			this.ID = id;
			this.Name = name;
			this.Kind = kind;
		}

		/// <summary>
		/// ID нас. пункта
		/// </summary>
		public int ID { get; private set; }

		/// <summary>
		/// Название нас. пункта
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Тип нас. пункта
		/// </summary>
		public string Kind { get; private set; }

		/// <summary>
		/// Переопределяет строковое представления объкта
		/// </summary>
		/// <returns>Строковое представления объкта в формате (г. Мариуполь)</returns>
		public override string ToString()
		{
			return this.Kind + " " +this.Name;
		}
	}
}
