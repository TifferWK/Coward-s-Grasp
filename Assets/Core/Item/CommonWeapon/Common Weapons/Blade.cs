using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Blade : CommonWeapon
{
    public override void Equip()
    {
        //equip that via inventory handler... apply buffs etc... 
    }

    public override void SpecialProperties()
    {
        name = "Blade";
        description = "When held in your off-hand, may use a bonus action to make an Attack roll against any adjacent enemy with this weapon";

    }

    public override void OnInit()
    {
        cost = 0;
        range = 1;
        weight = 1;
        damage = 1; 
    }

    public override int CastDamage()
    {
        int damage = 0; 

        if (weight == 1)
        {
            damage = diceLibrary.RollD4();
        }
        else if (weight == 2)
        {
            damage = diceLibrary.RollD6();
        }
        else if (weight == 3)
        {
            damage = diceLibrary.RollD4() + diceLibrary.RollD4();
        }
        else if (weight == 4)
        {
            damage = diceLibrary.RollD6() + diceLibrary.RollD6();
        }

        return damage; 
    }
}
