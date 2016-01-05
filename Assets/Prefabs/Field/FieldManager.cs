﻿using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public partial class FieldController
{
	private class FieldManager
	{
		private const string EXTENSION = "lvl";
		private readonly string PATH;

		public BlockController this[int x, int y]
		{
			get
			{
				return _blocks[x, y];
			}
			set
			{
				_blocks[x, y] = value;
			}
		}

		private BlockController[,] _blocks;

		public FieldManager()
		{
			PATH = Application.dataPath + "/Levels/";
		}

		public FieldManager(int width, int height) : this()
		{
			_blocks = new BlockController[width, height];
		}

		public bool Save(string name)
		{
#if !UNITY_EDITOR
		try
		{
#endif
			if (!Directory.Exists(PATH))
				Directory.CreateDirectory(PATH);

			var width = _blocks.GetLength(0);
			var height = _blocks.GetLength(1);
			var data = new FieldData();
			data.Blocks = new int[width, height];
			for (int x = 0; x < width; x++)
				for (int y = 0; y < height; y++)
				{
					var block = _blocks[x, y];
					if (block != null)
						data.Blocks[x, y] = (int)block.TypeItem;
				}

			var formatter = new BinaryFormatter();
			var path = PATH + name + "." + EXTENSION;
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

		public void Load(string name)
		{
#if !UNITY_EDITOR
		try
		{
#endif
			if (!Directory.Exists(PATH))
				Directory.CreateDirectory(PATH);

			var formatter = new BinaryFormatter();
			var path = PATH + name + "." + EXTENSION;
			var stream = new FileStream(path, FileMode.Open);
			var data = (FieldData)formatter.Deserialize(stream);
			stream.Dispose();

			Clear();
			var width = data.Width;
			var height = data.Height;
			_blocks = new BlockController[width, height];
			for (int x = 0; x < width; x++)
				for (int y = 0; y < height; y++)
					FieldController.Instance.SetCell(x, y, (Block)data.Blocks[x, y]);
#if !UNITY_EDITOR
		}
		catch
		{
			return false;
		}
#endif
		}

		private void Clear()
		{
			var width = _blocks.GetLength(0);
			var height = _blocks.GetLength(1);
			for (int x = 0; x < width; x++)
				for (int y = 0; y < height; y++)
					FieldController.Instance.SetCell(x, y, Block.Empty);
		}

		[Serializable]
		private class FieldData
		{
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
		}
	}
}