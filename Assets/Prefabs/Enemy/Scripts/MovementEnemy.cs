using UnityEngine;

public class MovementEnemy : Movement
{
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

		var animator = _enemyController.BonusAnimation;
		var obj = animator.gameObject;

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
		animator.enabled = false;

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
}
