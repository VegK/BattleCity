using System;
using UnityEngine;

public class Shooting : MonoBehaviour
{
	[SerializeField]
	private BulletController PrefabBullet;
	[SerializeField]
	protected float SpeedBullet = 4f;
	[SerializeField]
	protected int MaxBullet = 1;
	[SerializeField]
	protected float ShotDelay = 0.1f;

	private IDirection _direction;
	private int _bulletCount;
	private float _timeLastShoot;

	protected virtual void Awake()
	{
		_direction = GetComponent<IDirection>();
		if (_direction == null)
		{
			Debug.Log("Not find " + typeof(IDirection) + " component. Disable.");
			enabled = false;
		}

		_timeLastShoot = -ShotDelay;
	}

	protected void RunBullet()
	{
		if (_bulletCount >= MaxBullet)
			return;
		if (Time.time - _timeLastShoot < ShotDelay)
			return;

		var obj = Instantiate(PrefabBullet);
		var pos = transform.position;
		switch (_direction.DirectionMove)
		{
			case Direction.Top:
				pos.y += 0.4f;
				break;
			case Direction.Right:
				pos.x += 0.4f;
				break;
			case Direction.Bottom:
				pos.y -= 0.4f;
				break;
			case Direction.Left:
				pos.x -= 0.4f;
				break;
		}
		obj.transform.position = pos;

		obj.SpeedFlight = SpeedBullet;
		obj.DirectionFlight = _direction.DirectionMove;
		obj.DestroyEvent += DestroyBullet;

		FieldController.Instance.AddOtherObject(obj.gameObject);

		_bulletCount++;
		_timeLastShoot = Time.time;
	}

	private void DestroyBullet(object obj, EventArgs args)
	{
		_bulletCount--;
	}
}
