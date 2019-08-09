using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    //用途：プレイヤー全般のスクリプト
    //作成者：大久保
    //最終更新日：2019/08/04
    //最終更新者：大久保
    //更新内容：バフ時処理追加



    [Header("通常時の移動速度")]
    [SerializeField]
    private float speed;
    [Header("バフ後の移動速度")]
    [SerializeField]
    private float buffSpeed;
    private float nowSpeed;//現在の移動速度
    private Vector3 velocity;//移動量
    private float direction;//方向。弾の発射方向を決める
    private float defaultScaleX;//初期のスケールXの数値。画像の方向切り替え用

    [Header("通常時のジャンプ力")]
    [SerializeField]
    private float jumpPower;
    [Header("バフ後のジャンプ力")]
    [SerializeField]
    private float buffJumpPower;
    private float nowJumpPower;
    private bool isJump;//ジャンプ中か

    [Header("遠距離攻撃の弾オブジェクト")]
    [SerializeField]
    private PlayerBullet bullet;

    [Header("遠距離攻撃の弾のスピード")]
    [SerializeField]
    private float bulletSpeed;

    [Header("近距離攻撃の攻撃範囲オブジェクト")]
    [SerializeField]
    private PlayerAttackField attackField;

    [Header("中心からの攻撃距離")]
    [SerializeField]
    private float attackDistance;

    [Header("近距離攻撃継続時間")]
    [SerializeField]
    private float attackTime;
    private float attackTimer;//攻撃判定用

    private int hp;//体力
    //[SerializeField]
    //private Text hpText;
    [SerializeField]
    private Image hpGauge;

    private float influencePoint;//影響力
    //[SerializeField]
    //private Text influenceText;
    [SerializeField]
    private Image influenceGauge;

    [Header("秒間影響力回復量")]
    [SerializeField]
    private float influenceRecoveryAmount;

    [Header("バフ時影響力減少量")]
    [SerializeField]
    private float buffTimeRemoveInfluencePoint;

    [Header("影響力のモード、Falseが全体攻撃,trueがバフ")]
    [SerializeField]
    public static bool influenceMode;

    private bool isBuff;//影響力によるバフがかかっているか

    private bool isDead;//死んでいるかどうか

    [SerializeField]
    private Sprite attackSprite;//攻撃時画像
    private Sprite baseSprite;//通常時画像


    private Rigidbody2D rigidbody;

	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody2D>();//自身のRigidbody2Dを取得
        isJump = false;//ジャンプはしていないことに
        direction = 1;//方向は右向き
        defaultScaleX = transform.localScale.x;//スケールX初期値獲得
        SetHP(100);//体力は100に
        SetInfluencePoint(100);//影響力は100に
        //influenceMode = true;//影響力はバフに
        nowSpeed = speed;//スピードを通常時のものに
        nowJumpPower = jumpPower;//ジャンプ力を通常時のものに
        isBuff = false;//バフはかけない
        isDead = false;//死んでいない
        baseSprite = transform.GetComponent<SpriteRenderer>().sprite;//通常時画像は現在のものに
    }
	
	// Update is called once per frame
	void Update () {

        Move();
        Jump();
        Shot();
        Attack();
        AttackTimerCount();
        InfluenceAttack();
        InfluencePointUpdate();
        BuffTimeUpdate();
	}

    /// <summary>
    /// 移動処理
    /// </summary>
    void Move()
    {
        if (isDead)
            return;

        velocity = rigidbody.velocity;
        velocity.x = GetVelocity().x * nowSpeed;
        rigidbody.velocity = velocity;
        ChangeDirection();
    }
    
    /// <summary>
    /// 移動量取得
    /// </summary>
    /// <returns></returns>
    Vector2 GetVelocity()
    {
        Vector2 velocity = new Vector2(GetVelocityX(), 0);//自由に移動できるのは左右のみ
        return velocity;
    }
    /// <summary>
    /// 横移動量取得
    /// </summary>
    /// <returns></returns>
    float GetVelocityX()
    {
        float x = Input.GetAxis("Horizontal");
        return x;
    }

    /// <summary>
    /// 方向変換
    /// </summary>
    void ChangeDirection()
    {
        if(GetVelocityX()>0)
        {
            direction = 1;
        }
        else if(GetVelocityX()<0)
        {
            direction = -1;
        }
        ChangeSpriteDirection();
    }
    /// <summary>
    /// プレイヤー画像の方向転換
    /// </summary>
    void ChangeSpriteDirection()
    {
        Vector3 scale = transform.localScale;
        scale.x = defaultScaleX *direction;
        transform.localScale = scale;
    }

    /// <summary>
    /// ジャンプ処理
    /// </summary>
    void Jump()
    {
        if (isDead)
            return;
        if (isJump)
            return;

        if(Input.GetButtonDown("Jump"))
        {
            rigidbody.AddForce(GetJumpPower(), ForceMode2D.Impulse);
            isJump = true;
        }
    }
    /// <summary>
    /// ジャンプ力をVector2で返す
    /// </summary>
    /// <returns></returns>
    Vector2 GetJumpPower()
    {
        return new Vector2(0, nowJumpPower);
    }

    /// <summary>
    /// 遠距離攻撃
    /// </summary>
    void Shot()
    {
        if (isDead)
            return;
        if(Input.GetButtonDown("Shot"))
        {
            CreateBullet();
        }
    }

    /// <summary>
    /// 弾生成処理
    /// </summary>
    void CreateBullet()
    {
        bullet.speed = direction * bulletSpeed;
        Instantiate(bullet, transform.position, transform.rotation);
    }

    /// <summary>
    /// 近距離攻撃
    /// </summary>
    void Attack()
    {
        if (isDead)
            return;
        if(Input.GetButtonDown("Attack")&&attackTimer<=0)
        {
            attackField.endTime = attackTime;
            Vector3 distance = new Vector3(attackDistance * direction, 0, 0);
            var atkField = Instantiate(attackField, transform.position + distance, transform.rotation);
            atkField.transform.parent = transform;

            transform.GetComponent<SpriteRenderer>().sprite = attackSprite;
            attackTimer = attackTime;
        }
    }
    /// <summary>
    /// 攻撃時間用タイマー更新
    /// </summary>
    void AttackTimerCount()
    {
        if(attackTimer>0)
        {
            attackTimer -= Time.deltaTime;
        }
        if(attackTimer<=0)
        {
            transform.GetComponent<SpriteRenderer>().sprite = baseSprite;
        }
    }

    /// <summary>
    /// 体力増加
    /// </summary>
    /// <param name="num">増加量</param>
    public void AddHP(int num)
    {
        hp += num;

        HPLimit();
        HPGaugeVary();
        //hpText.text = hp + "";
    }
    /// <summary>
    /// 体力設定
    /// </summary>
    /// <param name="num">体力</param>
    public void SetHP(int num)
    {
        hp = num;

        HPLimit();
        HPGaugeVary();
        //hpText.text = hp + "";
    }
    /// <summary>
    /// 体力の範囲制限
    /// </summary>
    void HPLimit()
    {
        if(hp>100)
        {
            hp = 100;
        }
        if(hp<=0)
        {
            hp = 0;
            isDead = true;
        }
    }
    /// <summary>
    /// 体力ゲージの長さを数値に対応させる
    /// </summary>
    void HPGaugeVary()
    {
        hpGauge.fillAmount = hp/100f;
    }

    /// <summary>
    /// 影響力増加
    /// </summary>
    /// <param name="num">増加量</param>
    public void AddInfluencePoint(float num)
    {
        influencePoint += num;

        InfluencePointLimit();
        InfluencePointGaugeVary();
        //influenceText.text = influencePoint + "";
    }
    /// <summary>
    /// 影響力設定
    /// </summary>
    /// <param name="num">影響力</param>
    public void SetInfluencePoint(float num)
    {
        influencePoint = num;

        InfluencePointLimit();
        InfluencePointGaugeVary();
        //influenceText.text = influencePoint + "";
    }
    /// <summary>
    /// 影響力の範囲制限
    /// </summary>
    void InfluencePointLimit()
    {
        if (influencePoint > 100)
            SetInfluencePoint(100);
        if (influencePoint < 0)
            SetInfluencePoint(0);
    }
    /// <summary>
    /// 影響力のゲージの長さを数値に対応させる
    /// </summary>
    void InfluencePointGaugeVary()
    {
        influenceGauge.fillAmount = influencePoint / 100f;
    }

    /// <summary>
    /// 影響力の秒間回復
    /// </summary>
    void InfluencePointUpdate()
    {
        if (isDead)
            return;
        if (influencePoint >= 100)
            return;

        AddInfluencePoint((influenceRecoveryAmount * Time.deltaTime));
        if(influencePoint>100)
        {
            SetInfluencePoint(100);
        }
    }

    /// <summary>
    /// 影響力攻撃
    /// </summary>
    void InfluenceAttack()
    {
        if (isDead)
            return;
        if (influencePoint < 50)//影響力が50未満なら使えない
            return;

        if(Input.GetButtonDown("InflenceAttack"))
        {
            if (!influenceMode)//全体攻撃
            { }
            else//バフ
            {
                InfluenceBuff();
            }
        }
    }
    /// <summary>
    /// 影響力によるバフ
    /// </summary>
    void InfluenceBuff()
    {
        if (isBuff)
            return;

        nowSpeed = buffSpeed;
        nowJumpPower = buffJumpPower;
        AddInfluencePoint(-50);
        isBuff = true;
    }

    /// <summary>
    /// バフがかかっている時の処理
    /// </summary>
    void BuffTimeUpdate()
    {
        if (isDead)
            return;
        if (!isBuff)
            return;

        AddInfluencePoint((-influenceRecoveryAmount - buffTimeRemoveInfluencePoint) * Time.deltaTime);
        if(influencePoint<=0)
        {
            buffTimeEnd();
        }
    }

    /// <summary>
    /// バフを解除する時の処理
    /// </summary>
    void buffTimeEnd()
    {
        isBuff = false;
        nowSpeed = speed;
        nowJumpPower = jumpPower;
    }

    /// <summary>
    /// 死んでいるかどうか
    /// </summary>
    /// <returns></returns>
    public bool IsDead()
    {
        return isDead;
    }



    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Floor")
        {
            isJump = false;
        }
        if(col.gameObject.tag == "GameOverArea")
        {
            SetHP(0);
        }

        if(col.gameObject.tag == "EnemyBullet")
        {
            col.gameObject.GetComponent<EnemyBulletBase>().AddDamage();
        }
    }
}
