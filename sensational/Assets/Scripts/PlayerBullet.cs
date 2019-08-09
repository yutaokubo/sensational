using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBullet : MonoBehaviour {

    //用途：プレイヤーの弾のスクリプト
    //作成者：大久保
    //最終更新日：2019/08/10
    //最終更新者：大久保
    //更新内容：壁に当たって消えるように


    public float speed;//進む速度
    public int power;//攻撃力
    private Vector3 velocity;//移動量

	// Use this for initialization
	void Start () {
		
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
        velocity = new Vector3(speed, 0, 0);
        transform.position += velocity * Time.deltaTime;
    }

    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Floor")
        {
            Destroy(gameObject);
        }
    }
}
