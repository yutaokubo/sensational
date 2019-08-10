using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossClearChacker : MonoBehaviour {

    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private GameObject Boss;

    private bool isClear;

	// Use this for initialization
	void Start () {
        isClear = false;
	}
	
	// Update is called once per frame
	void Update () {
        Chack();
	}

    void Chack()
    {
        if (isClear)
            return;

        if(Boss == null)
        {
            isClear = true;
            Invoke("SceneChange", 1);
        }
    }

    void SceneChange()
    {
        gameManager.ChangeScene();
    }
}
