using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
	public float SpeedMove = 2f;

	[Header("Animations")]
	[SerializeField]
	protected AnimationClip AnimTop;
	[SerializeField]
	protected AnimationClip AnimRight;
	[SerializeField]
	protected AnimationClip AnimBottom;
	[SerializeField]
	protected AnimationClip AnimLeft;

	public Direction CurrentDirection { get; protected set; }

	protected bool AnimationEnabled;
	protected Animator Animator;
	private Collider2D _collider;
	private Direction _prevDirection;

	protected virtual void Awake()
	{
		Animator = GetComponent<Animator>();
		_collider = GetComponent<Collider2D>();
	}

	protected virtual void OnEnable()
	{
		switch (CurrentDirection)
		{
			case Direction.Top:
				AnimTop.SampleAnimation(gameObject, 0);
				break;
			case Direction.Right:
				AnimRight.SampleAnimation(gameObject, 0);
				break;
			case Direction.Bottom:
				AnimBottom.SampleAnimation(gameObject, 0);
				break;
			case Direction.Left:
				AnimLeft.SampleAnimation(gameObject, 0);
				break;
		}
	}

	protected virtual void FixedUpdate()
	{
		var beside = _collider.CheckColliderBeside(CurrentDirection);
		var move = Vector2.zero;

		switch (CurrentDirection)
		{
			case Direction.Top:
				Animator.Play(AnimTop.name);
				if (!beside)
					move.y = SpeedMove;
				break;
			case Direction.Right:
				Animator.Play(AnimRight.name);
				if (!beside)
					move.x = SpeedMove;
				break;
			case Direction.Bottom:
				Animator.Play(AnimBottom.name);
				if (!beside)
					move.y = -SpeedMove;
				break;
			case Direction.Left:
				Animator.Play(AnimLeft.name);
				if (!beside)
					move.x = -SpeedMove;
				break;
		}

		RoundPosition(CurrentDirection);
		transform.Translate(move * Time.deltaTime);
	}

	private void RoundPosition(Direction direction)
	{
		if (_prevDirection == direction)
			return;
		if (direction == Direction.Top && _prevDirection == Direction.Bottom ||
			direction == Direction.Bottom && _prevDirection == Direction.Top)
			return;
		if (direction == Direction.Right && _prevDirection == Direction.Left ||
			direction == Direction.Left && _prevDirection == Direction.Right)
			return;

		var pos = transform.position;

		if (direction == Direction.Top || direction == Direction.Bottom)
			pos.x = Mathf.Round(pos.x / Consts.SHARE) * Consts.SHARE;
		else if (direction == Direction.Right || direction == Direction.Left)
			pos.y = Mathf.Round(pos.y / Consts.SHARE) * Consts.SHARE;

		transform.position = pos;
		_prevDirection = direction;
	}
}
