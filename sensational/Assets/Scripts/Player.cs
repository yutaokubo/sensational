﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    //用途：プレイヤー全般のスクリプト
    //作成者：大久保
    //最終更新日：2019/07/12
    //最終更新者：大久保
    //更新内容：近距離攻撃追加



    [Header("通常時の移動速度")]
    [SerializeField]
    private float speed;
    [Header("バフ後の移動速度")]
    [SerializeField]
    private float buffSpeed;
    private float nowSpeed;//現在の移動速度
    private Vector3 velocity;//移動量
    private float direction;//方向。弾の発射方向を決める

    [Header("通常時のジャンプ力")]
    [SerializeField]
    private float jumpPower;
    [Header("バフ後のジャンプ力")]
    [SerializeField]
    private float byffJumpPower;
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

    private int hp;//体力
    [SerializeField]
    private Text hpText;

    private float influencePoint;//影響力
    [SerializeField]
    private Text inflenceText;

    [Header("秒間影響力回復量")]
    [SerializeField]
    private int inflenceRecoveryAmount;

    [Header("影響力のモード、Falseが全体攻撃,trueがバフ")]
    [SerializeField]
    private static bool influenceMode;


    private Rigidbody2D rigidbody;

	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody2D>();//自身のRigidbody2Dを取得
        isJump = false;//ジャンプはしていないことに
        direction = 1;//方向は右向き
        SetHP(100);//体力は100に
        SetInflencePoint(100);//影響力は100に
        influenceMode = true;//影響力はバフに
        nowSpeed = speed;//スピードは通常時のものに
        nowJumpPower = jumpPower;
    }
	
	// Update is called once per frame
	void Update () {

        Move();
        Jump();
        Shot();
        Attack();
        InflenceAttack();
        InflencePointUpdate();
	}

    /// <summary>
    /// 移動処理
    /// </summary>
    void Move()
    {
        velocity = rigidbody.velocity;
        velocity.x = GetVelocity().x * nowSpeed;
        //velocity = GetVelocity();
        //transform.position += velocity * speed*Time.deltaTime;
        rigidbody.velocity = velocity;
        ChangeDirection();
        //rigidbody.velocity = velocity * Speed;
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
    }

    /// <summary>
    /// ジャンプ処理
    /// </summary>
    void Jump()
    {
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
        if(Input.GetButtonDown("Attack"))
        {
            attackField.endTime = attackTime;
            Vector3 distance = new Vector3(attackDistance * direction, 0, 0);
            var atkField = Instantiate(attackField, transform.position + distance, transform.rotation);
            atkField.transform.parent = transform;
        }
    }

    /// <summary>
    /// 体力増加
    /// </summary>
    /// <param name="num">増加量</param>
    public void AddHP(int num)
    {
        hp += num;
        hpText.text = hp+""; 
    }
    /// <summary>
    /// 体力設定
    /// </summary>
    /// <param name="num">体力</param>
    public void SetHP(int num)
    {
        hp = num;
        hpText.text = hp + "";
    }

    /// <summary>
    /// 影響力増加
    /// </summary>
    /// <param name="num">増加量</param>
    public void AddInflencePoint(float num)
    {
        influencePoint += num;
        inflenceText.text = influencePoint + "";

        if (influencePoint > 100)
            SetInflencePoint(100);
        if (influencePoint < 0)
            SetInflencePoint(0);
    }
    /// <summary>
    /// 影響力設定
    /// </summary>
    /// <param name="num">影響力</param>
    public void SetInflencePoint(float num)
    {
        influencePoint = num;
        inflenceText.text = influencePoint + "";

        if (influencePoint > 100)
            SetInflencePoint(100);
        if (influencePoint < 0)
            SetInflencePoint(0);
    }

    /// <summary>
    /// 影響力の秒間回復
    /// </summary>
    void InflencePointUpdate()
    {
        if (influencePoint >= 100)
            return;

        AddInflencePoint((inflenceRecoveryAmount * Time.deltaTime));
        if(influencePoint>100)
        {
            SetInflencePoint(100);
        }
    }

    /// <summary>
    /// 影響力攻撃
    /// </summary>
    void InflenceAttack()
    {
        if (influencePoint < 50)//影響力が50未満なら使えない
            return;

        if(Input.GetButtonDown("InflenceAttack"))
        {
            if (!influenceMode)//全体攻撃
            { }
            else//バフ
            {
                InflenceBuff();
            }
        }
    }
    /// <summary>
    /// 影響力によるバフ
    /// </summary>
    void InflenceBuff()
    {
        nowSpeed = buffSpeed;
        nowJumpPower = jumpPower;
        AddInflencePoint(-50);
    }



    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Floor")
        {
            isJump = false;
        }
    }
}
