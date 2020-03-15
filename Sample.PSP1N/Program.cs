using System;
using System.IO;
using PSP1N;

namespace Sample
{
	class Program
	{
		private const string FileName = "test.bin";
		private const int CountPoint = 100;

		static void Main(string[] args)
		{
			int[] structurePackage = new int[] { 32, 10, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12 };
			PSP1NCodec codec = new PSP1NCodec(StartBit.Zero, structurePackage);
			Serializer serializator = new Serializer(codec);


			using (FileStream fs = new FileStream(FileName, FileMode.Create))
			{
				for (int i = 0; i < CountPoint; i++)
				{
					Point point = new Point
					{
						DateTime = DateTime.Now + TimeSpan.FromMilliseconds(i),
						ChannelsData = new int[10] { 1, 4095, 30, 40, 200, 600, 900, 1111, 3001, 4095 },
					};

					serializator.Serialize(fs, point);
				}
			}

			using (FileStream fs = new FileStream(FileName, FileMode.Open))
			{
				Point point;
				for (int i = 0; i < CountPoint; i++)
				{
					point = serializator.Desirialize<Point>(fs);
					Console.WriteLine($"{point.DateTime} {point.DateTime.Millisecond}ms");
					foreach (var item in point.ChannelsData)
					{
						Console.WriteLine(item);
					}
				}
			}
		}
	}
}
