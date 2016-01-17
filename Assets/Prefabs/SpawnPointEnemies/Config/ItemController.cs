using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BattleCity.GUI.Editor.ConfigSpawn
{
	public class ItemController : MonoBehaviour
	{
		[SerializeField]
		private Sprite SpriteEnemy1;
		[SerializeField]
		private Sprite SpriteEnemy2;
		[SerializeField]
		private Sprite SpriteEnemy3;
		[SerializeField]
		private Sprite SpriteEnemy4;

		[Space(3)]
		[SerializeField]
		private Image Graphic;
		[SerializeField]
		private Text Number;

		public event ClickHandler ClickEvent;
		public EnemyType Type
		{
			get
			{
				return _type;
			}
			set
			{
				_type = value;
				switch (value)
				{
					case EnemyType.Enemy1:
						Graphic.sprite = SpriteEnemy1;
						break;
					case EnemyType.Enemy2:
						Graphic.sprite = SpriteEnemy2;
						break;
					case EnemyType.Enemy3:
						Graphic.sprite = SpriteEnemy3;
						break;
					case EnemyType.Enemy4:
						Graphic.sprite = SpriteEnemy4;
						break;
				}
			}
		}
		public int Index
		{
			get
			{
				int res;
				if (!int.TryParse(Number.text, out res))
					Number.gameObject.SetActive(false);
				return res;
			}
			set
			{
				name = Type + "_" + value;
				Number.gameObject.SetActive(value > 0);
				Number.text = value.ToString();
			}
		}

		private EnemyType _type;

		public void OnPointerClick(BaseEventData eventData)
		{
			if (ClickEvent != null)
				ClickEvent(this);
		}

		public delegate void ClickHandler(ItemController item);
	}
}