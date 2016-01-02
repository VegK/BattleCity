using UnityEngine;
using System.Collections;

public class FieldController : MonoBehaviour
{
	[Header("Game objects")]
	public BoxCollider2D BorderTop;
	public BoxCollider2D BorderRight;
	public BoxCollider2D BorderBottom;
	public BoxCollider2D BorderLeft;
	[Space(5)]
	public GameObject Background;
	[Header("Settings")]
	[Range(3, 50)]
	public int Width = 13;
	[Range(3, 50)]
	public int Height = 13;

	private void OnDrawGizmos()
	{
		// Draw grid
		Gizmos.color = Color.white;
		for (int x = 0; x < Width; x++)
			for (int y = 0; y < Height; y++)
			{
				var pos = transform.position;
				pos.x += x;
				pos.y += y;
				Gizmos.DrawWireCube(pos, Vector2.one);
			}
	}
}
