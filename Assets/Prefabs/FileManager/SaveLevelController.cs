﻿using UnityEngine;
using UnityEngine.UI;

namespace BattleCity.GUI.Files
{
	public class SaveLevelController : MonoBehaviour
	{
		[SerializeField]
		private InputField FileName;

		public void Show()
		{
			FileName.text = FieldController.Instance.Name ?? string.Empty;
			gameObject.SetActive(true);
			FieldEditorController.Instance.MouseLock = true;
		}

		public void OnClickSave()
		{
			FieldController.Instance.Save(FileName.text);
			gameObject.SetActive(false);
			FieldEditorController.Instance.MouseLock = false;
		}

		public void OnClickCancel()
		{
			gameObject.SetActive(false);
			FieldEditorController.Instance.MouseLock = false;
		}
	}
}