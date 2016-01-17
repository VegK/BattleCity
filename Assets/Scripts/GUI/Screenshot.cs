using UnityEngine;

namespace BattleCity.GUI.Editor
{
	public static class Screenshot
	{
		private static Vector2 _position;
		private static Vector2 _size;

		public static byte[] GetScreenshot(out int height, out int width)
		{
			_position = FieldController.Instance.transform.position;
			_position.x -= 0.5f;
			_position.y -= 0.5f;
			_position = Camera.main.WorldToScreenPoint(_position);

			_size = new Vector2();
			_size.x = FieldController.Instance.Width - 0.5f;
			_size.y = FieldController.Instance.Height - 0.5f;
			_size = Camera.main.WorldToScreenPoint(_size);
			_size -= _position;

			var renderTexture = RenderTexture.GetTemporary(Screen.width, Screen.height);
			RenderTexture.active = renderTexture;
			Camera.main.targetTexture = renderTexture;
			Camera.main.Render();

			width = (int)_size.x;
			height = (int)_size.y;

			var screenshot = new Texture2D(width, height);
			screenshot.ReadPixels(new Rect(_position, _size), 0, 0);
			screenshot.Apply();

			Camera.main.targetTexture = null;
			Camera.main.Render();

			return screenshot.EncodeToPNG();
		}
	}
}