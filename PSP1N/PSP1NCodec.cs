using System;
using System.Collections.Generic;
using System.Linq;

namespace PSP1N
{
	/// <summary>
	/// Представляет кодек протокола PSP1N.
	/// </summary>
	public class PSP1NCodec : ICodec
	{
		private const int DataBits = 7;
		private readonly int[] _structurePackage;

		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="PSP1NCodec"/>
		/// </summary>
		/// <param name="startBit">Стартовый бит пакета.</param>
		/// <param name="structurePackage">
		/// Структура пакета, каждый элемент массива указывает число бит данных.
		/// </param>
		public PSP1NCodec(StartBit startBit, int[] structurePackage)
		{
			StartBit = startBit;
			_structurePackage = structurePackage;
		}

		public StartBit StartBit { get; }

		public int PackageSize
		{
			get
			{
				int totalBits = _structurePackage.Sum();
				int size = totalBits / DataBits;
				if (totalBits % DataBits > 0)
				{
					size++;
				}

				return size;
			}
		}

		int ICodec.DataBits => DataBits;

		public Stack<uint> Decode(byte[] encodeData)
		{
			//Декодирование массива байт
			int position = 0;
			Stack<uint> stack = new Stack<uint>();
			for (int i = 0; i < _structurePackage.Length; i++)
			{
				//packagePSP.Item[i].Value = 0;
				int size = _structurePackage[i];
				uint value = 0;
				while (size > 0)
				{
					byte x = (byte)(position % DataBits); //смещение вправо
					int y = position / DataBits;
					int freeBits = 8 - (x + size); //свободных бит в текущем байте
					if (freeBits <= 0) freeBits = 1;
					byte temp = (byte)(encodeData[y] << freeBits);
					temp = (byte)(temp >> (freeBits + x));
					uint temp2 = temp;
					value |= temp2 << (_structurePackage[i] - size);
					byte bit = (byte)(DataBits - (freeBits - 1) - x);
					position += bit;
					size -= bit;
				}

				stack.Push(value);
			}
			var z = stack.ToList();
			return new Stack<uint>(z);
		}

		public byte[] Encode(Stack<uint> data)
		{
			if (data.Count != _structurePackage.Length)
			{
				throw new Exception($"Ошибка заполнения пакета. Всего значений в пакете: {_structurePackage.Length}, " +
					$"а получено {data.Count} значений");
			}

			int positionPush = 0;
			byte[] matrix = new byte[PackageSize];
			uint[] itemValue = data.Reverse().ToArray();
			for (int i = 0; i < itemValue.Length; i++)
			{
				ValidateValue(itemValue[i], _structurePackage[i]);

				int valueBit = _structurePackage[i];
				int size = valueBit; //размер оставшихся не помещенных бит
				int x = 0;
				int y = 0;
				while (size > 0)
				{
					x = positionPush % DataBits; //
					y = positionPush / DataBits;
					int freeBits = DataBits - x; //свободных бит в текущем байте
					var offsetRight = valueBit - size; //смещение вправо

					matrix[y] |= (byte)(itemValue[i] >> offsetRight << x);
					size -= freeBits;

					if (StartBit == StartBit.One)
						unchecked
						{
							matrix[y] &= (byte)~(1 << DataBits);
						}
					else matrix[y] |= 1 << DataBits;


					if (size <= 0)
					{
						positionPush += freeBits + size;
						break;
					}
					else
					{
						positionPush += freeBits;
					}

				}

				if (StartBit == StartBit.Zero)
					unchecked
					{
						matrix[0] &= (byte)~(1 << DataBits);
					}
				else matrix[0] |= 1 << DataBits;
			}
			return matrix;
		}

		private void ValidateValue(uint value, int bits)
		{
			uint maxValue = 0;
			for (int j = 0; j < bits; j++)
			{
				maxValue |= (uint)(1 << j);
			}

			if (maxValue < value)
			{
				throw new ArgumentOutOfRangeException($"Значение {value} выше максимально допустимого {maxValue}");
			}
		}
	}
}
