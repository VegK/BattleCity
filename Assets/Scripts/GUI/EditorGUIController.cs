using UnityEngine;

namespace GUI
{
	public class EditorGUIController : MonoBehaviour
	{
		public SaveManagerController SaveManager;
		public LoadManagerController LoadManager;

		public void OnClickSave()
		{
			SaveManager.Show();
		}

		public void OnClickLoad()
		{
			LoadManager.Show();
	    }
	}
}