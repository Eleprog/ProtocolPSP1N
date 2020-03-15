using System.Collections.Generic;

namespace PSP1N
{
	/// <summary>
	/// Предоставляет интерфейс для преобразования числовых данных в массив байтов и обратно.
	/// </summary>
	public interface ICodec
	{
		/// <summary>
		/// Возвращает количество бит данных содержащихся в одном байте.
		/// </summary>
		int DataBits { get; }

		/// <summary>
		/// Возвращает количество байт в пакете.
		/// </summary>
		int PackageSize { get; }

		/// <summary>
		/// Возвращает стартовый бит пакета.
		/// </summary>
		StartBit StartBit { get; }

		/// <summary>
		/// Кодирует данные.
		/// </summary>
		/// <param name="dataStack">Данные для кодирования.</param>
		/// <returns>Массив закодированных байт.</returns>
		byte[] Encode(Stack<uint> dataStack);

		/// <summary>
		/// Декодирует данные.
		/// </summary>
		/// <param name="encodeData">Массив закодированных байт.</param>
		/// <returns>Раскодированные данные.</returns>
		Stack<uint> Decode(byte[] encodeData);
	}
}