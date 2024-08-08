using CowardsGrasp.Gameplay;
using CowardsGrasp.Qualities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CommonWeapon : Item
{
    public int range;
    public int damage;
    
    public void Start()
    {
        base.Start();
        
    }
    /// <summary>
    /// ____________________________________________________________
    /// abstract cases
    /// ____________________________________________________________
    /// </summary>

    public abstract void SpecialProperties();

    public abstract void Equip();

    public abstract void OnInit();

    public abstract int CastDamage(); 


        /// <summary>
        /// ____________________________________________________________
        /// override cases 
        /// ____________________________________________________________
        /// </summary>
    public override void Use()
    {

    }
   

    //this is for refreshing sprites and descriptions, etc... 
    public override void RefreshState()
    {
       
    }

    public override void Drop()
    {
        //drop it I guess? delist from the inventory later or smth
    }

    public override void Throw()
    {
        //agent needs to throw it, delist from inventory, etc... 
    }
    // Start is called before the first frame update
 
    
}

