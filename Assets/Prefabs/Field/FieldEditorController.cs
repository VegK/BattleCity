using UnityEngine;
using System.Collections;

public class FieldEditorController : MonoBehaviour
{
	[SerializeField]
	private Sprite Cell;

	public bool MouseLock;

	public static FieldEditorController Instance;
	public EnemyType[] OrderSpawnEnemies { get; set; }

	private GameObject _selectCell;
	private Vector2 _fieldPosition;
	private Vector2 _fieldSize;

	public void ClearField()
	{
		for (int x = 0; x < FieldController.Instance.Width; x++)
			for (int y = 0; y < FieldController.Instance.Height; y++)
				FieldController.Instance.SetCell(x, y, Block.Empty);
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
		_selectCell.AddComponent<SpriteRenderer>().sprite = Cell;
		_selectCell.transform.SetParent(transform);

		var pos = transform.position;
		pos.z -= 1;
		_selectCell.transform.position = pos;
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

		var block = GUI.PanelItemsController.Instance.GetSelectItem();
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
