using System;
using UnityEngine;

namespace BattleCity.Enemy
{
	public class AIEnemy : MonoBehaviour
	{
		private EnemyController _enemyController;
		private Collider2D _collider;
		private float _timeBirth;
		private DirectionRotate? _directionRotate;
		private System.Random _random;

		private void Awake()
		{
			_enemyController = GetComponent<EnemyController>();
			_collider = GetComponent<Collider2D>();
			_random = new System.Random(DateTime.Now.Millisecond);
		}

		private void Start()
		{
			_timeBirth = Time.time;
		}

		private void FixedUpdate()
		{
			if (_enemyController.IsFreezed)
				return;

			var roundedPos = transform.position;
			roundedPos.x = (float)System.Math.Round(roundedPos.x, 2);
			roundedPos.y = (float)System.Math.Round(roundedPos.y, 2);
			roundedPos.x = Mathf.Floor(Mathf.Abs(roundedPos.x) * 10);
			roundedPos.y = Mathf.Floor(Mathf.Abs(roundedPos.y) * 10);

			var posX = roundedPos.x / 10 % Consts.SHARE;
			var posY = roundedPos.y / 10 % Consts.SHARE;

			if (posX == 0 && posY == 0 && _random.Next(0, 32) == 0)
			{
				if (BehaviourMove())
					return;
			}

			if (!_collider.CheckColliderBeside(_enemyController.DirectionMove))
			{
				_directionRotate = null;
				return;
			}

			// Continue rotate
			if (_directionRotate.HasValue)
			{
				if (_random.Next(0, 16) == 0)
					Rotate(_directionRotate);
				return;
			}

			if ((posX == 0 || posY == 0) && _random.Next(0, 24) == 0)
				Turning();
			else
				if (_random.Next(0, 16) == 0)
					Rotate(null);
		}

		private bool BehaviourMove()
		{
			if (Time.time <= _timeBirth + SpawnPointEnemiesManager.TimeRespawn * 8)
			{
				Rotate(null);
				return true;
			}

			if (Time.time <= _timeBirth + SpawnPointEnemiesManager.TimeRespawn * 16)
				return MovePlayer();

			return MoveBase();
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

		private void Rotate(DirectionRotate? direction)
		{
			var newDirection = _enemyController.DirectionMove;

			_directionRotate = direction ?? (DirectionRotate)_random.Next(0, 2);

			// Clockwise
			if (_directionRotate == DirectionRotate.Clockwise)
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

		private enum DirectionRotate
		{
			Clockwise = 0,
			Counterclockwise = 1
		}
	}
}