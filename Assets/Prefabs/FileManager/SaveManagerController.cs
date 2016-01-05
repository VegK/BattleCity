using UnityEngine;
using UnityEngine.UI;

namespace GUI
{
	public class SaveManagerController : MonoBehaviour
	{
		[SerializeField]
		private InputField FileName;

		public void Show()
		{
			FileName.text = FieldController.Instance.GetName();
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