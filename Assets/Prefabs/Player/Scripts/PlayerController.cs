﻿using System;
using UnityEngine;

public class PlayerController : BlockController, ISpawn
{
	public Direction DirectionMove { get; set; }
	public Vector3 SpawnPoint { get; set; }

	public SpawnController PrefabSpawn;
	public ShieldPlayer PrefabShield;

	public int Life = 3;

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

	public void Spawn()
	{
		if (EditorMode)
			return;
		if (Life <= 0)
			return;
		Life--;
		if (TypeItem == Block.Player1)
			GUI.GameGUIController.Instance.Player1LifeCount = Life;
		else if (TypeItem == Block.Player2)
			GUI.GameGUIController.Instance.Player2LifeCount = Life;

		gameObject.SetActive(false);
		transform.position = SpawnPoint;

		var obj = Instantiate(PrefabSpawn);
		obj.transform.position = transform.position;
		obj.DestroyEvent += DestroySpawn;
	}

	private void Start()
	{
		Spawn();
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
