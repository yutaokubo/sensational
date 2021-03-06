﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : SingletonMono<PlayerManager>
{

    //用途：プレイヤーの管理
    //作成者：大久保
    //最終更新日：2019/08/05
    //最終更新者：大久保
    //更新内容：プレイヤーが死んでいるか常に確認
    //更新日：2019/08/05
    //更新者:村上
    //更新内容:getterの追加,Singleton化


    [SerializeField]
    private Player player;
    //getter
    public Player PlayerInstance
    {
        get{ return player; }
    }

    private bool isPlayerDead;//プレイヤーが死んでいるかどうか


    // Use this for initialization
    void Start () {
        isPlayerDead = false;//プレイヤーは死んでいない
	}
	
	// Update is called once per frame
	void Update () {
        ChackPlayerDead();
	}

    /// <summary>
    /// プレイヤーが死んでいるか確認
    /// </summary>
    void ChackPlayerDead()
    {
        if (isPlayerDead)
            return;
        if (player.IsDead())
            isPlayerDead = true;
    }

    /// <summary>
    /// プレイヤーが死んでいるかどうか
    /// </summary>
    /// <returns></returns>
    public bool IsPlayerDead()
    {
        return isPlayerDead;
    }
}
