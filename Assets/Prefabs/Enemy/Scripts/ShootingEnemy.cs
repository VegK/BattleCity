using UnityEngine;

public class ShootingEnemy : Shooting
{
	private EnemyController _enemyController;

	protected override void Awake()
	{
		base.Awake();
		_enemyController = GetComponent<EnemyController>();
	}

	private void FixedUpdate()
	{
		if (_enemyController.IsFreezed)
			return;
		if (Random.Range(0, 32) == 0)
			RunBullet();
	}
}
