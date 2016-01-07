using UnityEngine;

public class AIEnemy : MonoBehaviour
{
	private EnemyController _enemyController;
	private float _timeBirth;
	private bool _collision;

	private void Awake()
	{
		_enemyController = GetComponent<EnemyController>();
	}

	private void Start()
	{
		_timeBirth = Time.time;
	}

	private void FixedUpdate()
	{
		var roundedPos = transform.position;
		roundedPos.x = Mathf.Floor(Mathf.Abs(roundedPos.x) * 10);
		roundedPos.y = Mathf.Floor(Mathf.Abs(roundedPos.y) * 10);

		var posX = roundedPos.x % 10;
		var posY = roundedPos.y % 10;

		if (posX == 0 && posY == 0)
		{
			posX = roundedPos.x / 10 % Consts.SHARE;
			posY = roundedPos.y / 10 % Consts.SHARE;

			if (posX == 0 && posY == 0 && Random.Range(0, 16) == 0)
			{
				if (BehaviourMove())
					return;
			}
		}

		if (!_collision)
			return;
		if ((posX == 0 || posY == 0) && Random.Range(0, 4) == 0)
			Turning();
		else
			if (Random.Range(0, 2) == 0)
				Rotate();
		_collision = false;
	}

	private void OnCollisionStay2D(Collision2D other)
	{
		_collision = true;
	}

	private bool BehaviourMove()
	{
		if (Time.time <= _timeBirth + SpawnPointEnemies.TimeRespawn * 8)
			return MoveRandom();

		if (Time.time <= _timeBirth + SpawnPointEnemies.TimeRespawn * 16)
			return MovePlayer();

		return MoveBase();
	}

	private bool MoveRandom()
	{
		_enemyController.SetDirection((Direction)Random.Range(0, 4));
		return true;
	}

	private bool MovePlayer()
	{
		var selectP1 = (_enemyController.Index % 2 == 0);
		var typeBlock = selectP1 ? Block.Player1 : Block.Player2;

		var block = FieldController.Instance.FindBlock(typeBlock);
		if (block == null)
		{
			typeBlock = !selectP1 ? Block.Player1 : Block.Player2;
			block = FieldController.Instance.FindBlock(typeBlock);
			if (block == null)
				return false;
		}

		MoveTarget(block);
		return true;
	}

	private bool MoveBase()
	{
		var block = FieldController.Instance.FindBlock(Block.Base);
		if (block == null)
			return false;

		MoveTarget(block);
		return true;
	}

	private void MoveTarget(BlockController target)
	{
		var posEnemy = _enemyController.transform.position;
		posEnemy.x = Mathf.Floor(posEnemy.x * 10) / 10;
		posEnemy.y = Mathf.Floor(posEnemy.y * 10) / 10;

		var posTarget = target.transform.position;
		posTarget.x = Mathf.Floor(posTarget.x * 10) / 10;
		posTarget.y = Mathf.Floor(posTarget.y * 10) / 10;

		if (posTarget.x > posEnemy.x)
			_enemyController.SetDirection(Direction.Right);
		else if (posTarget.x < posEnemy.x)
			_enemyController.SetDirection(Direction.Left);

		if (posTarget.y > posEnemy.y)
			_enemyController.SetDirection(Direction.Top);
		else if (posTarget.y < posEnemy.y)
			_enemyController.SetDirection(Direction.Bottom);
	}

	private void Turning()
	{
		Direction newDirection;
		switch (_enemyController.DirectionMove)
		{
			default:
			case Direction.Top:
				newDirection = Direction.Bottom;
				break;
			case Direction.Right:
				newDirection = Direction.Left;
				break;
			case Direction.Bottom:
				newDirection = Direction.Top;
				break;
			case Direction.Left:
				newDirection = Direction.Right;
				break;
		}
		_enemyController.SetDirection(newDirection);
	}

	private void Rotate()
	{
		var newDirection = _enemyController.DirectionMove;

		// Clockwise
		if (Random.Range(0, 2) == 0)
		{
			newDirection++;
			if (!System.Enum.IsDefined(typeof(Direction), newDirection))
				newDirection = Direction.Top;
		}
		// Counterclockwise
		else
		{
			newDirection--;
			if (!System.Enum.IsDefined(typeof(Direction), newDirection))
				newDirection = Direction.Left;
		}

		_enemyController.SetDirection(newDirection);
	}
}
