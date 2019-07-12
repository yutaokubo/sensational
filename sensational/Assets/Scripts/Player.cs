using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    //用途：プレイヤー全般のスクリプト
    //作成者：大久保
    //最終更新日：2019/07/12
    //最終更新者：大久保



    [Header("プレイヤーの移動速度")]
    [SerializeField]
    private float Speed;
    private Vector3 velocity;//移動量

    [Header("プレイヤーのジャンプ力")]
    [SerializeField]
    private float jumpPower;
    private bool isJump;//ジャンプ中か

    private Rigidbody2D rigidbody;

	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody2D>();//自身のRigidbody2Dを取得
        isJump = false;//ジャンプはしていないことに
    }
	
	// Update is called once per frame
	void Update () {

        Move();
        Jump();
	}

    /// <summary>
    /// 移動処理
    /// </summary>
    void Move()
    {
        velocity = GetVelocity();
        transform.position += velocity * Speed*Time.deltaTime;
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

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Floor")
        {
            isJump = false;
        }
    }
}
