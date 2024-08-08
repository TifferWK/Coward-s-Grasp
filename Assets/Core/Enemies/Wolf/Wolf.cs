using UnityEngine;

public class Wolf : BaseMonster
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
        //wolf goes fast I guess...
        //speedy guy: move +1.

    }


    public override void AttackSetup()
    {
        //d4+1
    }

    public override void OverbearModifiers()
    {
        //+1 dmg, +2 ferocity
    }

    public override void EstablishPerChildandInstanceDetails()
    {
        //I do it this way because that's how Unity works... in short. 
        generalDescription = "Bark bark~"; //idea by Dave 
        specialAbilityDescription = "Move +1";
        alignment = CowardsGrasp.Qualities.Alignment.Beast;
        numberOfD8ForHit = 1;
        speed = 1;
        ferocity = 19;
        guard = 16;
        damage = 1;
        GenerateHP();

    }

}
