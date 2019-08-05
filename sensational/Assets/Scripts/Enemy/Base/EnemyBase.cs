using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour 
{
    [SerializeField,Header("初期ステータス")]
    protected EnemyStatus status;

    [SerializeField]
    protected int hp;
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
}
