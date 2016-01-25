using UnityEngine;

namespace BattleCity.GUI.Main
{
	public class GameOver : MonoBehaviour
	{
		[SerializeField]
		private Emerge ControlEmerge;

		public void Show()
		{
			gameObject.SetActive(true);
			ControlEmerge.Show(null);
		}

		public void Hide()
		{
			gameObject.SetActive(false);
		}
	}
}