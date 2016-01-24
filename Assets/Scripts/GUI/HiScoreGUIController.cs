using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCity.GUI.Main
{
	public class HiScoreGUIController : MonoBehaviour
	{
		[SerializeField]
		private Color StartColor;
		[SerializeField]
		private Color FinishColor;
		[SerializeField]
		private float SpeedFlashColor = 30f;
		[SerializeField]
		private Image HiScoreText;
		[SerializeField]
		private Transform ParentNumbers;

		[Header("Numbers")]
		[SerializeField]
		private Sprite Number0;
		[SerializeField]
		private Sprite Number1;
		[SerializeField]
		private Sprite Number2;
		[SerializeField]
		private Sprite Number3;
		[SerializeField]
		private Sprite Number4;
		[SerializeField]
		private Sprite Number5;
		[SerializeField]
		private Sprite Number6;
		[SerializeField]
		private Sprite Number7;
		[SerializeField]
		private Sprite Number8;
		[SerializeField]
		private Sprite Number9;

		[Header("Sounds")]
		[SerializeField]
		private AudioClip AudioHiScore;

		private static HiScoreGUIController _instance;
		private HashSet<Image> _numbers;
		private float _time;

		public static void Show(int highScore, EventHandler FinishedSoundEvent)
		{
			PlayerPrefs.SetInt("HiScore", highScore);
			_instance.VisibleHightScore(highScore);
			_instance.gameObject.SetActive(true);
			AudioManager.PlayMainSound(_instance.AudioHiScore);
			AudioManager.FinishPlayMainSoundEvent += FinishedSoundEvent;
			_instance._time = Time.time;
		}

		public static void Hide()
		{
			_instance.gameObject.SetActive(false);
		}

		private void Awake()
		{
			_instance = this;
			_numbers = new HashSet<Image>();
			Hide();
		}

		private void Update()
		{
			var time = Time.time - _time;
			var color = Color.Lerp(StartColor, FinishColor, time * SpeedFlashColor);

			HiScoreText.color = color;
			foreach (Image number in _numbers)
				number.color = color;

			if (color == FinishColor)
			{
				_time = Time.time;
				var tmp = StartColor;
				StartColor = FinishColor;
				FinishColor = tmp;
			}
		}

		private void VisibleHightScore(int score)
		{
			_instance._numbers.Clear();

			var count = ParentNumbers.childCount;
			for (int i = 0; i < count; i++)
				Destroy(ParentNumbers.GetChild(i).gameObject);

			var strScore = score.ToString();
			for (int i = 0; i < strScore.Length; i++)
				switch (strScore[i])
				{
					case '0':
						CreateNumber(Number0);
						break;
					case '1':
						CreateNumber(Number1);
						break;
					case '2':
						CreateNumber(Number2);
						break;
					case '3':
						CreateNumber(Number3);
						break;
					case '4':
						CreateNumber(Number4);
						break;
					case '5':
						CreateNumber(Number5);
						break;
					case '6':
						CreateNumber(Number6);
						break;
					case '7':
						CreateNumber(Number7);
						break;
					case '8':
						CreateNumber(Number8);
						break;
					case '9':
						CreateNumber(Number9);
						break;
				}
		}

		private GameObject CreateNumber(Sprite sprite)
		{
			var obj = new GameObject();
			obj.AddComponent<RectTransform>();
			var image = obj.AddComponent<Image>();
			image.sprite = sprite;

			obj.name = sprite.name;
			obj.transform.SetParent(ParentNumbers, false);

			_numbers.Add(image);
			return obj;
		}
	}
}