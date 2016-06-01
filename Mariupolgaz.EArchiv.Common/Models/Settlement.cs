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
		/// <param name="name">Название нас. пункта</param>
		public Settlement(int id, string name)
		{
			this.ID = id;
			this.Name = name;
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
		/// Переопределяет строковое представления объкта
		/// </summary>
		/// <returns>Строковое представления объкта в формате (г. Мариуполь)</returns>
		public override string ToString()
		{
			/*
			int ind = this.Name.LastIndexOf(' ');

			return this.Name.Substring(ind + 1).ToLower() +
				" " +
				this.Name.Substring(0, ind);
			*/
			return this.Name;
		}
	}
}
