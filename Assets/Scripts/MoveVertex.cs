using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class MoveVertex : MonoBehaviour
{
    GameObject gameManager;
    GameManager gm;
    Vector3 mousePos;
    Vector2 mousePos2D;
    RaycastHit2D hit;
    public GameObject vertexPrefab;
    public GameObject weightPrefab;
    public GameObject edgePrefab;
    Vector3 wordPos;

    Ray ray;
    RaycastHit raycastHit;

    public int newID = -1;
    // Start is called before the first frame update
    void Start()
    {
        // Przypisanie GameManager
        gameManager = GameObject.Find("GameManager");
        gm = gameManager.GetComponent<GameManager>();
        newID = -1;
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log(gm.MoveVertexState);

        if (gm.MoveVertexState)
        {
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
            if (newID == -1)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    //mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
                    mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mousePos2D = new Vector2(mousePos.x, mousePos.y);
                    hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

                    if (hit.collider != null)
                    {
                        newID = System.Convert.ToInt32(hit.collider.gameObject.name);
                    }
                }

            }
            else if (newID != -1)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
                    //mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mousePos2D = new Vector2(mousePos.x, mousePos.y);

                    ray = Camera.main.ScreenPointToRay(mousePos);
                    if (Physics.Raycast(ray, out raycastHit, 1000f))
                    {
                        wordPos = raycastHit.point;
                    }
                    else
                    {
                        wordPos = Camera.main.ScreenToWorldPoint(mousePos);
                    }

                    MoveEdge(newID);
                }
            }
        }
    }

    public void MoveEdge(int id)
    {
      
        // Znalezienie canvasa oraz tekstu przypisanego do utworzonego wierzcho³ka w celu wyœwietlenia jego ID na ekranie
        GameObject newVertex = GameObject.Find(id.ToString());
        Debug.Log(newVertex);
        GameObject canvasObject = newVertex.transform.GetChild(0).gameObject;
        Canvas canvas = canvasObject.GetComponent<Canvas>();

        newVertex.transform.position = new Vector3(wordPos.x, wordPos.y, 0);
        //canvas.transform.position = new Vector3(wordPos.x, wordPos.y, 0);

        mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
        GameObject textObject = canvas.transform.GetChild(0).gameObject;
        TextMeshProUGUI text = textObject.GetComponent<TextMeshProUGUI>();
        float coefficientX = Screen.width / 1920;
        float coefficientY = Screen.height / 1080;
        text.transform.position = new Vector3(mousePos.x + (222 * coefficientX), mousePos.y - (15 * coefficientY), mousePos.z);
        Debug.Log(text.transform.position);
        //text.transform.position = new Vector3(0,0,0);*/

        //calulate distance

        var list = gm.GetEdgeList;
          Debug.Log(list.Count);

       // var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.tag.Contains("egde")).ToList();
       // Debug.Log(objects.Count);
        
        foreach (var X in list)
        {
            if(X.Vertex1 == id || X.Vertex2 == id)
            {
                GameObject weight = GameObject.Find(X.WeightName);
                //if (weight != null)
                //    Destroy(weight);
               
                GameObject edge = GameObject.Find(X.Name); //dlatego nazwa krawedzi musi byc unikalna, inaczej przeniesie pierwszy lepszy o takiej nawziw ejesli jest wiele polaczen
               // if(edge!=null)
                     //Destroy(edge);
                
          
               // string edgeName = "Edge " + System.Convert.ToString(X.Vertex1) + "+" + System.Convert.ToString(X.Vertex2);
               // GameObject newObject = (GameObject)Instantiate(edgePrefab, new Vector3(0, 0, 1), Quaternion.identity);
               // newObject.name = X.Name;

                LineRenderer lineRenderer = edge.GetComponent<LineRenderer>();
                var recentPos = GameObject.Find(System.Convert.ToString(X.Vertex1));
                var newPos = GameObject.Find(System.Convert.ToString(X.Vertex2));

                recentPos.transform.position = new Vector3(recentPos.transform.position.x, recentPos.transform.position.y, -1);
                newPos.transform.position = new Vector3(newPos.transform.position.x, newPos.transform.position.y, -1);

                Vector3[] arr = { recentPos.transform.position, newPos.transform.position };

                
               // GameObject newObject2 = Instantiate(weightPrefab, new Vector3(0, 0, 1), Quaternion.identity);
                //newObject2.name = "w" + X.WeightName;
                //newObject2.transform.SetParent(canvas.transform);

               
                TextMeshProUGUI textt = weight.GetComponent<TextMeshProUGUI>();
                //textt.text = X.TextDistance.ToString();
                //Debug.Log(textt.text);
               // textt.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                textt.transform.position = new Vector3((recentPos.transform.position.x + newPos.transform.position.x + 2.115f) / 2, (recentPos.transform.position.y + newPos.transform.position.y) / 2, -1);
                
                
                //Debug.Log(textt.transform.position);
                
                lineRenderer.SetPositions(arr);
                lineRenderer.startColor = Color.black;
                lineRenderer.endColor = Color.black;
            }
        }
        newID = -1;
    }


}
