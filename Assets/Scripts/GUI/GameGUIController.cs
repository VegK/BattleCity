﻿using UnityEngine;

namespace BattleCity.GUI.Main
{
	public class GameGUIController : MonoBehaviour
	{
		[SerializeField]
		private Enemies UIEnemies;
		[SerializeField]
		private PlayerLife UIPlayer1Life;
		[SerializeField]
		private PlayerLife UIPlayer2Life;
		[SerializeField]
		private LevelNumber UILevelNumber;
		[SerializeField]
		private GameOver UIGameOver;
		[SerializeField]
		private Pause UIPause;

		public static GameGUIController Instance;

		public int EnemiesCount
		{
			get
			{
				return UIEnemies.Count;
			}
			set
			{
				UIEnemies.Count = value;
			}
		}
		public int Player1LifeCount
		{
			get
			{
				return UIPlayer1Life.Count;
			}
			set
			{
				UIPlayer1Life.gameObject.SetActive(true);
				UIPlayer1Life.Count = value;
			}
		}
		public int Player2LifeCount
		{
			get
			{
				return UIPlayer2Life.Count;
			}
			set
			{
				UIPlayer2Life.gameObject.SetActive(true);
				UIPlayer2Life.Count = value;
			}
		}
		public int LevelNumber
		{
			get
			{
				return UILevelNumber.Number;
			}
			set
			{
				UILevelNumber.Number = value;
			}
		}
		public int GetPlayerCount()
		{
			var res = 0;
			if (UIPlayer1Life.gameObject.activeInHierarchy)
				res++;
			if (UIPlayer2Life.gameObject.activeInHierarchy)
				res++;
			return res;
		}

		public void HidePlayer2Life()
		{
			UIPlayer2Life.gameObject.SetActive(false);
		}

		public void ShowGameOver()
		{
			UIGameOver.Show();
		}

		public void HideGameOver()
		{
			UIGameOver.Hide();
		}

		public void ShowPause()
		{
			UIPause.Show();
		}

		public void HidePause()
		{
			UIPause.Hide();
		}

		private void Awake()
		{
			Instance = this;
			UIPlayer1Life.gameObject.SetActive(false);
			UIPlayer2Life.gameObject.SetActive(false);

			UIGameOver.Hide();
			UIPause.Hide();
		}
	}
}