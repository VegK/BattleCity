using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointEnemiesManager : MonoBehaviour
{
	[SerializeField]
	private int EnemiesCount = 20;

	public static int IndexEnemy { get; private set; }
	public static int TimeRespawn
	{
		get
		{
			int level = 0;
			int playerCount = 1;

			if (GUI.GameGUIController.Instance != null)
			{
				level = GUI.GameGUIController.Instance.LevelNumber - 1;
				playerCount = GUI.GameGUIController.Instance.GetPlayerCount();
			}

			return (190 - level * 4 - (playerCount - 1) * 20) / 60;
		}
	}
	public static int MaxCountEnemies
	{
		get
		{
			int playerCount = 1;
			if (GUI.GameGUIController.Instance != null)
				playerCount = GUI.GameGUIController.Instance.GetPlayerCount();
			return (playerCount == 1) ? 4 : 6;
		}
	}

	private static SpawnPointEnemiesManager _instance;
	private static List<SpawnPointEnemies> _spawnPoints;
	private static int _enemiesOnField = 0;
	private static int _indexCurrentSpawnPoint = 0;
	private static bool _runCoroutine;

	static SpawnPointEnemiesManager()
	{
		_spawnPoints = new List<SpawnPointEnemies>();
	}


	public static int GetEnemiesCount()
	{
		if (_instance == null)
			return 0;
		return _instance.EnemiesCount;
	}

	public static void AddSpawn(SpawnPointEnemies spawnPoint)
	{
		if (_instance == null)
			return;

		_spawnPoints.Add(spawnPoint);
		if (!_runCoroutine)
			_instance.StartCoroutine(_instance.SpawnEnemies());
	}

	private void Awake()
	{
		_instance = this;
	}

	private IEnumerator SpawnEnemies()
	{
		_runCoroutine = true;
		var waitSeconds = TimeRespawn;
		while (true)
		{
			if (EnemiesCount == 0)
				yield break;
			yield return new WaitForSeconds(waitSeconds);

			if (_enemiesOnField >= MaxCountEnemies)
				continue;

			EnemiesCount--;
			_enemiesOnField++;
			GetCurrentSpawnPoint().Spawn(IndexEnemy, (s, e) => _enemiesOnField--);

			if (GUI.GameGUIController.Instance != null)
				GUI.GameGUIController.Instance.EnemiesCount = EnemiesCount;
		}
	}

	private static SpawnPointEnemies GetCurrentSpawnPoint()
	{
		var res = _spawnPoints[_indexCurrentSpawnPoint];
		_indexCurrentSpawnPoint++;
		if (_indexCurrentSpawnPoint >= _spawnPoints.Count)
			_indexCurrentSpawnPoint = 0;
		return res;
	}
}
