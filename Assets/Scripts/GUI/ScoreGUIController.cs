using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCity.GUI.Main
{
	public class ScoreGUIController : MonoBehaviour
	{
		[SerializeField]
		private Text HiScoreText;
		[SerializeField]
		private string LevelName = "STAGE";
		[SerializeField]
		private int Enemy1Points = 100;
		[SerializeField]
		private int Enemy2Points = 200;
		[SerializeField]
		private int Enemy3Points = 300;
		[SerializeField]
		private int Enemy4Points = 400;
		[SerializeField]
		private Text UIStage;
		[SerializeField]
		private Score UIPlayer1;
		[SerializeField]
		private Score UIPlayer2;

		[Header("Sounds")]
		[SerializeField]
		private AudioClip AudioCalculation;
		[SerializeField]
		private AudioClip AudioBonus;

		private static ScoreGUIController _instance;
		private EventHandler _finishEvent;
		private PlayerData _player1;
		private PlayerData _player2;
		private bool _skipCalc;

		public static void Show(EventHandler finishEvent, int levelNumber,
			PlayerData player1, PlayerData player2)
		{
			_instance.UIPlayer2.UIParent.SetActive(!GameManager.SinglePlayer);
			_instance._skipCalc = false;
			_instance.gameObject.SetActive(true);
			_instance._finishEvent = finishEvent;
			_instance.HiScoreText.text = GameManager.HiScore.ToString();
			_instance.FillParams(levelNumber, player1, player2);
		}

		public static void Hide()
		{
			_instance.gameObject.SetActive(false);
			_instance._finishEvent = null;

			_instance.UIStage.text = _instance.LevelName;
			_instance.UIPlayer1.UIScore.text = "0";
			_instance.UIPlayer2.UIScore.text = "0";

			_instance.UIPlayer1.UIEnemy1PTS.text = "0";
			_instance.UIPlayer1.UIEnemy1Tanks.text = "0";
			_instance.UIPlayer1.UIEnemy2PTS.text = "0";
			_instance.UIPlayer1.UIEnemy2Tanks.text = "0";
			_instance.UIPlayer1.UIEnemy3PTS.text = "0";
			_instance.UIPlayer1.UIEnemy3Tanks.text = "0";
			_instance.UIPlayer1.UIEnemy4PTS.text = "0";
			_instance.UIPlayer1.UIEnemy4Tanks.text = "0";

			_instance.UIPlayer2.UIEnemy1PTS.text = "0";
			_instance.UIPlayer2.UIEnemy1Tanks.text = "0";
			_instance.UIPlayer2.UIEnemy2PTS.text = "0";
			_instance.UIPlayer2.UIEnemy2Tanks.text = "0";
			_instance.UIPlayer2.UIEnemy3PTS.text = "0";
			_instance.UIPlayer2.UIEnemy3Tanks.text = "0";
			_instance.UIPlayer2.UIEnemy4PTS.text = "0";
			_instance.UIPlayer2.UIEnemy4Tanks.text = "0";

			_instance.UIPlayer1.UITotalTanks.text = "0";
			_instance.UIPlayer2.UITotalTanks.text = "0";

			_instance.UIPlayer1.UIBonus.SetActive(false);
			_instance.UIPlayer2.UIBonus.SetActive(false);
		}

		private void Awake()
		{
			_instance = this;
			Hide();
		}

		private void Update()
		{
			if (!_skipCalc && (Input.GetButtonDown("Player1_Pause") ||
				Input.GetButtonDown("Player2_Pause")))
			{
				_skipCalc = true;
				StopAllCoroutines();
				StartCoroutine(SkipCalcScore());
			}
		}

		private void FillParams(int levelNumber, PlayerData player1, PlayerData player2)
		{
			UIStage.text = LevelName + " " + levelNumber;
			UIPlayer1.UIScore.text = player1.Score.ToString();
			UIPlayer2.UIScore.text = player2.Score.ToString();

			_player1 = player1;
			_player2 = player2;

			StartCoroutine(CalcScore());
		}

		private IEnumerator CalcScore()
		{
			var wait = 0.14f;

			#region Enemy #1
			int max = Mathf.Max(_player1.Enemy1, _player2.Enemy1);
			if (max > 0)
			{
				yield return new WaitForSeconds(wait * 2);
				if (_skipCalc)
					yield break;
			}
			for (int i = 0; i <= max; i++)
			{
				if (i > 0)
					AudioManager.PlaySecondarySound(AudioCalculation);

				if (Input.anyKeyDown)
				{
					UIPlayer1.UIEnemy1PTS.text = (_player1.Enemy1 * Enemy1Points).ToString();
					UIPlayer1.UIEnemy1Tanks.text = _player1.Enemy1.ToString();

					UIPlayer2.UIEnemy1PTS.text = (_player2.Enemy1 * Enemy1Points).ToString();
					UIPlayer2.UIEnemy1Tanks.text = _player2.Enemy1.ToString();

					yield return new WaitForSeconds(wait);
					if (_skipCalc)
						yield break;
					break;
				}

				if (i <= _player1.Enemy1)
				{
					UIPlayer1.UIEnemy1PTS.text = (i * Enemy1Points).ToString();
					UIPlayer1.UIEnemy1Tanks.text = i.ToString();
				}
				if (i <= _player2.Enemy1)
				{
					UIPlayer2.UIEnemy1PTS.text = (i * Enemy1Points).ToString();
					UIPlayer2.UIEnemy1Tanks.text = i.ToString();
				}

				yield return new WaitForSeconds(wait);
				if (_skipCalc)
					yield break;
			}
			#endregion
			#region Enemy #2
			max = Mathf.Max(_player1.Enemy2, _player2.Enemy2);
			if (max > 0)
			{
				yield return new WaitForSeconds(wait * 2);
				if (_skipCalc)
					yield break;
			}
			for (int i = 0; i <= max; i++)
			{
				if (i > 0)
					AudioManager.PlaySecondarySound(AudioCalculation);

				if (Input.anyKeyDown)
				{
					UIPlayer1.UIEnemy2PTS.text = (_player1.Enemy2 * Enemy2Points).ToString();
					UIPlayer1.UIEnemy2Tanks.text = _player1.Enemy2.ToString();

					UIPlayer2.UIEnemy2PTS.text = (_player2.Enemy2 * Enemy2Points).ToString();
					UIPlayer2.UIEnemy2Tanks.text = _player2.Enemy2.ToString();

					yield return new WaitForSeconds(wait);
					if (_skipCalc)
						yield break;
					break;
				}

				if (i <= _player1.Enemy2)
				{
					UIPlayer1.UIEnemy2PTS.text = (i * Enemy2Points).ToString();
					UIPlayer1.UIEnemy2Tanks.text = i.ToString();
				}
				if (i <= _player2.Enemy2)
				{
					UIPlayer2.UIEnemy2PTS.text = (i * Enemy2Points).ToString();
					UIPlayer2.UIEnemy2Tanks.text = i.ToString();
				}

				yield return new WaitForSeconds(wait);
				if (_skipCalc)
					yield break;
			}
			#endregion
			#region Enemy #3
			max = Mathf.Max(_player1.Enemy3, _player2.Enemy3);
			if (max > 0)
			{
				yield return new WaitForSeconds(wait * 2);
				if (_skipCalc)
					yield break;
			}
			for (int i = 0; i <= max; i++)
			{
				if (i > 0)
					AudioManager.PlaySecondarySound(AudioCalculation);

				if (Input.anyKeyDown)
				{
					UIPlayer1.UIEnemy3PTS.text = (_player1.Enemy3 * Enemy3Points).ToString();
					UIPlayer1.UIEnemy3Tanks.text = _player1.Enemy3.ToString();

					UIPlayer2.UIEnemy3PTS.text = (_player2.Enemy3 * Enemy3Points).ToString();
					UIPlayer2.UIEnemy3Tanks.text = _player2.Enemy3.ToString();

					yield return new WaitForSeconds(wait);
					if (_skipCalc)
						yield break;
					break;
				}

				if (i <= _player1.Enemy3)
				{
					UIPlayer1.UIEnemy3PTS.text = (i * Enemy3Points).ToString();
					UIPlayer1.UIEnemy3Tanks.text = i.ToString();
				}
				if (i <= _player2.Enemy3)
				{
					UIPlayer2.UIEnemy3PTS.text = (i * Enemy3Points).ToString();
					UIPlayer2.UIEnemy3Tanks.text = i.ToString();
				}

				yield return new WaitForSeconds(wait);
				if (_skipCalc)
					yield break;
			}
			#endregion
			#region Enemy #4
			max = Mathf.Max(_player1.Enemy4, _player2.Enemy4);
			if (max > 0)
			{
				yield return new WaitForSeconds(wait * 2);
				if (_skipCalc)
					yield break;
			}
			for (int i = 0; i <= max; i++)
			{
				if (i > 0)
					AudioManager.PlaySecondarySound(AudioCalculation);

				if (Input.anyKeyDown)
				{
					UIPlayer1.UIEnemy4PTS.text = (_player1.Enemy4 * Enemy4Points).ToString();
					UIPlayer1.UIEnemy4Tanks.text = _player1.Enemy4.ToString();

					UIPlayer2.UIEnemy4PTS.text = (_player2.Enemy4 * Enemy4Points).ToString();
					UIPlayer2.UIEnemy4Tanks.text = _player2.Enemy4.ToString();

					yield return new WaitForSeconds(wait);
					if (_skipCalc)
						yield break;
					break;
				}

				if (i <= _player1.Enemy4)
				{
					UIPlayer1.UIEnemy4PTS.text = (i * Enemy4Points).ToString();
					UIPlayer1.UIEnemy4Tanks.text = i.ToString();
				}
				if (i <= _player2.Enemy4)
				{
					UIPlayer2.UIEnemy4PTS.text = (i * Enemy4Points).ToString();
					UIPlayer2.UIEnemy4Tanks.text = i.ToString();
				}

				yield return new WaitForSeconds(wait);
				if (_skipCalc)
					yield break;
			}
			#endregion
			#region Total
			yield return new WaitForSeconds(wait * 2);
			if (_skipCalc)
				yield break;

			var total1 = _player1.Enemy1 + _player1.Enemy2 + _player1.Enemy3 + _player1.Enemy4;
			UIPlayer1.UITotalTanks.text = total1.ToString();

			var total2 = _player2.Enemy1 + _player2.Enemy2 + _player2.Enemy3 + _player2.Enemy4;
			UIPlayer2.UITotalTanks.text = total2.ToString();

			yield return new WaitForSeconds(1);
			if (_skipCalc)
				yield break;
			#endregion

			Bonus(total1, total2);
			yield return new WaitForSeconds(wait * 10);
			if (_skipCalc)
				yield break;

			if (_finishEvent != null)
				_finishEvent(this, EventArgs.Empty);
		}

		private IEnumerator SkipCalcScore()
		{
			UIPlayer1.UIEnemy1PTS.text = (_player1.Enemy1 * Enemy1Points).ToString();
			UIPlayer1.UIEnemy1Tanks.text = _player1.Enemy1.ToString();
			UIPlayer1.UIEnemy2PTS.text = (_player1.Enemy2 * Enemy2Points).ToString();
			UIPlayer1.UIEnemy2Tanks.text = _player1.Enemy2.ToString();
			UIPlayer1.UIEnemy3PTS.text = (_player1.Enemy3 * Enemy3Points).ToString();
			UIPlayer1.UIEnemy3Tanks.text = _player1.Enemy3.ToString();
			UIPlayer1.UIEnemy4PTS.text = (_player1.Enemy4 * Enemy4Points).ToString();
			UIPlayer1.UIEnemy4Tanks.text = _player1.Enemy4.ToString();

			UIPlayer2.UIEnemy1PTS.text = (_player2.Enemy1 * Enemy1Points).ToString();
			UIPlayer2.UIEnemy1Tanks.text = _player2.Enemy1.ToString();
			UIPlayer2.UIEnemy2PTS.text = (_player2.Enemy2 * Enemy2Points).ToString();
			UIPlayer2.UIEnemy2Tanks.text = _player2.Enemy2.ToString();
			UIPlayer2.UIEnemy3PTS.text = (_player2.Enemy3 * Enemy3Points).ToString();
			UIPlayer2.UIEnemy3Tanks.text = _player2.Enemy3.ToString();
			UIPlayer2.UIEnemy4PTS.text = (_player2.Enemy4 * Enemy4Points).ToString();
			UIPlayer2.UIEnemy4Tanks.text = _player2.Enemy4.ToString();

			var total1 = _player1.Enemy1 + _player1.Enemy2 + _player1.Enemy3 + _player1.Enemy4;
			UIPlayer1.UITotalTanks.text = total1.ToString();

			var total2 = _player2.Enemy1 + _player2.Enemy2 + _player2.Enemy3 + _player2.Enemy4;
			UIPlayer2.UITotalTanks.text = total2.ToString();

			yield return new WaitForSeconds(1);
			Bonus(total1, total2);

			yield return new WaitForSeconds(2);

			if (_finishEvent != null)
				_finishEvent(this, EventArgs.Empty);
		}

		private void Bonus(int totalPlayer1, int totalPlayer2)
		{
			if (GameManager.SinglePlayer)
				return;
			if (totalPlayer1 == totalPlayer2)
				return;

			if (totalPlayer1 > totalPlayer2)
			{
				GameManager.Player1.Score += 1000;
				UIPlayer1.UIBonus.SetActive(true);
			}
			else if (totalPlayer1 < totalPlayer2)
			{
				GameManager.Player2.Score += 1000;
				UIPlayer2.UIBonus.SetActive(true);
			}

			AudioManager.PlaySecondarySound(AudioBonus);
		}

		[Serializable]
		public class Score
		{
			public GameObject UIParent;
			public Text UIScore;
			public Text UIEnemy1PTS;
			public Text UIEnemy1Tanks;
			public Text UIEnemy2PTS;
			public Text UIEnemy2Tanks;
			public Text UIEnemy3PTS;
			public Text UIEnemy3Tanks;
			public Text UIEnemy4PTS;
			public Text UIEnemy4Tanks;
			public Text UITotalTanks;
			public GameObject UIBonus;
		}
	}
}