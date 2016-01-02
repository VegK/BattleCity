using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FieldController))]
public class FieldControllerEditor : Editor
{
	private FieldController _field;
	private int _lastWidth = -1;
	private int _lastHeight = -1;

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		_field = target as FieldController;

		if (_field.Width != _lastWidth || _field.Height != _lastHeight)
		{
			ResizeBorders();
			ResizeBackground();
		}
		_lastWidth = _field.Width;
		_lastHeight = _field.Height;
	}

	private void ResizeBorders()
	{
		Vector2 size, pos;

		if (_field.BorderTop != null)
		{
			size = _field.BorderTop.size;
			size.x = _field.Width;
			_field.BorderTop.size = size;

			pos = _field.BorderTop.transform.position;
			pos.x = _field.Width / 2f - 0.5f;
			pos.y = _field.Height;
			_field.BorderTop.transform.position = pos;
		}

		if (_field.BorderRight != null)
		{
			size = _field.BorderRight.size;
			size.y = _field.Height;
			_field.BorderRight.size = size;

			pos = _field.BorderRight.transform.position;
			pos.x = _field.Width;
			pos.y = _field.Height / 2f - 0.5f;
			_field.BorderRight.transform.position = pos;
		}

		if (_field.BorderBottom != null)
		{
			size = _field.BorderBottom.size;
			size.x = _field.Width;
			_field.BorderBottom.size = size;

			pos = _field.BorderBottom.transform.position;
			pos.x = _field.Width / 2f - 0.5f;
			pos.y = -1;
			_field.BorderBottom.transform.position = pos;
		}

		if (_field.BorderLeft != null)
		{
			size = _field.BorderLeft.size;
			size.y = _field.Height;
			_field.BorderLeft.size = size;

			pos = _field.BorderLeft.transform.position;
			pos.x = -1;
			pos.y = _field.Height / 2f - 0.5f;
			_field.BorderLeft.transform.position = pos;
		}
	}

	private void ResizeBackground()
	{
		if (_field.Background == null)
			return;

		Vector3 vec;

		vec = _field.Background.transform.localScale;
		vec.x = _field.Width;
		vec.y = _field.Height;
		_field.Background.transform.localScale = vec;

		vec = _field.Background.transform.position;
		vec.x = _field.Width / 2f - 0.5f;
		vec.y = _field.Height / 2f - 0.5f;
		_field.Background.transform.position = vec;
	}
}
