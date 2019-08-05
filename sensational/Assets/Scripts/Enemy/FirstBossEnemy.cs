using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ボス1

public class FirstBossEnemy : EnemyBase
{
    [SerializeField]
    private float shotTiming = 1;//弾のタイミング
    [SerializeField]
    private List<BulletStruct> firstShot = new List<BulletStruct>();//ショット1のリスト

    private float time = 0;//内部時間

    //初期化
    protected override void Start()
    {
        base.Start();
        time = 0;
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
                if(transform.position.x<PlayerManager.Instance.PlayerInstance.transform.position.x)
                {
                    shotInstance.speed *= -1;
                }
            }
            time = 0;
        }
        time += Time.deltaTime;
    }
}
