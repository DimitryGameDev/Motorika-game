using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    [SerializeField] private Transform StartGenerationPoint; // Player position or position = player pos
    [SerializeField] private GameObject[] platformPrefabs; // Check your platform - they must be the same in x,z
    [SerializeField] private int countOfPlatforms; // 5 - 10 platforms - gold result 
    [SerializeField] private int countOfBasePlatforms; // Base platforms
    [SerializeField] private float platformLength; // Z size
    [SerializeField] private float countPlatformBeforeDestroy; // Count platform behind
    // if u need  
    private float platformWidth;
    private float platformHeight;

    private int platformCount;

    private List<GameObject> activePlatforms = new List<GameObject>(); // List active platforms

    private void Start()
    {
        for (int i = 0; i < countOfPlatforms; i++)
        {
            GeneratePlatform();
        }
    }

    private void Update()
    {
        if (activePlatforms[0].transform.position.z + platformLength * countPlatformBeforeDestroy < StartGenerationPoint.position.z)
        {
            DestroyPlatform();
            GeneratePlatform();
        }
    }

    private void GeneratePlatform()
    {
        int randomIndex = 0;

        if (platformCount > countOfBasePlatforms)
            randomIndex = Random.Range(0, platformPrefabs.Length);

        GameObject newPlatform = Instantiate(platformPrefabs[randomIndex], gameObject.transform);

        float platformX = platformWidth;
        float platformY = platformHeight;
        float platformZ = 0f;

        if (activePlatforms.Count > 0)
        {
            platformZ = activePlatforms[activePlatforms.Count - 1].transform.position.z + platformLength;
        }
        else
        {
            platformZ = StartGenerationPoint.position.z - platformLength / 2;
        }

        newPlatform.transform.position = new Vector3(platformX, platformY, platformZ);

        activePlatforms.Add(newPlatform);
        platformCount++;
    }

    private void DestroyPlatform()
    {
        GameObject oldPlatform = activePlatforms[0];
        activePlatforms.RemoveAt(0);
        Destroy(oldPlatform);
    }
}