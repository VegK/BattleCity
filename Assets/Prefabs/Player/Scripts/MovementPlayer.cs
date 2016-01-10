using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class MovementPlayer : Movement
{
	private PlayerController _playerController;
	private Axis? _calcFirstAxis;
	private bool _holdHorizontal, _holdVertical;
	private string _buttonNameHorizontal, _buttonNameVertical;

	protected override void Awake()
	{
		base.Awake();

		_playerController = GetComponent<PlayerController>();
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

	private enum Axis
	{
		Horizontal,
		Vertical
	}
}
