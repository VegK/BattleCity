﻿using System;
using UnityEngine;

/// <summary>
/// Class enemy, at create new GameObject he default unactive.
/// </summary>
public class EnemyController : MonoBehaviour, IDirection, IDestroy
{
	[SerializeField]
	private ExplosionController PrefabExplosion;
	[SerializeField]
	private Sprite SpritePoints;
	public int Points = 100;
	[Space(5)]
	[SerializeField]
	private BonusController PrefabBonus;
	public Animator BonusAnimation;

	public Direction DirectionMove
	{
		get
		{
			return _movement.CurrentDirection;
		}
	}
	public int Index { get; set; }
	public bool IsBonus
	{
		get
		{
			return BonusAnimation.gameObject.activeSelf;
		}
		set
		{
			BonusAnimation.gameObject.SetActive(value);
		}
	}
	public bool IsFreezed
	{
		get
		{
			return (Time.time <= FieldController.Instance.TimeFreezed);
		}
	}
	public event EventHandler DestroyEvent;

	private bool _isShuttingApplication;
	private BoxCollider2D _boxCollider;
	private MovementEnemy _movement;

	public void Show()
	{
		gameObject.SetActive(true);
	}

	public void SetDirection(Direction value)
	{
		_movement.SetDirection(value);
	}

	public void Explosion()
	{
		gameObject.SetActive(false);

		var obj = Instantiate(PrefabExplosion);
		var pos = transform.position;
		pos.z = PrefabExplosion.transform.position.z;
		obj.transform.position = transform.position;

		if (IsBonus)
		{
			var bns = Instantiate(PrefabBonus);
			pos = FieldController.Instance.GetBonusRandomPosition();
			bns.transform.position = pos;
			FieldController.Instance.Bonus = bns;
		}

		obj.DestroyEvent += (s, e) => { Destroy(gameObject); };
		obj.Show(ExplosionController.ExplosionType.Object);
	}

	public void ClearEvent()
	{
		DestroyEvent = null;
	}

	private void Awake()
	{
		gameObject.SetActive(false);
		_boxCollider = GetComponent<BoxCollider2D>();
		_movement = GetComponent<MovementEnemy>();
		FieldController.Instance.AddEnemy(this);
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
			if (layer != "BulletPlayer1" && layer != "BulletPlayer2")
				Physics2D.IgnoreCollision(_boxCollider, item, true);
		}

		if (IsBonus)
			FieldController.Instance.Bonus = null;
	}

	private void OnDestroy()
	{
		if (_isShuttingApplication)
			return;

		var obj = new GameObject(SpritePoints.name);
		obj.transform.position = transform.position;
		obj.AddComponent<SpriteRenderer>().sprite = SpritePoints;
		Destroy(obj, 0.5f);
		FieldController.Instance.AddOtherObject(obj);

		if (DestroyEvent != null)
			DestroyEvent(this, EventArgs.Empty);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Bullet")
			Explosion();
	}

	private void OnApplicationQuit()
	{
		_isShuttingApplication = true;
	}
}
