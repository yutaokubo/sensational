using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//エネミーの弾のベースクラス

public abstract class EnemyBulletBase : MonoBehaviour
{
    [SerializeField]
    protected int damage = 5;
    public float speed;//進む速度
    protected Vector3 velocity;//移動量

    
    #region  初期化
    //タグ付け
    protected virtual void Awake() 
    {
        gameObject.tag = "EnemyBullet";
    }
    #endregion
    
    //ダメージ
    public virtual void AddDamage()
    {
        Destroy(gameObject);
        PlayerManager.Instance.PlayerInstance.AddHP(damage*-1);
    }
}
