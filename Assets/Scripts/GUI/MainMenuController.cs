﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace BattleCity.GUI.Main
{
	public class MainMenuController : MonoBehaviour
	{
		[SerializeField]
		private GameObject Cursor;

		public static bool Lock { get; set; }

		private static MainMenuController _instance;

		public static void Show()
		{
			Lock = false;
			EventSystem.current.SetSelectedGameObject(EventSystem.current.firstSelectedGameObject);
			_instance.gameObject.SetActive(true);
		}

		public static void Hide()
		{
			_instance.gameObject.SetActive(false);
		}

		public void OnClickPlayer1()
		{
			Lock = true;
			GameManager.StartGame(true, (s, e) => { Hide(); });
		}

		public void OnClickPlayer2()
		{
			Lock = true;
			GameManager.StartGame(false, (s, e) => { Hide(); });
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
			if (data != null)
			{
				EventSystem.current.SetSelectedGameObject(data.pointerEnter);
			}
			else if (eventData.selectedObject != null)
			{
				var pos = Cursor.transform.localPosition;
				pos.y = eventData.selectedObject.transform.localPosition.y - 0.6f;
				Cursor.transform.localPosition = pos;
			}
		}

		public void OnPointerDeselect(BaseEventData eventData)
		{
			if (Lock)
				return;

			if (EventSystem.current.currentSelectedGameObject == null)
			{
				var first = EventSystem.current.firstSelectedGameObject;
				EventSystem.current.SetSelectedGameObject(first);
			}
		}

		private void Awake()
		{
			_instance = this;
		}

		private void OnEnable()
		{
			Lock = false;
		}
	}
}