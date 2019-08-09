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
        Return,
        Normal,
    }

    [SerializeField,Header("特殊ショットの発射タイミング")]
    private float shotTiming = 1.0f;//弾のタイミング
    [SerializeField, Header("通常弾の発射タイミング")]
    private float normalShotTiming = 1.0f;
    [SerializeField, Header("近距離攻撃の間隔")]
    private float shortAttackTime = 0.5f;
    [SerializeField]
    private List<BulletStruct> firstShot = new List<BulletStruct>();//ショット1のリスト
    [SerializeField]
    private List<BulletStruct> normalShot = new List<BulletStruct>();//ホーミング
    [SerializeField]
    private BulletStruct shortAttack;//近距離攻撃

    [SerializeField, Header("地面での移動速度")]
    private float moveSpeed = 1;
    [SerializeField, Header("移動場所")]
    private Vector2 movePosition;
    [SerializeField, Header("移動時間")]
    private float moveTime = 2.0f;
    [SerializeField, Header("到着してから攻撃出すまでの時間")]
    private float attackTime = 1.0f;
    [SerializeField, Header("全弾放ってから戻ってくるまでの時間")]
    private float stayTime = 1.0f;
    [SerializeField, Header("通常状態から特殊攻撃状態になる時間")]
    private float normalTime = 8.0f;
    [SerializeField, Header("地面への移動場所Y軸")]
    private float groundPosition = -2.0f;
    [SerializeField, Header("地面への移動場所X軸ランダム範囲")]
    private float groundXMin = 0;
    [SerializeField]
    private float groundXMax = 5;

    [SerializeField,Header("特殊攻撃？")]
    private Sprite Stand_Sprite;
    [SerializeField, Header("base")]
    private Sprite Base_Sprite;
    [SerializeField, Header("Attack")]
    private Sprite Attack_Sprite;

    private float time = 0;//内部時間
    private Vector2 moveVelocity;
    FinalBossState eState = FinalBossState.Move;
    SpriteRenderer sr;
    //初期化
    protected override void Start()
    {
        base.Start();
        sr = GetComponent<SpriteRenderer>();
        time = 0;
        eState = FinalBossState.Normal;
    }
    //ループ
    private void Update() 
    {
        float distance = PlayerManager.Instance.PlayerInstance.transform.position.x - transform.position.x;
        if(distance>0)
        {
            gameObject.transform.localScale = new Vector3(-Mathf.Abs(gameObject.transform.localScale.x), gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        }
        else if(distance<0)
        {
            gameObject.transform.localScale = new Vector3(Mathf.Abs(gameObject.transform.localScale.x), gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        }

        switch(eState)
		{
			case FinalBossState.Attack:
                Attack();
                break;
			case FinalBossState.Move:
                Move();
                break;
			case FinalBossState.Stay:
                Stay();
                break;
            case FinalBossState.Return:
                Return();
                break;
            case FinalBossState.Normal:
                Normal(distance);
                break;
        }
    }

    #region 地面
    private float homingAttackNowTime = 0;
    private float shortAttackNowTime = 0;
    private EnemyBulletBase shortAttackInstance = null;
    private void Normal(float distance)
    {
        //初期化
        if(time == 0)
        {
            homingAttackNowTime = 0;
            shortAttackNowTime = 0;
        }
        time += Time.deltaTime;
        homingAttackNowTime += Time.deltaTime;
        if (shortAttackInstance == null)
        {
            sr.sprite = Base_Sprite;
            shortAttackNowTime += Time.deltaTime;
        }
        if(time>normalTime)
        {
            eState = FinalBossState.Move;
            time = 0;
        }
        NormalMove(distance,moveSpeed);
        if (normalShotTiming < homingAttackNowTime)
        {
            foreach (var _shot in normalShot)
            {
                var shotInstance = Instantiate(_shot.bulletBase, transform.position + new Vector3(_shot.position.x, _shot.position.y, 0), Quaternion.identity);
            }
            homingAttackNowTime = 0;
        }
        if(shortAttackTime < shortAttackNowTime)
        {
            sr.sprite = Attack_Sprite;
            shortAttackInstance = Instantiate(shortAttack.bulletBase, Vector3.zero, Quaternion.identity);
            shortAttackInstance.transform.parent = transform;
            shortAttackInstance.transform.localPosition = new Vector3(shortAttack.position.x, shortAttack.position.y, 0);
            shortAttackNowTime = 0;
        }
    }
    #endregion

	//上空への移動
    private void Move()
    {
        if (time == 0)
        {
            sr.sprite = Stand_Sprite;
            moveVelocity = new Vector3(movePosition.x, movePosition.y, 0) - transform.position;
            moveVelocity /= moveTime;
        }
        if (time + Time.deltaTime > moveTime)
        {
            transform.position = movePosition;
            eState = FinalBossState.Attack;
            time = 0;
            attackRandomNum = Random.Range(4, 6);
            attackNowNumber = 0;
        }
        else
        {
            time += Time.deltaTime;
            transform.position += new Vector3(moveVelocity.x, moveVelocity.y, 0) * Time.deltaTime;
        }
    }
    private int attackRandomNum;//攻撃回数
    private int attackNowNumber;//現在の攻撃回数
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
            attackNowNumber++;
            if(attackNowNumber>attackRandomNum)
            {
                eState = FinalBossState.Stay;
            }
        }
    }
    //待機
    private void Stay()
    {
        time += Time.deltaTime;
        if (time > attackTime)
        {
            eState = FinalBossState.Return;
            time = 0;
        }
    }
    private float randomXGroundPos;
    //地面へ戻る
    private void Return() 
    {
        if (time == 0)
        {
            randomXGroundPos = Random.Range(groundXMin, groundXMax);
            moveVelocity = new Vector3(randomXGroundPos, groundPosition, 0) - transform.position;
            moveVelocity /= moveTime;
        }
        if (time + Time.deltaTime > moveTime)
        {
            sr.sprite = Base_Sprite;
            transform.position = new Vector3(randomXGroundPos, groundPosition, 0);
            eState = FinalBossState.Normal;
            time = 0;
        }
        else
        {
            time += Time.deltaTime;
            transform.position += new Vector3(moveVelocity.x, moveVelocity.y, 0) * Time.deltaTime;
        }

    }
}
