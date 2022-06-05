using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class SaveGraph : MonoBehaviour
{

    public GameObject gameManager;
    public GameManager gm;

    //int lastrow
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        gm = gameManager.GetComponent<GameManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    public void checkIntegrity()
    {
        
        GameObject[] vertices = GameObject.FindGameObjectsWithTag("vertex");

        var edgeCount = gm.edgeList.Count;
        var edgeTab = gm.edgeList;
        var incidenceTab = new int[vertices.ToList().Count, edgeCount];

        for (int i = 0; i < edgeCount; i++)
        {
            incidenceTab[edgeTab[i].Vertex2 - 1, i] = int.Parse(edgeTab[i].TextDistance.ToString()); //poczatek krawedzi
            incidenceTab[edgeTab[i].Vertex1 - 1, i] = -int.Parse(edgeTab[i].TextDistance.ToString()); // koniec krawedzi
        }

        var jaggedTab = incidenceTab.ToJagged();

        bool[] result = new bool[vertices.ToList().Count];

        for(int i=0; i<vertices.ToList().Count; i++)
        {
            result[i] = IntegrityResult(vertices, edgeCount, jaggedTab,i);
        }

        if (checkResult(result))
        {
           // GameObject tempText = gm.modalText;

            //var text = tempText.GetComponent<TextMeshPro>();
            gm.modalText.text = "Spójny";

            gm.modal.SetActive(true);
        }
        else
        {
          //  GameObject tempText = gm.modalText;
            
           // var text = tempText.GetComponent<TextMeshPro>();
           // Debug.Log(text);
            gm.modalText.text = "Niespójny";

            gm.modal.SetActive(true);

        }
    }


    public bool IntegrityResult(GameObject[] vertices, int edgeCount, int[][]jaggedTab, int vertexId)
    {
        bool[] visited = new bool[vertices.ToList().Count];
        List<int> queue = new List<int>();

        queue.Add(vertexId);
        visited[vertexId] = true;

        while (queue.Count > 0)
        {
            var id = queue[0];
            var incidentIds = GetIncidentIds(vertices.ToList().Count, edgeCount, id, jaggedTab);
            foreach (var idToAdd in incidentIds)
            {
                if (visited[idToAdd] == false)
                {
                    queue.Add(idToAdd);
                    visited[idToAdd] = true;
                }
            }
            queue.Remove(id);

        }

        if (checkResult(visited))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool checkResult(bool[] result)
    {
        foreach (var res in result)
        {
            if(res == false)
            {
                return false;
            }
        }
        return true;
    }

    public List<int> GetIncidentIds(int verticesCount, int edgeCount, int currentId, int[][] jaggedTab)
    {
        var edgesToCheck = new List<int>();
        var childVerticesIds = new List<int>();
        
        for(int i=0; i<edgeCount; i++) // sprawdzenie ktore krawedzie zaczynaja sie w danym wierzcho³ku
        {
            if(jaggedTab[currentId][i] > 0)
            {
                edgesToCheck.Add(i);
            }
        }
        
        foreach(var edge in edgesToCheck) //sprawdzenie konców krawedzi
        {
            for(int j=0; j<verticesCount; j++)
            {
                if (jaggedTab[j][edge] < 0)
                {
                    childVerticesIds.Add(j);
                }
            }
        }
        return childVerticesIds;

    }

    public void OnRoadClick()
    {
        if (!gm.objectv2.active)
        {
            gm.objectv1.SetActive(true);
            gm.objectv2.SetActive(true);
        }
        else
        {
            gm.objectv1.SetActive(false);
            gm.objectv2.SetActive(false);
        }
       
    }

    public void FindRoad()
    {
        gm.objectv1.SetActive(false);
        gm.objectv2.SetActive(false);

        int v; //wierczholek biezacy
       // int u; //wieichdzolek roboczy
        GameObject[] vertices = GameObject.FindGameObjectsWithTag("vertex");
        var vs = int.Parse(gm.v1.text)-1;
        var vk = int.Parse(gm.v2.text)-1;
//
       /// Debug.Log("V1"+vs);
       // Debug.Log("V2"+vk);

        // bool skipped = false;

        var S = new List<int>();
        int[] P = new int[vertices.ToList().Count];
        bool[] visited = new bool[vertices.ToList().Count];

        //macierz incydencji
        

        var edgeCount = gm.edgeList.Count;
        var edgeTab = gm.edgeList;
        var incidenceTab = new int[vertices.ToList().Count, edgeCount];

        for (int i = 0; i < edgeCount; i++)
        {
            incidenceTab[edgeTab[i].Vertex2 - 1, i] = int.Parse(edgeTab[i].TextDistance.ToString()); //poczatek krawedzi
            incidenceTab[edgeTab[i].Vertex1 - 1, i] = -int.Parse(edgeTab[i].TextDistance.ToString()); // koniec krawedzi
        }

        var jaggedTab = incidenceTab.ToJagged();

        P[vs] = -1;
        S.Add(vs);
        visited[vs] = true;

        while(S.Count > 0)
        {
            v = S[0];
            S.RemoveAt(0);

            if (vk == v)
            {
                var list = new List<int>();
                string stringg = "Znaleziono drogê: ";
                while (v > -1)
                { 
                    list.Add(v + 1);
                    v = P[v];
                }
                for(int i=list.Count()-1; i>=0; i--)
                {
                    stringg = stringg + " " +list[i].ToString();
                    //Debug.Log(list[i]);
                }
                gm.modalText.text = stringg;
                gm.modal.SetActive(true);
                return;
            }
               
            var incidentIds = GetIncidentIds(vertices.ToList().Count, edgeCount, v, jaggedTab);
            foreach(var u in incidentIds)
            {
                if(visited[u] == false)
                {
                    P[u] = v;
                    S.Add(u);
                    visited[u] = true;
                }
            }

        }
        gm.modalText.text = "BRAK";
        gm.modal.SetActive(true);

    }

    public void hideModal()
    {
        gm.modal.SetActive(false);
    }

    public void saveGraph()
    {
        GameObject[] vertices = GameObject.FindGameObjectsWithTag("vertex");

        var edgeCount = gm.edgeList.Count;
        var edgeTab = gm.edgeList;
        var incidenceTab = new int[vertices.ToList().Count, edgeCount];

        for (int i=0; i < edgeCount; i++)
        {
            incidenceTab[edgeTab[i].Vertex2 - 1, i] = int.Parse(edgeTab[i].TextDistance.ToString()); //poczatek krawedzi
            incidenceTab[edgeTab[i].Vertex1 - 1, i] = - int.Parse(edgeTab[i].TextDistance.ToString()); // koniec krawedzi
        }

        File.WriteAllLines(@"D:\Grafy\Grafy\MyFile.txt", incidenceTab.ToJagged().Select(line => String.Join(" ", line)));
    }

    public int[] ReverseTab(int[] tab)
    {
        var listTab = tab.ToList();
        var newTab = new int[tab.ToList().Count];

        for(int i=0; i< tab.Count(); i++)
        {
            newTab[i] = listTab[listTab.Count() - 1 - i];
        }

        return newTab;
    }
}



public static class ArrayExtensions
{
    // In order to convert any 2d array to jagged one
    // let's use a generic implementation
    public static T[][] ToJagged<T>(this T[,] value)
    {
        if (System.Object.ReferenceEquals(null, value))
            return null;

        // Jagged array creation
        T[][] result = new T[value.GetLength(0)][];

        for (int i = 0; i < value.GetLength(0); ++i)
            result[i] = new T[value.GetLength(1)];

        // Jagged array filling
        for (int i = 0; i < value.GetLength(0); ++i)
            for (int j = 0; j < value.GetLength(1); ++j)
                result[i][j] = value[i, j];

        return result;
    }
}

