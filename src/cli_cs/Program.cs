using System;
using System.IO;

namespace cli_cs
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			using (var sw = new StreamWriter ("xxx.txt")) {
				sw.WriteLine ("hello");
			}
		}
	}
}
