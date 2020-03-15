using System;
using System.IO;
using PSP1N;

namespace Sample
{
	class Program
	{
		private const string FileName = "test.bin";
		private const int CountPoint = 1000000;
		private static Random _random = new Random();
		private static DateTime _dateTime = DateTime.Now;

		static void Main(string[] args)
		{
			int[] structurePackage = new int[] { 32, 10, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12 };
			PSP1NCodec codec = new PSP1NCodec(StartBit.Zero, structurePackage);
			Serializer serializator = new Serializer(codec);

			int progress = 0;
			using (FileStream fs = new FileStream(FileName, FileMode.Create))
			{
				for (int i = 0; i < CountPoint; i++)
				{
					Point point = CreatePoint(i);
					serializator.Serialize(fs, point);

					int unit = CountPoint / 100;
					if (i % unit == 0)
					{
						Console.Clear();
						Console.WriteLine(++progress + "%");
					}
				}
			}

			using (FileStream fs = new FileStream(FileName, FileMode.Open))
			{
				for (int i = 0; i < CountPoint; i++)
				{
					Point point = serializator.Desirialize<Point>(fs);
					Console.WriteLine(point);
				}
			}
		}

		static Point CreatePoint(int count)
		{
			Point point = new Point
			{
				DateTime = _dateTime + TimeSpan.FromMilliseconds(count),
				ChannelsData = new int[10] { 1, 22, 30, 40, 200, 600, 900, 1111, 3001, _random.Next(4096) },
			};

			return point;
		}
	}
}
