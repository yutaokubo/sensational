using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroll : MonoBehaviour {

    private Player player;

    [SerializeField, Header("カメラの高さ")]
    private float y;

    [SerializeField,Header("左側のスクロール制限")]
    private float leftLimit;
    [SerializeField, Header("右側のスクロール制限")]
    private float rightLimit;

	// Use this for initialization
	void Start () {
        player = PlayerManager.Instance.PlayerInstance;
	}
	
	// Update is called once per frame
	void Update () {
        Scroll();
        Limit();
	}

    void Scroll()
    {
        transform.position = new Vector3(player.transform.position.x, y, -10);
    }

    void Limit()
    {
        if(transform.position.x<=leftLimit)
        {
            transform.position = new Vector3(leftLimit, y, -10);
        }
        if(transform.position.x >=rightLimit)
        {
            transform.position = new Vector3(rightLimit, y, -10);
        }
    }
}
