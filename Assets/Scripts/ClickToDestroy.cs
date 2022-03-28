using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToDestroy : MonoBehaviour
{
    GameObject gameManager;
    GameManager gm;

    void Start()
    {
        // Przypisanie GameManager
        gameManager = GameObject.Find("GameManager");
        gm = gameManager.GetComponent<GameManager>();
    }

    void OnMouseDown()
    {
        // Je¿eli zosta³ wciœniêty przycisk od usuwania wierzcho³ków
        if (gm.RemoveVertexState)
        {
            // Usuñ naciœniêty wierzcho³ek
            Destroy(gameObject);
        }
    }
}
