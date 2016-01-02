using UnityEngine;
using System.Linq;

public class Destruction : MonoBehaviour
{
	public Sprite BrickTop;
	public Sprite BrickRight;
	public Sprite BrickBottom;
	public Sprite BrickLeft;
	public Sprite BrickOne1;
	public Sprite BrickOne2;

	private SpriteRenderer _spriteRenderer;
	private BoxCollider2D _boxCollider2d;
	private TypeDestruction _typeDestruction = TypeDestruction.None;

	private void Awake()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_boxCollider2d = GetComponent<BoxCollider2D>();
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Bullet")
		{
			var normal = other.contacts[0].normal;
			if (normal.y < 0)
				switch (_typeDestruction)
				{
					case TypeDestruction.None:
						DestructionTop();
						break;
					case TypeDestruction.Right:
						SetBrickOne2();
						break;
					case TypeDestruction.Left:
						SetBrickOne1();
						break;
					default:
						DestructionFull();
						break;
				}
			else if (normal.x < 0)
				switch (_typeDestruction)
				{
					case TypeDestruction.None:
						DestructionRight();
						break;
					case TypeDestruction.Top:
						SetBrickOne1();
						break;
					case TypeDestruction.Bottom:
						SetBrickOne2();
						break;
					default:
						DestructionFull();
						break;
				}
			else if (normal.y > 0)
				switch (_typeDestruction)
				{
					case TypeDestruction.None:
						DestructionBottom();
						break;
					case TypeDestruction.Right:
						SetBrickOne1();
						break;
					case TypeDestruction.Left:
						SetBrickOne2();
						break;
					default:
						DestructionFull();
						break;
				}
			else if (normal.x > 0)
				switch (_typeDestruction)
				{
					case TypeDestruction.None:
						DestructionLeft();
						break;
					case TypeDestruction.Top:
						SetBrickOne2();
						break;
					case TypeDestruction.Bottom:
						SetBrickOne1();
						break;
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

	private void SetBrickOne1()
	{
		_spriteRenderer.sprite = BrickOne1;
		// TODO: resize box collider
	}

	private void SetBrickOne2()
	{
		_spriteRenderer.sprite = BrickOne2;
		// TODO: resize box collider
	}

	private void DestructionFull()
	{
		Destroy(gameObject);
	}

	private enum TypeDestruction
	{
		None,
		Top,
		Right,
		Bottom,
		Left
	}
}
