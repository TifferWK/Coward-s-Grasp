using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//custom goodies.... 
using CowardsGrasp.Qualities;
using CowardsGrasp.Utilities;

public abstract class BaseMonster : MonoBehaviour
{
    public Alignment alignment;//get allignment from utilities 
    public int numberOfD8ForHit; //for defense I think?
    public int speed; //pretty straightforward
    public int ferocity; //need to double check what this means in his docs... 
    public int guard; //need to double check what this means in his docs... 
    public int damage; //this is just the number of dice used for attack... dice type nonuniform apparently..? 
    public int playerDamageModifier; //this is for extra defense for certain overbear mods... 
    public string specialAbilityDescription;
    public string generalDescription; 
    public Texture2D[] stateRepresentation; //all the silly little monster sprites... 

    protected DiceLibrary diceLibrary; //use my dice... 
    protected int currentHP; 

    protected virtual void Start()
    {
        //get a new dice for my little monster guy... 
        diceLibrary = new DiceLibrary();
    }

    public void GenerateHP()
    {
        int totalRoll = 0;

        for (int i = 0; i < numberOfD8ForHit; i++)
        {
            int hpRoll = diceLibrary.RollD8();
            totalRoll += hpRoll;
        }
        currentHP = totalRoll; // Store the generated HP
    }

    public abstract void EstablishPerChildandInstanceDetails(); 

    public abstract void OnTheirTurn();
    public abstract void OnPlayersTurn(); 
    public abstract void UseAbility();
    public abstract void AttackSetup(); //this is where we use the dice and pass the vals associated...
    public abstract void OverbearModifiers();

}
