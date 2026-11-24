using System;
using UnityEngine;

public class RoomManager : MonoBehaviour
{

    public static RoomManager Instance { get; private set; }

    public bool canAttack = false;
    public bool canGlide = false;
    public bool canWallJump = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);

    }
}
