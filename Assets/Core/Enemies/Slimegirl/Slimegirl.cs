using Unity.VisualScripting;
using UnityEngine;

public class Slimegirl : BaseMonster
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
        //Heal: Once: restore d6 HP to a creature

       // Attack Rolls against Slimegirls are Weak; Fumble: disarmed

    }


    public override void AttackSetup()
    {
        //d4
    }

    public override void OverbearModifiers()
    {
        //+2 Player Damage. 
    }

    public override void EstablishPerChildandInstanceDetails()
    {
        //I do it this way because that's how Unity works... in short. 
        generalDescription = "This space was left intentionally blank."; //idea by Dave 
        specialAbilityDescription = "Heal: Once: restore d6 HP to a creature. Attack Rolls against Slimegirls are Weak; Fumble: disarmed";
        alignment = CowardsGrasp.Qualities.Alignment.Good;
        numberOfD8ForHit = 1;
        speed = 0;
        ferocity = 16;
        guard = 22;
        damage = 1; //d4
        GenerateHP();

    }

}
