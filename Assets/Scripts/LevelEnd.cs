using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour {

    private GameManager Gm;

    public string CommonGameObjectName;

    public float HorizontalDirectionToRayCast;
    public float VerticalDirectionToRayCast;

    public float RaycastDepth;

    public LayerMask PlayerLayer;

    private int currentLevel;
    private Vector2 position;
    private Vector2 direction;

    private void Awake()
    {
        
    }

    private void Start()
    {
        Gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        currentLevel = Gm.curLevel;
        position = new Vector2(GetComponent<Transform>().position.x, GetComponent<Transform>().position.y);
        direction = new Vector2(HorizontalDirectionToRayCast, VerticalDirectionToRayCast);
        direction.Normalize();
    }

    void Update()
    {
        //Debug.Log(Physics2D.Raycast(position, direction, RaycastDepth, PlayerLayer).collider);
        //if ( Physics2D.Raycast(position, direction, RaycastDepth, PlayerLayer).collider!=null){
        //    Debug.Log("Niisan");
        //    Gm.LevelEnded(currentLevel);
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( collision.name == "Player")
        {
            Gm.LevelEnded(currentLevel);
            Destroy(this);
        }
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

}
