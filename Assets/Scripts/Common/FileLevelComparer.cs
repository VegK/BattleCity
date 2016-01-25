using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace BattleCity
{
	public class FileLevelComparer : IComparer<FileInfo>
	{
		public int Compare(FileInfo x, FileInfo y)
		{
			var regex = new Regex(@"([0-9]+).lvl");

			// run the regex on both strings
			var xRegexResult = regex.Match(x.Name);
			var yRegexResult = regex.Match(y.Name);

			// check if they are both numbers
			if (xRegexResult.Success && yRegexResult.Success)
			{
				return int.Parse(xRegexResult.Groups[1].Value).CompareTo(int.Parse(yRegexResult.Groups[1].Value));
			}

			// otherwise return as string comparison
			return x.Name.CompareTo(y.Name);
		}
	}
}