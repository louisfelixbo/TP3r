using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneMeshDeformComponent : MonoBehaviour
{
    private float[] theta;
    private float radStepPerSecond = 1;
    private Vector3[] verticesCopy;
    private int colonne;
    private int rangés; 
    private float amplitude = 1;
    private float newStep;
    private float periodeColonne;
    private PlaneMeshGeneratorComponent meshGenerator;
    private Mesh m;
    private void WaveMotion(float step)
    {
        for (int i = 0; i < colonne + 1; i++)
        {
            for (int j = 0; j <= rangés; j++)
            {
                verticesCopy[j*(colonne + 1)+i].z+=(Mathf.Sin(theta[i] + step)- Mathf.Sin(theta[i]))*amplitude;
            }
            theta[i] += step;
        }
    } 
    private void Start()
    {
        colonne = meshGenerator.nColumns;
        rangés = meshGenerator.nRows;
        theta = new float[colonne + 1];
        periodeColonne = (2 * Mathf.PI) / colonne;
        m = GetComponent<MeshFilter>().mesh;
        meshGenerator = GetComponent<PlaneMeshGeneratorComponent>();
        verticesCopy = m.vertices;
        for (int i = 0; i <= colonne; i++)
        {
            theta[i] = periodeColonne * i;
            for (int j = 0; j <= rangés; j++) 
            { 
                verticesCopy[j * (colonne + 1) + i].z += amplitude * Mathf.Sin(theta[i]);
            }
        }
        m.vertices = verticesCopy;
    }

    void Update() 
    { 
        verticesCopy = m.vertices; 
        newStep = radStepPerSecond * Time.deltaTime; 
        WaveMotion(newStep); 
        m.vertices = verticesCopy; 
        m.RecalculateNormals();
    }
}
