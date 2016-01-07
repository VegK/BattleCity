using UnityEngine;

public class ShootingPlayer : Shooting
{
	private PlayerController _playerController;

	protected override void Awake()
	{
		base.Awake();

		_playerController = GetComponent<PlayerController>();
		if (_playerController == null)
		{
			Debug.LogError("Player GameObject not attached " + typeof(PlayerController));
			enabled = false;
		}
	}
	
	private void FixedUpdate()
	{
		if (_playerController.EditorMode)
			return;
		if (Input.GetKey(KeyCode.Space))
			RunBullet();
	}
}
