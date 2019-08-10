using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{

    //用途：近接攻撃をする敵
    //作成者：大久保
    //最終更新日：2019/08/10
    //最終更新者：大久保
    //更新内容：作成

    [SerializeField, Header("体力")]
    private int hp;

    private bool isStartUp;//行動開始しているか

    [SerializeField, Header("移動スピード")]
    private float speed;
    private Vector2 velocity;//移動量

    [SerializeField, Header("移動時間")]
    private float moveTime;
    [SerializeField, Header("待機時間")]
    private float waitTime;

    private float routineTimer;//行動制御用タイマー

    private bool isMove;//移動しているかどうか
    private bool isWait;//待機状態かどうか


    [SerializeField, Header("攻撃力")]
    private int attackPower;
    [SerializeField, Header("中心からの攻撃距離")]
    private float attackDistance;
    [SerializeField, Header("攻撃持続時間")]
    private float attackTime;
    private float attackTimer;//攻撃用タイマー

    private int direction;//方向判定用変数；
    private float defaultScaleX;//方向変化用


    [SerializeField, Header("攻撃時画像")]
    private Sprite attackSprite;
    private Sprite baseSprite;//通常時画像

    [SerializeField, Header("攻撃時判定")]
    private EnemyAttackField attackField;

    private Rigidbody2D rigidbody;

    // Use this for initialization
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();//自身のRigidbody2Dを取得
        isStartUp = false;//まだ動いていない
        direction = -1;//左向き
        defaultScaleX = transform.localScale.x;//方向変化用にスケール取得
        baseSprite = transform.GetComponent<SpriteRenderer>().sprite;//通常時画像取得
        isMove = false;//移動していない
        isWait = true;//待機している
        ChangeSpriteDirection();
        routineTimer = moveTime;
    }

    // Update is called once per frame
    void Update()
    {
        RoutineTimerUpdate();
    }

    /// <summary>
    /// 時間での行動制御
    /// </summary>
    void RoutineTimerUpdate()
    {
        if (!isStartUp)
            return;

        routineTimer += Time.deltaTime;
        if (routineTimer >= moveTime && routineTimer < moveTime + waitTime)//移動を終えたら
        {
            if (isMove)
            {
                isMove = false;
                isWait = true;
                Attack();
            }
        }
        if (routineTimer >= moveTime + waitTime)//待機を終えたら
        {
            routineTimer = 0;
            isMove = true;
            isWait = false;

        }
        Move();
        Wait();
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    void Move()
    {
        if (!isMove)
            return;

        velocity = rigidbody.velocity;
        velocity.x = direction * speed;
        rigidbody.velocity = velocity;
        //transform.position += new Vector3(direction*speed,0,0)*Time.deltaTime;
    }
    /// <summary>
    /// 攻撃処理
    /// </summary>
    void Attack()
    {
        attackField.endTime = attackTime;
        attackField.power = attackPower;
        Vector3 distance = new Vector3(attackDistance * direction, 0, 0);
        var atkField = Instantiate(attackField, transform.position + distance, transform.rotation);
        atkField.transform.parent = transform;

        transform.GetComponent<SpriteRenderer>().sprite = attackSprite;
        attackTimer = 0;
    }
    /// <summary>
    /// 待機処理
    /// </summary>
    void Wait()
    {
        if (!isWait)
            return;

        attackTimer += Time.deltaTime;
        if (attackTimer >= 0.3f)
        {
            transform.GetComponent<SpriteRenderer>().sprite = baseSprite;
        }
        if (attackTimer >= attackTime + 0.1f)
        {
            ChangeDirection();
        }
    }


    /// <summary>
    /// 方向転換
    /// </summary>
    void ChangeDirection()
    {
        float distance = PlayerManager.Instance.PlayerInstance.transform.position.x - transform.position.x;
        if (distance > 0)
        {
            direction = 1;
        }
        else if (distance < 0)
        {
            direction = -1;
        }
        ChangeSpriteDirection();
    }
    /// <summary>
    /// 画像がプレイヤーの方向に向くように
    /// </summary>
    void ChangeSpriteDirection()
    {
        Vector3 scale = transform.localScale;
        scale.x = defaultScaleX * direction;
        transform.localScale = scale;
    }

    /// <summary>
    /// ダメージ処理
    /// </summary>
    /// <param name="damage">受けるダメージ</param>
    void Damage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnBecameVisible()
    {
        isStartUp = true;//行動開始
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "PlayerBullet")
        {
            Damage(col.gameObject.GetComponent<PlayerBullet>().power);
        }
        if (col.gameObject.tag == "PlayerAttackField")
        {
            Damage(col.gameObject.GetComponent<PlayerAttackField>().power);
        }
    }
    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            rigidbody.velocity = Vector2.zero;
            Debug.Log(rigidbody.velocity);
        }
    }
}
