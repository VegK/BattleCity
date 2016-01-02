using System;
using System.Collections;
using UnityEngine;

public class BulletController : MonoBehaviour
{
	public Sprite BulletTop;
	public Sprite BulletRight;
	public Sprite BulletBottom;
	public Sprite BulletLeft;

	public Direction DirectionFlight
	{
		get
		{
			return _directionFlight;
		}
		set
		{
			switch (value)
			{
				case Direction.Top:
					_spriteRenderer.sprite = BulletTop;
					break;
				case Direction.Right:
					_spriteRenderer.sprite = BulletRight;
					break;
				case Direction.Bottom:
					_spriteRenderer.sprite = BulletBottom;
					break;
				case Direction.Left:
					_spriteRenderer.sprite = BulletLeft;
					break;
			}
			_directionFlight = value;
		}
	}
	public float SpeedFlight { get; set; }

	public event EventHandler DestroyEvent;

	private SpriteRenderer _spriteRenderer;
	private Direction _directionFlight;

	private void Awake()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void FixedUpdate()
	{
		var move = Vector2.zero;
		switch (DirectionFlight)
		{
			case Direction.Top:
				move.y = SpeedFlight;
				break;
			case Direction.Right:
				move.x = SpeedFlight;
				break;
			case Direction.Bottom:
				move.y = -SpeedFlight;
				break;
			case Direction.Left:
				move.x = -SpeedFlight;
				break;
		}
		transform.Translate(move * Time.deltaTime);
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		Destroy(gameObject);
		if (DestroyEvent != null)
			DestroyEvent(this, EventArgs.Empty);
	}
}
