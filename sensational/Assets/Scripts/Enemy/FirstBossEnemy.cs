using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ボス1

public class FirstBossEnemy : EnemyBase
{
    [SerializeField]
    private float shotTiming = 1;//弾のタイミング
    [SerializeField, Header("通常弾の発射タイミング")]
    private float normalShotTiming = 2.0f;
    [SerializeField, Header("近距離攻撃の間隔")]
    private float shortAttackTime = 1.3f;

    [SerializeField, Header("地面での移動速度")]
    private float moveSpeed = 1;


    [SerializeField]
    private List<BulletStruct> firstShot = new List<BulletStruct>();//ショット1のリスト
    [SerializeField]
    private List<BulletStruct> normalShot = new List<BulletStruct>();//ホーミング
    [SerializeField]
    private BulletStruct shortAttack;//近距離攻撃

    [SerializeField, Header("base")]
    private Sprite Base_Sprite;
    [SerializeField, Header("Attack")]
    private Sprite Attack_Sprite;


    private float time = 0;//内部時間
    private SpriteRenderer sr;
    private float homingAttackNowTime = 0;
    private float shortAttackNowTime = 0;
    private EnemyBulletBase shortAttackInstance = null;


    //初期化
    protected override void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        base.Start();
        time = 0;
        homingAttackNowTime = 0;
        shortAttackNowTime = 0;
    }
    //ループ
    private void Update()
    {
        if (time > shotTiming)
        {
            foreach (var _shot in firstShot)
            {
                var shotInstance = Instantiate(_shot.bulletBase, transform.position + new Vector3(_shot.position.x, _shot.position.y, 0), Quaternion.identity);
                //プレイヤーが右側にいたら逆に打つ
                if (transform.position.x < PlayerManager.Instance.PlayerInstance.transform.position.x)
                {
                    shotInstance.speed *= -1;
                }
            }
            time = 0;
        }
        time += Time.deltaTime;
        NormalMove();
    }
    private void NormalMove()
    {
        float distance = PlayerManager.Instance.PlayerInstance.transform.position.x - transform.position.x;
        if (distance > 0)
        {
            gameObject.transform.localScale = new Vector3(-Mathf.Abs(gameObject.transform.localScale.x), gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        }
        else if (distance < 0)
        {
            gameObject.transform.localScale = new Vector3(Mathf.Abs(gameObject.transform.localScale.x), gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        }

        homingAttackNowTime += Time.deltaTime;
        if (shortAttackInstance == null)
        {
            sr.sprite = Base_Sprite;
            shortAttackNowTime += Time.deltaTime;
        }
        NormalMove(distance, moveSpeed);
        //遠距離攻撃
        if (normalShotTiming < homingAttackNowTime)
        {
            foreach (var _shot in normalShot)
            {
                var shotInstance = Instantiate(_shot.bulletBase, transform.position + new Vector3(_shot.position.x, _shot.position.y, 0), Quaternion.identity);
            }
            homingAttackNowTime = 0;
        }
        //近接攻撃
        if (shortAttackTime < shortAttackNowTime)
        {
            sr.sprite = Attack_Sprite;
            shortAttackInstance = Instantiate(shortAttack.bulletBase, Vector3.zero, Quaternion.identity);
            shortAttackInstance.transform.parent = transform;
            shortAttackInstance.transform.localPosition = new Vector3(shortAttack.position.x, shortAttack.position.y, 0);
            shortAttackNowTime = 0;
        }
    }
}
