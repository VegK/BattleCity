using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GUI
{
	public class LoadLevelSceneController : MonoBehaviour
	{
		[SerializeField]
		private Image TopPanel;
		[SerializeField]
		private Image BottomPanel;
		[SerializeField]
		private Text LevelNumber;
		[SerializeField]
		private string NameLevel = "STAGE";
		[SerializeField]
		private float SpeedMovePanels = 1f;
		[SerializeField]
		private float DelayBeforePressKey = 1f;
		[SerializeField]
		private Vector2 ShowPositionTopPanel = new Vector2(0, 50);
		[SerializeField]
		private Vector2 ShowPositionBottomPanel = new Vector2(0, -50);
		[SerializeField]
		private Vector2 HidePositionTopPanel = new Vector2(0, 150);
		[SerializeField]
		private Vector2 HidePositionBottomPanel = new Vector2(0, -150);

		private static LoadLevelSceneController _instance;
		private event EventHandler _finishEvent;
		private event EventHandler _pressAnyKeyEvent;
		private float _timeDelta;
		private bool _finish;
		private float _delayBeforePressKey;

		public static void Show(int numberLevel, EventHandler finishShowEvent,
			EventHandler pressAnyKeyEvent)
		{
			_instance._finishEvent = finishShowEvent;
			_instance._pressAnyKeyEvent = pressAnyKeyEvent;

			_instance.TopPanel.transform.localPosition = _instance.HidePositionTopPanel;
			_instance.BottomPanel.transform.localPosition = _instance.HidePositionBottomPanel;

			_instance.LevelNumber.text = _instance.NameLevel + " " + numberLevel;
			_instance.gameObject.SetActive(true);
			_instance.StartCoroutine(_instance.MovePanels(true));
		}

		public static void Hide(EventHandler finishHideEvent)
		{
			_instance._finishEvent = finishHideEvent;
			_instance._pressAnyKeyEvent = null;

			_instance.TopPanel.transform.localPosition = _instance.ShowPositionTopPanel;
			_instance.BottomPanel.transform.localPosition = _instance.ShowPositionBottomPanel;

			_instance.gameObject.SetActive(true);
			_instance.StartCoroutine(_instance.MovePanels(false));
		}

		private void Awake()
		{
			_instance = this;
			_timeDelta = Time.deltaTime;
			if (_timeDelta == 0)
				_timeDelta = 0.02f;
			gameObject.SetActive(false);
		}

		private void OnEnable()
		{
			_delayBeforePressKey = DelayBeforePressKey;
		}

		private void Update()
		{
			if (_finish)
			{
				if (_delayBeforePressKey > 0)
					_delayBeforePressKey -= _timeDelta;
				else
					if (Input.anyKey && _pressAnyKeyEvent != null)
						_pressAnyKeyEvent(this, EventArgs.Empty);
			}
		}

		private void OnApplicationQuit()
		{
			_finishEvent = null;
			_pressAnyKeyEvent = null;
		}

		private IEnumerator MovePanels(bool show)
		{
			_finish = false;
			LevelNumber.gameObject.SetActive(false);

			Vector3 startTop;
			Vector3 startBottom;
			Vector3 finishTop;
			Vector3 finishBottom;
			if (show)
			{
				startTop = HidePositionTopPanel;
				startBottom = HidePositionBottomPanel;
				finishTop = ShowPositionTopPanel;
				finishBottom = ShowPositionBottomPanel;
			}
			else
			{
				startTop = ShowPositionTopPanel;
				startBottom = ShowPositionBottomPanel;
				finishTop = HidePositionTopPanel;
				finishBottom = HidePositionBottomPanel;
			}

			var timer = 0f;
			while (TopPanel.transform.localPosition != finishTop ||
				BottomPanel.transform.localPosition != finishBottom)
			{
				var time = timer * SpeedMovePanels;

				var pos = Vector3.Lerp(startTop, finishTop, time);
				TopPanel.transform.localPosition = pos;

				pos = Vector3.Lerp(startBottom, finishBottom, time);
				BottomPanel.transform.localPosition = pos;

				timer += _timeDelta;
				yield return null;
			}

			if (show)
				LevelNumber.gameObject.SetActive(true);
			else
				gameObject.SetActive(false);

			if (_finishEvent != null)
				_finishEvent(this, EventArgs.Empty);
			_finish = true;
		}
	}
}