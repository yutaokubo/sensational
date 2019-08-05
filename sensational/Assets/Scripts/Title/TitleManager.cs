using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

public class TitleManager : MonoBehaviour {

    //用途：タイトルシーン全体の管理
    //作成者：大久保
    //最終更新日：2019/08/05
    //最終更新者：大久保
    //更新内容：カーソルを動かせるように

    private int gameMode;//影響力選択用の変数

    [SerializeField]
    private Cursor cursor;//カーソル
    

    // Use this for initialization
    void Start () {
        gameMode = 1;
	}
	
	// Update is called once per frame
	void Update () {
        ModeSelect();
        GameStart();
	}

    /// <summary>
    /// 影響力選択の制限
    /// </summary>
    void GameModeLimit()
    {
        if (gameMode < 1)
            gameMode = 1;
        if (gameMode > 2)
            gameMode = 2;
    }
    /// <summary>
    /// 影響力選択用の変数を取得
    /// </summary>
    /// <returns></returns>
    public int GameModeNumber()
    {
        GameModeLimit();
        return gameMode;
    }
    /// <summary>
    /// 影響力選択処理
    /// </summary>
    void ModeSelect()
    {
        if (Input.GetAxisRaw("Vertical") == 1)
        {
            gameMode -= 1;
            GameModeLimit();
            cursor.CursorMove(gameMode);
        }
        else if (Input.GetAxisRaw("Vertical") == -1)
        {
            gameMode += 1;
            GameModeLimit();
            cursor.CursorMove(gameMode);
        }
    }
    /// <summary>
    /// 影響力確定
    /// </summary>
    void ModeDecision()
    {
        GameModeLimit();
        if(gameMode == 1)
        {
            Player.influenceMode = false;
        }
        else
        {
            Player.influenceMode = true;
        }
    }

    /// <summary>
    /// ゲームスタート処理
    /// </summary>
    void GameStart()
    {
        if (Input.GetButtonDown("Jump"))
        {
            ModeDecision();
            EditorSceneManager.LoadScene("SampleScene");
        }
    }
}
