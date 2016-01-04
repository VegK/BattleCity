using UnityEngine;

public static class BlockExtension
{
	public static BlockController GetPrefab(this Block block)
	{
		switch (block)
		{
			case Block.BrickWallFull:
				return FieldController.Instance.PrefabBrickWallFull;
			case Block.BrickWallTop:
				return FieldController.Instance.PrefabBrickWallTop;
			case Block.BrickWallRight:
				return FieldController.Instance.PrefabBrickWallRight;
			case Block.BrickWallBottom:
				return FieldController.Instance.PrefabBrickWallBottom;
			case Block.BrickWallLeft:
				return FieldController.Instance.PrefabBrickWallLeft;
			case Block.MetalWallFull:
				return FieldController.Instance.PrefabMetalWallFull;
			case Block.MetalWallTop:
				return FieldController.Instance.PrefabMetalWallTop;
			case Block.MetalWallRight:
				return FieldController.Instance.PrefabMetalWallRight;
			case Block.MetalWallBottom:
				return FieldController.Instance.PrefabMetalWallBottom;
			case Block.MetalWallLeft:
				return FieldController.Instance.PrefabMetalWallLeft;
			case Block.Forest:
				return FieldController.Instance.PrefabForest;
			case Block.Water:
				return FieldController.Instance.PrefabWater;
			case Block.Ice:
				return FieldController.Instance.PrefabIce;
			case Block.Base:
				return FieldController.Instance.PrefabBase;
			case Block.Player1:
				return FieldController.Instance.PrefabPlayer1Block;
			default:
			case Block.Empty:
				return null;
		}
	}
}
