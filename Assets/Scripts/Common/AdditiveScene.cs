using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class AdditiveScene : MonoBehaviour
{
	public string[] SceneName = { "GameGUI" };

	public LoadEvent BeforeLoad = new LoadEvent();
	public LoadEvent AfterLoad = new LoadEvent();
	
	private IEnumerator Start()
	{
		BeforeLoad.Invoke();
		foreach (string name in SceneName)
			yield return SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
		AfterLoad.Invoke();
	}
}

[Serializable]
public class LoadEvent : UnityEvent
{

}