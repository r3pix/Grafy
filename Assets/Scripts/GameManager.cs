using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    bool addVertexState = false;
    bool removeVertexState = false;
    bool addEdgeState = false;
    bool removeEdgeState = false;
    bool isWritten = false;
    bool moveVertexState = false;
    //bool findFireStationLocationState = false;

    public List<int> idList;
    public List<Edge> edgeList;

    Vector3 mousePos;
    Vector2 mousePos2D;
    RaycastHit2D hit;

    public int recentID = 1;
    public int newID = -1;
    public string distanceField;

    public GameObject canvas;
    public GameObject edgePrefab;
    public GameObject inputField;
    public GameObject weightPrefab;
    public InputField field;
    public GameObject modal;
    public GameObject modalTextObject;
    public TextMeshProUGUI modalText;

    public GameObject objectv1;
    public InputField v1;
    public GameObject objectv2;
    public InputField v2;
    //public GameObject fireTruckPrefab;

    // Start is called before the first frame update
    void Start()
    {
        // Utworzenie pustych list ID oraz kraw�dzi oraz ustawienie zmiennych u�ywanych do tworzenia kraw�dzi
        idList = new List<int>();
        edgeList = new List<Edge>();
        recentID = -1;
        newID = -1;

        inputField = GameObject.Find("InputField");
        field = inputField.GetComponent<InputField>();
        inputField.transform.position = new Vector3(inputField.transform.position.x, inputField.transform.position.y, -9);
        inputField.SetActive(false);

        modal= GameObject.Find("Modal");
        modalTextObject = modal.transform.Find("Result").gameObject;
        modalText = modalTextObject.GetComponent<TextMeshProUGUI>();
        modal.SetActive(false);

        objectv1 = GameObject.Find("V1");
        v1 = objectv1.GetComponent<InputField>();
        //  inputField.transform.position = new Vector3(inputField.transform.position.x, inputField.transform.position.y, -9);
        objectv1.SetActive(false);

        objectv2 = GameObject.Find("V2");
        v2 = objectv2.GetComponent<InputField>();
        // inputField.transform.position = new Vector3(inputField.transform.position.x, inputField.transform.position.y, -9);
        objectv2.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        // Je�eli zost�� wcisni�ty przycisk do dodawania kraw�dzi
        if (addEdgeState)
        {
            // Je�eli zosta� wci�ni�ty lewy przycisk myszy
            if (Input.GetMouseButtonDown(0))
            {
                // Sprawdzenie, czy jaki� wierzcho�ek jest naciskany
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos2D = new Vector2(mousePos.x, mousePos.y);
                hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
                if (hit.collider != null)
                {
                    // Zmienne recentID oraz newID to zmienne, kt�re okre�laj� wci�ni�ty wierzcho�ek
                    // - newID to w�a�nie naci�ni�ty, a recentID - poprzedni
                    // - w zale�no�ci od ich stanu albo tworzona jest kraw�d�, albo zmienne te s� tak ustawiane,
                    // aby by�a mo�liwo�� utworzenia innej kraw�dzi
                    newID = System.Convert.ToInt32(hit.collider.gameObject.name);

                    if (recentID == -1)
                    {
                        recentID = newID;
                        newID = -1;
                    }
                    else if(newID == recentID)
                    {
                        newID = -1;
                    }
                    else
                    {
                        DisplayTextField();
                        
                    }
                }
            }
        }
        else
        {
            inputField.SetActive(false);
        }
    }

    public void DisplayTextField()
    {
        inputField.SetActive(true);
        field.Select();
        field.ActivateInputField();
    }

    public void HideTextField()
    {
        inputField.SetActive(false);
    }

    public void CreateEdge(int id1, int id2)
    {
        GameObject weight1 = GameObject.Find("wEdge " + System.Convert.ToString(id1) + "+" + System.Convert.ToString(id2));
        GameObject weight2 = GameObject.Find("wEdge " + System.Convert.ToString(id2) + "+" + System.Convert.ToString(id1));
      
        // Utworzenie nowej kraw�dzi o okre�lonej nazwie "Edge x+y" (potrzebne w edytorze) + dodac id krawedzi

        string edgeName = "Edge " + System.Convert.ToString(id1) + "+" + System.Convert.ToString(id2);//+edgeList.Count.ToString();
        GameObject newObject = (GameObject)Instantiate(edgePrefab, new Vector3(0, 0, 1), Quaternion.identity);
        newObject.name = edgeName+edgeList.Count;
        // Dodanie po��czenia do listy kraw�dzi
        
        if(weight1==null && weight2 == null) //krawedzie musza miecc unikalne nazwy, nazwy wag sa wspoldzielone (jesli nazwy nie sa unikalne to nie beda sie przesuwac krawedzie)
        {
            edgeList.Add(new Edge(id2, id1, GameObject.Find(System.Convert.ToString(id2)), GameObject.Find(System.Convert.ToString(id1)), distanceField, "w" + edgeName, edgeName + edgeList.Count));
           
        }
        else if (weight1 == null)
        {
            edgeList.Add(new Edge(id2, id1, GameObject.Find(System.Convert.ToString(id2)), GameObject.Find(System.Convert.ToString(id1)), distanceField, "wEdge " + System.Convert.ToString(id2) + "+" + System.Convert.ToString(id1), edgeName + edgeList.Count));
            
        }
        else if (weight2 == null)
        {
            edgeList.Add(new Edge(id2, id1, GameObject.Find(System.Convert.ToString(id2)), GameObject.Find(System.Convert.ToString(id1)), distanceField, "wEdge " + System.Convert.ToString(id1) + "+" + System.Convert.ToString(id2), edgeName + edgeList.Count));
            
        }
   
        // Utworzenie wizualnej kraw�dzi - oczywi�cie tylko do wy�wietlania, gdy�
        // sama kraw�d� jest ju� zapisana w li�cie kraw�dzi
        LineRenderer lineRenderer = newObject.GetComponent<LineRenderer>();
        var recentPos = GameObject.Find(System.Convert.ToString(id1));
        var newPos = GameObject.Find(System.Convert.ToString(id2));

        recentPos.transform.position = new Vector3(recentPos.transform.position.x, recentPos.transform.position.y, -1);
        newPos.transform.position = new Vector3(newPos.transform.position.x, newPos.transform.position.y, -1);

        Vector3[] arr = { recentPos.transform.position, newPos.transform.position };

        GameObject newObject2;

        if (weight1==null && weight2 == null)
        {
            newObject2 = Instantiate(weightPrefab, new Vector3(0, 0, 1), Quaternion.identity);
            newObject2.name = "w" + edgeName;
            newObject2.transform.SetParent(canvas.transform);

            TextMeshProUGUI text = newObject2.GetComponent<TextMeshProUGUI>();
            text.text = distanceField;
            text.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            text.transform.position = new Vector3((recentPos.transform.position.x + newPos.transform.position.x + 2.115f) / 2, (recentPos.transform.position.y + newPos.transform.position.y) / 2, -1);
        }
        else if(weight2==null)
        {
            newObject2 = weight1;
            TextMeshProUGUI text = newObject2.GetComponent<TextMeshProUGUI>();
            text.text = text.text + ", " + distanceField;
        }
        else if(weight1 == null)
        {
            newObject2 = weight2;
            TextMeshProUGUI text = newObject2.GetComponent<TextMeshProUGUI>();
            text.text = text.text+", "+distanceField;
        }
        lineRenderer.SetPositions(arr);
        lineRenderer.startColor = Color.black;
        lineRenderer.endColor = Color.black;

    }

    // Funkcja zwracaj�ca indeks z listy kraw�dzi tej kraw�dzi, kt�ra jest mi�dzy dwoma wierzcho�kami
    // je�eli nie istnieje po��czenie, to zwraca -1
    public int returnIndexOfEdge(int m, int n)
    {
        for (int i = 0; i < edgeList.Count; i++)
        {
            if (edgeList[i].Vertex1 == m && edgeList[i].Vertex2 == n || edgeList[i].Vertex1 == n && edgeList[i].Vertex2 == m)
            {
                return i;
            }
        }
        return -1;
    }

    

    // Znalezienie najmniejszego wolnego ID, kt�re mo�na potem przypisa� do wierzcho�ka
    public int findLowestIDAvailable()
    {
        GameObject[] vertices = GameObject.FindGameObjectsWithTag("vertex");
        List<int> newIDList = new List<int>();
        foreach(GameObject vertex in vertices)
        {
            newIDList.Add(System.Convert.ToInt32(vertex.name));
        }
        if (newIDList.Count == 0)
        {
            idList = newIDList;
            return 1;
        }
        bool isThere;
        int value = 1;
        for(int i=1; i<int.MaxValue; i++)
        {
            isThere = false;
            foreach(int j in newIDList)
            {
                if (i == j)
                    isThere = true;
            }
            if (!isThere)
            {
                value = i;
                break;
            }
        }
        idList = newIDList;
        return value;
    }

    // D�ugo�� listy kraw�dzi, u�ywane do tworzenia macierzy incyencji �siedztwa
    public int EdgeListLength
    {
        get { return edgeList.Count; }
    }

    public List<Edge> GetEdgeList
    {
        get {return edgeList; }
    }


    // Usuwanie wszystkich wierzcho�k�w
    public void removeAllVertices()
    {
        addVertexState = false;
        removeVertexState = false;
        addEdgeState = false;
        removeEdgeState = false;
        //findFireStationLocationState = false;
        GameObject[] vertices = GameObject.FindGameObjectsWithTag("vertex");
        foreach (GameObject vertex in vertices)
            GameObject.Destroy(vertex);
        idList = new List<int>();
        removeAllEdges();
    }

    // Usuwanie wszystkich kraw�dzi
    public void removeAllEdges()
    {
        addVertexState = false;
        removeVertexState = false;
        addEdgeState = false;
        removeEdgeState = false;
        //findFireStationLocationState = false;
        GameObject[] edges = GameObject.FindGameObjectsWithTag("edge");
        foreach (GameObject edge in edges)
            GameObject.Destroy(edge);
        edgeList = new List<Edge>();

        GameObject[] edgess = GameObject.FindGameObjectsWithTag("weightPrefab");
        foreach (GameObject edge in edgess)
            GameObject.Destroy(edge);

    }

 
    // Wyj�cie z aplikacji
    public void QuitApp()
    {
        Application.Quit();
    }

    // Reset aplikacji
    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Metody u�ywane do sprawdzania, czy dany przycisk zosta� wci�ni�ty
    // -------------------------------------------------------------------------------
    public void changeAddVertexState()
    {
        addVertexState = !addVertexState;
        removeVertexState = false;
        addEdgeState = false;
        removeEdgeState = false;
        moveVertexState = false;
        //findFireStationLocationState = false;
    }
    public void changeMoveVertexState()
    {
        addVertexState = false;
        removeVertexState = false;
        addEdgeState = false;
        removeEdgeState = false;
        moveVertexState = !moveVertexState;
        Debug.Log(moveVertexState);
        //findFireStationLocationState = false;
    }

    public bool MoveVertexState
    {
        get { return moveVertexState;  }
    }
    public bool AddVertexState
    {
        get { return addVertexState; }
    }
    public void changeRemoveVertexState()
    {
        addVertexState = false;
        removeVertexState = !removeVertexState;
        addEdgeState = false;
        removeEdgeState = false;
        moveVertexState = false;
        //findFireStationLocationState = false;
    }
    public bool RemoveVertexState
    {
        get { return removeVertexState; }
    }
    public void changeAddEdgeState()
    {
        addVertexState = false;
        removeVertexState = false;
        addEdgeState = !addEdgeState;
        removeEdgeState = false;
        moveVertexState = false;
        //findFireStationLocationState = false;
    }
    public bool AddEdgeState
    {
        get { return addEdgeState; }
    }
    public void changeRemoveEdgeState()
    {

        addVertexState = false;
        removeVertexState = false;
        addEdgeState = false;
        removeEdgeState = !removeEdgeState;
        moveVertexState = false;
        //findFireStationLocationState = false;
    }
    public bool RemoveEdgeState
    {
        get { return removeEdgeState; }
    }
    /*
    public void changeFindFireStationState()
    {
        addVertexState = false;
        removeVertexState = false;
        addEdgeState = false;
        removeEdgeState = false;
        findFireStationLocationState = true;
    }
    public bool FindFireStationLocation
    {
        get { return findFireStationLocationState; }
    }*/
    // -------------------------------------------------------------------------------
    public void ReadStringInput(string inputText)
    {
        distanceField = field.text;
        Debug.Log(distanceField);
        CreateEdge(recentID, newID);

        // Odpowiednie ustawienie zmiennych recentID oraz newID, aby da�o si� utworzy� kolejne kraw�dzie
        recentID = -1;
        newID = -1;
        HideTextField();
    }
}

// Klasa u�ywana do tworzenia i przechowywania kraw�dzi
public class Edge
{
    int vertex1;
    int vertex2;
    float distance;
    string name;
    float textDistance;
    string weightName;

    public Edge(int vertex1, int vertex2, GameObject v1, GameObject v2, string distanceField, string weightName, string name)
    {
        this.vertex1 = vertex1;
        this.vertex2 = vertex2;
        distance = calculateDistance(v1, v2);
        textDistance = float.Parse(distanceField);
        this.weightName = weightName;
        this.name = name;
    }

    float calculateDistance(GameObject v1, GameObject v2)
    {
        return Mathf.Sqrt(Mathf.Pow(v2.transform.position.x - v1.transform.position.x, 2) + Mathf.Pow(v2.transform.position.y - v1.transform.position.y, 2));
    }

    public int Vertex1
    {
        get { return vertex1; }
    }

    public int Vertex2
    {
        get { return vertex2; }
    }

    public float TextDistance
    {
        get { return textDistance; }
    }

    public string Name
    {
        get { return name; }
    }

    public string WeightName
    {
        get { return weightName; }
    }

    public float Distance
    {
        get { return distance; }
    }

    public override string ToString()
    {
        return "v1: " + vertex1 + ", v2: " + vertex2 + ", dist: " + distance;
    }
}