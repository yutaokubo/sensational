using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : ScriptableObject
{
	[SerializeField]
    private int hp = 10;
	public int HP
	{
        get { return hp; } 
    }
}
