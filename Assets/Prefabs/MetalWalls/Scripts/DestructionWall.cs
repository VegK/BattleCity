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
		[SerializeField]
		protected AudioClip AudioArmorPiercingBullet;

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

				var audioClip = AudioBullet;

				if (bullet.ArmorPiercing && !NotDestruction)
				{
					audioClip = AudioArmorPiercingBullet;
					Destroy(gameObject);
				}

				var layer = LayerMask.LayerToName(other.gameObject.layer);
				if (layer == "BulletPlayer1" || layer == "BulletPlayer2")
					AudioManager.PlaySecondarySound(audioClip);
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