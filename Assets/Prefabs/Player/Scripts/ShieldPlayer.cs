using System;
using UnityEngine;

public class ShieldPlayer : MonoBehaviour
{
	public float TimeLife = 3.5f;

	public event EventHandler DestroyEvent;

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
