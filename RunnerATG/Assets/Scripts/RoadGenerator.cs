using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    public GameObject[] RoadPrefab;
    private List<GameObject> roads = new List<GameObject>();
    public float maxSpeed = 25;
    private float speed = 0;
    public int maxRoadCount = 4;
    public int plusRoadCount = 3;
    public int delRoadCount = 7;
    [SerializeField]
    private float roadOffset = 40f;

    bool startSpeedBust;

    void Start()
    {
        startSpeedBust = false;
        ReseLevel();
        //StartLevel();
    }

    void Update()
    {
        if (speed == 0) return;
        // Перемещение дорог на основе текущей скорости
        foreach (GameObject road in roads)
        {
            road.transform.position -= new Vector3(0, 0, speed * Time.deltaTime);
            if (Input.GetKey(KeyCode.A))
                road.transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
            if (Input.GetKey(KeyCode.D))
                road.transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
        }

        // Создание новой дороги, когда первая дорога выходит за пределы области видимости
        if (roads[0].transform.position.z < -110)
            CreatNextRoad();

        // Увеличение максимальной скорости постепенно, если startSpeedBust активен
        if (startSpeedBust)
            if (startSpeedBust)
            maxSpeed += 0.1f * Time.deltaTime;
    }

    private void CreatNextRoad()
    {
        // Создание новой дороги в определенной позиции
        Vector3 pos = Vector3.zero;
        if (roads.Count > 0)
        {
            pos = roads[roads.Count - 1].transform.position + new Vector3(0, 0, Random.Range(30f, 60f));
        }

        for (int i = -plusRoadCount; i <= plusRoadCount; i++)
        {
            Vector3 roadPos = new Vector3(Random.Range(-10f, 10f) * 10f, 1, pos.z);
            GameObject newRoad = Instantiate(RoadPrefab[Random.Range(0, RoadPrefab.Length)], roadPos, Quaternion.identity, transform);
            roads.Add(newRoad);
        }
        // Удаление дорог, когда их количество превышает заданное значение
        if (roads.Count > maxRoadCount * delRoadCount)
        {
            for (int i = 0; i < delRoadCount; i++)
            {
                Destroy(roads[0]);
                roads.RemoveAt(0);
            }
        }
    }

    // Начало уровня с заданной скоростью
    public void StartLevel()
    {
        speed = maxSpeed;
        startSpeedBust = true;
    }

    // Сброс уровня и удаление существующих дорог
    public void ReseLevel()
    {
        speed = 0;
        foreach (GameObject road in roads)
        {
            Destroy(road);
        }
        roads.Clear();
        for (int i = 0; i < maxRoadCount; i++)
        {
            CreatNextRoad();
        }
    }
}
