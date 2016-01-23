using UnityEngine;

namespace BattleCity
{
	public class BaseController : BlockController
	{
		[SerializeField]
		private Sprite Destroyed;
		[SerializeField]
		private ExplosionController PrefabExplosion;

		[Header("Sounds")]
		[SerializeField]
		private AudioClip AudioDestroy1;
		[SerializeField]
		private AudioClip AudioDestroy2;

		private SpriteRenderer _spriteRenderer;
		private BoxCollider2D _boxCollider;

		protected override void Awake()
		{
			base.Awake();
			_spriteRenderer = GetComponent<SpriteRenderer>();
			_boxCollider = GetComponent<BoxCollider2D>();
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.tag == "Bullet")
			{
				var bullet = other.GetComponent<BulletController>();
				if (bullet != null)
				{
					if (bullet.FirstCollision)
						return;
					bullet.FirstCollision = true;
				}

				AudioManager.PlaySecondarySound(AudioDestroy1);
				AudioManager.PlaySecondarySound(AudioDestroy2);
				_spriteRenderer.sprite = Destroyed;

				_boxCollider.enabled = false;

				var obj = Instantiate(PrefabExplosion);
				var pos = transform.position;
				pos.z = PrefabExplosion.transform.position.z;
				obj.transform.position = transform.position;
				obj.Show(ExplosionController.ExplosionType.Object);

				GameManager.GameOver();
			}
		}
	}
}