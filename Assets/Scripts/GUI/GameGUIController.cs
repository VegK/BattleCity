using UnityEngine;
using System.Collections;

namespace GUI
{
	public class GameGUIController : MonoBehaviour
	{
		[SerializeField]
		private Enemies EnemiesGUI;
		[SerializeField]
		private PlayerLife Player1LifeGUI;
		[SerializeField]
		private PlayerLife Player2LifeGUI;
		[SerializeField]
		private LevelNumber LevelNumberGUI;

		public static GameGUIController Instance
		{
			get
			{
				return _instance;
			}
		}

		public int EnemiesCount
		{
			get
			{
				return EnemiesGUI.Count;
			}
			set
			{
				EnemiesGUI.Count = value;
			}
		}

		public int Player1LifeCount
		{
			get
			{
				return Player1LifeGUI.Count;
			}
			set
			{
				Player1LifeGUI.Count = value;
			}
		}

		public int Player2LifeCount
		{
			get
			{
				return Player2LifeGUI.Count;
			}
			set
			{
				Player2LifeGUI.Count = value;
			}
		}

		public int LevelNumber
		{
			get
			{
				return LevelNumberGUI.Number;
			}
			set
			{
				LevelNumberGUI.Number = value;
			}
		}

		private static GameGUIController _instance;

		private void Awake()
		{
			_instance = this;
		}
	}
}