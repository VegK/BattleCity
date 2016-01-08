using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
	public float SpeedMove = 2f;

	[Header("Animations")]
	public AnimationClip AnimTop;
	public AnimationClip AnimRight;
	public AnimationClip AnimBottom;
	public AnimationClip AnimLeft;

	public Direction CurrentDirection { get; protected set; }

	protected bool AnimationEnabled;
	protected Animator Animator;
	private Direction _prevDirection;

	protected virtual void Awake()
	{
		Animator = GetComponent<Animator>();
	}

	protected virtual void FixedUpdate()
	{
		var checkPoint = transform.position;
		var move = Vector2.zero;
		switch (CurrentDirection)
		{
			case Direction.Top:
				Animator.Play(AnimTop.name);
				checkPoint.x += 0.1f;
				checkPoint.y += 0.6f;
				if (Physics2D.OverlapPoint(checkPoint) == null)
				{
					checkPoint.x -= 0.2f;
					if (Physics2D.OverlapPoint(checkPoint) == null)
						move.y = SpeedMove;
				}
				break;
			case Direction.Right:
				Animator.Play(AnimRight.name);
				checkPoint.x += 0.6f;
				checkPoint.y += 0.1f;
				if (Physics2D.OverlapPoint(checkPoint) == null)
				{
					checkPoint.y -= 0.2f;
					if (Physics2D.OverlapPoint(checkPoint) == null)
						move.x = SpeedMove;
				}
				break;
			case Direction.Bottom:
				Animator.Play(AnimBottom.name);
				checkPoint.x += 0.1f;
				checkPoint.y -= 0.6f;
				if (Physics2D.OverlapPoint(checkPoint) == null)
				{
					checkPoint.x -= 0.2f;
					if (Physics2D.OverlapPoint(checkPoint) == null)
						move.y = -SpeedMove;
				}
				break;
			case Direction.Left:
				Animator.Play(AnimLeft.name);
				checkPoint.x -= 0.6f;
				checkPoint.y += 0.1f;
				if (Physics2D.OverlapPoint(checkPoint) == null)
				{
					checkPoint.y -= 0.2f;
					if (Physics2D.OverlapPoint(checkPoint) == null)
						move.x = -SpeedMove;
				}
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
