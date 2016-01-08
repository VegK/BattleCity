using UnityEngine;
using System.Collections;

public class BaseController : BlockController
{
	public Sprite Destroyed;
	public ExplosionController PrefabExplosion;

	private SpriteRenderer _spriteRenderer;
	private BoxCollider2D _boxCollider;

	private void Awake()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_boxCollider = GetComponent<BoxCollider2D>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Bullet")
		{
			_spriteRenderer.sprite = Destroyed;

			_boxCollider.enabled = false;

			var obj = Instantiate(PrefabExplosion);
			var pos = transform.position;
			pos.z = PrefabExplosion.transform.position.z;
			obj.transform.position = transform.position;
			obj.Show(ExplosionController.ExplosionType.Object);
		}
	}
}
