using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BaseCharacter
{
    public override void Start()
    {
        base.Start();
    }

    void OnMouseDown()
    {
        StartCoroutine(battleSystem.AttackEnemy(this));
    }
}
