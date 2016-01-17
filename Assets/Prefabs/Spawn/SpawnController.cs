using System;
using UnityEngine;

namespace BattleCity
{
	public class SpawnController : MonoBehaviour, IDestroy
	{
		public float SpeedAnimation = 1.5f;
		public float TimeLife = 1f;

		public event EventHandler DestroyEvent;

		private Animator _animator;

		public void ClearEvent()
		{
			DestroyEvent = null;
		}

		private void Awake()
		{
			_animator = GetComponent<Animator>();
			_animator.speed = SpeedAnimation;
		}

		private void Start()
		{
			Destroy(gameObject, TimeLife);
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
	}
}