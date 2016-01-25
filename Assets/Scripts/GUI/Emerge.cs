using System;
using UnityEngine;

namespace BattleCity.GUI.Main
{
	public class Emerge : MonoBehaviour
	{
		[SerializeField]
		private float Speed = 1;
		[SerializeField]
		private DirectionEmerge Direction;

		public event EventHandler FinishEmerge;

		private RectTransform _rectBase;
		private Vector2 _startPosition;
		private Vector2 _finishPosition;
		private float _time;
		private bool _update;

		public void Show(EventHandler finish)
		{
			_update = true;
			FinishEmerge = finish;

			_startPosition = _rectBase.anchoredPosition;
			switch (Direction)
			{
				case DirectionEmerge.BottomTop:
					if (_rectBase.sizeDelta.y == 0)
						_startPosition.y -= Screen.height / 2;
					_startPosition.y -= _rectBase.sizeDelta.y;
					break;
				case DirectionEmerge.LeftRight:
					if (_rectBase.sizeDelta.x == 0)
						_startPosition.x -= Screen.width / 2;
					_startPosition.x -= _rectBase.sizeDelta.x;
					break;
				case DirectionEmerge.RightLeft:
					if (_rectBase.sizeDelta.x == 0)
						_startPosition.x += Screen.width / 2;
					_startPosition.x += _rectBase.sizeDelta.x;
					break;
				case DirectionEmerge.TopBottom:
					if (_rectBase.sizeDelta.y == 0)
						_startPosition.y += Screen.height / 2;
					_startPosition.y += _rectBase.sizeDelta.y;
					break;
			}

			_rectBase.anchoredPosition = _startPosition;
			_time = Time.realtimeSinceStartup;
		}

		public void SetFinishPosition()
		{
			_rectBase.anchoredPosition = _finishPosition;

			if (FinishEmerge != null)
				FinishEmerge(this, EventArgs.Empty);
			_update = false;
		}

		private void Awake()
		{
			_rectBase = GetComponent<RectTransform>();

			_finishPosition = _rectBase.anchoredPosition;
		}

		private void OnEnable()
		{
			_update = false;
		}

		private void Update()
		{
			if (!_update)
				return;

			var time = Time.realtimeSinceStartup - _time;
			var pos = Vector2.Lerp(_startPosition, _finishPosition, time * Speed);
			_rectBase.anchoredPosition = pos;

			if (pos == _finishPosition)
			{
				if (FinishEmerge != null)
					FinishEmerge(this, EventArgs.Empty);
				_update = false;
			}
		}

		private enum DirectionEmerge
		{
			BottomTop,
			LeftRight,
			RightLeft,
			TopBottom
		}
	}
}