using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace GUI
{
	public class MainMenuController : MonoBehaviour
	{
		[SerializeField]
		private GameObject Cursor;

		public static bool Lock { get; set; }

		public void OnClickPlayer1()
		{
			Lock = true;
			GameManager.StartGame(1, (s, e) => { gameObject.SetActive(false); });
		}

		public void OnClickPlayer2()
		{
			Lock = true;
			GameManager.StartGame(2, (s, e) => { gameObject.SetActive(false); });
		}

		public void OnClickConstruction()
		{
			Lock = true;
			SceneManager.LoadScene("Editor");
		}

		public void OnPointerEnter(BaseEventData eventData)
		{
			if (Lock)
				return;

			var data = eventData as PointerEventData;
			if (data == null)
				return;

			var pos = Cursor.transform.localPosition;
			pos.y = data.pointerEnter.transform.localPosition.y - 0.6f;
			Cursor.transform.localPosition = pos;
		}

		private void OnEnable()
		{
			Lock = false;
		}
	}
}