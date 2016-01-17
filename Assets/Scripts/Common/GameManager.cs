using BattleCity.GUI.Main;
using System;
using System.Collections;
using UnityEngine;

namespace BattleCity
{
	public class GameManager : MonoBehaviour
	{
		[SerializeField]
		private Canvas Black;
		[SerializeField]
		private int _startLifePlayer1 = 3;
		[SerializeField]
		private int _startLifePlayer2 = 3;

		public static bool Pause
		{
			get
			{
				return _instance._pause;
			}
			set
			{
				if (_instance._gameOver)
					return;
				if (value)
				{
					Time.timeScale = 0;
					GameGUIController.Instance.ShowPause();
				}
				else
				{
					Time.timeScale = _instance._defaultTimeScale;
					GameGUIController.Instance.HidePause();
				}
				_instance._pause = value;
			}
		}
		public static bool SinglePlayer { get; private set; }
		public static int Player1Life
		{
			get
			{
				return _instance._player1Life;
			}
			set
			{
				_instance._player1Life = value;
				if (value < 0)
				{
					if (Player2Life < 0)
						GameOver();
					return;
				}
				GameGUIController.Instance.Player1LifeCount = value;
			}
		}
		public static PlayerData Player1;
		public static int Player2Life
		{
			get
			{
				return _instance._player2Life;
			}
			set
			{
				_instance._player2Life = value;
				if (value < 0)
				{
					if (Player1Life < 0)
						GameOver();
					return;
				}
				GameGUIController.Instance.Player2LifeCount = value;
			}
		}
		public static PlayerData Player2;

		private static GameManager _instance;
		private float _defaultTimeScale;
		private bool _pause;
		private bool _gameOver;
		private int _player1Life;
		private int _player2Life;

		public void LoadApplication()
		{
			Black.gameObject.SetActive(false);
			MainMenuController.Show();
		}

		public static void StartGame(bool singlePlayer, EventHandler overlapScreen)
		{
			SinglePlayer = singlePlayer;

			Player1.Score = 0;
			Player1.ResetEnemy();
			Player1Life = _instance._startLifePlayer1;

			Player2.Score = 0;
			Player2.ResetEnemy();
			Player2Life = _instance._startLifePlayer2;
			if (SinglePlayer)
				GameGUIController.Instance.HidePlayer2Life();

			_instance.StartCoroutine(_instance.NextLevel(overlapScreen, true));
		}

		public static void NextLevel(EventHandler overlapScreen)
		{
			_instance.StartCoroutine(_instance.NextLevel(overlapScreen, false));
		}

		private IEnumerator NextLevel(EventHandler overlapScreen, bool skipCalcScore)
		{
			Reset();
			if (!skipCalcScore)
			{
				var loop = true;
				ScoreGUIController.Show((s, e) => { loop = false; },
					LevelManager.LevelNumber, Player1, Player2);
				while (true)
				{
					if (Input.anyKey && !loop)
						break;
					yield return null;
				}
				if (overlapScreen == null)
					overlapScreen = (s, e) => { ScoreGUIController.Hide(); };
				else
					overlapScreen += (s, e) => { ScoreGUIController.Hide(); };
			}
			StartCoroutine(NextLevelLoadScreen(overlapScreen));
		}

		public static void GameOver()
		{
			if (_instance._gameOver)
				return;
			_instance._gameOver = true;

			_instance.StartCoroutine(_instance.ShowGameOver());
		}

		private IEnumerator NextLevelLoadScreen(EventHandler overlapScreen)
		{
			var defaultScale = Time.timeScale;
			Time.timeScale = 0;

			var loop = true;
			var waitAnyKey = true;
			var level = LevelManager.LevelNumber + 1;

			LoadLevelSceneController.Show(level, (s, e) => { loop = false; },
				(s, e) => { waitAnyKey = false; });
			while (loop)
				yield return null;

			if (overlapScreen != null)
				overlapScreen(this, EventArgs.Empty);

			LevelManager.NextLevel();
			while (waitAnyKey)
				yield return null;

			loop = true;
			LoadLevelSceneController.Hide((s, e) => { loop = false; });
			while (loop)
				yield return null;

			Time.timeScale = defaultScale;
			SpawnPointEnemiesManager.StartSpawn();
			Player1.ResetEnemy();
			Player2.ResetEnemy();
		}

		private IEnumerator ShowGameOver()
		{
			var player = FieldController.Instance.FindBlock(Block.Player1);
			if (player != null)
				player.EditorMode = true;

			player = FieldController.Instance.FindBlock(Block.Player2);
			if (player != null)
				player.EditorMode = true;

			// Show GameOver in game
			GameGUIController.Instance.ShowGameOver();
			yield return new WaitForSeconds(10);

			Reset();

			// Show calc score
			var loop = true;
			ScoreGUIController.Show((s, e) => { loop = false; },
				LevelManager.LevelNumber, Player1, Player2);
			while (true)
			{
				if (Input.anyKey && !loop)
					break;
				yield return null;
			}
			ScoreGUIController.Hide();

			// Show final screen with GameOver
			FinalScreenController.Show();
			yield return new WaitForSeconds(5);
			while (!Input.anyKeyDown)
				yield return null;
			FinalScreenController.Hide();

			MainMenuController.Show();
		}

		private void Reset()
		{
			_gameOver = false;
			_pause = false;

			LevelManager.Reset();
			SpawnPointEnemiesManager.Reset();
			GameGUIController.Instance.HidePause();
			GameGUIController.Instance.HideGameOver();
			FieldController.Instance.Clear();
		}

		private void Awake()
		{
			_instance = this;
			_defaultTimeScale = Time.timeScale;
			Black.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
		}
	}
}