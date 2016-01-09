using UnityEngine;

public class AdditionalButtons : MonoBehaviour
{
	private bool _keyEscapePress;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			_keyEscapePress = !_keyEscapePress;
			GameManager.Pause = _keyEscapePress;
		}
	}
}
