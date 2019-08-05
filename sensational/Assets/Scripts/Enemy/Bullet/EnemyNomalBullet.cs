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
    /// <summary>
    /// 移動処理
    /// </summary>
    void Move()
    {
        velocity = new Vector3(speed, 0, 0);
        transform.position += velocity * Time.deltaTime;
    }

    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
