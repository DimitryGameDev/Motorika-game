using System.Collections.Generic;
using UnityEngine;

public class BackgroundGenerator : MonoBehaviour
{
    [SerializeField] private Transform StartGenerationPoint; // Player position or position = player pos
    [SerializeField] private GameObject[] backgroundPrefabs; // Check your platform - they must be the same in x,z
    [SerializeField] private int countOfImage; // 5 - 10 platforms - gold result 

    [SerializeField] private float imagePosX;
    [SerializeField] private float imagePosY;
    [SerializeField] private float imagePosZ;
    [SerializeField] private float countImageBeforeDestroy; // Count platform behind

    private List<GameObject> activeImage = new List<GameObject>(); // List active platforms

    private void Start()
    {
        for (int i = 0; i < countOfImage; i++)
        {
            GeneratePlatform();
        }
    }

    private void Update()
    {
        if (activeImage[0].transform.position.z + imagePosZ * countImageBeforeDestroy < StartGenerationPoint.position.z)
        {
            DestroyPlatform();
            GeneratePlatform();
        }
    }

    private void GeneratePlatform()
    {
        for (int i = 0; i < backgroundPrefabs.Length; i++)
        {
            GameObject image = Instantiate(backgroundPrefabs[i], gameObject.transform);

            float platformZ = 0f;
            var firstPlatforms = StartGenerationPoint.position.z - imagePosZ * 0.33f;

            if (activeImage.Count < backgroundPrefabs.Length)
            {
                platformZ = firstPlatforms;
            }
            else
            {
                platformZ = activeImage[activeImage.Count - 1].transform.position.z + imagePosZ * i;
            }

            image.transform.position = new Vector3(imagePosX, imagePosY, platformZ);

            activeImage.Add(image);
        }
    }

    private void DestroyPlatform()
    {
        GameObject oldPlatform = activeImage[0];
        activeImage.RemoveAt(0);
        Destroy(oldPlatform);
    }
}