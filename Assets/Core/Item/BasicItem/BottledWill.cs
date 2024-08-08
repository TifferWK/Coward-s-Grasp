using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CowardsGrasp.Gameplay;

public class BottledWill : Item
{
    public int swigs;
    public Agent user;
    public bool isEmpty;
    public void Start()
    {
        base.Start();
        weight = 1;
        cost = 100;
        swigs = 4;
        nameOfItem = "Bottled Will";
        description = "4 swigs, healing 1d6 each: swig 1 as a bonus action, swig up to 3 as a full action";
    }

    public override void Use()
    {
        if (isEmpty == false)
        {
            GiveWill();
        }
        else if (isEmpty == true)
        {
            RefreshState();
        }

    }
    public void GiveWill()
    {
        if (swigs != 0)
        {
           //give 1 will  
            RefreshState();
            if (swigs == 0)
            {
                isEmpty = true;
                RefreshState();
            }
        }
        else if (swigs == 0)
        {
            isEmpty = true;
            RefreshState();
        }
    }

    //this is for refreshing sprites and descriptions, etc... 
    public override void RefreshState()
    {
        if (isEmpty)
        {
            nameOfItem = "Empty Bottled Will Bottle";
            description = "It's empty.";
            cost = 0;
        }

        if (isEmpty == false)
        {
            if (swigs == 4)
            {
                cost = 100;
            }
            else if (swigs == 3)
            {
                nameOfItem = "3/4 of a Bottled Will";
                description = "Three doses remain.";
                cost = 75;
            }
            else if (swigs == 2)
            {
                nameOfItem = "1/2 of a Bottled Will";
                description = "Two doses remain, be frugal.";
                cost = 50;
            }
            else if (swigs == 2)
            {
                nameOfItem = "1/2 of a Bottled Will";
                description = "One dose remains.";
                cost = 25;
            }
        }
    }

    public override void Drop()
    {
        //drop it I guess? delist from the inventory later or smth
    }

    public override void Throw()
    {
        //agent needs to throw it, delist from inventory, etc... 
    }

}
