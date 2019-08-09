using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour {

    //用途：ホーミング弾を撃つ敵
    //作成者：大久保
    //最終更新日：2019/08/10
    //最終更新者：大久保
    //更新内容：作成

    [SerializeField, Header("体力")]
    private int hp;

    private bool isStartUp;//行動開始しているか

    [SerializeField, Header("発射する弾")]
    private EnemyBulletBase bullet;

    [SerializeField, Header("攻撃間隔")]
    private float shotInterval;
    private float shotTimer;//攻撃用タイマー

    private int direction;//方向判定用変数；
    private float defaultScaleX;//方向変化用

    
    [SerializeField, Header("攻撃時画像")]
    private Sprite attackSprite;
    private Sprite baseSprite;//通常時画像

    // Use this for initialization
    void Start () {
        isStartUp = false;//まだ動いていない
        direction = -1;//左向き
        defaultScaleX = transform.localScale.x;//方向変化用にスケール取得
        baseSprite = transform.GetComponent<SpriteRenderer>().sprite;//通常時画像取得
    }
	
	// Update is called once per frame
	void Update () {
        shotTimerUpdate();
        ChangeDirection();
	}

    /// <summary>
    /// 攻撃用カウント処理
    /// </summary>
    void shotTimerUpdate()
    {
        if (!isStartUp)
            return;

        shotTimer+=Time.deltaTime;
        if(shotTimer >=shotInterval)
        {
            shotTimer = 0;
            Attack();
        }
        if(shotTimer >= 0.5f)
        {
            transform.GetComponent<SpriteRenderer>().sprite = baseSprite;
        }
    }
    /// <summary>
    /// 弾発射
    /// </summary>
    void Attack()
    {
        Instantiate(bullet, transform.position, transform.rotation);

        transform.GetComponent<SpriteRenderer>().sprite = attackSprite;
    }
    
    /// <summary>
    /// ダメージ処理
    /// </summary>
    /// <param name="damage">受けるダメージ</param>
    void Damage(int damage)
    {
        hp -= damage;
        if(hp<=0)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 方向転換
    /// </summary>
    void ChangeDirection()
    {
        float distance = PlayerManager.Instance.PlayerInstance.transform.position.x - transform.position.x;
        if(distance>0)
        {
            direction = 1;
        }
        else if(distance<0)
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
        if(col.gameObject.tag == "PlayerAttackField")
        {
            Damage(col.gameObject.GetComponent<PlayerAttackField>().power);
        }
    }


}
