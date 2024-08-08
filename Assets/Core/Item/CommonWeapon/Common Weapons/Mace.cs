using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mace : CommonWeapon
{
    public override void Equip()
    {
        //equip that via inventory handler... apply buffs etc... 
    }

    public override void SpecialProperties()
    {
        name = "Mace";
        description = "May use a bonus action on a killing blow to subdue (rather than kill) a target brought to 0 HP. If held in both hands, may use a bonus action to take -1 Alacrity this turn to roll damage twice and take the higher result.";

    }

    public override void OnInit()
    {
        cost = 0;
        range = 1;
        weight = 2;
        damage = 1;
    }

    public override int CastDamage() //mace case
    {
        int damage = 0;

        if (weight == 2)
        {
            damage = diceLibrary.RollD6() + 1;
        }
        else if (weight == 3)
        {
            damage = diceLibrary.RollD8() + 1;
        }
        else if (weight == 4)
        {
            damage = diceLibrary.RollD12() + 1;
        }

        return damage;
    }
}
