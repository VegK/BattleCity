using System;
using System.Collections;
using UnityEngine;

public class SpawnPointEnemies : BlockController, ISpawn
{
	public SpawnController PrefabSpawn;
	[Header("Enemies")]
	public EnemyController Enemy1;
	public EnemyController Enemy2;
	public EnemyController Enemy3;
	public EnemyController Enemy4;

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
	public Vector3 SpawnPoint { get; set; }

	private int _indexEnemy = 0;

	private void Start()
	{
		if (!EditorMode)
			StartCoroutine(SpawnEnemies());
	}

	private IEnumerator SpawnEnemies()
	{
		var waitSeconds = TimeRespawn;
		while (true)
		{
			yield return new WaitForSeconds(waitSeconds);
			Spawn();
		}
	}

	private void Spawn()
	{
		transform.position = SpawnPoint;

		var obj = Instantiate(PrefabSpawn);
		obj.transform.position = transform.position;
		obj.DestroyEvent += DestroySpawn;
	}

	private void DestroySpawn(object sender, EventArgs e)
	{
		CreateEnemy();
	}

	private void CreateEnemy()
	{
		var obj = Instantiate(Enemy1);
		obj.transform.position = SpawnPoint;
		obj.transform.SetParent(transform.parent, false);

		obj.Index = _indexEnemy++;
	}
}
