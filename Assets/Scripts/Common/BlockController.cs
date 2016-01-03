using System;
using UnityEngine;

public class BlockController : MonoBehaviour
{
	public Block TypeItem;

	private int _childsCount;

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
