using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossEnemy : EnemyBase 
{
	private enum FinalBossState
	{
		Stay,
		Attack,
		Move,
	}

    [SerializeField]
    private float shotTiming = 1;//弾のタイミング
    [SerializeField]
    private List<BulletStruct> firstShot = new List<BulletStruct>();//ショット1のリスト

    [SerializeField, Header("移動場所")]
    private Vector2 movePosition;
    [SerializeField, Header("移動時間")]
    private float moveTime = 2;
    [SerializeField, Header("到着してから攻撃出すまでの時間")]
    private float attackTime = 1;

    private float time = 0;//内部時間
    private Vector2 moveVelocity;
    FinalBossState eState = FinalBossState.Move;
    //初期化
    protected override void Start()
    {
        base.Start();
        time = 0;
        eState = FinalBossState.Move;
    }
    //ループ
    private void Update() 
    {
        switch(eState)
		{
			case FinalBossState.Attack:
                Attack();
                break;
			case FinalBossState.Move:
                Move();
                break;
			case FinalBossState.Stay:
                break;
        }
    }
	//移動
    private void Move()
    {
        if (time == 0)
        {
            moveVelocity = new Vector3(movePosition.x, movePosition.y, 0) - transform.position;
            moveVelocity /= moveTime;
        }
        if (time + Time.deltaTime > moveTime)
        {
            transform.position = movePosition;
            eState = FinalBossState.Attack;
            time = 0;
        }
        else
        {
            time += Time.deltaTime;
            transform.position += new Vector3(moveVelocity.x, moveVelocity.y, 0) * Time.deltaTime;
        }
    }
    //攻撃
    private void Attack()
    {
        time += Time.deltaTime;
        if (time > attackTime)
        {
            foreach (var _shot in firstShot)
            {
                var shotInstance = Instantiate(_shot.bulletBase, transform.position + new Vector3(_shot.position.x, _shot.position.y, 0), Quaternion.identity);
            }
            time = 0;
            //eState = FinalBossState.Stay;
        }
    }
}
