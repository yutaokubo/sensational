using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopBossBullet : EnemyBulletBase
{

    [Header("画面端ループ回数"),SerializeField]
    private int loopTimes = 1;
    [Header("ループ端の位置調整（0で画面端）"), SerializeField]
    private float plusEdge = 0;

    private int loopNum = 0;

    private void Start() 
	{
        loopNum = 0;
    }

    private void Update () 
	{
        velocity = new Vector3(speed, 0, 0);
        Move();
        OneLoopMove();
    }

	//画面端ループ移動
	private void OneLoopMove()
	{
        if (transform.position.x > GetCameraEdgeBottomRight().x + plusEdge)
        {
            transform.position = new Vector3(GetCameraEdgeTopLeft().x, transform.position.y, transform.position.z);
            loopNum++;
        }
		else if(transform.position.x<GetCameraEdgeTopLeft().x - plusEdge)
		{
            transform.position = new Vector3(GetCameraEdgeBottomRight().x, transform.position.y, transform.position.z);
            loopNum++;
        }
		if(loopNum>loopTimes)
		{
            Destroy(gameObject);
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
