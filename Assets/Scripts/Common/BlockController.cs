﻿using System;
using UnityEngine;

public class BlockController : MonoBehaviour
{
	public Block TypeItem;

	public virtual bool EditorMode
	{
		get
		{
			return _editorMode;
		}
		set
		{
			_editorMode = value;
			if (_animator != null)
				_animator.enabled = !_editorMode;
		}
	}

	private Animator _animator;
	private bool _editorMode;
	private int _childsCount;

	private void Awake()
	{
		_animator = GetComponent<Animator>();
	}

	private void Start()
	{
		var childs = GetComponentsInChildren<IDestroy>();
		_childsCount = childs.Length;
		foreach (IDestroy child in childs)
			child.DestroyEvent += DestroyChild;
	}

	private void DestroyChild(object obj, EventArgs args)
	{
		_childsCount--;
		if (_childsCount <= 0)
			Destroy(gameObject);
	}
}
