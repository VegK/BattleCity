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

	public Vector3 SpawnPoint { get; set; }

	public void Spawn(int indexEnemy, EventHandler destroyEnemy)
	{
		transform.position = SpawnPoint;

		var obj = Instantiate(PrefabSpawn);
		obj.transform.position = transform.position;
		obj.DestroyEvent += (s, e) =>
		{
			var enemy = Instantiate(GetEnemy());
			enemy.name = "Enemy" + indexEnemy;
			enemy.transform.position = SpawnPoint;
			enemy.transform.SetParent(transform.parent, false);

			enemy.Index = indexEnemy;
			if (indexEnemy % 7 == 4)
				enemy.IsBonus = true;
			enemy.DestroyEvent += destroyEnemy;
			enemy.Show();
		};
	}

	protected override void Start()
	{
		base.Start();
		if (EditorMode)
		{
			enabled = false;
		}
		else
		{
			Destroy(GetComponent<SpriteRenderer>());
			SpawnPointEnemiesManager.AddSpawn(this);
		}
	}

	private EnemyController GetEnemy()
	{
		var enemies = new EnemyController[] { Enemy1, Enemy2, Enemy3 };
		return enemies[UnityEngine.Random.Range(0, 3)];
	}
}
