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
			default:
			case Block.Empty:
				return null;
		}
	}
}
