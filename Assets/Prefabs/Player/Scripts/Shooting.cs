using UnityEngine;
using System.Collections;
using System;

public class Shooting : MonoBehaviour
{
	public BulletController PrefabBullet;
	public float SpeedBullet = 4f;
	public int MaxBullet = 1;

	private PlayerController _playerController;
	private int _bulletCount;

	private void Awake()
	{
		_playerController = GetComponent<PlayerController>();
		if (_playerController == null)
		{
			Debug.Log("ERROR: Player GameObject not attached " + typeof(PlayerController));
			enabled = false;
		}
	}
	
	private void FixedUpdate()
	{
		if (Input.GetKeyDown(KeyCode.Space))
			RunBullet();
	}

	private void RunBullet()
	{
		if (_bulletCount >= MaxBullet)
			return;

		var obj = Instantiate(PrefabBullet);
		var pos = transform.position;
		switch (_playerController.DirectionMove)
		{
			case Direction.Top:
				pos.y += 0.5f;
				break;
			case Direction.Right:
				pos.x += 0.5f;
				break;
			case Direction.Bottom:
				pos.y -= 0.5f;
				break;
			case Direction.Left:
				pos.x -= 0.5f;
				break;
		}
		obj.transform.position = pos;

		obj.SpeedFlight = SpeedBullet;
		obj.DirectionFlight = _playerController.DirectionMove;
		obj.DestroyEvent += DestroyBullet;

		_bulletCount++;
	}

	private void DestroyBullet(object obj, EventArgs args)
	{
		_bulletCount--;
	}
}
