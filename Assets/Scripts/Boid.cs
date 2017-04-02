using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boid : MonoBehaviour
{
    public float movementSpeed = 0.2f;
    public float maxRange = 2.0f;
    public float minRange = 0.5f;
    public float repellingForce = 1.5f;
    public Vector3 moveToPos = new Vector3(0, 0, 0);

    UI userInterface;

    float xMin = -17.0f;
    float xMax = 17.0f;
    float yMin = -8.5f;
    float yMax = 8.5f;

    List<GameObject> surrBoids = new List<GameObject>();
    
    // Use this for initialization
    void Start()
    {
        userInterface = GameObject.FindGameObjectWithTag("Manager").GetComponent<UI>();
        Vector3 boidPosAvg = transform.position;
        Vector3 boidMovAvg = transform.position;
        Vector3 boidPushAvg = transform.position;
        moveToPos = FindNextPos();
    }

    // Update is called once per frame
    void Update()
    {
        RestrictBoid();
        FindBoidsInRange();

        if(surrBoids.Count > 0)
        {
            foreach (GameObject b in surrBoids)
            {
                Debug.DrawLine(transform.position, b.transform.position, Color.green);
            }

            moveToPos = FindNextPos();
        }

        Debug.DrawLine(transform.position, moveToPos, Color.red);

        if (Vector3.Distance(moveToPos, transform.position) > 0.5f)
            transform.position = Vector3.MoveTowards(transform.position, moveToPos, movementSpeed * Time.deltaTime);
        else
            moveToPos = FindNextPos();

        surrBoids.Clear();
        UpdateValues(userInterface);
    }

    //Stop the boid from leaving the confines of the screen
    void RestrictBoid()
    {
        if (transform.position.x > xMax)
            transform.position = new Vector3(xMin + 2, transform.position.y, transform.position.z);
        else if (transform.position.x < xMin)
            transform.position = new Vector3(xMax - 2, transform.position.y, transform.position.z);

        if (transform.position.y > yMax)
            transform.position = new Vector3(transform.position.x, yMin + 2, transform.position.z);
        else if (transform.position.y < yMin)
            transform.position = new Vector3(transform.position.x, yMax - 2, transform.position.z);
    }

    void FindBoidsInRange()
    {
        if (GameObject.FindGameObjectsWithTag("Boid") != null)
        {
            foreach (GameObject b in GameObject.FindGameObjectsWithTag("Boid"))
            {
                float dist = Vector3.Distance(b.transform.position, transform.position);
                if (dist < maxRange && dist > minRange)
                    surrBoids.Add(b);
                else
                    continue;
            }
        }
    }

    void UpdateValues(UI ui)
    {
        movementSpeed = ui.moveSpeedV;
        maxRange = ui.maxRangeV;
        minRange = ui.minRangeV;
    }

    Vector3 FindNextPos()
    {
        Vector3 newPos = transform.position;

        if (surrBoids.Count > 0)
        {
            newPos += AveragePos();
            newPos += AverageMoveToPos();
            //newPos += AverageSeparation();

            newPos = new Vector3(newPos.x / 3, newPos.y / 3, 0);
        }
        else
            newPos = new Vector3(Random.Range(-10, 10), Random.Range(-5, 5), 0);

        return newPos;
    }

    Vector3 AveragePos()
    {
        Vector3 avgPos = new Vector3(0,0,0);

        foreach(GameObject b in surrBoids)
        {
            avgPos += b.transform.position;
        }

        avgPos = new Vector3(avgPos.x / surrBoids.Count, avgPos.y / surrBoids.Count, 0);

        return avgPos;
    }

    Vector3 AverageMoveToPos()
    {
        Vector3 avgPos = new Vector3(0, 0, 0);

        foreach(GameObject b in surrBoids)
        {
            avgPos += b.GetComponent<Boid>().moveToPos;
        }

        avgPos = new Vector3(avgPos.x / surrBoids.Count, avgPos.y / surrBoids.Count, 0);

        return avgPos;
    }
}
