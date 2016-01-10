using UnityEngine;

public class ShootingPlayer : Shooting
{
	private PlayerController _playerController;
	private string _buttonNameFire;

	protected override void Awake()
	{
		_playerController = GetComponent<PlayerController>();
		_buttonNameFire = _playerController.TypeItem + "_Fire";
		base.Awake();
	}

	private void FixedUpdate()
	{
		if (_playerController.EditorMode)
			return;
		if (Input.GetButton(_buttonNameFire))
			RunBullet();
	}
}
