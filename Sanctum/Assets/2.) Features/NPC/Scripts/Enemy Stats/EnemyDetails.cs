namespace NPC.Enemy
{
    [System.Serializable]
    public class EnemyDetails
    {
        public KeywordDictionary.EnemyType EnemyType;
        public KeywordDictionary.EnemyArchType EnemyArchetype;
        public EnemyStats Stats { get; set; }
        
        // Constructor to be used when creating a NEW character
        public EnemyDetails(KeywordDictionary.EnemyType type, KeywordDictionary.EnemyArchType archType)
        {
            EnemyType = type;
            EnemyArchetype = archType;
            AssignBaseStats();
        }
        
        private void AssignBaseStats()
        {
            switch (EnemyType)
            {
                case KeywordDictionary.EnemyType.Walker:
                {
                    Stats = new EnemeyWalkerStats();
                    break;
                }
                case KeywordDictionary.EnemyType.Runner:
                {
                    Stats = new EnemeyRunnerStats();
                    break;
                }
                case KeywordDictionary.EnemyType.Screamer:
                {
                    Stats = new EnemeyScreamerStats();
                    break;
                }
                case KeywordDictionary.EnemyType.Bloater:
                {
                    Stats = new EnemeyBloaterStats();
                    break;
                }
                case KeywordDictionary.EnemyType.Shooter:
                {
                    Stats = new EnemyShooterStats();
                    break;
                }
                case KeywordDictionary.EnemyType.Sniper:
                {
                    Stats = new EnemeySniperStats();
                    break;
                }
                case KeywordDictionary.EnemyType.Riot:
                {
                    Stats = new EnemeyRiotStats();
                    break;
                }
            }
        }
    }
}