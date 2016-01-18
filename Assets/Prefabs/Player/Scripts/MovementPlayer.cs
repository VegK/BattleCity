using UnityEngine;

namespace BattleCity.Player
{
	public class MovementPlayer : Movement
	{
		[Space(5)]
		[SerializeField]
		private float SpeedSlipOnIce = 2f;

		private PlayerController _playerController;
		private Axis? _calcFirstAxis;
		private bool _holdHorizontal;
		private bool _holdVertical;
		private string _buttonNameHorizontal;
		private string _buttonNameVertical;
		private Vector2 _startSlipIce;
		private Vector2 _finishSlipIce;
		private float _timeSlipIce;

		protected override void Awake()
		{
			base.Awake();

			_playerController = GetComponent<PlayerController>();
			_playerController.UpgradeEvent += SetAnimationsLevel;

			_buttonNameHorizontal = _playerController.TypeItem + "_Horizontal";
			_buttonNameVertical = _playerController.TypeItem + "_Vertical";
		}

		protected override void OnEnable()
		{
			CurrentDirection = Direction.Top;
			base.OnEnable();
		}

		protected override void FixedUpdate()
		{
			if (_playerController.EditorMode)
				return;

			var horizontal = Input.GetAxis(_buttonNameHorizontal);
			var vertical = Input.GetAxis(_buttonNameVertical);

			Animator.enabled = (horizontal != 0 || vertical != 0);
			if (!Animator.enabled)
			{
				SlipIce();

				_calcFirstAxis = null;
				_holdHorizontal = false;
				_holdVertical = false;
				return;
			}

			if (!_calcFirstAxis.HasValue)
			{
				if (horizontal != 0)
					_calcFirstAxis = Axis.Vertical;
				else if (vertical != 0)
					_calcFirstAxis = Axis.Horizontal;
			}
			else
			{
				if (!_holdHorizontal)
					_calcFirstAxis = Axis.Horizontal;
				else if (!_holdVertical)
					_calcFirstAxis = Axis.Vertical;
			}

			if (_calcFirstAxis == Axis.Horizontal)
			{
				if (horizontal > 0)
					CurrentDirection = Direction.Right;
				else if (horizontal < 0)
					CurrentDirection = Direction.Left;
				else if (vertical > 0)
					CurrentDirection = Direction.Top;
				else if (vertical < 0)
					CurrentDirection = Direction.Bottom;
			}
			else if (_calcFirstAxis == Axis.Vertical)
			{
				if (vertical > 0)
					CurrentDirection = Direction.Top;
				else if (vertical < 0)
					CurrentDirection = Direction.Bottom;
				else if (horizontal > 0)
					CurrentDirection = Direction.Right;
				else if (horizontal < 0)
					CurrentDirection = Direction.Left;
			}

			_holdHorizontal = (horizontal != 0);
			_holdVertical = (vertical != 0);

			base.FixedUpdate();
		}

		private void SlipIce()
		{
			var pos = transform.position;
			var x = Mathf.RoundToInt(pos.x);
			var y = Mathf.RoundToInt(pos.y);
			var block = FieldController.Instance.GetCell(x, y);
			if (block == null || block.TypeItem != Block.Ice)
				return;

			var beside = Collider.CheckColliderBeside(CurrentDirection);
			if (beside)
				return;

			if (_holdHorizontal || _holdVertical)
			{
				_timeSlipIce = Time.time;
				_startSlipIce = transform.position;

				_finishSlipIce = _startSlipIce;
				switch (CurrentDirection)
				{
					case Direction.Top:
						_finishSlipIce.y += 1;
						break;
					case Direction.Right:
						_finishSlipIce.x += 1;
						break;
					case Direction.Bottom:
						_finishSlipIce.y -= 1;
						break;
					case Direction.Left:
						_finishSlipIce.x -= 1;
						break;
				}
			}

			var time = (Time.time - _timeSlipIce) * SpeedSlipOnIce;
			transform.position = Vector2.Lerp(_startSlipIce, _finishSlipIce, time);
		}

		private enum Axis
		{
			Horizontal,
			Vertical
		}
	}
}