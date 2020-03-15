using System;
using System.Collections.Generic;
using PSP1N;

namespace Sample
{
	public class Point : ISerializer
	{
		public DateTime DateTime { get; set; } = new DateTime(1970, 1, 1);
		public int[] ChannelsData { get; set; } = new int[10];

		public void GetData(Stack<uint> data)
		{
			uint seconds = (uint)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
			data.Push(seconds);
			data.Push((uint)DateTime.Millisecond);
			foreach (var channel in ChannelsData)
			{
				data.Push((uint)channel);
			}
		}

		public void SetData(Stack<uint> data)
		{
			DateTime = DateTime.AddSeconds(data.Pop());
			DateTime = DateTime.AddMilliseconds(data.Pop());
			for (int i = 0; i < ChannelsData.Length; i++)
			{
				ChannelsData[i] = (int)data.Pop();
			}
		}
	}
}
