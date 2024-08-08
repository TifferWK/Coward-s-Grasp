using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CowardsGrasp.Qualities
{
    public enum Alignment
    {
        Evil,
        Neutral,
        Good,
        Chaotic,  
        Beast,
        Lawful,
        Slave,
        Fae
    }
    public enum Season
    {
        Spring,
        Summer,
        Fall,
        Winter
    }

    public class CampaignDetails
    {
        public string campaignName;
    }

    public enum EquipPoint
    {
        armor,
        left_hand,
        right_hand

    }

}
