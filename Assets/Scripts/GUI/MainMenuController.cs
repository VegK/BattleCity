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

		[SerializeField]
		private GameObject Cursor;

		public static bool Lock { get; set; }

		private static MainMenuController _instance;
		private static bool _emerge;

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

		public void OnClickConstruction()
		{
			if (_emerge)
				return;
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
			_emerge = false;
		}

		private void Update()
		{
			if (_emerge && Input.anyKeyDown)
				ControlEmerge.SetFinishPosition();
		}
	}
}