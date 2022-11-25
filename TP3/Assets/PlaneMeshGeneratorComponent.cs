using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneMeshGeneratorComponent : MonoBehaviour
{
    [SerializeField] private float Width;
    [SerializeField] private float Height;
    [SerializeField] public int nColumns;
    [SerializeField] public int nRows;
    private int[] baseTris;
    private float deplacementX;
    private float deplacementZ;
    private float distanceX;
    private float distanceZ;
    private int nombreFaces;
    private int nombreSommets;
    private int Colonne;
    private Vector2[] GenerateUvs()
    {
        Vector2[] uvs = new Vector2[nombreSommets];
        int currentVertex = 0;
        float hauteurY = 1 / (float)nRows;
        float longueurX = 1 / (float)nColumns;
        float compteurY = 1;
        float compteurX = 0;

        for (int i = 0; i <= nRows; i++)
        {
            for (int j = 0; j <= nColumns; j++)
            {
                uvs[currentVertex++] = new Vector2(compteurX, compteurY);
                compteurX = compteurX+longueurX;
            }
            compteurX = 0;
            compteurY = compteurY-hauteurY;
        }
        return uvs;
    }
    private Vector3[] GenerateVertices()
    {
        int currentVertex = 0;
        Vector3[] vertices = new Vector3[nombreSommets];
        for (int i = 0; i <= nRows; i++)
        {
            for (int j = 0; j <= nColumns; j++)
            {
                vertices[currentVertex++] = new Vector3(deplacementX, 0, -deplacementZ);
                deplacementX = deplacementX +distanceX;
            }
            deplacementX = 0;
            deplacementZ = deplacementZ +distanceZ;
        }
        return vertices;
    }
    private int[] GenerateTris()
    {
        int compteur = 0;
        int change = nColumns;
        int[] tris = new int[6 * nombreFaces];
        int currentTriIndex = 0;
        for (int i = 0; i < nombreFaces; i++)
        {
            if (compteur == change)
            {
                compteur++;
                change += Colonne;
            }
            for (int j = 0; j < 6; j++)
            {
                tris[currentTriIndex++] = baseTris[j] + compteur;
            }
            compteur++;
        }
        return tris;
    }
    private void Awake()
    {
        baseTris =new[]
        {
            0, 1, Colonne,
            1, Colonne + 1, Colonne
        };
        nombreSommets =(nRows + 1)*(Colonne) ;
        distanceZ =Height/nRows;
        Colonne =nColumns+1;
        distanceX =Width/nColumns;
        nombreFaces =nColumns*nRows;
        Mesh m = new Mesh();
        GetComponent<MeshFilter>().mesh = m;
        m.vertices = GenerateVertices();
        m.triangles = GenerateTris();
        m.uv = GenerateUvs();
        m.RecalculateNormals();

    }
}