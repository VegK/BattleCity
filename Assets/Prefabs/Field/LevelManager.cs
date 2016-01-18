using BattleCity.GUI.Editor;
using BattleCity.GUI.Main;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace BattleCity
{
	public class LevelManager : MonoBehaviour
	{
		[HideInInspector]
		public List<string> Levels;

		public static string CurrentLevel
		{
			get
			{
				var i = _instance._index - 1;
				if (i >= _instance.Levels.Count)
					i = i % _instance.Levels.Count;
				return _instance.Levels[i];
			}
		}
		public static int LevelNumber
		{
			get
			{
				return _instance._index;
			}
		}

		private static LevelManager _instance;
		private int _index = 0;
		private HashSet<FieldData> _levels;

		public static void Reset()
		{
			_instance._index = 0;
		}

		public static void NextLevel()
		{
			var levelsCount = _instance._levels.Count;
			while (levelsCount > 0)
			{
				levelsCount--;
				_instance._index++;
				GameGUIController.Instance.LevelNumber = LevelNumber;
				var enemiesCount = SpawnPointEnemiesManager.GetEnemiesCount();
				GameGUIController.Instance.EnemiesCount = enemiesCount;
				if (Load(CurrentLevel))
					break;
			}
		}

		public static bool Save(string name, BlockController[,] blocks,
			EnemyType[] orderSpawnEnemies)
		{
			return _instance.SaveLevel(name, blocks, orderSpawnEnemies);
		}

		public static bool Load(string name)
		{
			var data = _instance.GetLevel(name);
			if (data == null)
				return false;

			FieldController.Instance.Load(name, GameManager.SinglePlayer,
				data.Width, data.Height, data.Blocks, data.OrderSpawnEnemies);
			return true;
		}

		public static Texture2D GetPreview(string name)
		{
			var level = _instance.GetLevel(name);
			if (level == null)
				return null;

			var res = new Texture2D(level.Preview.Width, level.Preview.Height);
			res.LoadImage(level.Preview.Image);
			return res;
		}

		public static List<string> GetNameLevels()
		{
			return _instance._levels.Select(l => l.Name).ToList();
		}

		private bool SaveLevel(string name, BlockController[,] blocks,
			EnemyType[] orderSpawnEnemies)
		{
#if !UNITY_EDITOR
		try
		{
#endif
			if (!Directory.Exists(Consts.PATH))
				Directory.CreateDirectory(Consts.PATH);

			var data = GetLevel(name) ?? new FieldData();
			data.Name = name;

			int imageWidth, imageHeight;
			data.Preview.Image = Screenshot.GetScreenshot(out imageWidth, out imageHeight);
			data.Preview.Width = imageWidth;
			data.Preview.Height = imageHeight;

			data.OrderSpawnEnemies = orderSpawnEnemies;

			var width = blocks.GetLength(0);
			var height = blocks.GetLength(1);
			data.Blocks = new int[width, height];
			for (int x = 0; x < width; x++)
				for (int y = 0; y < height; y++)
				{
					var block = blocks[x, y];
					if (block != null)
						data.Blocks[x, y] = (int)block.TypeItem;
				}

			if (!_levels.Any(fd => fd.Name == name))
				_levels.Add(data);

			var formatter = new BinaryFormatter();
			var path = Consts.PATH + name + "." + Consts.EXTENSION;
			using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
				formatter.Serialize(fs, data);
#if !UNITY_EDITOR
		}
		catch
		{
			return false;
		}
#endif
			return true;
		}

		private void Awake()
		{
			_instance = this;
			BufferLevels();
		}

		private void BufferLevels()
		{
			_levels = new HashSet<FieldData>();
			if (!Directory.Exists(Consts.PATH))
				return;

			var ext = "." + Consts.EXTENSION;
			var dir = new DirectoryInfo(Consts.PATH);
			var files = dir.GetFiles("*" + ext);
			foreach (FileInfo file in files)
			{
				if (file.Extension == ext)
				{
#if !UNITY_EDITOR
		try
		{
#endif
					var formatter = new BinaryFormatter();
					var path = Consts.PATH + file.Name;
					using (var stream = new FileStream(path, FileMode.Open))
					{
						var data = formatter.Deserialize(stream) as FieldData;
						if (data != null)
						{
							data.Name = Path.GetFileNameWithoutExtension(file.Name);
							_levels.Add(data);
						}
					}
#if !UNITY_EDITOR
		}
		catch { }
#endif
				}
			}
		}

		private FieldData GetLevel(string name)
		{
			return _levels.FirstOrDefault(l => l.Name == name);
		}

		[Serializable]
		private class FieldData
		{
			[NonSerialized]
			public string Name;

			public Screenshot Preview { get; set; }

			public EnemyType[] OrderSpawnEnemies { get; set; }

			public int Width
			{
				get
				{
					return Blocks.GetLength(0);
				}
			}

			public int Height
			{
				get
				{
					return Blocks.GetLength(1);
				}
			}

			public int[,] Blocks;

			public FieldData()
			{
				Preview = new Screenshot();
			}

			[Serializable]
			public class Screenshot
			{
				public int Width { get; set; }
				public int Height { get; set; }
				public byte[] Image { get; set; }
			}
		}
	}
}