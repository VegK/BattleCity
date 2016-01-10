using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	[HideInInspector]
	public List<string> Levels;

	public string CurrentLevel
	{
		get
		{
			var i = _index;
			if (i >= Levels.Count)
				i = _index % Levels.Count;
			return Levels[i];
		}
	}
	public static int LevelNumber
	{
		get
		{
			return _instance._index + 1;
		}
	}

	private static LevelManager _instance;
	private int _index = -1;

	public static void NextLevel(bool singlePlayer)
	{
		_instance._index++;
		FieldController.Instance.Load(_instance.CurrentLevel, singlePlayer);
		GUI.GameGUIController.Instance.LevelNumber = _instance._index + 1;
		GUI.GameGUIController.Instance.EnemiesCount = SpawnPointEnemiesManager.GetEnemiesCount();
	}

	private void Awake()
	{
		_instance = this;
	}
}
