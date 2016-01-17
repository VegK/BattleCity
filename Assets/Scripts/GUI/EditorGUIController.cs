using BattleCity.GUI.Editor.ConfigSpawn;
using BattleCity.GUI.Files;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BattleCity.GUI.Editor
{
	public class EditorGUIController : MonoBehaviour
	{
		public SaveLevelController SaveManager;
		public LoadLevelController LoadManager;
		public ConfigController ConfigSpawn;

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