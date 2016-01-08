using UnityEngine;
using System.Collections;

public partial class FieldController : MonoBehaviour
{
	[Header("Game objects")]
	public Transform BorderTop;
	public Transform BorderRight;
	public Transform BorderBottom;
	public Transform BorderLeft;
	[Space(5)]
	public GameObject Background;
	[Header("Prefabs")]
	public BlockController PrefabBrickFull;
	public BlockController PrefabBrickTop;
	public BlockController PrefabBrickRight;
	public BlockController PrefabBrickBottom;
	public BlockController PrefabBrickLeft;
	public BlockController PrefabBrickLeftTop;
	public BlockController PrefabBrickRightTop;
	public BlockController PrefabBrickRightBottom;
	public BlockController PrefabBrickLeftBottom;
	public BlockController PrefabMetalFull;
	public BlockController PrefabMetalTop;
	public BlockController PrefabMetalRight;
	public BlockController PrefabMetalBottom;
	public BlockController PrefabMetalLeft;
	public BlockController PrefabForest;
	public BlockController PrefabWater;
	public BlockController PrefabIce;
	public BlockController PrefabBase;
	public PlayerController PrefabPlayer1;
	public SpawnPointEnemies PrefabSpawnEnemies;
	[Header("Settings")]
	[Range(3, 50)]
	public int Width = 13;
	[Range(3, 50)]
	public int Height = 13;

	public static FieldController Instance;

	private FieldManager _field;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		_field = new FieldManager(Width, Height, FieldEditorController.Instance != null);
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

	public void SetCell(int x, int y, Block type)
	{
		if (_field[x, y] != null)
		{
			Destroy(_field[x, y].gameObject);
			_field[x, y] = null;
		}

		var prefab = type.GetPrefab();
		if (prefab == null)
			return;

		var item = Instantiate(prefab);
		item.name = type.ToString();
		item.transform.SetParent(transform);

		var pos = prefab.transform.position;
		pos.x = x;
		pos.y = y;
		item.transform.position = pos;

		if (item is ISpawn)
			(item as ISpawn).SpawnPoint = pos;

		_field[x, y] = item;
	}

	public string GetName()
	{
		return _field.Name;
	}

	public bool Save(string name)
	{
		return _field.Save(name);
	}

	public void Load(string name)
	{
		_field.Load(name);
	}

	public BlockController FindBlock(Block type)
	{
		for (int x = 0; x < Width; x++)
			for (int y = 0; y < Height; y++)
			{
				var block = _field[x, y];
				if (block != null && block.TypeItem == type)
					return block;
			}
		return null;
	}
}
