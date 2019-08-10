using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        Invoke("ReturnTitle", 1f);
    }

    void ReturnTitle()
    {
        SceneManager.LoadScene("Title");
    }

    public void ChangeScene()
    {
        if (isGameOver)
            return;

        string sceneName = SceneManager.GetActiveScene().name;

        switch(sceneName)
        {
            case "Stage1":
                SceneManager.LoadScene("Boss1");
                break;
            case "Boss1":
                SceneManager.LoadScene("Stage2");
                break;
            case "Stage2":
                SceneManager.LoadScene("Boss2");
                break;
            case "Boss2":
                SceneManager.LoadScene("Stage3");
                break;
            case "Stage3":
                SceneManager.LoadScene("Boss3");
                break;
            case "Boss3":
                SceneManager.LoadScene("Title");
                break;
        }
    }
}
