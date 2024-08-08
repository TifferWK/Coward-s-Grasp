using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CowardsGrasp.Utilities
{
    public class DiceLibrary
    {
        private SFC64 sfc64Random;


        public void DiceRoller(ulong seed) //genny a seed...
        {
            sfc64Random = new SFC64(seed);
        }


        public void DiceRoller() //def constructor to init a random seed... 
        {
            sfc64Random = new SFC64();
        }

        //add 1 to each for exclusive upper bound... 
        public int RollD4() => RollDice(5);
        public int RollD6() => RollDice(7);
        public int RollD8() => RollDice(9);
        public int RollD10() => RollDice(11);
        public int RollD12() => RollDice(13);
        public int RollD20() => RollDice(21);
        public int RollD100() => RollDice(101);

        private int RollDice(int exclusiveUpperBoundSidesNumber)
        {
            return sfc64Random.DiceConstraint(1, exclusiveUpperBoundSidesNumber);
        }
    }

    //basically cribbed this wholesale from Eric of OH4 in conversation with him, please take a look at his https://eden.oh4.co/file?ci=tip&name=Core/Random.cpp&ln=131 
    public class SFC64
    {
        private ulong a, b, c, counter;


        public SFC64()
        {
            System.Random rand = new System.Random(); //for onlookers: you'll get an ambiguity ding for this, Unity has a random... I use the default system random in lieu of Eric's random... 
            a = (ulong)rand.Next();
            b = (ulong)rand.Next();
            c = (ulong)rand.Next();
            counter = 1;
        }


        public SFC64(ulong seed) //seed constructor... you can throw in your own seed... I have plans for later, would like to see what would happen based off of Max's notion of "sometimes, the dice roll you make changes based off of if you're happy or sad when you use them for real." 
        {
            a = seed;
            b = seed;
            c = seed;
            counter = 1;
        }

        public ulong NextUInt64()
        {
            ulong tmp = a + b + counter++;
            a = b ^ (b >> 11);
            b = c + (c << 3);
            c = ((c << 24) | (c >> 40)) + tmp;
            return tmp;
        }

        public int DiceConstraint(int minValue, int maxValue)
        {
            //recast the data because of NextUInt64()... 
            ulong ulongMin = (ulong)minValue;
            ulong ulongRange = (ulong)(maxValue - minValue);
            return (int)(ulongMin + (NextUInt64() % ulongRange));
        }


    }
}






//example of use... 

/*
public class ExampleUsage
{
    public void UseDiceRoller()
    {
        // init...
        DiceRoller diceRoller = new DiceRoller();
        Console.WriteLine("Rolling a d20: " + diceRoller.RollD20());

        // init with specific seed.. 
        DiceRoller seededDiceRoller = new DiceRoller(123456789);
        Console.WriteLine("Rolling a d100: " + seededDiceRoller.RollD100());
    }
}
*/