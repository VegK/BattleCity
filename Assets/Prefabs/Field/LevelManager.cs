using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;

public class LevelManager : MonoBehaviour
{
	public static string CurrentLevel
	{
		get
		{
			if (_instance._index >= _instance.Levels.Count)
				_instance._index = 0;
			return _instance.Levels[_instance._index];
		}
	}

	[HideInInspector]
	public List<string> Levels;

	private static LevelManager _instance;
	private int _index = -1;

	private void Awake()
	{
		_instance = this;
	}

	private void Start()
	{
		NextLevel();
	}

	public static void NextLevel()
	{
		_instance._index++;
		FieldController.Instance.Load(CurrentLevel);
	}
}
