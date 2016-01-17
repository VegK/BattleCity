using UnityEngine;

namespace BattleCity.GUI.Main
{
	public class GameOver : MonoBehaviour
	{
		[SerializeField]
		private Canvas UICanvas;
		[SerializeField]
		private float Speed = 1f;
		[SerializeField]
		private Vector2 StartPoint = new Vector2(0, -110);

		private float _time;
		private Vector2 _startPoint;
		private Vector2 _finishPoint;

		public void Show()
		{
			_time = Time.time;
			_finishPoint = new Vector2(Screen.width / 2, Screen.height / 2);

			var scale = UICanvas.transform.localScale;
			var correct = new Vector2(-StartPoint.x * scale.x, -StartPoint.y * scale.y);
			_startPoint = _finishPoint - correct;

			transform.position = _startPoint;
			gameObject.SetActive(true);
		}

		public void Hide()
		{
			gameObject.SetActive(false);
		}

		private void Update()
		{
			var time = (Time.time - _time) * Speed;
			var pos = Vector2.Lerp(_startPoint, _finishPoint, time);
			transform.position = pos;
		}
	}
}