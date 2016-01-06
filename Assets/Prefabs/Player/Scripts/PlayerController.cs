using System;
using UnityEngine;

public class PlayerController : BlockController
{
	public Direction DirectionMove { get; set; }

	public SpawnController PrefabSpawn;
	public ShieldController PrefabShield;

	private ShieldController _shield;

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

	private void Start()
	{
		if (!EditorMode)
		{
			gameObject.SetActive(false);

			var obj = Instantiate(PrefabSpawn);
			obj.transform.position = transform.position;
			obj.DestroyEvent += DestroySpawn;
		}
	}

	private void DestroySpawn(object sender, EventArgs e)
	{
		gameObject.SetActive(true);
		ActiveShield(1.5f);
	}

	private void DestroyShield(object sender, EventArgs e)
	{
		_shield = null;
		DeactiveShield();
	}
}
