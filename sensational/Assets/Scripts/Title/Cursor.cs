using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour {

    //用途：タイトルシーンのカーソルの動き
    //作成者：大久保
    //最終更新日：2019/08/05
    //最終更新者：大久保
    //更新内容：タイトルシーンでカーソルが動くように

    public List<GameObject> cursorPoint;//カーソルの位置一覧

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void CursorMove(int num)
    {
        if (num >= cursorPoint.Count)
            num = 2;

        transform.position = cursorPoint[num - 1].transform.position;
    }
}
