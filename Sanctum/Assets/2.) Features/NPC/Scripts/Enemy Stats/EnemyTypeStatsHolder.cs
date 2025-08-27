public static class EnemyTypeStatsHolder
{
}

namespace NPC.Enemy
{
    [System.Serializable]
    public class EnemeyWalkerStats : EnemyStats
    {
        public EnemeyWalkerStats() : base(
            health: 25,
            armor: 0,
            walkingSpeed: 2,
            chasingSpeed: 2,
            rotationSpeed: 2,
            attackDamageBase: 20,
            attackDamageModifier: 1,
            weakspotMultiplier: 1.25f)
        {
        }
    }

    [System.Serializable]
    public class EnemeyRunnerStats : EnemyStats
    {
        public EnemeyRunnerStats() : base(
            health: 50,
            armor: 10,
            walkingSpeed: 2,
            chasingSpeed: 2,
            rotationSpeed: 2,
            attackDamageBase: 25,
            attackDamageModifier: 1,
            weakspotMultiplier: 1.25f)
        {
        }
    }

    [System.Serializable]
    public class EnemeyScreamerStats : EnemyStats
    {
        public EnemeyScreamerStats() : base(
            health: 30,
            armor: 0,
            walkingSpeed: 2,
            chasingSpeed: 2,
            rotationSpeed: 2,
            attackDamageBase: 10,
            attackDamageModifier: 1,
            weakspotMultiplier: 1.25f)
        {
        }
    }

    [System.Serializable]
    public class EnemeyBloaterStats : EnemyStats
    {
        public EnemeyBloaterStats() : base(
            health: 75,
            armor: 0,
            walkingSpeed: 2,
            chasingSpeed: 2,
            rotationSpeed: 2,
            attackDamageBase: 50,
            attackDamageModifier: 1,
            weakspotMultiplier: 1.25f)
        {
        }
    }

    [System.Serializable]
    public class EnemyShooterStats : EnemyStats
    {
        public EnemyShooterStats() : base(
            health: 40,
            armor: 10,
            walkingSpeed: 2,
            chasingSpeed: 2,
            rotationSpeed: 2,
            attackDamageBase: 25,
            attackDamageModifier: 1,
            weakspotMultiplier: 1.25f)
        {
        }
    }

    [System.Serializable]
    public class EnemeySniperStats : EnemyStats
    {
        public EnemeySniperStats() : base(
            health: 20,
            armor: 0,
            walkingSpeed: 2,
            chasingSpeed: 2,
            rotationSpeed: 2,
            attackDamageBase: 50,
            attackDamageModifier: 1,
            weakspotMultiplier: 1.25f)
        {
        }
    }

    [System.Serializable]
    public class EnemeyRiotStats : EnemyStats
    {
        public EnemeyRiotStats() : base(
            health: 50,
            armor: 50,
            walkingSpeed: 2,
            chasingSpeed: 2,
            rotationSpeed: 2,
            attackDamageBase: 30,
            attackDamageModifier: 1,
            weakspotMultiplier: 1.25f)
        {
        }
    }
}