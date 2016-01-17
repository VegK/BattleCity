using UnityEditor;
using UnityEngine;

namespace BattleCity
{
	[CustomEditor(typeof(FieldController))]
	public class FieldControllerEditor : Editor
	{
		private FieldController Target
		{
			get
			{
				return target as FieldController;
			}
		}
		private int _lastWidth = -1;
		private int _lastHeight = -1;

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (Target.Width != _lastWidth || Target.Height != _lastHeight)
			{
				ResizeBorders();
				ResizeBackground();
			}
			_lastWidth = Target.Width;
			_lastHeight = Target.Height;
		}

		private void ResizeBorders()
		{
			Vector2 size, pos;

			if (Target.BorderTop != null)
			{
				size = Target.BorderTop.localScale;
				size.x = Target.Width;
				Target.BorderTop.localScale = size;

				pos = Target.BorderTop.transform.position;
				pos.x = Target.Width / 2f - 0.5f;
				pos.y = Target.Height;
				Target.BorderTop.transform.position = pos;
			}

			if (Target.BorderRight != null)
			{
				size = Target.BorderRight.localScale;
				size.y = Target.Height;
				Target.BorderRight.localScale = size;

				pos = Target.BorderRight.transform.position;
				pos.x = Target.Width;
				pos.y = Target.Height / 2f - 0.5f;
				Target.BorderRight.transform.position = pos;
			}

			if (Target.BorderBottom != null)
			{
				size = Target.BorderBottom.localScale;
				size.x = Target.Width;
				Target.BorderBottom.localScale = size;

				pos = Target.BorderBottom.transform.position;
				pos.x = Target.Width / 2f - 0.5f;
				pos.y = -1;
				Target.BorderBottom.transform.position = pos;
			}

			if (Target.BorderLeft != null)
			{
				size = Target.BorderLeft.localScale;
				size.y = Target.Height;
				Target.BorderLeft.localScale = size;

				pos = Target.BorderLeft.transform.position;
				pos.x = -1;
				pos.y = Target.Height / 2f - 0.5f;
				Target.BorderLeft.transform.position = pos;
			}
		}

		private void ResizeBackground()
		{
			if (Target.Background == null)
				return;

			Vector3 vec;

			vec = Target.Background.transform.localScale;
			vec.x = Target.Width;
			vec.y = Target.Height;
			Target.Background.transform.localScale = vec;

			vec = Target.Background.transform.position;
			vec.x = Target.Width / 2f - 0.5f;
			vec.y = Target.Height / 2f - 0.5f;
			Target.Background.transform.position = vec;
		}
	}
}