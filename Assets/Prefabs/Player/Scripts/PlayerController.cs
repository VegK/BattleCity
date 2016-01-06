using System;

public class PlayerController : BlockController
{
	public Direction DirectionMove { get; set; }

	public bool Lock = false;

	public SpawnController PrefabSpawn;

	private void Start()
	{
		gameObject.SetActive(false);

		var obj = Instantiate(PrefabSpawn);
		obj.transform.position = transform.position;
		obj.DestroyEvent += DestroySpawn;
	}

	private void DestroySpawn(object sender, EventArgs e)
	{
		gameObject.SetActive(true);
	}
}
