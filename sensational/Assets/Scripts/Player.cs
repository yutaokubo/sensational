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

    private Rigidbody2D rigidbody;

	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody2D>();//自身のRigidbody2Dを取得
    }
	
	// Update is called once per frame
	void Update () {

        Move();
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

}
