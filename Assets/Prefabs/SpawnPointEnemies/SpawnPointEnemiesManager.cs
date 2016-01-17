using BattleCity.Blocks;
using BattleCity.GUI.Main;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
	public class SpawnPointEnemiesManager : MonoBehaviour
	{
		public static int IndexEnemy { get; private set; }
		public static int TimeRespawn
		{
			get
			{
				int level = 0;
				int playerCount = 1;

				if (GameGUIController.Instance != null)
				{
					level = GameGUIController.Instance.LevelNumber - 1;
					playerCount = GameGUIController.Instance.GetPlayerCount();
				}

				return (190 - level * 4 - (playerCount - 1) * 20) / 60;
			}
		}
		public static int MaxCountEnemies
		{
			get
			{
				int playerCount = 1;
				if (GameGUIController.Instance != null)
					playerCount = GameGUIController.Instance.GetPlayerCount();
				return (playerCount == 1) ? 4 : 6;
			}
		}

		private static SpawnPointEnemiesManager _instance;
		private static List<SpawnPointEnemies> _spawnPoints;
		private static EnemyType[] _orderEnemiesSpawn;
		private static int _enemiesOnField = 0;
		private static int _indexCurrentSpawnPoint = 0;
		private static int _enemiesCount;

		static SpawnPointEnemiesManager()
		{
			_spawnPoints = new List<SpawnPointEnemies>();
		}

		public static void Reset()
		{
			IndexEnemy = 0;
			_spawnPoints.Clear();
			_indexCurrentSpawnPoint = 0;
			_instance.StopAllCoroutines();
		}

		public static int GetEnemiesCount()
		{
			if (_instance == null)
				return 0;
			return _enemiesCount;
		}

		public static void AddSpawn(SpawnPointEnemies spawnPoint)
		{
			if (_instance == null)
				return;

			_spawnPoints.Add(spawnPoint);
		}

		public static void SetOrderSpawnEnemies(EnemyType[] orderSpawnEnemies)
		{
			_orderEnemiesSpawn = orderSpawnEnemies;
		}

		public static void StartSpawn()
		{
			if (_instance == null)
				return;

			_enemiesCount = _orderEnemiesSpawn.Length;
			_instance.StopCoroutine(_instance.SpawnEnemies());
			_instance.StartCoroutine(_instance.SpawnEnemies());
		}

		private void Awake()
		{
			_instance = this;
			_orderEnemiesSpawn = new EnemyType[0];
		}

		private IEnumerator SpawnEnemies()
		{
			var waitSeconds = TimeRespawn;
			while (true)
			{
				if (GameGUIController.Instance != null)
					GameGUIController.Instance.EnemiesCount = _enemiesCount;

				if (_enemiesCount == 0)
					yield break;
				yield return new WaitForSeconds(waitSeconds);

				if (_enemiesOnField >= MaxCountEnemies)
					continue;

				_enemiesCount--;
				_enemiesOnField++;
				IndexEnemy++;
				GetCurrentSpawnPoint().Spawn(IndexEnemy,
					_orderEnemiesSpawn[IndexEnemy - 1],
					new EventHandler(EnemyDestroy));
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

		private void EnemyDestroy(object sender, EventArgs e)
		{
			_enemiesOnField--;
			if (_enemiesOnField == 0 && _enemiesCount == 0)
			{
				StopCoroutine(CheckEnemiesAndLoadNextLevel());
				StartCoroutine(CheckEnemiesAndLoadNextLevel());
			}
		}

		private IEnumerator CheckEnemiesAndLoadNextLevel()
		{
			yield return new WaitForSeconds(TimeRespawn);
			GameManager.NextLevel(null);
		}
	}
}