using System;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDirection
{
	public Direction DirectionMove
	{
		get
		{
			return _movement.CurrentDirection;
		}
	}
	public int Index { get; set; }
	public event EventHandler DestroyEvent;

	private MovementEnemy _movement;

	public void SetDirection(Direction value)
	{
		_movement.SetDirection(value);
	}

	private void Awake()
	{
		_movement = GetComponent<MovementEnemy>();
	}

	private void OnDestroy()
	{
		if (DestroyEvent != null)
			DestroyEvent(this, EventArgs.Empty);
	}
}
