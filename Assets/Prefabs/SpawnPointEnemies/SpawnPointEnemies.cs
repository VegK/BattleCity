using BattleCity.Enemy;
using System;
using UnityEngine;

namespace BattleCity.Blocks
{
	public class SpawnPointEnemies : BlockController, ISpawn
	{
		[SerializeField]
		private SpawnController PrefabSpawn;

		[Header("Enemies")]
		[SerializeField]
		private EnemyController Enemy1;
		[SerializeField]
		private EnemyController Enemy2;
		[SerializeField]
		private EnemyController Enemy3;
		[SerializeField]
		private EnemyController Enemy4;

		public Vector3 SpawnPoint { get; set; }

		public void Spawn(int indexEnemy, EnemyType enemyType, EventHandler destroyEnemy)
		{
			transform.position = SpawnPoint;

			var obj = Instantiate(PrefabSpawn);
			obj.transform.position = transform.position;
			obj.DestroyEvent += (s, e) =>
			{
				var enemy = Instantiate(GetEnemy(enemyType));
				enemy.name = "Enemy" + indexEnemy;
				enemy.transform.position = SpawnPoint;
				enemy.transform.SetParent(transform.parent, false);

				enemy.Index = indexEnemy;
				if (indexEnemy % 7 == 4)
					enemy.IsBonus = true;
				enemy.DestroyEvent += destroyEnemy;
				enemy.Show();
			};

			FieldController.Instance.AddOtherObject(obj.gameObject);
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

		private EnemyController GetEnemy(EnemyType type)
		{
			switch (type)
			{
				default:
				case EnemyType.Enemy1:
					return Enemy1;
				case EnemyType.Enemy2:
					return Enemy2;
				case EnemyType.Enemy3:
					return Enemy3;
				case EnemyType.Enemy4:
					return Enemy4;
			}
		}
	}
}