using UnityEngine;

namespace BattleCity
{
	public class Consts
	{
		public const float SHARE = 0.5f;
		public const string EXTENSION = "lvl";
		public static readonly string PATH;

		public const int TimeShieldAfterSpawn = 3;
		public const int TimeShield = 10;
		public const int TimeFreeze = 10;
		public const int TimeShovel = 20;

		public const float TimeDestroyObjectPoints = 0.5f;
		public const int PointsBonusBomb = 500;

		public const int MaxLevelUpgradePlayer = 3;
		public const int TimeLockedMovementPlayer = 5;

		static Consts()
		{
			PATH = Application.dataPath + "/Levels/";
		}
	}
}