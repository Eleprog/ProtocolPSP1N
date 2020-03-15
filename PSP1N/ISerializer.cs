using System.Collections.Generic;

namespace PSP1N
{
	/// <summary>
	/// Предоставляет функционал сериализации объекта.
	/// </summary>
	public interface ISerializer
	{
		/// <summary>
		/// Положить данные в стэк.
		/// </summary>
		/// <param name="data">Стэк для хранения данных.</param>
		void SetData(Stack<uint> data);

		/// <summary>
		/// Получить данные из стэка.
		/// </summary>
		/// <param name="data">Данные хранящиеся в стэке.</param>
		void GetData(Stack<uint> data);
	}
}
