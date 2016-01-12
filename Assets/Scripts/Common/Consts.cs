using UnityEngine;

public class Consts
{
	public const float SHARE = 0.5f;
	public const string EXTENSION = "lvl";
	public static readonly string PATH;

	public const int TimeShieldAfterSpawn = 3;
	public const int TimeShield = 10;
	public const int TimeFreeze = 10;

	static Consts()
	{
		PATH = Application.dataPath + "/Levels/";
	}
}
