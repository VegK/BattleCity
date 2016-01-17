using UnityEngine;
using UnityEngine.UI;

namespace BattleCity.GUI.Main
{
	public class LevelNumber : MonoBehaviour
	{
		[SerializeField]
		private Text Text;
		[SerializeField]
		private int _number = 1;

		public int Number
		{
			get
			{
				return _number;
			}
			set
			{
				if (value < 0)
					return;

				Text.text = (value > 99) ? "99" : value.ToString();
				_number = value;
			}
		}
	}
}