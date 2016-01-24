using BattleCity.GUI.Editor;
using UnityEngine;

namespace BattleCity
{
	public class FieldEditorController : MonoBehaviour
	{
		[SerializeField]
		private Sprite Cell;

		public static FieldEditorController Instance;
		public bool MouseLock { get; set; }
		public EnemyType[] OrderSpawnEnemies { get; set; }

		private readonly int[,] DEFAULT_FIELD =			{				{  0,  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 17 },				{  0,  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,  0 },				{  0,  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,  0 },				{  0,  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,  0 },				{ 15,  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,  0 },				{  3, 21, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,  0 },				{ 14,  4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 17 },				{  5, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,  0 },				{ 16,  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,  0 },				{  0,  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,  0 },				{  0,  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,  0 },				{  0,  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,  0 },				{  0,  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 17 }			};

		private GameObject _selectCell;
		private Vector2 _fieldPosition;
		private Vector2 _fieldSize;

		public void ClearField()
		{
			for (int x = 0; x < FieldController.Instance.Width; x++)
				for (int y = 0; y < FieldController.Instance.Height; y++)
					FieldController.Instance.SetCell(x, y, Block.Empty);

			var width = DEFAULT_FIELD.GetLength(0);
			var height = DEFAULT_FIELD.GetLength(1);
			for (int x = 0; x < width; x++)
				for (int y = 0; y < height; y++)
					FieldController.Instance.SetCell(x, y, (Block)DEFAULT_FIELD[x, y]);
		}

		private void Awake()
		{
			Instance = this;
		}

		private void Start()
		{
			MouseLock = false;

			OrderSpawnEnemies = new EnemyType[0];
			_fieldPosition = FieldController.Instance.GetPosition();
			_fieldSize = FieldController.Instance.GetSize();

			_selectCell = new GameObject("MouseCell");
			_selectCell.SetActive(false);
			var renderer = _selectCell.AddComponent<SpriteRenderer>();
			renderer.sprite = Cell;
			renderer.sortingOrder = 100;
			_selectCell.transform.SetParent(transform);

			var pos = transform.position;
			pos.z -= 1;
			_selectCell.transform.position = pos;

			var width = DEFAULT_FIELD.GetLength(0);
			var height = DEFAULT_FIELD.GetLength(1);
			for (int x = 0; x < width; x++)
				for (int y = 0; y < height; y++)
					FieldController.Instance.SetCell(x, y, (Block)DEFAULT_FIELD[x, y]);
		}

		private void Update()
		{
			if (IsMouseLeaveField() || MouseLock)
			{
				_selectCell.SetActive(false);
				return;
			}
			_selectCell.SetActive(true);

			MouseMove();
			MouseLeftClick();
			MouseRightClick();
		}

		private bool IsMouseLeaveField()
		{
			var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			return (pos.x < _fieldPosition.x || pos.x >= _fieldSize.x ||
				pos.y < _fieldPosition.y || pos.y >= _fieldSize.y);
		}

		private Vector2 GetMouseCellPosition()
		{
			var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			pos.x = Mathf.Round(pos.x);
			pos.y = Mathf.Round(pos.y);
			return pos;
		}

		private void MouseMove()
		{
			Vector3 pos = GetMouseCellPosition();
			pos.z = _selectCell.transform.position.z;
			_selectCell.transform.position = pos;
		}

		private void MouseLeftClick()
		{
			if (!Input.GetMouseButtonDown(0))
				return;

			var block = PanelItemsController.Instance.GetSelectItem();
			if (!block.HasValue)
				return;

			var mousePosition = GetMouseCellPosition();
			var x = (int)mousePosition.x;
			var y = (int)mousePosition.y;

			FieldController.Instance.SetCell(x, y, block.Value);
		}

		private void MouseRightClick()
		{
			if (!Input.GetMouseButtonDown(1))
				return;

			var mousePosition = GetMouseCellPosition();
			var x = (int)mousePosition.x;
			var y = (int)mousePosition.y;

			FieldController.Instance.SetCell(x, y, Block.Empty);
		}
	}
}