using UnityEngine;

namespace BattleCity.GUI.Main
{
	public class FinalScreenController : MonoBehaviour
	{
		private static FinalScreenController _instance;

		public static void Show()
		{
			_instance.gameObject.SetActive(true);
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