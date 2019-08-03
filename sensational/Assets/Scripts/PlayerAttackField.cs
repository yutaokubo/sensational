﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackField : MonoBehaviour {

    //用途：プレイヤーの攻撃範囲
    //作成者：大久保
    //最終更新日：2019/07/12
    //最終更新者：大久保
    //更新内容：作成

    public float endTime;//判定が消えるまでの秒数
    private float endTimer;//判定が消えるまでを計るタイマー


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        EndTimerCount();
	}

    void EndTimerCount()
    {
        endTimer += 1 * Time.deltaTime;
        if(endTimer>=endTime)
        {
            Destroy(this.gameObject);
        }
    }
}