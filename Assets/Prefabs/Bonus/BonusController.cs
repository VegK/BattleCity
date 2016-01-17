using UnityEngine;

namespace BattleCity
{
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
		[SerializeField]
		private Sprite Sprite500Points;

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

		private void Awake()
		{
			_spriteRenderer = GetComponent<SpriteRenderer>();
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
			{
				if (Type == Bonus.Bomb)
				{
					var obj = new GameObject(Sprite500Points.name);
					obj.transform.position = transform.position;
					obj.AddComponent<SpriteRenderer>().sprite = Sprite500Points;
					Destroy(obj, Consts.TimeDestroyObjectPoints);
					FieldController.Instance.AddOtherObject(obj);
				}

				Destroy(gameObject);
			}
		}
	}
}