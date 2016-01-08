using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	public string CurrentLevel
	{
		get
		{
			if (_index >= Levels.Count)
				_index = 0;
			return Levels[_index];
		}
	}

	[HideInInspector]
	public List<string> Levels;

	private int _index = -1;

	public void NextLevel()
	{
		_index++;
		FieldController.Instance.Load(CurrentLevel);
		GUI.GameGUIController.Instance.LevelNumber = _index + 1;
		GUI.GameGUIController.Instance.EnemiesCount = SpawnPointEnemiesManager.GetEnemiesCount();
	}
}
