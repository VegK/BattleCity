using System;

public interface IDestroy
{
	event EventHandler DestroyEvent;

	void ClearEvent();
}