//Homing弾、Homing以外は同じ
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletHoming : EnemyBulletBase
{
    public float speed = -1;//進む速度
    private Vector2 velocity;//移動量
    void Start()
    {
        velocity = PlayerManager.Instance.MainPlayer.gameObject.transform.position - transform.position;
    }

	void Update () 
    {
        Move();
	}
    /// <summary>
    /// 移動処理
    /// </summary>
    void Move()
    {
        transform.position += new Vector3(velocity.x, velocity.y, 0) * speed * Time.deltaTime;
    }

    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
