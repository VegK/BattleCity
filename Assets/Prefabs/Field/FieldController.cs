using UnityEngine;
using System.Collections;

public class FieldController : MonoBehaviour
{
	[Header("Game objects")]
	public BoxCollider2D BorderTop;
	public BoxCollider2D BorderRight;
	public BoxCollider2D BorderBottom;
	public BoxCollider2D BorderLeft;
	[Space(5)]
	public GameObject Background;
	[Header("Prefabs")]
	public BlockController PrefabBrickWallFull;
	public BlockController PrefabBrickWallTop;
	public BlockController PrefabBrickWallRight;
	public BlockController PrefabBrickWallBottom;
	public BlockController PrefabBrickWallLeft;
	public BlockController PrefabMetalWallFull;
	public BlockController PrefabMetalWallTop;
	public BlockController PrefabMetalWallRight;
	public BlockController PrefabMetalWallBottom;
	public BlockController PrefabMetalWallLeft;
	public BlockController PrefabForest;
	public BlockController PrefabWater;
	public BlockController PrefabIce;
	public BlockController PrefabBase;
	public BlockController PrefabPlayer1Block;
	public PlayerController PrefabPlayer1;
	[Header("Settings")]
	[Range(3, 50)]
	public int Width = 13;
	[Range(3, 50)]
	public int Height = 13;

	public static FieldController Instance;

	private BlockController[,] _field;

	private void Awake()
	{
		Instance = this;
		_field = new BlockController[Width, Height];
	}

	public Vector2 GetPosition()
	{
		var res = transform.position;
		res.x -= 0.5f;
		res.y -= 0.5f;
		return res;
	}

	public Vector2 GetSize()
	{
		var res = new Vector2();
		res.x = Width - 0.5f;
		res.y = Height - 0.5f;
		return res;
	}

	public BlockController GetCell(int x, int y)
	{
		return _field[x, y];
	}

	public void SetCell(int x, int y, BlockController value)
	{
		if (value == null && _field[x, y] != null)
			Destroy(_field[x, y].gameObject);

		_field[x, y] = value;
	}

	private void OnDrawGizmos()
	{
		// Draw grid
		Gizmos.color = Color.white;
		for (int x = 0; x < Width; x++)
			for (int y = 0; y < Height; y++)
			{
				var pos = transform.position;
				pos.x += x;
				pos.y += y;
				Gizmos.DrawWireCube(pos, Vector2.one);
			}
	}
}
