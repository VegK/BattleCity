using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCity.GUI.Files
{
	public class LoadLevelController : MonoBehaviour
	{
		public FileInfoController PrefabFile;
		public Transform Content;
		public Text FileName;
		public Image Preview;
		public Button ButtonLoad;

		private ToggleGroup _toggleGroup;
		private FileInfoController _selectFileInfo;
		private HashSet<FileInfoController> _listFileInfo;

		public void Show()
		{
			FileName.text = string.Empty;
			Preview.sprite = null;
			ButtonLoad.interactable = false;

			gameObject.SetActive(true);
			FieldEditorController.Instance.MouseLock = true;
		}

		public void Hide()
		{
			_selectFileInfo = null;
			gameObject.SetActive(false);
			if (Preview.sprite != null)
				Destroy(Preview.sprite);
		}

		public void OnClickLoad()
		{
			LevelManager.Load(_selectFileInfo.Text);
			Hide();
			FieldEditorController.Instance.MouseLock = false;
		}

		public void OnClickCancel()
		{
			Hide();
			FieldEditorController.Instance.MouseLock = false;
		}

		private void Awake()
		{
			_toggleGroup = Content.GetComponent<ToggleGroup>();
			_listFileInfo = new HashSet<FileInfoController>();
		}

		private void OnEnable()
		{
			LoadListFiles();
		}

		private void LoadListFiles()
		{
			foreach (FileInfoController info in _listFileInfo)
				Destroy(info.gameObject);
			_listFileInfo.Clear();

			var listName = LevelManager.GetNameLevels();
			foreach (string name in listName)
			{
				var info = Instantiate(PrefabFile);
				info.name = name;
				info.transform.SetParent(Content, false);

				var toggle = info.GetComponent<Toggle>();
				if (toggle != null)
					toggle.group = _toggleGroup;

				info.Text = name;
				info.ClickEvent += FileInfo_ClickEvent;

				_listFileInfo.Add(info);
			}
		}

		private void FileInfo_ClickEvent(FileInfoController fileInfo)
		{
			if (Preview.sprite != null)
			{
				Destroy(Preview.sprite);
				Preview.sprite = null;
			}

			_selectFileInfo = fileInfo;
			FileName.text = fileInfo.Text;
			ButtonLoad.interactable = true;

			var texture = LevelManager.GetPreview(fileInfo.Text);
			if (texture == null)
				return;

			var size = new Vector2(texture.width, texture.height);
			var rect = new Rect(Vector2.zero, size);
			var pivot = texture.texelSize / 2;
			Preview.sprite = Sprite.Create(texture, rect, pivot);
		}
	}
}