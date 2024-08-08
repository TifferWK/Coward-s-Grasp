using UnityEngine;

public class Goblin : BaseMonster
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
        //goblin does a "Fizzers"...
        //Fizzers: Once: Deal d6 damage to the player or any creature adjacent to the player, selected randomly.

    }


    public override void AttackSetup()
    {
       //d6
    }

    public override void OverbearModifiers()
    {
        //+2 dmg
    }

    public override void EstablishPerChildandInstanceDetails()
    {
        //I do it this way because that's how Unity works... in short. 
        generalDescription = "Green and equipped with a spear.";
        specialAbilityDescription = "Fizzers: Once: Deal d6 damage to the player or any creature adjacent to the player, selected randomly.";
        alignment = CowardsGrasp.Qualities.Alignment.Chaotic;
        numberOfD8ForHit = 2;
        speed = 1;
        ferocity = 21;
        guard = 16;
        damage = 1; 
        GenerateHP(); 

    }
  
}

