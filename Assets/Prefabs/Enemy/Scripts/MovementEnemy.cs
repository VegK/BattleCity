using System;
using UnityEngine;

public class MovementEnemy : Movement
{
	[Header("Animation level")]
	[SerializeField]
	private AnimationMove[] AnimLevel;

	[Header("Animations bonus")]
	[SerializeField]
	private AnimationClip AnimBonusTop;
	[SerializeField]
	private AnimationClip AnimBonusRight;
	[SerializeField]
	private AnimationClip AnimBonusBottom;
	[SerializeField]
	private AnimationClip AnimBonusLeft;

	private EnemyController _enemyController;

	public void SetDirection(Direction value)
	{
		CurrentDirection = value;
	}

	protected override void Awake()
	{
		base.Awake();
		_enemyController = GetComponent<EnemyController>();
		_enemyController.ChangedArmorEvent += SetAnimationsLevel;
	}

	protected override void OnEnable()
	{
		CurrentDirection = Direction.Bottom;
		var obj = _enemyController.BonusAnimation.gameObject;
		switch (CurrentDirection)
		{
			case Direction.Top:
				AnimBonusTop.SampleAnimation(obj, 0);
				break;
			case Direction.Right:
				AnimBonusRight.SampleAnimation(obj, 0);
				break;
			case Direction.Bottom:
				AnimBonusBottom.SampleAnimation(obj, 0);
				break;
			case Direction.Left:
				AnimBonusLeft.SampleAnimation(obj, 0);
				break;
		}
		base.OnEnable();
	}

	protected override void FixedUpdate()
	{
		var isFreezed = _enemyController.IsFreezed;

		Animator.enabled = !isFreezed;
		_enemyController.BonusAnimation.enabled = !isFreezed;

		if (isFreezed)
			return;

		base.FixedUpdate();

		if (_enemyController.IsBonus)
			switch (CurrentDirection)
			{
				case Direction.Top:
					_enemyController.BonusAnimation.Play(AnimBonusTop.name);
					break;
				case Direction.Right:
					_enemyController.BonusAnimation.Play(AnimBonusRight.name);
					break;
				case Direction.Bottom:
					_enemyController.BonusAnimation.Play(AnimBonusBottom.name);
					break;
				case Direction.Left:
					_enemyController.BonusAnimation.Play(AnimBonusLeft.name);
					break;
			}
	}

	private void SetAnimationsLevel(int value)
	{
		if (value <= 0)
			return;
		if (AnimLevel.Length == 0)
			return;

		var index = value % AnimLevel.Length - 1;
		AnimTop = AnimLevel[index].AnimTop;
		AnimRight = AnimLevel[index].AnimRight;
		AnimBottom = AnimLevel[index].AnimBottom;
		AnimLeft = AnimLevel[index].AnimLeft;
	}

	[Serializable]
	public class AnimationMove
	{
		public AnimationClip AnimTop;
		public AnimationClip AnimRight;
		public AnimationClip AnimBottom;
		public AnimationClip AnimLeft;
	}
}
