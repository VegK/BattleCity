using System;
using UnityEngine;

public class PlayerController : BlockController, ISpawn, IDirection
{
	public SpawnController PrefabSpawn;
	public ShieldPlayer PrefabShield;
	public ExplosionController PrefabExplosion;

	public int Life = 3;

	public Direction DirectionMove
	{
		get
		{
			return _movement.CurrentDirection;
		}
	}
	public Vector3 SpawnPoint { get; set; }

	private BoxCollider2D _boxCollider;
	private MovementPlayer _movement;
	private ShieldPlayer _shield;

	public void ActiveShield(float time)
	{
		tag = "Shield";

		_shield = Instantiate(PrefabShield);
		var pos = Vector3.zero;
		pos.z = _shield.transform.position.z;
		_shield.transform.position = pos;
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
			Physics2D.IgnoreCollision(_boxCollider, item, true);
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

			obj.DestroyEvent += (s, e) => { Spawn(); };
			obj.Show(ExplosionController.ExplosionType.Object);
		}
	}

	private void Spawn()
	{
		if (EditorMode)
			return;
		if (Life <= 0)
		{
			if (GUI.GameGUIController.Instance.Player1LifeCount == 0 &&
				GUI.GameGUIController.Instance.Player2LifeCount == 0)
				GameManager.GameOver();
			return;
		}
		Life--;
		if (GUI.GameGUIController.Instance != null)
		{
			if (TypeItem == Block.Player1)
				GUI.GameGUIController.Instance.Player1LifeCount = Life;
			else if (TypeItem == Block.Player2)
				GUI.GameGUIController.Instance.Player2LifeCount = Life;
		}

		gameObject.SetActive(false);
		transform.position = SpawnPoint;

		var obj = Instantiate(PrefabSpawn);
		obj.transform.position = transform.position;
		obj.DestroyEvent += DestroySpawn;
	}

	private void DestroySpawn(object sender, EventArgs e)
	{
		gameObject.SetActive(true);
		ActiveShield(3);
	}

	private void DestroyShield(object sender, EventArgs e)
	{
		_shield = null;
		DeactiveShield();
	}
}
