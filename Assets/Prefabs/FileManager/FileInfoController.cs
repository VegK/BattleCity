using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BattleCity.GUI.Files
{
	public class FileInfoController : MonoBehaviour
	{
		[SerializeField]
		private Text UIText;

		public event FileInfoClickHandler ClickEvent;
		public string Text
		{
			get
			{
				return UIText.text;
			}
			set
			{
				UIText.text = value;
			}
		}

		public void OnClickSelect(BaseEventData data)
		{
			if (ClickEvent != null)
				ClickEvent(this);
		}
	}

	public delegate void FileInfoClickHandler(FileInfoController fileInfo);
}