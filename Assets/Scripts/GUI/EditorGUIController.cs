﻿using UnityEngine;
using UnityEngine.SceneManagement;

namespace GUI
{
	public class EditorGUIController : MonoBehaviour
	{
		public SaveManagerController SaveManager;
		public LoadManagerController LoadManager;
		public Editor.ConfigSpawn.ConfigController ConfigSpawn;

		public void OnClickBack()
		{
			SceneManager.LoadScene("Standard");
		}

		public void OnClickSave()
		{
			SaveManager.Show();
		}

		public void OnClickLoad()
		{
			LoadManager.Show();
	    }

		public void OnClickClearField()
		{
			FieldEditorController.Instance.ClearField();
		}

		public void OnClickConfigSpawn()
		{
			ConfigSpawn.Show();
		}
	}
}