//Homing弾、Homing以外は同じ
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletHoming : EnemyBulletBase
{
    void Start()
    {
        velocity = PlayerManager.Instance.PlayerInstance.gameObject.transform.position - transform.position;
        velocity = velocity.normalized;
    }

    void Update()
    {
        Move();
    }

    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
