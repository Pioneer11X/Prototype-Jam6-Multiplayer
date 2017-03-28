using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour {

    public GameManager Gm;

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
        currentLevel = int.Parse( this.GetComponent<Transform>().parent.name.Replace(CommonGameObjectName, "") );
        position = new Vector2(GetComponent<Transform>().position.x, GetComponent<Transform>().position.y);
        direction = new Vector2(HorizontalDirectionToRayCast, VerticalDirectionToRayCast);
        direction.Normalize();
    }

    private void Update()
    {
        if ( Physics2D.Raycast(position, direction, RaycastDepth, PlayerLayer)){
            Gm.LevelEnded(currentLevel);
        }
    }

}
