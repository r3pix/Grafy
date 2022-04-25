using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        bool[] visited = new bool[vertices.ToList().Count];
        List<int> queue = new List<int>();

        queue.Add(0);
        visited[0] = true;
       
        while (queue.Count > 0)
        {
            var id = queue[0];
            var incidentIds = GetIncidentIds(vertices.ToList().Count,edgeCount,id,jaggedTab);
            foreach(var idToAdd in incidentIds)
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
            Debug.Log("Spójny");
        }
        else
        {
            Debug.Log("Niespójny");
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

