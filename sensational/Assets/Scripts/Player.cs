using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    //用途：プレイヤー全般のスクリプト
    //作成者：大久保
    //最終更新日：2019/07/12
    //最終更新者：大久保
    //更新内容：近距離攻撃追加



    [Header("プレイヤーの移動速度")]
    [SerializeField]
    private float speed;
    private Vector3 velocity;//移動量
    private float direction;//方向。弾の発射方向を決める

    [Header("プレイヤーのジャンプ力")]
    [SerializeField]
    private float jumpPower;
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


    private Rigidbody2D rigidbody;

	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody2D>();//自身のRigidbody2Dを取得
        isJump = false;//ジャンプはしていないことに
        direction = 1;//方向は右向き
    }
	
	// Update is called once per frame
	void Update () {

        Move();
        Jump();
        Shot();
        Attack();
	}

    /// <summary>
    /// 移動処理
    /// </summary>
    void Move()
    {
        velocity = GetVelocity();
        transform.position += velocity * speed*Time.deltaTime;
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
        return new Vector2(0, jumpPower);
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



    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Floor")
        {
            isJump = false;
        }
    }
}
