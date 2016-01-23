namespace BattleCity
{
	public struct PlayerData
	{
		public event ChangeScoreHandler ChangeScoreEvent;

		public int Score
		{
			get
			{
				return _score;
			}
			set
			{
				if (value == _score)
					return;
				_score = value;
				if (ChangeScoreEvent != null)
					ChangeScoreEvent(value);
			}
		}
		public int LifeScore;
		public int Upgrade;
		public int Enemy1;
		public int Enemy2;
		public int Enemy3;
		public int Enemy4;

		private int _score;

		public void AddPoint(EnemyType enemy, int points)
		{
			Score += points;
			switch (enemy)
			{
				case EnemyType.Enemy1:
					Enemy1++;
					break;
				case EnemyType.Enemy2:
					Enemy2++;
					break;
				case EnemyType.Enemy3:
					Enemy3++;
					break;
				case EnemyType.Enemy4:
					Enemy4++;
					break;
			}
		}

		public void ResetEnemy()
		{
			Enemy1 = 0;
			Enemy2 = 0;
			Enemy3 = 0;
			Enemy4 = 0;
		}

		public delegate void ChangeScoreHandler(int score);
	}
}