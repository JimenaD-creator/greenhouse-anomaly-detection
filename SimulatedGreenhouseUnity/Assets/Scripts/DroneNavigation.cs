using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;

public class DroneNavigation : MonoBehaviour
{
    public List<Vector3> waypoints;
    public float errorDist = 0.1f; // Por si
    public float timeStop = 0.5f; // CUanto tiempo se detiene

    private NavMeshAgent agent;
    private int currIndex = 0;
    private bool isChecking = true;
    private bool isPaused = false;
    public RemoverScript remover;
    public HarvesterScript harvester;
    public GreenhouseInitializer greenhouse;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        // remover = FindObjectOfType<RemoverScript>();
        // harvester = FindObjectOfType<HarvesterScript>();

        waypoints = greenhouse.GetCoordinates();

        if (waypoints == null || waypoints.Count == 0)
        {
            Debug.LogError("Lista Vacia");
            isChecking = false;
            return;
        }

        goNextPlant();
    }

    void Update()
    {
        if (!isChecking || isPaused) return;
        if (!agent.pathPending && agent.remainingDistance < errorDist)
        {
            StartCoroutine(pausaChequeo());
        }
    }

    IEnumerator pausaChequeo()
    {
        isPaused = true;
        agent.isStopped = true; // Detiene

        yield return new WaitForSeconds(timeStop);
        agent.isStopped = false;

        currIndex++;
        if (currIndex >= waypoints.Count)
        {
            currIndex = 0;
        }

        goNextPlant();
        isPaused = false;
    }

    void goNextPlant()
    {
        if (agent.isActiveAndEnabled)
        {
            agent.SetDestination(waypoints[currIndex]);
        }
    }

    // Deteccion y ese Rollo

    private string tagBien = "Bien"; 
    private string tagInfected = "Infected"; 
    private string tagMaduro = "Harvest";

    
    private void OnTriggerEnter(Collider other)
    {
   
        if (other.CompareTag(tagBien))
        {
            Debug.Log("Tomate Bien");
            return;
        }

        if (other.CompareTag(tagInfected))
        {
            Debug.Log("Tomate Infectado");
            Vector3 pos = other.transform.position;
            remover.AddRemoveRequest(pos);
            return;
        }

        if (other.CompareTag(tagMaduro))
        {
            Debug.Log("Tomate Maduro");
            Vector3 pos = other.transform.position;
            harvester.AddHarvestRequest(pos);
            return;
        }
    }

    public void removeCoordinate(Vector3 pos)
    {
        float tolerance = 0.1f;

        // Buscar la primera posición que esté dentro del margen
        for (int i = 0; i < waypoints.Count; i++)
        {
            if (Vector3.Distance(waypoints[i], pos) <= tolerance)
            {
                Debug.Log("Coordenada removida (aproximada): " + waypoints[i]);
                waypoints.RemoveAt(i);
                return; // salimos después de eliminar
            }
        }

        Debug.Log("No se encontró ninguna coordenada dentro del rango");
    }

}
