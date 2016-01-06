using UnityEngine;
using System.Collections;

public class ExplosionController : MonoBehaviour
{
	public AnimationClip AnimBullet;
	public AnimationClip AnimObject;

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

	private void Awake()
	{
		_animator = GetComponent<Animator>();
		if (_animator != null)
			_animator.enabled = false;
	}

	private void DestoyObject()
	{
		Destroy(gameObject);
	}

	public enum ExplosionType
	{
		Bullet,
		Object
	}
}
