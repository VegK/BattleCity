﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	public PlayerController PrefabPlayer2;
	public SpawnPointEnemies PrefabSpawnEnemies;
	[Header("Settings")]
	[Range(3, 50)]
	public int Width = 13;
	[Range(3, 50)]
	public int Height = 13;

	public string Name { get; private set; }
	public bool EditorMode
	{
		get
		{
			return (FieldEditorController.Instance != null);
		}
	}
	public BonusController Bonus
	{
		get
		{
			return _bonus;
		}
		set
		{
			if (_bonus != null && (value == null || value != _bonus))
				Destroy(_bonus.gameObject);
			_bonus = value;
		}
	}
	public float TimeFreezed { get; private set; }

	public static FieldController Instance;

	private BlockController[,] _blocks;
	private BonusController _bonus;
	private HashSet<GameObject> _otherObjects;
	private HashSet<EnemyController> _enemiesObjects;

	private void Awake()
	{
		Instance = this;
		_otherObjects = new HashSet<GameObject>();
		_enemiesObjects = new HashSet<EnemyController>();
	}

	private void Start()
	{
		_blocks = new BlockController[Width, Height];
		TimeFreezed = -(Consts.TimeFreeze + 1);
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

	private void DestroyAdditionalObjects()
	{
		Bonus = null;

		foreach (GameObject obj in _otherObjects)
		{
			if (obj == null)
				continue;

			var destroy = obj.GetComponent<IDestroy>();
			if (destroy != null)
				destroy.ClearEvent();
			Destroy(obj);
		}
		_otherObjects.Clear();

		foreach (EnemyController obj in _enemiesObjects)
		{
			if (obj == null)
				continue;

			obj.ClearEvent();
			Destroy(obj);
		}
		_enemiesObjects.Clear();
	}

	public void AddOtherObject(GameObject obj)
	{
		_otherObjects.RemoveWhere(o => o == null);
		if (obj != null)
			_otherObjects.Add(obj);
	}

	public void AddEnemy(EnemyController obj)
	{
		_enemiesObjects.RemoveWhere(o => o == null || o.gameObject == null);
		if (obj != null)
			_enemiesObjects.Add(obj);
	}

	public void ExplosionEnemies()
	{
		foreach (EnemyController enemy in _enemiesObjects)
			enemy.Explosion(false);
	}

	public void FreezedEnemies()
	{
		TimeFreezed = Time.time + Consts.TimeFreeze;
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

	public Vector2 GetBonusRandomPosition()
	{
		int resX, resY;
		bool loop;

		do
		{
			resX = Random.Range(0, Width - 1);
			resY = Random.Range(0, Height - 1);

			var next = false;
			for (int x = resX; x <= resX + 1; x++)
			{
				if (x == Width - 2)
					break;

				for (int y = resY; y <= resY + 1; y++)
				{
					if (y == Height - 2)
						break;

					var block = _blocks[x, y];
					if (block == null)
						continue;

					if (block.TypeItem == Block.Player1 ||
						block.TypeItem == Block.Player2 ||
						block.TypeItem == Block.Base)
					{
						next = true;
						break;
					}
				}
				if (next)
					break;
			}
			loop = next;
		} while (loop);

		return new Vector2(resX + 0.5f, resY + 0.5f);
	}

	public BlockController GetCell(int x, int y)
	{
		return _blocks[x, y];
	}

	public void SetCell(int x, int y, Block type)
	{
		if (type == Block.Player1 || type == Block.Player2 || type == Block.Base)
		{
			var block = FindBlock(type);
			if (block != null)
			{
				Destroy(_blocks[block.X, block.Y].gameObject);
				_blocks[block.X, block.Y] = null;
			}
		}

		if (_blocks[x, y] != null)
		{
			Destroy(_blocks[x, y].gameObject);
			_blocks[x, y] = null;
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
		item.EditorMode = EditorMode;
		item.X = x;
		item.Y = y;

		_blocks[x, y] = item;
	}

	public bool Save(string name)
	{
		Name = name;
		return LevelManager.Save(name, _blocks);
	}

	public void Load(string name, bool singlePlayer, int width, int height, int[,] blocks)
	{
		Clear();
		DestroyAdditionalObjects();

		Name = name;
		Width = width;
		Height = height;

		_blocks = new BlockController[Width, Height];
		for (int x = 0; x < Width; x++)
			for (int y = 0; y < Height; y++)
				SetCell(x, y, (Block)blocks[x, y]);

		if (singlePlayer)
		{
			var player2 = FindBlock(Block.Player2);
			if (player2 != null)
				Destroy(player2.gameObject);
		}
	}

	private void Clear()
	{
		var width = _blocks.GetLength(0);
		var height = _blocks.GetLength(1);
		for (int x = 0; x < width; x++)
			for (int y = 0; y < height; y++)
				FieldController.Instance.SetCell(x, y, Block.Empty);
	}

	public BlockController FindBlock(Block type)
	{
		for (int x = 0; x < Width; x++)
			for (int y = 0; y < Height; y++)
			{
				var block = _blocks[x, y];
				if (block != null && block.TypeItem == type)
					return block;
			}
		return null;
	}
}
