using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BattleCity.GUI.Main
{
	public class MainMenuController : MonoBehaviour
	{
		[SerializeField]
		private Emerge ControlEmerge;

		[SerializeField]
		private Text HiScore;
		[SerializeField]
		private Text ScorePlayer1;
		[SerializeField]
		private Text ScorePlayer2;

		[Header("Menu")]
		[SerializeField]
		private GameObject Cursor;
		[SerializeField]
		private GameObject MainMenu;
		[SerializeField]
		private GameObject NetworkMenu;

		[Header("Input Name Room")]
		[SerializeField]
		private GameObject PanelInputNameRoom;
		[SerializeField]
		private Text NameRoom;
		[SerializeField]
		private Text CaptionButtonOk;

		public static bool Lock { get; set; }

		private static MainMenuController _instance;
		private static bool _emerge;
		private InputNameRoomMode _modeInput;

		public static void Show(int scorePlayer1, int scorePlayer2)
		{
			_instance.HiScore.text = GameManager.HiScore.ToString();
			_instance.ScorePlayer1.text = scorePlayer1.ToString();
			_instance.ScorePlayer2.text = scorePlayer2.ToString();

			EventSystem.current.SetSelectedGameObject(EventSystem.current.firstSelectedGameObject);
			_instance.gameObject.SetActive(true);

			Lock = true;
			_emerge = true;
			_instance.ControlEmerge.Show((s, e) =>
			{
				Lock = false;
				_emerge = false;
			});
		}

		public static void Hide()
		{
			_instance.gameObject.SetActive(false);
		}

		#region Main menu methods
		public void OnClickPlayer1()
		{
			if (_emerge)
				return;
			Lock = true;
			GameManager.StartGame(true, (s, e) => { Hide(); });
		}

		public void OnClickPlayer2()
		{
			if (_emerge)
				return;
			Lock = true;
			GameManager.StartGame(false, (s, e) => { Hide(); });
		}

		public void OnClickGameNetwork()
		{
			if (_emerge)
				return;

			MainMenu.SetActive(false);
			NetworkMenu.SetActive(true);

			var select = NetworkMenu.transform.GetChild(0);
			if (select != null)
				EventSystem.current.SetSelectedGameObject(select.gameObject);
		}

		public void OnClickConstruction()
		{
			if (_emerge)
				return;
			Lock = true;
			SceneManager.LoadScene("Editor");
		}
		#endregion
		#region Network menu methods
		public void OnClickCreateGameNetwork()
		{
			Lock = true;
			_modeInput = InputNameRoomMode.Create;

			NameRoom.text = string.Empty;
			CaptionButtonOk.text = "Create";
			PanelInputNameRoom.SetActive(true);
		}

		public void OnClickJoinGameNetwork()
		{
			Lock = true;
			_modeInput = InputNameRoomMode.Join;

			NameRoom.text = string.Empty;
			CaptionButtonOk.text = "Join";
			PanelInputNameRoom.SetActive(true);
		}

		public void OnClickBackMainMenu()
		{
			MainMenu.SetActive(true);
			NetworkMenu.SetActive(false);

			var select = MainMenu.transform.GetChild(0);
			if (select != null)
				EventSystem.current.SetSelectedGameObject(select.gameObject);
		}

		public void OnClickInputNameRoomOk()
		{
			switch (_modeInput)
			{
				case InputNameRoomMode.Create:
					Debug.Log("create");
					break;
				case InputNameRoomMode.Join:
					Debug.Log("join");
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			Lock = true;
			PanelInputNameRoom.SetActive(false);
		}

		public void OnClickInputNameRoomCancel()
		{
			Lock = false;
			PanelInputNameRoom.SetActive(false);

			var select = NetworkMenu.transform.GetChild(0);
			if (select != null)
				EventSystem.current.SetSelectedGameObject(select.gameObject);
		}

		#endregion

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

		private void Start()
		{
			MainMenu.SetActive(true);
			NetworkMenu.SetActive(false);
			PanelInputNameRoom.SetActive(false);
		}

		private void OnEnable()
		{
			Lock = false;
			_emerge = false;
		}

		private void Update()
		{
			if (_emerge && Input.anyKeyDown)
				ControlEmerge.SetFinishPosition();
		}

		public enum InputNameRoomMode
		{
			Create,
			Join
		}
	}
}