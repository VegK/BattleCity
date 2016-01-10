using UnityEngine;

public static class BlockExtension
{
	public static BlockController GetPrefab(this Block block)
	{
		switch (block)
		{
			case Block.BrickFull:
				return FieldController.Instance.PrefabBrickFull;
			case Block.BrickTop:
				return FieldController.Instance.PrefabBrickTop;
			case Block.BrickRight:
				return FieldController.Instance.PrefabBrickRight;
			case Block.BrickBottom:
				return FieldController.Instance.PrefabBrickBottom;
			case Block.BrickLeft:
				return FieldController.Instance.PrefabBrickLeft;
			case Block.BrickLeftTop:
				return FieldController.Instance.PrefabBrickLeftTop;
			case Block.BrickRightTop:
				return FieldController.Instance.PrefabBrickRightTop;
			case Block.BrickRightBottom:
				return FieldController.Instance.PrefabBrickRightBottom;
			case Block.BrickLeftBottom:
				return FieldController.Instance.PrefabBrickLeftBottom;
			case Block.MetalFull:
				return FieldController.Instance.PrefabMetalFull;
			case Block.MetalTop:
				return FieldController.Instance.PrefabMetalTop;
			case Block.MetalRight:
				return FieldController.Instance.PrefabMetalRight;
			case Block.MetalBottom:
				return FieldController.Instance.PrefabMetalBottom;
			case Block.MetalLeft:
				return FieldController.Instance.PrefabMetalLeft;
			case Block.Forest:
				return FieldController.Instance.PrefabForest;
			case Block.Water:
				return FieldController.Instance.PrefabWater;
			case Block.Ice:
				return FieldController.Instance.PrefabIce;
			case Block.Base:
				return FieldController.Instance.PrefabBase;
			case Block.Player1:
				return FieldController.Instance.PrefabPlayer1;
			case Block.Player2:
				return FieldController.Instance.PrefabPlayer2;
			case Block.EnemyRespawn:
				return FieldController.Instance.PrefabSpawnEnemies;
			default:
			case Block.Empty:
				return null;
		}
	}
}
