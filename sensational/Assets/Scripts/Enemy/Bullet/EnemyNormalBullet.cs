//通常弾、プレイヤーの弾とほぼ同じ
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNormalBullet : EnemyBulletBase
{
	void Update () 
    {
        Move();
	}

    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
