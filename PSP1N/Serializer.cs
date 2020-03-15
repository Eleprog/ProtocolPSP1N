using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PSP1N
{
	/// <summary>
	/// Сериализатор объектов.
	/// </summary>
	public class Serializer
	{
		private readonly ICodec _coder;

		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="Serializer"/>.
		/// </summary>
		/// <param name="coder">Кодек для сериализации.</param>
		public Serializer(ICodec coder)
		{
			_coder = coder;
		}

		/// <summary>
		/// Сериализует объект в поток.
		/// </summary>
		public void Serialize(Stream stream, ISerializer objectData)
		{
			Stack<uint> stack = new Stack<uint>();
			objectData.GetData(stack);
			byte[] data = _coder.Encode(stack);
			stream.Write(data, 0, data.Length);
		}

		/// <summary>
		/// Десериализует объект из потока.
		/// </summary>
		/// <typeparam name="T">Тип десириализуемого объекта.</typeparam>
		/// <returns>Объект сереализации или null - если достигнут конец файла.</returns>
		public T Desirialize<T>(Stream stream) where T : ISerializer, new()
		{
			T serializer = new T();
			int firstByte;
			do
			{
				firstByte = stream.ReadByte();
				if (firstByte == -1)
				{
					return default;
				}
			}
			while (firstByte >> _coder.DataBits != (int)_coder.StartBit);

			byte[] data = new byte[_coder.PackageSize];
			data[0] = (byte)firstByte;
			int countByte = stream.Read(data, 1, _coder.PackageSize - 1);

			foreach (var b in data.Skip(1))
			{
				if (b >> _coder.DataBits == (int)_coder.StartBit)
				{
					throw new Exception("Пакет поврежден.");
				}
			}

			if (countByte < _coder.PackageSize - 1)
			{
				throw new Exception("Недостаточно байт в пакете.");
			}

			Stack<uint> stack = _coder.Decode(data);

			serializer.SetData(stack);

			return serializer;
		}
	}
}
