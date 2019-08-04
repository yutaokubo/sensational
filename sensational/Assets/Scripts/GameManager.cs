using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    //用途：ゲームのメイン全体の管理
    //作成者：大久保
    //最終更新日：2019/08/05
    //最終更新者：大久保
    //更新内容：ゲームオーバーになっているか常にチェックするように


    [SerializeField]
    private PlayerManager playerManager;//プレイヤーマネージャー

    [SerializeField]
    private Text gameOverText;//ゲームオーバーのテキスト

    private bool isGameOver;//ゲームオーバーになっているか

    // Use this for initialization
    void Start () {
        isGameOver = false;
        gameOverText.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        ChackGameOver();
	}

    /// <summary>
    /// ゲームオーバー条件を満たすかどうか確認
    /// </summary>
    void ChackGameOver()
    {
        if (isGameOver)
            return;

        if(playerManager.IsPlayerDead())
        {
            GameOver();
        }
    }
    /// <summary>
    /// ゲームオーバーにする
    /// </summary>
    void GameOver()
    {
        isGameOver = true;
        gameOverText.enabled = true;
    }
}
