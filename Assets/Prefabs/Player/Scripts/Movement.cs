using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Movement : MonoBehaviour
{
	public float Speed = 2f;

	private PlayerController _playerController;
	private Animator _animator;
	private bool _keyUpPress;
	private bool _keyRightPress;
	private bool _keyDownPress;
	private bool _keyLeftPress;
	private HashSet<KeyCode> _keysPress;
	private KeyCode _prevKey;

	private void Awake()
	{
		_playerController = GetComponent<PlayerController>();
		if (_playerController == null)
		{
			Debug.Log("ERROR: Player GameObject not attached " + typeof(PlayerController));
			enabled = false;
			return;
		}

		_animator = GetComponent<Animator>();
		_animator.enabled = false;
		_keysPress = new HashSet<KeyCode>();
	}

	private void FixedUpdate()
	{
		if (_playerController.Lock)
			return;

		// Animation movement player
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

		var key = _keysPress.LastOrDefault();

		if (_keyUpPress && key == KeyCode.UpArrow)
		{
			_animator.Play("Player1_0_MoveTop");
		}
		else if (_keyRightPress && key == KeyCode.RightArrow)
		{
			_animator.Play("Player1_0_MoveRight");
		}
		else if (_keyDownPress && key == KeyCode.DownArrow)
		{
			_animator.Play("Player1_0_MoveBottom");
		}
		else if (_keyLeftPress && key == KeyCode.LeftArrow)
		{
			_animator.Play("Player1_0_MoveLeft");
		}

		_animator.enabled = _keyUpPress || _keyRightPress || _keyDownPress || _keyLeftPress;

		if (_keysPress.Contains(KeyCode.UpArrow) && !_keyUpPress)
			_keysPress.Remove(KeyCode.UpArrow);
		if (_keysPress.Contains(KeyCode.RightArrow) && !_keyRightPress)
			_keysPress.Remove(KeyCode.RightArrow);
		if (_keysPress.Contains(KeyCode.DownArrow) && !_keyDownPress)
			_keysPress.Remove(KeyCode.DownArrow);
		if (_keysPress.Contains(KeyCode.LeftArrow) && !_keyLeftPress)
			_keysPress.Remove(KeyCode.LeftArrow);

		if (!_animator.enabled)
			return;

		// Movement gameobject player
		var move = Vector2.zero;
		switch (key)
		{
			case KeyCode.UpArrow:
				move.y = Speed;
				break;
			case KeyCode.RightArrow:
				move.x = Speed;
				break;
			case KeyCode.DownArrow:
				move.y = -Speed;
				break;
			case KeyCode.LeftArrow:
				move.x = -Speed;
				break;
		}
		RoundPosition(key);
		transform.Translate(move * Time.deltaTime);

		// Set direction movement
		switch (key)
		{
			case KeyCode.UpArrow:
				_playerController.DirectionMove = Direction.Top;
				break;
			case KeyCode.RightArrow:
				_playerController.DirectionMove = Direction.Right;
				break;
			case KeyCode.DownArrow:
				_playerController.DirectionMove = Direction.Bottom;
				break;
			case KeyCode.LeftArrow:
				_playerController.DirectionMove = Direction.Left;
				break;
		}
	}

	private void RoundPosition(KeyCode key)
	{
		if (_prevKey == key)
			return;
		if (key == KeyCode.UpArrow && _prevKey == KeyCode.DownArrow ||
			key == KeyCode.DownArrow && _prevKey == KeyCode.UpArrow)
			return;
		if (key == KeyCode.RightArrow && _prevKey == KeyCode.LeftArrow ||
			key == KeyCode.LeftArrow && _prevKey == KeyCode.RightArrow)
			return;

		var share = 0.5f;
		var pos = transform.position;

		if (key == KeyCode.UpArrow || key == KeyCode.DownArrow)
			pos.x = Mathf.Round(pos.x / share) * share;
		else if (key == KeyCode.RightArrow || key == KeyCode.LeftArrow)
			pos.y = Mathf.Round(pos.y / share) * share;

		transform.position = pos;
		_prevKey = key;
	}
}
