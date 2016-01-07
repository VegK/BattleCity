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

	private MovementEnemy _movement;

	public void SetDirection(Direction value)
	{
		_movement.SetDirection(value);
	}

	private void Awake()
	{
		_movement = GetComponent<MovementEnemy>();
	}
}
