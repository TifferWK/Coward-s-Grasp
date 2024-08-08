using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Troll : BaseMonster
{


    protected override void Start()
    {
        base.Start();
        EstablishPerChildandInstanceDetails();
        // this cues HP 
    }

    public override void OnTheirTurn()
    {
        AttackSetup();
        OverbearModifiers();
    }

    public override void OnPlayersTurn()
    {

    }

    public override void UseAbility()
    {
        //if precombat
        //regen 2HP 

        //otherwise general magic resistance
        //Magic Resistance(?)

    }


    public override void AttackSetup()
    {
        //d8+1
    }

    public override void OverbearModifiers()
    {
        //+3 dmg +4 guard
    }

    public override void EstablishPerChildandInstanceDetails()
    {
        //I do it this way because that's how Unity works... in short. 
        generalDescription = "Pay the toll."; 
        specialAbilityDescription = "Pre-Combat: Regen 2 HP, perpetual Magic Resistance";
        alignment = CowardsGrasp.Qualities.Alignment.Neutral;
        numberOfD8ForHit = 3;
        speed = 0;
        ferocity = 18;
        guard = 26;
        damage = 1;
        GenerateHP();

    }

}
