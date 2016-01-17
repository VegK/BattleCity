using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCity.GUI.Editor
{
	public class PanelItemsController : MonoBehaviour
	{
		[SerializeField]
		private ToggleGroup Group;

		public static PanelItemsController Instance;

		private void Awake()
		{
			Instance = this;
		}

		public Block? GetSelectItem()
		{
			var toggle = Group.ActiveToggles().FirstOrDefault();
			if (toggle == null)
				return null;

			var ctrl = toggle.gameObject.GetComponent<BlockItem>();
			if (ctrl == null)
				return null;

			return ctrl.TypeItem;
		}
	}
}