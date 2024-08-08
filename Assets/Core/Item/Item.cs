using CowardsGrasp.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour 
{
    public int weight;
    public int cost;
    public string nameOfItem; 
    public string description;
    public Texture2D[] itemSprites;
    public DiceLibrary diceLibrary;
    protected virtual void Start()
    {
        //new dice for item  
        diceLibrary = new DiceLibrary();
    }

    public abstract void Use();
    public abstract void RefreshState();
    public abstract void Drop();
    public abstract void Throw(); 

}
