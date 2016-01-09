using UnityEngine;

public class GameManager : MonoBehaviour
{
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

	private static GameManager _instance;
	private float _defaultTimeScale;
	private bool _pause;
	private bool _gameOver;

	public void NonStaticNextLevel()
	{
		NextLevel();
	}

	public static void NextLevel()
	{
		SpawnPointEnemiesManager.Reset();
		LevelManager.NextLevel();
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
		DontDestroyOnLoad(gameObject);
	}
}
