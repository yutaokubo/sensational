using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondBossEnemy : EnemyBase
{
    [SerializeField,Header("特殊技の発射タイミング")]
    private float shotTiming = 3;//弾のタイミング
    [SerializeField, Header("通常弾の発射タイミング")]
    private float normalShotTiming = 2.0f;
    [SerializeField, Header("近距離攻撃の間隔")]
    private float shortAttackTime = 1.3f;

    [SerializeField, Header("地面での移動速度")]
    private float moveSpeed = 1;


    [SerializeField,Header("特殊弾")]
    private EnemyBulletBase specialShot;
    [SerializeField, Header("特殊弾2")]
    private List<BulletStruct> specialShot_Two = new List<BulletStruct>();
    [SerializeField]
    private float yPos;

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
    private int nowShot = 0;
    //ループ
    private void Update()
    {
        if (time > shotTiming)
        {
            int num = Random.Range(3, 6);
			float distance = PlayerManager.Instance.PlayerInstance.transform.position.x - transform.position.x;

            float x = (GetCameraEdgeBottomRight().x - GetCameraEdgeTopLeft().x) / (num+1);
            if(distance> 0) Shot(specialShot_Two, transform.position, -1);
            else Shot(specialShot_Two, transform.position);
            for (int i = 0; i < num; i++)
            {
                Instantiate(specialShot, new Vector3(GetCameraEdgeTopLeft().x + x * (i+1) + Random.Range(-x+0.5f, x-0.5f), yPos, 0), Quaternion.identity);
            }
            time = 0;
        }
        time += Time.deltaTime;
        NormalMover();
    }
    private void Shot(List<BulletStruct> shot, Vector3? _plus = null,int _k = 1,float _random = 0 )
    {
        foreach (var _shot in shot)
        {
            float rand = Random.Range(-_random, _random);
			EnemyBulletBase shotInstance;
			if(_plus!=null)
			{
                shotInstance = Instantiate(_shot.bulletBase, new Vector3(_shot.position.x + _plus.Value.x, _shot.position.y + _plus.Value.y, 0 + _plus.Value.z), Quaternion.identity);
            }
			else
			{
                shotInstance = Instantiate(_shot.bulletBase, new Vector3(_shot.position.x, _shot.position.y, 0), Quaternion.identity);
            }
            shotInstance.speed *= _k;
        }
    }
    private void NormalMover()
    {
        float distance = PlayerManager.Instance.PlayerInstance.transform.position.x - transform.position.x;
        if (distance > 0f)
        {
            gameObject.transform.localScale = new Vector3(-Mathf.Abs(gameObject.transform.localScale.x), gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        }
        else if (distance < 0f)
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
    #region カメラ座標(端)取得
    private Vector2 GetCameraEdgeTopLeft()
    {
		//左上取得
        Vector2 topLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        topLeft.Scale(new Vector3(1f, -1f, 1f));
        return topLeft;
    }
    private Vector2 GetCameraEdgeBottomRight()
    {
        // 画面の右下を取得
        Vector3 bottomRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));
        // 上下反転させる
        bottomRight.Scale(new Vector3(1f, -1f, 1f));
        return bottomRight;
    }
    #endregion

}
