using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShortAttack : EnemyBulletBase
{
    [SerializeField]
    private float deleteTime = 1.5f;
    private float time = 0;
    //ダメージ
    public override void AddDamage()
    {
        PlayerManager.Instance.PlayerInstance.AddHP(damage*-1);
    }
	private void Update() 
	{
        time += Time.deltaTime;
		if(time>deleteTime)
		{
            Destroy(gameObject);
        }
        RotationOnly();
    }
}
