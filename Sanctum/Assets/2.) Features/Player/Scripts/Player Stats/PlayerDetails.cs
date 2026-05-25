
    [System.Serializable]
    public class PlayerDetails
    {
        public PlayerStats Stats { get; set; }


        // Constructor to be used when creating a NEW character
        public PlayerDetails()
        {
            ResetToBaseStats();
        }

        // Constructor to be used when creating an Existing character
        public PlayerDetails(PlayerStats stats)
        {
            Stats = stats;
        }

        private void ResetToBaseStats()
        {
            Stats = new CharacterBalancedStats();
        }
    }
