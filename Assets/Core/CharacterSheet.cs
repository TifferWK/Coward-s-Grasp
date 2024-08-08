using System.Collections.Generic;
using CowardsGrasp.Qualities; //for alignment... 


public class CharacterSheet
{
   

    public string Player { get; set; }
    public string Campaign { get; set; }
    public string Name { get; set; }
    public Alignment CharacterAlignment { get; set; }
    public string ClassAndLevels { get; set; }

    // Attributes
    public int Strength { get; set; }
    public int Dexterity { get; set; }
    public int Intelligence { get; set; }
    public int Presence { get; set; }

    // Derived Attributes
    public int Move { get; set; }
    public int Defense { get; set; }
    public int Alacrity { get; set; }
    public int Precision { get; set; }
    public int Prowess { get; set; }
    public int Investigation { get; set; }
    public int Intuition { get; set; }
    public int CWeight { get; set; }

    // Health Points
    public int HP { get; set; }

    // Paths, Circles, Skills, Abilities
    public List<PathCircleSkillAbility> PathCircleSkillAbilities { get; set; }

    // Items and Spell Slots
    public List<ItemSpellSlot> ItemsAndSpellSlots { get; set; }

    // Resources
    public int Gold { get; set; }
    public int Food { get; set; }
    public int Exp { get; set; }
    public int Lore { get; set; }

    public CharacterSheet()
    {
        PathCircleSkillAbilities = new List<PathCircleSkillAbility>();
        ItemsAndSpellSlots = new List<ItemSpellSlot>();
    }
}

public class PathCircleSkillAbility
{
    public string Name { get; set; }
    public string Resource { get; set; }
    public int Total { get; set; }
    public int Current { get; set; }
}

public class ItemSpellSlot
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int WeightOrFocusTotal { get; set; }
    public int Damage { get; set; }
}


