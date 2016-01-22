using System;
using UnityEngine;

namespace BattleCity
{
	public class BulletController : MonoBehaviour, IDestroy
	{
		[SerializeField]
		private Sprite BulletTop;
		[SerializeField]
		private Sprite BulletRight;
		[SerializeField]
		private Sprite BulletBottom;
		[SerializeField]
		private Sprite BulletLeft;
		[SerializeField]
		private ExplosionController PrefabExplosion;

		public event EventHandler DestroyEvent;
		public Direction DirectionFlight
		{
			get
			{
				return _directionFlight;
			}
			set
			{
				switch (value)
				{
					case Direction.Top:
						_spriteRenderer.sprite = BulletTop;
						break;
					case Direction.Right:
						_spriteRenderer.sprite = BulletRight;
						break;
					case Direction.Bottom:
						_spriteRenderer.sprite = BulletBottom;
						break;
					case Direction.Left:
						_spriteRenderer.sprite = BulletLeft;
						break;
				}
				_directionFlight = value;
			}
		}
		public float SpeedFlight { get; set; }
		public bool ArmorPiercing { get; set; }
		public bool FirstCollision { get; set; }

		private SpriteRenderer _spriteRenderer;
		private Direction _directionFlight;
		private bool _destroy = false;
		private bool _explosion = false;

		public void ClearEvent()
		{
			DestroyEvent = null;
		}

		private void Awake()
		{
			_spriteRenderer = GetComponent<SpriteRenderer>();
		}

		private void FixedUpdate()
		{
			var move = Vector2.zero;
			switch (DirectionFlight)
			{
				case Direction.Top:
					move.y = SpeedFlight;
					break;
				case Direction.Right:
					move.x = SpeedFlight;
					break;
				case Direction.Bottom:
					move.y = -SpeedFlight;
					break;
				case Direction.Left:
					move.x = -SpeedFlight;
					break;
			}
			transform.Translate(move * Time.deltaTime);
		}

		private void LateUpdate()
		{
			if (_destroy)
			{
				Destroy(gameObject);
				if (DestroyEvent != null)
					DestroyEvent(this, EventArgs.Empty);

				if (_explosion)
				{
					var obj = Instantiate(PrefabExplosion);
					obj.transform.position = transform.position;
					obj.Show(ExplosionController.ExplosionType.Bullet);

					FieldController.Instance.AddOtherObject(obj.gameObject);
				}
				return;
			}
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			_destroy = true;

			var tag = other.tag;
			_explosion = (tag != "Player");
			_explosion &= (tag != "Enemy");
			_explosion &= (tag != "Shield");
			_explosion &= (tag != "Base");
			_explosion &= (tag != "Bullet");
		}

		private void OnApplicationQuit()
		{
			DestroyEvent = null;
		}
	}
}