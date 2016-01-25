using System;

namespace BattleCity.Enemy
{
	public class ShootingEnemy : Shooting
	{
		private EnemyController _enemyController;
		private System.Random _random;

		protected override void Awake()
		{
			base.Awake();
			_enemyController = GetComponent<EnemyController>();
			_random = new System.Random(DateTime.Now.Millisecond);
		}

		private void FixedUpdate()
		{
			if (_enemyController.IsFreezed)
				return;
			if (_random.Next(0, 32) == 0)
				RunBullet();
		}
	}
}