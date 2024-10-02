using System.Collections.Generic;
using UnityEngine;

public class BackgroundGeneratorNew : MonoBehaviour
{
    public GameObject[] backgroundPrefabs;
    public float backgroundSpacing = 5f;
    public float offsetX = 5f;
    public float offsetY = 5f;
    public float destroyDistance = 20f;
    public Transform player;

    private List<GameObject> backgrounds = new List<GameObject>();
    private float nextBackgroundPosition;

    void Start()
    {
        nextBackgroundPosition = -destroyDistance;
        GenerateBackground();
    }

    void Update()
    {
        if (player.position.z > nextBackgroundPosition - destroyDistance)
        {
            GenerateBackground();
        }

        if (backgrounds.Count > 0 && player.position.z > backgrounds[0].transform.position.z + destroyDistance)
        {
            DestroyBackground(backgrounds[0]);
        }
    }

    private void GenerateBackground()
    {
        GameObject selectedPrefab = backgroundPrefabs[Random.Range(0, backgroundPrefabs.Length)];
        GameObject newBackground = Instantiate(selectedPrefab);
        float yPos = Random.Range(-offsetY, offsetY);
        newBackground.transform.position = new Vector3(newBackground.transform.position.x - offsetX, newBackground.transform.position.y + yPos, newBackground.transform.position.z + nextBackgroundPosition);
        backgrounds.Add(newBackground);
        nextBackgroundPosition += backgroundSpacing;
    }

    private void DestroyBackground(GameObject background)
    {
        backgrounds.Remove(background);
        Destroy(background);
    }
}