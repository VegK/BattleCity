using UnityEngine;
using UnityEngine.UI;

namespace BattleCity.GUI.Main
{
	public class PlayerLife : MonoBehaviour
	{
		[SerializeField]
		private Text Text;
		[SerializeField]
		private int _count = 0;

		public int Count
		{
			get
			{
				return _count;
			}
			set
			{
				if (value < 0)
					return;

				Text.text = (value > 9) ? "9" : value.ToString();
				_count = value;
			}
		}
	}
}