using UnityEngine;

namespace BattleCity.GUI.Main
{
	public class FinalScreenController : MonoBehaviour
	{
		[Header("Sounds")]
		[SerializeField]
		private AudioClip AudioGameOver;

		private static FinalScreenController _instance;

		public static void Show()
		{
			_instance.gameObject.SetActive(true);
			AudioManager.PlayMainSound(_instance.AudioGameOver);
		}

		public static void Hide()
		{
			_instance.gameObject.SetActive(false);
		}

		private void Awake()
		{
			_instance = this;
			gameObject.SetActive(false);
		}
	}
}