using UnityEngine;

public class Imp : BaseMonster
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
        //sticker
        //Sticker: Range 2: deal d4 damage (Dex checked)

    }


    public override void AttackSetup()
    {
        //d4
    }

    public override void OverbearModifiers()
    {
        //+1 speed
    }

    public override void EstablishPerChildandInstanceDetails()
    {
        //I do it this way because that's how Unity works... in short. 
        generalDescription = "Naughty little guy; a hand of dark forces, albeit small."; 
        specialAbilityDescription = "Sticker: Range 2: deal d4 damage (Dex checked)";
        alignment = CowardsGrasp.Qualities.Alignment.Evil;
        numberOfD8ForHit = 1;
        speed = 2;
        ferocity = 20;
        guard = 20;
        damage = 2; //2 d4
        GenerateHP();

    }

}
