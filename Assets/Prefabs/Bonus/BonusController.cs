using UnityEngine;

public class BonusController : MonoBehaviour
{
	[SerializeField]
	private Sprite SpriteLife;
	[SerializeField]
	private Sprite SpriteStar;
	[SerializeField]
	private Sprite SpriteBomb;
	[SerializeField]
	private Sprite SpriteHelmet;
	[SerializeField]
	private Sprite SpriteShovel;
	[SerializeField]
	private Sprite SpriteTime;

	public float SpeedFlash = 6f;

	public Bonus Type
	{
		get
		{
			return _type;
		}
		set
		{
			switch (value)
			{
				case Bonus.Life:
					_spriteRenderer.sprite = SpriteLife;
					break;
				case Bonus.Star:
					_spriteRenderer.sprite = SpriteStar;
					break;
				case Bonus.Helmet:
					_spriteRenderer.sprite = SpriteHelmet;
					break;
				case Bonus.Shovel:
					_spriteRenderer.sprite = SpriteShovel;
					break;
				case Bonus.Bomb:
					_spriteRenderer.sprite = SpriteBomb;
					break;
				case Bonus.Time:
					_spriteRenderer.sprite = SpriteTime;
					break;
			}
			_type = value;
		}
	}

	private SpriteRenderer _spriteRenderer;
	private Bonus _type;
	private float _time;
	private float _deltaTime;
	private float _alfaStart = 1;
	private float _alfaFinish = 0;

	private void Awake()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_deltaTime = Time.deltaTime;
		if (_deltaTime == 0)
			_deltaTime = 0.02f;
	}

	private void Update()
	{
		var color = _spriteRenderer.color;

		if (color.a == _alfaFinish)
		{
			_time = 0;
			_alfaFinish = _alfaStart + _alfaFinish;
			_alfaStart = _alfaFinish - _alfaStart;
			_alfaFinish = _alfaFinish - _alfaStart;
		}

		_time += _deltaTime;
		color.a = Mathf.Lerp(_alfaStart, _alfaFinish, _time * SpeedFlash);
		_spriteRenderer.color = color;
	}

	private void OnEnable()
	{
		var rnd = Random.Range(0, 8);
		if (!System.Enum.IsDefined(typeof(Bonus), rnd))
			rnd -= 6;
		Type = (Bonus)rnd;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player" || other.tag == "Shield")
			Destroy(gameObject);
	}
}
