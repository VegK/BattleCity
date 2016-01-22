using System;
using UnityEngine;

namespace BattleCity
{
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
		[SerializeField]
		protected bool ArmorPiercing = false;

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

		protected bool RunBullet()
		{
			if (_bulletCount >= MaxBullet)
				return false;
			if (Time.time - _timeLastShoot < ShotDelay)
				return false;

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

			obj.DestroyEvent += DestroyBullet;
			obj.SpeedFlight = SpeedBullet;
			obj.DirectionFlight = _direction.DirectionMove;
			obj.ArmorPiercing = ArmorPiercing;

			FieldController.Instance.AddOtherObject(obj.gameObject);

			_bulletCount++;
			_timeLastShoot = Time.time;
			return true;
		}

		private void DestroyBullet(object obj, EventArgs args)
		{
			_bulletCount--;
		}
	}
}