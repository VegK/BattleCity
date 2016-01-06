using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : Editor
{
	private LevelManager Target
	{
		get
		{
			return target as LevelManager;
		}
	}

	private ReorderableList _levels;
	private string[] _listLevelName;

	private void OnEnable()
	{
		LoadListFiles();
		_levels = new ReorderableList(serializedObject,
			serializedObject.FindProperty("Levels"),
			true, true, true, true);

		_levels.drawHeaderCallback += DrawHeader;
		_levels.drawElementCallback += DrawElement;

		_levels.onAddDropdownCallback += AddDropdown;
		_levels.onRemoveCallback += RemoveItem;
	}

	private void OnDisable()
	{
		_levels.drawHeaderCallback -= DrawHeader;
		_levels.drawElementCallback -= DrawElement;

		_levels.onAddDropdownCallback -= AddDropdown;
		_levels.onRemoveCallback -= RemoveItem;
	}

	private void DrawHeader(Rect rect)
	{
		UnityEngine.GUI.Label(rect, "List of levels");
	}

	private void DrawElement(Rect rect, int index, bool active, bool focused)
	{
		EditorGUI.BeginChangeCheck();
		EditorGUI.LabelField(rect, Target.Levels[index]);
		if (EditorGUI.EndChangeCheck())
			EditorUtility.SetDirty(target);
	}

	private void AddDropdown(Rect buttonRect, ReorderableList l)
	{
		var menu = new GenericMenu();
		foreach (string levelName in _listLevelName)
			menu.AddItem(new GUIContent(levelName), false, AddDropdownItem, levelName);
		menu.ShowAsContext();
	}

	private void AddDropdownItem(object target)
	{
		var index = _levels.serializedProperty.arraySize;
		_levels.serializedProperty.arraySize++;
		_levels.index = index;

		var element = _levels.serializedProperty.GetArrayElementAtIndex(index);
		element.stringValue = (string)target;

		serializedObject.ApplyModifiedProperties();
	}

	private void RemoveItem(ReorderableList list)
	{
		list.serializedProperty.DeleteArrayElementAtIndex(list.index);
		serializedObject.ApplyModifiedProperties();
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("Reload list added files"))
			LoadListFiles();
		_levels.DoLayoutList();
	}

	private void LoadListFiles()
	{
		var list = new List<string>();

		var ext = "." + Consts.EXTENSION;
		var dir = new DirectoryInfo(Consts.PATH);
		foreach (FileInfo file in dir.GetFiles("*" + ext))
			if (file.Extension == ext)
				list.Add(Path.GetFileNameWithoutExtension(file.Name));

		_listLevelName = list.ToArray();
	}
}
