using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CowardsGrasp.Gameplay;
using CowardsGrasp.Utilities;

public class HealthPotion : Item
{
    public int swigs;
    public Agent user;
    public bool isEmpty;
    public void Start()
    {
        base.Start();
        weight = 1;
        cost = 200;
        swigs = 4;
        nameOfItem = "Healing Potion";
        description = "4 swigs, healing 1d6 each: swig 1 as a bonus action, swig up to 3 as a full action";
    }

    public override void Use()
    {
        if (isEmpty == false)
        {
            HealTarget();
        }
        else if(isEmpty == true)
        {
            RefreshState();
        }

    }
    public void HealTarget()
    {
        if (swigs != 0)
        {
          user.hp+= diceLibrary.RollD6();
            RefreshState();
            if (swigs == 0)
            {
                isEmpty = true;
                RefreshState();
            }
        }
        else if(swigs == 0)
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
            nameOfItem = "Empty Health Potion Bottle";
            description = "It's empty. Hope you didn't need to heal.";
            cost = 0;
        }

        if (isEmpty == false)
        {
            if (swigs == 4)
            {
                cost = 200;
            }
            else if (swigs == 3)
            {
                nameOfItem = "3/4 of a Health Potion";
                description = "Three doses remain.";
                cost = 150;
            }
            else if (swigs == 2)
            {
                nameOfItem = "1/2 of a Health Potion";
                description = "Two doses remain, be frugal.";
                cost = 100;
            }
            else if (swigs == 2)
            {
                nameOfItem = "1/2 of a Health Potion";
                description = "One dose remains.";
                cost = 50;
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
