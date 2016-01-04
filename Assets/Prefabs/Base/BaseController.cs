using UnityEngine;
using System.Collections;

public class BaseController : BlockController
{
	public Sprite Destroyed;

	private SpriteRenderer _spriteRenderer;

	private void Awake()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Bullet")
		{
			_spriteRenderer.sprite = Destroyed;
		}
	}
}
