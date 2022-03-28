using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CreateVertexOnClick : MonoBehaviour
{
    GameObject gameManager;
    GameManager gm;

    public GameObject vertexPrefab;

    Ray ray;
    RaycastHit hit;

    // Use this for initialization
    void Start()
    {
        // Przypisanie GameManager
        gameManager = GameObject.Find("GameManager");
        gm = gameManager.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Je¿eli zosta³ wciœniêty przycisk "Dodaj wierzcho³ek"
        if (gm.AddVertexState)
        {
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
           
            // Je¿eli zosta³ wciœniêty lewy przycisk myszy
            if (Input.GetMouseButtonDown(0))
            {

                // Je¿eli w miejscu klikniêcia nie ma innego obiektu (w tym przypadku przycisków interfejsu)
                if(!EventSystem.current.IsPointerOverGameObject())
                {
                    // Ustalenie pozycji tworzonego wierzcho³ka wzglêdem pozycji na ekranie
                    Vector3 wordPos;
                    ray = Camera.main.ScreenPointToRay(mousePos);
                    if (Physics.Raycast(ray, out hit, 1000f))
                    {
                        wordPos = hit.point;
                    }
                    else
                    {
                        wordPos = Camera.main.ScreenToWorldPoint(mousePos);
                    }

                    // Ustalenie nazwy wierzcho³ka (jego ID)
                    int index = gm.findLowestIDAvailable();

                    // Utworzenie wierzcho³ka i jego nazwy
                    GameObject newVertex = (GameObject)Instantiate(vertexPrefab, new Vector3(wordPos.x, wordPos.y, 0), Quaternion.identity);
                    newVertex.name = index.ToString();

                    // Znalezienie canvasa oraz tekstu przypisanego do utworzonego wierzcho³ka w celu wyœwietlenia jego ID na ekranie
                    GameObject canvasObject = newVertex.transform.GetChild(0).gameObject;
                    Canvas canvas = canvasObject.GetComponent<Canvas>();

                    GameObject textObject = canvas.transform.GetChild(0).gameObject;
                    TextMeshProUGUI text = textObject.GetComponent<TextMeshProUGUI>();

                    text.text = newVertex.name; 

                    // Ustawienie tekstu w zale¿noœci od rozdzielczoœci programu
                    float coefficientX = Screen.width / 1920;
                    float coefficientY = Screen.height / 1080;
                    text.transform.position = new Vector3(mousePos.x + (222 * coefficientX), mousePos.y - (15 * coefficientY), mousePos.z);

                    // Dodanie ID utworzonego wierzcho³ka do listy wszystkich wierzcho³ków
                    gm.idList.Add(index);
                }
            }
        }
    }
}