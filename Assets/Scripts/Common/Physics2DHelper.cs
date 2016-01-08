using System.Collections.Generic;
using UnityEngine;

public static class Physics2DHelper
{
	public static bool Overlap(this Collider2D collider, Vector2 point1, Vector2 point2,
		out Collider2D[] listOverlap)
	{
		var res = new List<Collider2D>();
		var layer = collider.gameObject.layer;
		var colliders = Physics2D.OverlapAreaAll(point1, point2);

		foreach (Collider2D item in colliders)
			if (!Physics2D.GetIgnoreLayerCollision(layer, item.gameObject.layer))
				res.Add(item);

		listOverlap = res.ToArray();
		return (listOverlap.Length > 0);
	}

	public static bool CheckColliderBeside(this Collider2D collider, Direction direction)
	{
		var position = collider.transform.position;

		switch (direction)
		{
			case Direction.Top:
				position.x += 0.1f;
				position.y += 0.55f;
				if (IgnoreAllCollisionsInPoint(position, collider))
				{
					position.x -= 0.2f;
					if (IgnoreAllCollisionsInPoint(position, collider))
						return false;
				}
				break;
			case Direction.Right:
				position.x += 0.55f;
				position.y += 0.1f;
				if (IgnoreAllCollisionsInPoint(position, collider))
				{
					position.y -= 0.2f;
					if (IgnoreAllCollisionsInPoint(position, collider))
						return false;
				}
				break;
			case Direction.Bottom:
				position.x += 0.1f;
				position.y -= 0.55f;
				if (IgnoreAllCollisionsInPoint(position, collider))
				{
					position.x -= 0.2f;
					if (IgnoreAllCollisionsInPoint(position, collider))
						return false;
				}
				break;
			case Direction.Left:
				position.x -= 0.55f;
				position.y += 0.1f;
				if (IgnoreAllCollisionsInPoint(position, collider))
				{
					position.y -= 0.2f;
					if (IgnoreAllCollisionsInPoint(position, collider))
						return false;
				}
				break;
		}
		return true;
	}

	private static bool IgnoreAllCollisionsInPoint(Vector2 point, Collider2D collider)
	{
		var layer = collider.gameObject.layer;
		var colliders = Physics2D.OverlapPointAll(point);
		foreach (Collider2D item in colliders)
			if (!Physics2D.GetIgnoreLayerCollision(layer, item.gameObject.layer))
				return false;
		return true;
	}
}
