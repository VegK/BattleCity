using UnityEngine;

public class ShootingEnemy : Shooting
{
	private void FixedUpdate()
	{
		if (Random.Range(0, 32) == 0)
			RunBullet();
	}
}
