using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class MovementPlayer : Movement
{
	private PlayerController _playerController;
	private bool _keyUpPress;
	private bool _keyRightPress;
	private bool _keyDownPress;
	private bool _keyLeftPress;
	private HashSet<KeyCode> _keysPress;

	protected override void Awake()
	{
		base.Awake();

		_playerController = GetComponent<PlayerController>();
		if (_playerController == null)
		{
			enabled = false;
			return;
		}

		_keysPress = new HashSet<KeyCode>();
	}

	protected override void OnEnable()
	{
		CurrentDirection = Direction.Top;
		_keysPress.Clear();
		base.OnEnable();
	}

	protected override void FixedUpdate()
	{
		if (_playerController.EditorMode)
			return;

		_keyUpPress = Input.GetKey(KeyCode.UpArrow);
		_keyRightPress = Input.GetKey(KeyCode.RightArrow);
		_keyDownPress = Input.GetKey(KeyCode.DownArrow);
		_keyLeftPress = Input.GetKey(KeyCode.LeftArrow);

		if (_keyUpPress && !_keysPress.Contains(KeyCode.UpArrow))
			_keysPress.Add(KeyCode.UpArrow);
		if (_keyRightPress && !_keysPress.Contains(KeyCode.RightArrow))
			_keysPress.Add(KeyCode.RightArrow);
		if (_keyDownPress && !_keysPress.Contains(KeyCode.DownArrow))
			_keysPress.Add(KeyCode.DownArrow);
		if (_keyLeftPress && !_keysPress.Contains(KeyCode.LeftArrow))
			_keysPress.Add(KeyCode.LeftArrow);

		if (_keysPress.Contains(KeyCode.UpArrow) && !_keyUpPress)
			_keysPress.Remove(KeyCode.UpArrow);
		if (_keysPress.Contains(KeyCode.RightArrow) && !_keyRightPress)
			_keysPress.Remove(KeyCode.RightArrow);
		if (_keysPress.Contains(KeyCode.DownArrow) && !_keyDownPress)
			_keysPress.Remove(KeyCode.DownArrow);
		if (_keysPress.Contains(KeyCode.LeftArrow) && !_keyLeftPress)
			_keysPress.Remove(KeyCode.LeftArrow);

		Animator.enabled = _keyUpPress || _keyRightPress || _keyDownPress || _keyLeftPress;
		if (!Animator.enabled)
			return;

		var key = _keysPress.LastOrDefault();
		switch (key)
		{
			default:
			case KeyCode.UpArrow:
				CurrentDirection = Direction.Top;
				break;
			case KeyCode.RightArrow:
				CurrentDirection = Direction.Right;
				break;
			case KeyCode.DownArrow:
				CurrentDirection = Direction.Bottom;
				break;
			case KeyCode.LeftArrow:
				CurrentDirection = Direction.Left;
				break;
		}

		base.FixedUpdate();
	}
}
