using System;
using UnityEngine;

namespace BattleCity
{
	public class DestructionWall : MonoBehaviour, IDestroy
	{
		[SerializeField]
		private bool NotDestruction;

		[Header("Sounds")]
		[SerializeField]
		protected AudioClip AudioBullet;

		public event EventHandler DestroyEvent;

		public void ClearEvent()
		{
			DestroyEvent = null;
		}

		protected virtual void OnTriggerEnter2D(Collider2D other)
		{
			if (other.tag == "Bullet")
			{
				var bullet = other.GetComponent<BulletController>();
				if (bullet == null)
					return;

				if (bullet.ArmorPiercing && !NotDestruction)
					Destroy(gameObject);

				var layer = LayerMask.LayerToName(other.gameObject.layer);
				if (layer == "BulletPlayer1" || layer == "BulletPlayer2")
					AudioManager.PlaySecondarySound(AudioBullet);
			}
		}

		protected virtual void OnDestroy()
		{
			if (DestroyEvent != null)
				DestroyEvent(this, null);
		}

		protected virtual void OnApplicationQuit()
		{
			DestroyEvent = null;
		}
	}
}