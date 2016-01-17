using UnityEngine;
using UnityEngine.UI;

namespace BattleCity.GUI.Main
{
	public class Enemies : MonoBehaviour
	{
		[SerializeField]
		private Sprite EnemySprite;
		public int Count
		{
			get
			{
				return _count;
			}
			set
			{
				if (value >= 0)
					if (value < _count)
					{
						for (int i = value + 1; i <= _count; i++)
							Destroy(transform.GetChild(i - 1).gameObject);
					}
					else if (value > _count)
					{
						for (int i = _count + 1; i <= value; i++)
							Create(i.ToString());
					}

				_count = (value < 0) ? 0 : value;
			}
		}

		[SerializeField]
		private int _count = 20;

		private void Start()
		{
			Reset();
		}

		private GameObject Create(string name)
		{
			var obj = new GameObject(name);
			obj.transform.SetParent(transform);
			obj.transform.localScale = Vector3.one;

			var img = obj.AddComponent<Image>();
			img.sprite = EnemySprite;

			return obj;
		}

		public void Reset()
		{
			for (int i = 0; i < transform.childCount; i++)
				Destroy(transform.GetChild(i).gameObject);

			for (int i = 1; i <= Count; i++)
				Create(i.ToString());
		}
	}
}
