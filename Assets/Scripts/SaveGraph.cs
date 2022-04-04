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
    
    public void saveGraph()
    {
        GameObject[] vertices = GameObject.FindGameObjectsWithTag("vertex");

        var count = gm.edgeList.Count;
        var tabb = gm.edgeList;
        var tab = new int[vertices.ToList().Count, count];

        for (int i=0; i < count; i++)
        {
            tab[tabb[i].Vertex1 - 1, i] = int.Parse(tabb[0].TextDistance.ToString());
            tab[tabb[i].Vertex2 - 1, i] = - int.Parse(tabb[0].TextDistance.ToString());
        }

        File.WriteAllLines(@"D:\Grafy\Grafy\MyFile.txt", tab.ToJagged().Select(line => String.Join(" ", line)));
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

