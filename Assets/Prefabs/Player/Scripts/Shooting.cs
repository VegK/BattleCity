using UnityEngine;
using System.Collections;
using System;

public class Shooting : MonoBehaviour
{
	public BulletController PrefabBullet;
	public float SpeedBullet = 4f;
	public int MaxBullet = 1;
	public float ShotDelay = 0.1f;

	private PlayerController _playerController;
	private int _bulletCount;
	private float _timeLastShoot;

	private void Awake()
	{
		_playerController = GetComponent<PlayerController>();
		if (_playerController == null)
		{
			Debug.Log("ERROR: Player GameObject not attached " + typeof(PlayerController));
			enabled = false;
		}

		_timeLastShoot = -ShotDelay;
	}
	
	private void FixedUpdate()
	{
		if (_playerController.EditorMode)
			return;
		if (Input.GetKey(KeyCode.Space))
			RunBullet();
	}

	private void RunBullet()
	{
		if (_bulletCount >= MaxBullet)
			return;
		if (Time.time - _timeLastShoot < ShotDelay)
			return;

		var obj = Instantiate(PrefabBullet);
		var pos = transform.position;
		switch (_playerController.DirectionMove)
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
		obj.DirectionFlight = _playerController.DirectionMove;
		obj.DestroyEvent += DestroyBullet;

		_bulletCount++;
		_timeLastShoot = Time.time;
	}

	private void DestroyBullet(object obj, EventArgs args)
	{
		_bulletCount--;
	}
}
