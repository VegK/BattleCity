namespace BattleCity.Blocks
{
	public static class BlockExtension
	{
		public static BlockController GetPrefab(this Block block)
		{
			var instance = FieldController.Instance;
			switch (block)
			{
				case Block.BrickFull:
					return instance.PrefabBrickFull;
				case Block.BrickTop:
					return instance.PrefabBrickTop;
				case Block.BrickRight:
					return instance.PrefabBrickRight;
				case Block.BrickBottom:
					return instance.PrefabBrickBottom;
				case Block.BrickLeft:
					return instance.PrefabBrickLeft;
				case Block.BrickLeftTop:
					return instance.PrefabBrickLeftTop;
				case Block.BrickRightTop:
					return instance.PrefabBrickRightTop;
				case Block.BrickRightBottom:
					return instance.PrefabBrickRightBottom;
				case Block.BrickLeftBottom:
					return instance.PrefabBrickLeftBottom;
				case Block.MetalFull:
					return instance.PrefabMetalFull;
				case Block.MetalTop:
					return instance.PrefabMetalTop;
				case Block.MetalRight:
					return instance.PrefabMetalRight;
				case Block.MetalBottom:
					return instance.PrefabMetalBottom;
				case Block.MetalLeft:
					return instance.PrefabMetalLeft;
				case Block.Forest:
					return instance.PrefabForest;
				case Block.Water:
					return instance.PrefabWater;
				case Block.Ice:
					return instance.PrefabIce;
				case Block.Base:
					return instance.PrefabBase;
				case Block.Player1:
					return instance.PrefabPlayer1;
				case Block.Player2:
					return instance.PrefabPlayer2;
				case Block.EnemyRespawn:
					return instance.PrefabSpawnEnemies;
				default:
				case Block.Empty:
					return null;
			}
		}
	}
}