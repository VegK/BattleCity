using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class AdditiveScene : MonoBehaviour
{
	public string SceneName = "GameGUI";

	public LoadEvent BeforeLoad = new LoadEvent();
	public LoadEvent AfterLoad = new LoadEvent();
	
	private IEnumerator Start()
	{
		BeforeLoad.Invoke();
		yield return SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
		AfterLoad.Invoke();
	}
}

[Serializable]
public class LoadEvent : UnityEvent
{

}