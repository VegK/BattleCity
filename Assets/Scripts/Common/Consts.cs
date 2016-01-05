using UnityEngine;

public class Consts
{
	public const string EXTENSION = "lvl";
	public static readonly string PATH;

	static Consts()
	{
		PATH = Application.dataPath + "/Levels/";
	}
}
