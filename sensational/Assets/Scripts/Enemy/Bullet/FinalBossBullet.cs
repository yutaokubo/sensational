using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//FinalBossの弾
public class FinalBossBullet : EnemyBulletBase
{

	void Start () 
	{
        velocity.x = Random.Range(-100,100);
        velocity = velocity.normalized;
    }
	void Update () 
    {
        Move();
	}
    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
