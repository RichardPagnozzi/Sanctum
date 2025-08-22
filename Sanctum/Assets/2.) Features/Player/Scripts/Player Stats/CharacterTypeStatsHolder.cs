public static class CharacterTypeStatsHolder
{
}

[System.Serializable]
public class CharacterBalancedStats : PlayerStats
{
    public CharacterBalancedStats() : base(
        // Vitals
        level: 1,
        health: 50,
        energy: 100,
        energyRechargeAmmount: 1,
        armor: 10,

        // Movement modifiers
        movementSpeed: 4f,
        movementRotationSpeed: 20f,
        sprintSpeed: 8f,
        sprintCost: 0.1f,
        gravityValue: -9.81f,
        aimAtObjectRange: 50,
        maxJumps: 1,
        jumpHeight: 3,
        jumpDelay: 0.25f,
        jumpResetDelay: 1f,

        // Damage modifiers
        attackDamageModifier: 1.0f,
        attackVelocityModifier: 1.0f,
        criticalChanceModifier: 1.0f,
        criticalDamageModifier: 1.0f,
        armorPiercingModifier: 1.0f,
        ammoPoolModifier: 1,
        // Inventory
        inventorySlots: 16,
        activeItemSlots: 4,
        startingInventorySlots: 6,
        startingActiveItemSlots: 1,
        jumpCost: 10f
    )
    {
    }
}

[System.Serializable]
public class CharacterFastStats : PlayerStats
{
    public CharacterFastStats() : base(
        // Vitals
        level: 1,
        health: 35,
        energy: 90,
        energyRechargeAmmount: 0.75f,
        armor: 10,

        // Movement modifiers
        movementSpeed: 4f,
        movementRotationSpeed: 20f,
        sprintSpeed: 12f,
        sprintCost: 0.1f,
        gravityValue: -9.81f,
        aimAtObjectRange: 50,
        maxJumps: 1,
        jumpHeight: 3,
        jumpDelay: 0.25f,
        jumpResetDelay: 1f,

        // Damage modifiers
        attackDamageModifier: 1.0f,
        attackVelocityModifier: 1.0f,
        criticalChanceModifier: 1.0f,
        criticalDamageModifier: 1.0f,
        armorPiercingModifier: 1.0f,
        ammoPoolModifier: 1,
        // Inventory
        inventorySlots: 16,
        activeItemSlots: 4,
        startingInventorySlots: 6,
        startingActiveItemSlots: 1,
        jumpCost: 10f
    )
    {
    }
}

[System.Serializable]
public class CharacterToughStats : PlayerStats
{
    public CharacterToughStats() : base(
        // Vitals
        level: 1,
        health: 65,
        energy: 75,
        energyRechargeAmmount: 1,
        armor: 20,

        // Movement modifiers
        movementSpeed: 4f,
        movementRotationSpeed: 20f,
        sprintSpeed: 7f,
        sprintCost: 0.1f,
        gravityValue: -9.81f,
        aimAtObjectRange: 50,
        maxJumps: 1,
        jumpHeight: 3,
        jumpDelay: 0.25f,
        jumpResetDelay: 1f,

        // Damage modifiers
        attackDamageModifier: 1.0f,
        attackVelocityModifier: 1.0f,
        criticalChanceModifier: 1.0f,
        criticalDamageModifier: 1.0f,
        armorPiercingModifier: 1.0f,
        ammoPoolModifier: 1,
        // Inventory
        inventorySlots: 16,
        activeItemSlots: 4,
        startingInventorySlots: 6,
        startingActiveItemSlots: 1,
        jumpCost: 10f
    )
    {
    }
}

[System.Serializable]
public class CharacterAthleticStats : PlayerStats
{
    public CharacterAthleticStats() : base(
        // Vitals
        level: 1,
        health: 55,
        energy: 100,
        energyRechargeAmmount: 2,
        armor: 10,

        // Movement modifiers
        movementSpeed: 5f,
        movementRotationSpeed: 20f,
        sprintSpeed: 8f,
        sprintCost: 0.1f,
        gravityValue: -9.81f,
        aimAtObjectRange: 50,
        maxJumps: 1,
        jumpHeight: 3,
        jumpDelay: 0.25f,
        jumpResetDelay: 1f,

        // Damage modifiers
        attackDamageModifier: 1.0f,
        attackVelocityModifier: 1.0f,
        criticalChanceModifier: 1.0f,
        criticalDamageModifier: 1.0f,
        armorPiercingModifier: 1.0f,
        ammoPoolModifier: 1,
        // Inventory
        inventorySlots: 16,
        activeItemSlots: 4,
        startingInventorySlots: 6,
        startingActiveItemSlots: 1,
        jumpCost: 10f
    )
    {
    }
}