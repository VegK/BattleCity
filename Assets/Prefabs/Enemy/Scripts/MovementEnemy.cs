public class MovementEnemy : Movement
{
	public void SetDirection(Direction value)
	{
		CurrentDirection = value;
	}

	protected override void OnEnable()
	{
		CurrentDirection = Direction.Bottom;
		base.OnEnable();
	}
}
