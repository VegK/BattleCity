﻿using UnityEngine;

namespace BattleCity
{
	public class Consts
	{
		public const string VERSION = "1.1";

		public const float SHARE = 0.5f;
		public const string EXTENSION = "lvl";
		public static readonly string PATH;

		public const int DefaultHighScore = 20000;
		public const int TimeShieldAfterSpawn = 3;
		public const int TimeShield = 10;
		public const int TimeFreeze = 10;
		public const int TimeShovel = 20;

		public const float TimeDestroyObjectPoints = 0.5f;
		public const int PointsBonus = 500;

		public const int MaxLevelUpgradePlayer = 3;
		public const int TimeLockedMovementPlayer = 5;
		public const int ScoreForLife = 20000;

		static Consts()
		{
			PATH = Application.dataPath + "/Levels/";
		}
	}
}