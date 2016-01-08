using System;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDirection
{
	public ExplosionController PrefabExplosion;

	public Direction DirectionMove
	{
		get
		{
			return _movement.CurrentDirection;
		}
	}
	public int Index { get; set; }
	public event EventHandler DestroyEvent;

	private MovementEnemy _movement;

	public void SetDirection(Direction value)
	{
		_movement.SetDirection(value);
	}

	private void Awake()
	{
		_movement = GetComponent<MovementEnemy>();
	}

	private void OnDestroy()
	{
		if (DestroyEvent != null)
			DestroyEvent(this, EventArgs.Empty);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Bullet")
		{
			gameObject.SetActive(false);

			var obj = Instantiate(PrefabExplosion);
			var pos = transform.position;
			pos.z = PrefabExplosion.transform.position.z;
			obj.transform.position = transform.position;

			obj.DestroyEvent += (s, e) => { Destroy(gameObject); };
			obj.Show(ExplosionController.ExplosionType.Object);
		}
	}
}
