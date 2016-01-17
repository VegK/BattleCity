﻿using System;
using UnityEngine;

namespace BattleCity.Player
{
	public class PlayerController : BlockController, ISpawn, IDirection
	{
		[SerializeField]
		private SpawnController PrefabSpawn;
		[SerializeField]
		private ShieldPlayer PrefabShield;
		[SerializeField]
		private ExplosionController PrefabExplosion;

		public event UpgradeHandler UpgradeEvent;
		public Direction DirectionMove
		{
			get
			{
				return _movement.CurrentDirection;
			}
		}
		public Vector3 SpawnPoint { get; set; }
		public int Upgrade
		{
			get
			{
				return _upgrade;
			}
			set
			{
				_upgrade = value;
				if (UpgradeEvent != null)
					UpgradeEvent(value);
			}
		}

		private BoxCollider2D _boxCollider;
		private MovementPlayer _movement;
		private ShieldPlayer _shield;
		private int _upgrade;

		public void ActiveShield(float time)
		{
			tag = "Shield";

			_shield = Instantiate(PrefabShield);
			_shield.transform.position = Vector3.zero;
			_shield.transform.SetParent(transform, false);

			_shield.TimeLife = time;
			_shield.DestroyEvent += DestroyShield;
		}

		public void DeactiveShield()
		{
			tag = "Player";

			if (_shield != null)
			{
				Destroy(_shield.gameObject);
				_shield = null;
			}
		}

		protected override void Awake()
		{
			base.Awake();
			_boxCollider = GetComponent<BoxCollider2D>();
			_movement = GetComponent<MovementPlayer>();
		}

		protected override void Start()
		{
			base.Start();
			Spawn();
		}

		private void OnEnable()
		{
			Vector2 pos = transform.position;
			var size = new Vector2(_boxCollider.size.x, _boxCollider.size.y);
			pos = pos - size / 2;

			Collider2D[] list;
			_boxCollider.Overlap(pos, pos + size, out list);
			foreach (Collider2D item in list)
			{
				var layer = LayerMask.LayerToName(item.gameObject.layer);
				if (layer != "BulletPlayer1" && layer != "BulletPlayer2" &&
					layer != "BulletEnemy")
					Physics2D.IgnoreCollision(_boxCollider, item, true);
			}
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.tag == "Bullet" && tag != "Shield")
			{
				gameObject.SetActive(false);

				var obj = Instantiate(PrefabExplosion);
				var pos = transform.position;
				pos.z = PrefabExplosion.transform.position.z;
				obj.transform.position = transform.position;

				obj.Show(ExplosionController.ExplosionType.Object);

				if (TypeItem == Block.Player1)
				{
					GameManager.Player1Life--;
					if (GameManager.Player1Life < 0)
						return;
				}
				else if (TypeItem == Block.Player2)
				{
					GameManager.Player2Life--;
					if (GameManager.Player2Life < 0)
						return;
				}
				else
					return;
				obj.DestroyEvent += (s, e) => { Spawn(); };
			}
			else if (other.tag == "Bonus")
			{
				var bonus = other.GetComponent<BonusController>();
				if (bonus != null)
					switch (bonus.Type)
					{
						case Bonus.Star:
							if (Upgrade < Consts.MaxLevelUpgradePlayer)
								Upgrade++;
							break;
						case Bonus.Bomb:
							if (TypeItem == Block.Player1)
								GameManager.Player1.Score += Consts.PointsBonusBomb;
							else if (TypeItem == Block.Player2)
								GameManager.Player2.Score += Consts.PointsBonusBomb;
							FieldController.Instance.ExplosionEnemies();
							break;
						case Bonus.Helmet:
							ActiveShield(Consts.TimeShield);
							break;
						case Bonus.Shovel:
							FieldController.Instance.ProtectBase();
							break;
						case Bonus.Life:
							if (TypeItem == Block.Player1)
								GameManager.Player1Life++;
							else if (TypeItem == Block.Player2)
								GameManager.Player2Life++;
							break;
						case Bonus.Time:
							FieldController.Instance.FreezedEnemies();
							break;
					}
			}
		}

		private void Spawn()
		{
			if (EditorMode)
				return;

			gameObject.SetActive(false);
			transform.position = SpawnPoint;
			Upgrade = 0;

			var obj = Instantiate(PrefabSpawn);
			obj.transform.position = transform.position;
			obj.DestroyEvent += DestroySpawn;
		}

		private void DestroySpawn(object sender, EventArgs e)
		{
			gameObject.SetActive(true);
			ActiveShield(Consts.TimeShieldAfterSpawn);
		}

		private void DestroyShield(object sender, EventArgs e)
		{
			_shield = null;
			DeactiveShield();
		}

		public delegate void UpgradeHandler(int upgrade);
	}
}