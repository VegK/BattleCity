using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCity.GUI.Editor.ConfigSpawn
{
	public class ConfigController : MonoBehaviour
	{
		[SerializeField]
		private ItemController PrefabItem;

		[Space(3)]
		[SerializeField]
		private Transform ListTanks;
		[SerializeField]
		private string Count = "COUNT:";
		[SerializeField]
		private Text ListTanksCount;

		private List<ItemController> _enemies = new List<ItemController>();

		public void Show()
		{
			var list = FieldEditorController.Instance.OrderSpawnEnemies;
			if (list != null)
				foreach (EnemyType item in list)
					AddItem(item);

			gameObject.SetActive(true);
			FieldEditorController.Instance.MouseLock = true;
		}

		public void Hide()
		{
			for (int i = 0; i < _enemies.Count; i++)
				Destroy(_enemies[i].gameObject);
			_enemies.Clear();

			FieldEditorController.Instance.MouseLock = false;
			gameObject.SetActive(false);
		}

		public void OnClickAddItem(int typeEnemy)
		{
			if (!Enum.IsDefined(typeof(EnemyType), typeEnemy))
				return;

			AddItem((EnemyType)typeEnemy);
		}

		public void OnClickSave()
		{
			var list = _enemies.Select(e => e.Type).ToArray();
			FieldEditorController.Instance.OrderSpawnEnemies = list;
			Hide();
		}

		public void OnClickCancel()
		{
			Hide();
		}

		private void Start()
		{
			ListTanksCount.text = Count + " 0";
		}

		private void AddItem(EnemyType type)
		{
			var obj = Instantiate(PrefabItem);
			obj.Index = _enemies.Count + 1;
			obj.Type = type;
			obj.ClickEvent += RemoveItem;
			obj.transform.SetParent(ListTanks, false);

			_enemies.Add(obj);

			ListTanksCount.text = Count + " " + _enemies.Count;
		}

		private void RemoveItem(ItemController item)
		{
			var index = _enemies.IndexOf(item);
			_enemies.RemoveAt(index);
			Destroy(item.gameObject);

			for (int i = index; i < _enemies.Count; i++)
				_enemies[i].Index = i + 1;

			ListTanksCount.text = Count + " " + _enemies.Count;
		}
	}
}