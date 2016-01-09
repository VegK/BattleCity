using UnityEngine;
using UnityEngine.UI;

namespace GUI
{
	public class Pause : MonoBehaviour
	{
		public float SpeedFlash = 1f;

		private Image _image;
		private float _time;
		private float _alfaStart = 1;
		private float _alfaFinish = 0;

		public void Show()
		{
			gameObject.SetActive(true);
		}

		public void Hide()
		{
			gameObject.SetActive(false);
		}

		private void Awake()
		{
			_image = GetComponent<Image>();
		}

		private void OnEnable()
		{
			_time = Time.time;
			_alfaStart = 1;
			_alfaFinish = 0;

			var color = _image.color;
			color.a = 1;
			_image.color = color;
		}

		private void Update()
		{
			var color = _image.color;

			if (color.a == _alfaFinish)
			{
				_time = Time.time;
				_alfaFinish = _alfaStart + _alfaFinish;
				_alfaStart = _alfaFinish - _alfaStart;
				_alfaFinish = _alfaFinish - _alfaStart;
			}

			var time = (Time.time - _time) * SpeedFlash;
			color.a = Mathf.Lerp(_alfaStart, _alfaFinish, time);
			_image.color = color;
		}
	}
}