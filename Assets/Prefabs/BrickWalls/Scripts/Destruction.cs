﻿using System;
using UnityEngine;

public class Destruction : MonoBehaviour, IDestroy
{
	public Sprite BrickTop;
	public Sprite BrickRight;
	public Sprite BrickBottom;
	public Sprite BrickLeft;
	public Sprite BrickOne1;
	public Sprite BrickOne2;

	public event EventHandler DestroyEvent;

	private SpriteRenderer _spriteRenderer;
	private BoxCollider2D _boxCollider2d;
	private TypeDestruction _typeDestruction = TypeDestruction.None;

	public void ClearEvent()
	{
		DestroyEvent = null;
	}

	private void Awake()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_boxCollider2d = GetComponent<BoxCollider2D>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Bullet")
		{
			var bullet = other.GetComponent<BulletController>();
			if (bullet == null)
				return;

			var direction = bullet.DirectionFlight;
			if (direction == Direction.Bottom)
				switch (_typeDestruction)
				{
					case TypeDestruction.None:
						DestructionTop();
						break;
					case TypeDestruction.Right:
						SetBrickOneBottomLeft();
						break;
					case TypeDestruction.Left:
						SetBrickOneBottomRight();
						break;
					case TypeDestruction.Full:
					default:
						DestructionFull();
						break;
				}
			else if (direction == Direction.Left)
				switch (_typeDestruction)
				{
					case TypeDestruction.None:
						DestructionRight();
						break;
					case TypeDestruction.Top:
						SetBrickOneBottomLeft();
						break;
					case TypeDestruction.Bottom:
						SetBrickOneTopLeft();
						break;
					case TypeDestruction.Full:
					default:
						DestructionFull();
						break;
				}
			else if (direction == Direction.Top)
				switch (_typeDestruction)
				{
					case TypeDestruction.None:
						DestructionBottom();
						break;
					case TypeDestruction.Right:
						SetBrickOneTopLeft();
						break;
					case TypeDestruction.Left:
						SetBrickOneTopRight();
						break;
					case TypeDestruction.Full:
					default:
						DestructionFull();
						break;
				}
			else if (direction == Direction.Right)
				switch (_typeDestruction)
				{
					case TypeDestruction.None:
						DestructionLeft();
						break;
					case TypeDestruction.Top:
						SetBrickOneBottomRight();
						break;
					case TypeDestruction.Bottom:
						SetBrickOneTopRight();
						break;
					case TypeDestruction.Full:
					default:
						DestructionFull();
						break;
				}
		}
	}

	private void DestructionTop()
	{
		_typeDestruction = TypeDestruction.Top;
		_spriteRenderer.sprite = BrickBottom;

		var vec = _boxCollider2d.size;
		vec.y = 0.25f;
		_boxCollider2d.size = vec;

		vec = _boxCollider2d.offset;
		vec.y = -0.125f;
		_boxCollider2d.offset = vec;
	}

	private void DestructionRight()
	{
		_typeDestruction = TypeDestruction.Right;
		_spriteRenderer.sprite = BrickLeft;

		var vec = _boxCollider2d.size;
		vec.x = 0.25f;
		_boxCollider2d.size = vec;

		vec = _boxCollider2d.offset;
		vec.x = -0.125f;
		_boxCollider2d.offset = vec;
	}

	private void DestructionBottom()
	{
		_typeDestruction = TypeDestruction.Bottom;
		_spriteRenderer.sprite = BrickTop;

		var vec = _boxCollider2d.size;
		vec.y = 0.25f;
		_boxCollider2d.size = vec;

		vec = _boxCollider2d.offset;
		vec.y = 0.125f;
		_boxCollider2d.offset = vec;
	}

	private void DestructionLeft()
	{
		_typeDestruction = TypeDestruction.Left;
		_spriteRenderer.sprite = BrickRight;

		var vec = _boxCollider2d.size;
		vec.x = 0.25f;
		_boxCollider2d.size = vec;

		vec = _boxCollider2d.offset;
		vec.x = 0.125f;
		_boxCollider2d.offset = vec;
	}

	private void SetBrickOneTopLeft()
	{
		_typeDestruction = TypeDestruction.Full;
		_spriteRenderer.sprite = BrickOne1;

		var vec = _boxCollider2d.size;
		vec.x = 0.25f;
		vec.y = 0.25f;
		_boxCollider2d.size = vec;

		_boxCollider2d.offset = Vector2.zero;

		vec = transform.position;
		vec.x -= 0.125f;
		vec.y += 0.125f;
		transform.position = vec;
	}

	private void SetBrickOneTopRight()
	{
		_typeDestruction = TypeDestruction.Full;
		_spriteRenderer.sprite = BrickOne2;

		var vec = _boxCollider2d.size;
		vec.x = 0.25f;
		vec.y = 0.25f;
		_boxCollider2d.size = vec;

		_boxCollider2d.offset = Vector2.zero;

		vec = transform.position;
		vec.x += 0.125f;
		vec.y += 0.125f;
		transform.position = vec;
	}

	private void SetBrickOneBottomRight()
	{
		_typeDestruction = TypeDestruction.Full;
		_spriteRenderer.sprite = BrickOne1;

		var vec = _boxCollider2d.size;
		vec.x = 0.25f;
		vec.y = 0.25f;
		_boxCollider2d.size = vec;

		_boxCollider2d.offset = Vector2.zero;

		vec = transform.position;
		vec.x += 0.125f;
		vec.y -= 0.125f;
		transform.position = vec;
	}

	private void SetBrickOneBottomLeft()
	{
		_typeDestruction = TypeDestruction.Full;
		_spriteRenderer.sprite = BrickOne2;

		var vec = _boxCollider2d.size;
		vec.x = 0.25f;
		vec.y = 0.25f;
		_boxCollider2d.size = vec;

		_boxCollider2d.offset = Vector2.zero;

		vec = transform.position;
		vec.x -= 0.125f;
		vec.y -= 0.125f;
		transform.position = vec;
	}

	private void DestructionFull()
	{
		Destroy(gameObject);
	}

	private void OnDestroy()
	{
		if (DestroyEvent != null)
			DestroyEvent(this, null);
	}

	private void OnApplicationQuit()
	{
		DestroyEvent = null;
	}

	private enum TypeDestruction
	{
		None,
		Top,
		Right,
		Bottom,
		Left,
		Full
	}
}
