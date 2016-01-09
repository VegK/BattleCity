using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
	public GameObject Cursor;

	public void OnClickPlayer1()
	{
		SceneManager.LoadScene("Standard");
	}

	public void OnClickPlayer2()
	{

	}

	public void OnClickConstruction()
	{
		SceneManager.LoadScene("Editor");
	}

	public void OnPointerEnter(BaseEventData eventData)
	{
		var data = eventData as PointerEventData;
		if (data == null)
			return;

		var pos = Cursor.transform.localPosition;
		pos.y = data.pointerEnter.transform.localPosition.y - 0.6f;
		Cursor.transform.localPosition = pos;
	}
}
