using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    public GameObject Boid;
    public Text boidCounter;

    public Slider moveSpeedSlider;
    public Slider maxRangeSlider;
    public Slider minRangeSlider;

    public float moveSpeedV;
    public float maxRangeV;
    public float minRangeV;

    int boidCount;

    void Start()
    {
        UpdateVars();
        boidCount = GameObject.FindGameObjectsWithTag("Boid").Length;
        boidCounter.text = "Boid Count: " + boidCount;
    }

    void Update()
    {
        UpdateVars();
    }

    public void SpawnBoid()
    {
        Vector3 spawnP = new Vector3(Random.Range(-16, 16), Random.Range(-8, 8), 0);
        Instantiate(Boid, spawnP, transform.rotation);

        boidCount += 1;
        boidCounter.text = "Boid Count: " + boidCount;
    }

    public void SpawnTen()
    {
        for (int i = 0; i < 10; ++i)
        {
            SpawnBoid();
        }
    }

    void UpdateVars()
    {
        moveSpeedV = moveSpeedSlider.value;
        minRangeV = minRangeSlider.value;
        maxRangeV = maxRangeSlider.value;
    }
}
