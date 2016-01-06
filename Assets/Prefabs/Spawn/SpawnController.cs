using System;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
	public float SpeedAnimation= 1.5f;
	public float TimeLife = 1f;

	public event EventHandler DestroyEvent;

	private Animator _animator;

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
}
