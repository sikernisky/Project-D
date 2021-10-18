using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broccoli : Food
{
    public override int BaseReward { get ; set; }
    public override string Name { get; set; }
    public override PathType Path { get; set; }


    private void Start()
    {
        BaseReward = 50;
        Name = "Broccoli";
        Path = PathType.Veggie;
    }


    public override void PerformAbility()
    {
        GameObject target = GameObject.Find("Hello");
        
        
    }

}
