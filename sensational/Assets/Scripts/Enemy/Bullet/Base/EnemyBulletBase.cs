using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//エネミーの弾のベースクラス

public abstract class EnemyBulletBase : MonoBehaviour
{
    [Header("回転するかどうか"), SerializeField]
    protected bool rotationFlag = true;
    protected float rotationSpeed = 0;
    [SerializeField]
    protected int damage = 5;
    public float speed;//進む速度
    public Vector3 velocity;//移動量


    #region  初期化
    //タグ付け
    protected virtual void Awake()
    {
        gameObject.tag = "EnemyBullet";
        rotationSpeed = Random.Range(2.0f, 5.0f);
        if (Random.Range(0, 2) == 1)
        {
            rotationSpeed *= -1;
        }
    }
    #endregion

    //ダメージ
    public virtual void AddDamage()
    {
        Destroy(gameObject);
        PlayerManager.Instance.PlayerInstance.AddHP(damage*-1);
    }
    /// <summary>
    /// 移動処理
    /// </summary>
    protected void Move()
    {
        transform.position += new Vector3(velocity.x, velocity.y, 0) * speed * Time.deltaTime;
        //回転
        if (rotationFlag)
        {
            transform.Rotate(new Vector3(0, 0, rotationSpeed));
        }
    }
    protected void RotationOnly()
    {
        //回転
        if (rotationFlag)
        {
            transform.Rotate(new Vector3(0, 0, rotationSpeed));
        }
    }
}
