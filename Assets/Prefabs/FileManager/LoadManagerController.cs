using UnityEngine;
using System.IO;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GUI
{
	public class LoadManagerController : MonoBehaviour
	{
		public FileInfoController PrefabFile;
		public Transform Content;
		public Text FileName;
		public Image Preview;

		private ToggleGroup _toggleGroup;
		private FileInfoController _selectFileInfo;

		public void Show()
		{
			gameObject.SetActive(true);
			FieldEditorController.Instance.MouseLock = true;
		}

		public void OnClickLoad()
		{
			FieldController.Instance.Load(_selectFileInfo.Text);
			gameObject.SetActive(false);
			FieldEditorController.Instance.MouseLock = false;
		}

		public void OnClickCancel()
		{
			gameObject.SetActive(false);
			FieldEditorController.Instance.MouseLock = false;
		}

		private void Awake()
		{
			_toggleGroup = Content.GetComponent<ToggleGroup>();
		}

		private void OnEnable()
		{
			LoadListFiles();
		}

		private void LoadListFiles()
		{
			for (int i = 0; i < Content.childCount; i++)
				Destroy(Content.GetChild(i).gameObject);

			var ext = "." + Consts.EXTENSION;
			var dir = new DirectoryInfo(Consts.PATH);
			foreach (FileInfo file in dir.GetFiles("*" + ext))
				if (file.Extension == ext)
				{
					var obj = Instantiate(PrefabFile);
					obj.name = file.Name;
					obj.transform.SetParent(Content, false);

					var toggle = obj.GetComponent<Toggle>();
					if (toggle != null)
						toggle.group = _toggleGroup;

					obj.Text = Path.GetFileNameWithoutExtension(file.Name);
					obj.ClickEvent += FileInfo_ClickEvent;
				}
		}

		private void FileInfo_ClickEvent(FileInfoController fileInfo)
		{
			_selectFileInfo = fileInfo;
			FileName.text = fileInfo.Text;
		}
	}

	public delegate void FileInfoClickHandler(FileInfoController fileInfo);
}