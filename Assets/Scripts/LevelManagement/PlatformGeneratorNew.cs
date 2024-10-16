using System.Collections.Generic;
using UnityEngine;

public class PlatformGeneratorNew : MonoBehaviour
{
    [SerializeField] private Transform StartGenerationPoint; // Player position or position = player pos
    [SerializeField] private GameObject[] platformPrefabs; // Check your platform - they must be the same in x,z
    [SerializeField] private int countOfPlatforms; // 5 - 10 platforms - gold result 
    [SerializeField] private int countOfBasePlatforms; // Base platforms
    [SerializeField] private float platformLength; // Z size
    [SerializeField] private float countPlatformBeforeDestroy; // Count platform behind
    [Header("Level")]
    [SerializeField] private int maxLevel = 5;
    [SerializeField] private int countPlatformAddLevel; // Count Platform Before Add Level

    private List<GameObject> activePlatforms = new List<GameObject>(); // List active platforms

    private int level;
    public int Level => level;

    private int levelIndexPlatforms; // Correlation between level and platforms 
    private int platformCount;

    // if u need  
    private float platformWidth;
    private float platformHeight;

    private void Awake()
    {
        level = 1;

        for (int i = 0; i < countOfPlatforms; i++)
        {
            GeneratePlatform();
        }
    }

    private void Update()
    {
        ChangeIndex();

        if (platformCount > countPlatformAddLevel && level < maxLevel)
        {
            level++;
            platformCount = 0;
        }

        if (StartGenerationPoint != null && activePlatforms[0].transform.position.z + platformLength * countPlatformBeforeDestroy < StartGenerationPoint.position.z)
        {
            DestroyPlatform();
            GeneratePlatform();
        }
    }

    private void GeneratePlatform()
    {
        int randomIndex = Random.Range(0, levelIndexPlatforms);
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

    private void ChangeIndex()
    {
        levelIndexPlatforms = Mathf.Clamp((platformPrefabs.Length * level) / maxLevel, 1, platformPrefabs.Length);
    }
}