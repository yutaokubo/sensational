using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : SingletonMono<PlayerManager>
{
    [SerializeField]
    private Player mainPlayer;
    public Player MainPlayer
    {
        get { return mainPlayer; }
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
