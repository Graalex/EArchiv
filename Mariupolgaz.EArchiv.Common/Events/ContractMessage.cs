namespace Mariupolgaz.EArchiv.Common.Events
{
	/// <summary>
	/// Определяет сообщение для события выбора контракта
	/// </summary>
	public class ContractMessage
	{
		/// <summary>
		/// Создает экземпляр <see cref="ContractMessage"/>
		/// </summary>
		/// <param name="code">Код контракта в 1С</param>
		/// <param name="edrpou">ЕДРПОУ организации</param>
		public ContractMessage(string code, string edrpou)
		{
			this.Code = code;
			this.Edrpou = edrpou;
		}

		/// <summary>
		/// Код контракта в 1С
		/// </summary>
		public string Code { get; private set; }

		/// <summary>
		/// ЕДРПОУ организации
		/// </summary>
		public string Edrpou { get; private set; }
	}
}
