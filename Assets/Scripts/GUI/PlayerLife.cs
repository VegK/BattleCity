using UnityEngine;
using UnityEngine.UI;

namespace GUI
{
	public class PlayerLife : MonoBehaviour
	{
		[SerializeField]
		private Text Text;

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

		[SerializeField]
		private int _count = 0;
	}
}