using UnityEngine;

public class Ogre : BaseMonster
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
        //Reckless
        //Reckless: approaches the player, but might not attack the player. Roll their target randomly.

    }


    public override void AttackSetup()
    {
        //2d8+4
    }

    public override void OverbearModifiers()
    {
        //Player Damage +2
    }

    public override void EstablishPerChildandInstanceDetails()
    {
        //I do it this way because that's how Unity works... in short. 
        generalDescription = "Naughty little guy; a hand of dark forces, albeit small.";
        specialAbilityDescription = "Sticker: Range 2: deal d4 damage (Dex checked)";
        alignment = CowardsGrasp.Qualities.Alignment.Neutral;
        numberOfD8ForHit = 5;
        speed = 2;
        ferocity = 28;
        guard = 20;
        damage = 2; //2 d8
        GenerateHP();

    }

}