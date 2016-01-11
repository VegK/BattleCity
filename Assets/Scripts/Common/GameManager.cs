using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField]
	private Canvas Black;

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
				GUI.GameGUIController.Instance.ShowPause();
			}
			else
			{
				Time.timeScale = _instance._defaultTimeScale;
				GUI.GameGUIController.Instance.HidePause();
			}
			_instance._pause = value;
		}
	}

	public static bool SinglePlayer { get; private set; }

	private static GameManager _instance;
	private float _defaultTimeScale;
	private bool _pause;
	private bool _gameOver;

	public void LoadApplication()
	{
		Black.gameObject.SetActive(false);
	}

	public static void StartGame(int countPlayers, EventHandler overlapScreen)
	{
		SinglePlayer = (countPlayers == 1);
		NextLevel(overlapScreen);
	}

	public static void NextLevel(EventHandler overlapScreen)
	{
		_instance.Reset();
		SpawnPointEnemiesManager.Reset();
		_instance.StartCoroutine(_instance.NextLevelLoadScreen(overlapScreen));
	}

	public static void GameOver()
	{
		var player = FieldController.Instance.FindBlock(Block.Player1);
		if (player != null)
			player.EditorMode = true;

		player = FieldController.Instance.FindBlock(Block.Player2);
		if (player != null)
			player.EditorMode = true;

		if (!_instance._gameOver)
			GUI.GameGUIController.Instance.ShowGameOver();
		_instance._gameOver = true;
	}

	private IEnumerator NextLevelLoadScreen(EventHandler overlapScreen)
	{
		var defaultScale = Time.timeScale;
		Time.timeScale = 0;

		var loop = true;
		var waitAnyKey = true;
		var level = LevelManager.LevelNumber + 1;

		GUI.LoadLevelSceneController.Show(level, (s, e) => { loop = false; },
			(s, e) => { waitAnyKey = false; });
		while (loop)
			yield return null;

		if (overlapScreen != null)
			overlapScreen(this, EventArgs.Empty);

		LevelManager.NextLevel();
		while (waitAnyKey)
			yield return null;

		loop = true;
		GUI.LoadLevelSceneController.Hide((s, e) => { loop = false; });
		while (loop)
			yield return null;

		Time.timeScale = defaultScale;
	}

	private void Reset()
	{
		_gameOver = false;
		_pause = false;

		GUI.GameGUIController.Instance.HidePause();
		GUI.GameGUIController.Instance.HideGameOver();
	}

	private void Awake()
	{
		_instance = this;
		_defaultTimeScale = Time.timeScale;
		Black.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
	}
}
