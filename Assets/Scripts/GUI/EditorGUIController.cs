using UnityEngine;

public class EditorGUIController : MonoBehaviour
{
	public void OnClickSave()
	{
		FieldController.Instance.Save("1");
	}

	public void OnClickLoad()
	{
		FieldController.Instance.Load("1");
	}
}
