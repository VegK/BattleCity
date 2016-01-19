using UnityEngine;

namespace BattleCity
{
	public class FlashSprite : MonoBehaviour
	{
		[SerializeField]
		private SpriteRenderer Target;
		[SerializeField]
		private float SpeedFlash = 6f;
		[SerializeField]
		private float AlfaStart = 1;
		[SerializeField]
		private float AlfaFinish = 0;

		private float _time;
		private float _timeDelta;
		private float _alfaStart;
		private float _alfaFinish;

		private void Awake()
		{
			_timeDelta = Time.deltaTime;
			if (_timeDelta == 0)
				_timeDelta = 0.02f;
		}

		private void OnEnable()
		{
			_timeDelta = Time.deltaTime;
			if (_timeDelta == 0)
				_timeDelta = 0.02f;

			_time = 0;
			_alfaStart = AlfaStart;
			_alfaFinish = AlfaFinish;
		}

		private void OnDisable()
		{
			var color = Target.color;
			color.a = AlfaStart;
			Target.color = color;
		}

		private void Update()
		{
			if (Target == null || !Target.gameObject.activeSelf)
				return;

			var color = Target.color;

			if (color.a == _alfaFinish)
			{
				_time = 0;
				_alfaFinish = _alfaStart + _alfaFinish;
				_alfaStart = _alfaFinish - _alfaStart;
				_alfaFinish = _alfaFinish - _alfaStart;
			}

			_time += _timeDelta;
			color.a = Mathf.Lerp(_alfaStart, _alfaFinish, _time * SpeedFlash);
			Target.color = color;
		}
	}
}