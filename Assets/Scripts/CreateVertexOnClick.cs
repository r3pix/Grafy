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
        // Je�eli zosta� wci�ni�ty przycisk "Dodaj wierzcho�ek"
        if (gm.AddVertexState)
        {
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
           
            // Je�eli zosta� wci�ni�ty lewy przycisk myszy
            if (Input.GetMouseButtonDown(0))
            {

                // Je�eli w miejscu klikni�cia nie ma innego obiektu (w tym przypadku przycisk�w interfejsu)
                if(!EventSystem.current.IsPointerOverGameObject())
                {
                    // Ustalenie pozycji tworzonego wierzcho�ka wzgl�dem pozycji na ekranie
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

                    // Ustalenie nazwy wierzcho�ka (jego ID)
                    int index = gm.findLowestIDAvailable();

                    // Utworzenie wierzcho�ka i jego nazwy
                    GameObject newVertex = (GameObject)Instantiate(vertexPrefab, new Vector3(wordPos.x, wordPos.y, 0), Quaternion.identity);
                    newVertex.name = index.ToString();

                    // Znalezienie canvasa oraz tekstu przypisanego do utworzonego wierzcho�ka w celu wy�wietlenia jego ID na ekranie
                    GameObject canvasObject = newVertex.transform.GetChild(0).gameObject;
                    Canvas canvas = canvasObject.GetComponent<Canvas>();

                    GameObject textObject = canvas.transform.GetChild(0).gameObject;
                    TextMeshProUGUI text = textObject.GetComponent<TextMeshProUGUI>();

                    text.text = newVertex.name; 

                    // Ustawienie tekstu w zale�no�ci od rozdzielczo�ci programu
                    float coefficientX = Screen.width / 1920;
                    float coefficientY = Screen.height / 1080;
                    text.transform.position = new Vector3(mousePos.x + (222 * coefficientX), mousePos.y - (15 * coefficientY), mousePos.z);

                    // Dodanie ID utworzonego wierzcho�ka do listy wszystkich wierzcho�k�w
                    gm.idList.Add(index);
                }
            }
        }
    }
}