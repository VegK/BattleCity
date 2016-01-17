using UnityEngine;

namespace BattleCity.GUI
{
	public class FlashSprite : MonoBehaviour
	{
		[SerializeField]
		private SpriteRenderer Tagret;
		[SerializeField]
		private float SpeedFlash = 6f;

		private float _time;
		private float _timeDelta;
		private float _alfaStart = 1;
		private float _alfaFinish = 0;

		private void Awake()
		{
			_timeDelta = Time.deltaTime;
			if (_timeDelta == 0)
				_timeDelta = 0.02f;
		}

		private void OnEnable()
		{
			_timeDelta = Time.deltaTime;

			_time = 0;
			_alfaStart = 1;
			_alfaFinish = 0;
		}

		private void Update()
		{
			if (Tagret == null || !Tagret.gameObject.activeSelf)
				return;

			var color = Tagret.color;

			if (color.a == _alfaFinish)
			{
				_time = 0;
				_alfaFinish = _alfaStart + _alfaFinish;
				_alfaStart = _alfaFinish - _alfaStart;
				_alfaFinish = _alfaFinish - _alfaStart;
			}

			_time += _timeDelta;
			color.a = Mathf.Lerp(_alfaStart, _alfaFinish, _time * SpeedFlash);
			Tagret.color = color;
		}
	}
}