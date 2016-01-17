using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;

namespace BattleCity
{
	public class BuildPostprocessor
	{
		[PostProcessBuild(1)]
		public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
		{
			var path = Path.GetDirectoryName(pathToBuiltProject);
			var fileName = Path.GetFileNameWithoutExtension(pathToBuiltProject);
			var to = path + "/" + fileName + "_Data/Levels/";
			Directory.CreateDirectory(to);

			var files = new DirectoryInfo(Consts.PATH).GetFiles("*." + Consts.EXTENSION);
			foreach (FileInfo file in files)
				FileUtil.CopyFileOrDirectory(Consts.PATH + file.Name, to + file.Name);
		}
	}
}