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
        // Je�eli zosta� wci�ni�ty przycisk od usuwania wierzcho�k�w
        if (gm.RemoveVertexState)
        {
            // Usu� naci�ni�ty wierzcho�ek
            Destroy(gameObject);
        }
    }
}
