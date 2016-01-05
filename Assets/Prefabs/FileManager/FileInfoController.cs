using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GUI
{
	public class FileInfoController : MonoBehaviour
	{
		[SerializeField]
		private Text UIText;

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

		public event FileInfoClickHandler ClickEvent;

		public void OnClickSelect(BaseEventData data)
		{
			if (ClickEvent != null)
				ClickEvent(this);
		}
	}
}