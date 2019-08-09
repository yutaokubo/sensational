using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour 
{
    [SerializeField,Header("初期ステータス")]
    protected EnemyStatus status;

    [SerializeField]
    protected int hp;
    protected virtual void Awake() 
    {
        gameObject.tag = "Enemy";
    }
    protected virtual void Start () 
	{
        if(status == null)return;
        hp = status.HP;
    }

    [System.Serializable]
    protected struct BulletStruct
    {
        public EnemyBulletBase bulletBase;
        public Vector2 position;
    }
    protected virtual void Damage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerBullet")
        {
            Damage(other.gameObject.GetComponent<PlayerBullet>().power);
        }
        if (other.gameObject.tag == "PlayerAttackField")
        {
            Damage(other.gameObject.GetComponent<PlayerAttackField>().power);
        }
    }
}
