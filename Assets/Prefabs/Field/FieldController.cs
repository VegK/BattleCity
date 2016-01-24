using BattleCity.Blocks;
using BattleCity.Enemy;
using BattleCity.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
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
		public BlockController PrefabMetalLeftTop;
		public BlockController PrefabMetalRightTop;
		public BlockController PrefabMetalRightBottom;
		public BlockController PrefabMetalLeftBottom;
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

		public static FieldController Instance;
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

		private BlockController[,] _blocks;
		private BonusController _bonus;
		private HashSet<GameObject> _otherObjects;
		private HashSet<EnemyController> _enemiesObjects;
		private bool _protectBase;

		public void AddOtherObject(GameObject obj)
		{
			_otherObjects.RemoveWhere(o => o == null);
			if (obj != null)
			{
				var dest = obj.GetComponent<IDestroy>();
				if (dest != null)
					dest.DestroyEvent += (s, e) => { _otherObjects.Remove(obj); };
				_otherObjects.Add(obj);
			}
		}

		public void AddEnemy(EnemyController obj)
		{
			if (obj == null)
				return;

			obj.DestroyEvent += (s, e) => { _enemiesObjects.Remove(obj); };
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

		public void ProtectBase()
		{
			_protectBase = true;
			StartCoroutine(SetProtectBase());
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
				for (int x = resX - 1; x <= resX + 2; x++)
				{
					if (x < 0)
						continue;
					if (x == Width - 2)
						break;

					for (int y = resY - 1; y <= resY + 2; y++)
					{
						if (y < 0)
							continue;
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
			var list = FieldEditorController.Instance.OrderSpawnEnemies;
			return LevelManager.Save(name, _blocks, list);
		}

		public void Load(string name, bool singlePlayer, int width, int height,
			int[,] blocks, EnemyType[] spawnEnemies)
		{
			Clear();

			Name = name;
			Width = width;
			Height = height;

			if (FieldEditorController.Instance != null)
				FieldEditorController.Instance.OrderSpawnEnemies = spawnEnemies;
			SpawnPointEnemiesManager.SetOrderSpawnEnemies(spawnEnemies);

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

		public void Clear()
		{
			TimeFreezed = Time.time - (Consts.TimeFreeze + 1);
			StopCoroutine(SetProtectBase());
			_protectBase = false;

			DestroyAdditionalObjects();

			var width = _blocks.GetLength(0);
			var height = _blocks.GetLength(1);
			for (int x = 0; x < width; x++)
				for (int y = 0; y < height; y++)
					FieldController.Instance.SetCell(x, y, Block.Empty);
		}

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
			// Draw grid field
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
				Destroy(obj.gameObject);
			}
			_enemiesObjects.Clear();
		}

		private IEnumerator SetProtectBase()
		{
			var block = FindBlock(Block.Base);
			if (block == null)
				yield break;

			SetMetalWallsBase(block.X, block.Y);

			for (int i = 0; i < Consts.TimeShovel - 3; i++)
			{
				yield return new WaitForSeconds(1);
				if (!_protectBase)
					yield break;
			}

			var loop = 3;
			while (loop-- > 0)
			{
				SetBrickWallsBase(block.X, block.Y);
				yield return new WaitForSeconds(0.5f);
				if (!_protectBase)
					yield break;

				SetMetalWallsBase(block.X, block.Y);
				yield return new WaitForSeconds(0.5f);
				if (!_protectBase)
					yield break;
			}

			SetBrickWallsBase(block.X, block.Y);
		}

		private void SetMetalWallsBase(int baseX, int baseY)
		{
			var x = baseX;
			var y = baseY + 1;
			if (y < Height)
				SetCell(x, y, Block.MetalBottom);

			x = baseX + 1;
			y = baseY + 1;
			if (x < Width && y < Height)
				SetCell(x, y, Block.MetalLeftBottom);

			x = baseX + 1;
			y = baseY;
			if (x < Width)
				SetCell(x, y, Block.MetalLeft);

			x = baseX + 1;
			y = baseY - 1;
			if (x < Width && y > 0)
				SetCell(x, y, Block.MetalLeftTop);

			x = baseX;
			y = baseY - 1;
			if (y > 0)
				SetCell(x, y, Block.MetalTop);

			x = baseX - 1;
			y = baseY - 1;
			if (x > 0 && y > 0)
				SetCell(x, y, Block.MetalRightTop);

			x = baseX - 1;
			y = baseY;
			if (x > 0)
				SetCell(x, y, Block.MetalRight);

			x = baseX - 1;
			y = baseY + 1;
			if (x > 0 && y < Height)
				SetCell(x, y, Block.MetalRightBottom);
		}

		private void SetBrickWallsBase(int baseX, int baseY)
		{
			var x = baseX;
			var y = baseY + 1;
			if (y < Height)
				SetCell(x, y, Block.BrickBottom);

			x = baseX + 1;
			y = baseY + 1;
			if (x < Width && y < Height)
				SetCell(x, y, Block.BrickLeftBottom);

			x = baseX + 1;
			y = baseY;
			if (x < Width)
				SetCell(x, y, Block.BrickLeft);

			x = baseX + 1;
			y = baseY - 1;
			if (x < Width && y > 0)
				SetCell(x, y, Block.BrickLeftTop);

			x = baseX;
			y = baseY - 1;
			if (y > 0)
				SetCell(x, y, Block.BrickTop);

			x = baseX - 1;
			y = baseY - 1;
			if (x > 0 && y > 0)
				SetCell(x, y, Block.BrickRightTop);

			x = baseX - 1;
			y = baseY;
			if (x > 0)
				SetCell(x, y, Block.BrickRight);

			x = baseX - 1;
			y = baseY + 1;
			if (x > 0 && y < Height)
				SetCell(x, y, Block.BrickRightBottom);
		}
	}
}