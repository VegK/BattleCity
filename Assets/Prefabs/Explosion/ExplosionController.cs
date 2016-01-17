using System;
using UnityEngine;

namespace BattleCity
{
	public class ExplosionController : MonoBehaviour, IDestroy
	{
		[SerializeField]
		private AnimationClip AnimBullet;
		[SerializeField]
		private AnimationClip AnimObject;

		public event EventHandler DestroyEvent;

		private Animator _animator;

		public void Show(ExplosionType type)
		{
			if (_animator != null)
			{
				switch (type)
				{
					case ExplosionType.Bullet:
						_animator.Play(AnimBullet.name);
						break;
					case ExplosionType.Object:
						_animator.Play(AnimObject.name);
						break;
				}
				_animator.enabled = true;
			}
		}

		public void ClearEvent()
		{
			DestroyEvent = null;
		}

		private void Awake()
		{
			_animator = GetComponent<Animator>();
			if (_animator != null)
				_animator.enabled = false;
		}

		/// <summary>
		/// Method for animation Event.
		/// </summary>
		private void DestoyObject()
		{
			Destroy(gameObject);
		}

		private void OnDestroy()
		{
			if (DestroyEvent != null)
				DestroyEvent(this, EventArgs.Empty);
		}

		private void OnApplicationQuit()
		{
			DestroyEvent = null;
		}

		public enum ExplosionType
		{
			Bullet,
			Object
		}
	}
}