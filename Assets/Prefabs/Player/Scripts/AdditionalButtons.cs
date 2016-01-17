using UnityEngine;

namespace BattleCity.Player
{
	public class AdditionalButtons : MonoBehaviour
	{
		private bool _keyEscapePress;
		private string _buttonNamePause;

		private void Awake()
		{
			var player = GetComponent<PlayerController>();
			_buttonNamePause = player.TypeItem + "_Pause";
		}

		private void Update()
		{
			if (Input.GetButtonDown(_buttonNamePause))
			{
				_keyEscapePress = !_keyEscapePress;
				GameManager.Pause = _keyEscapePress;
			}
		}
	}
}