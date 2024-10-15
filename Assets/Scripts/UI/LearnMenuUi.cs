using UnityEngine;

public class LearnMenuUi : MonoBehaviour
{
    [SerializeField] private GameObject pauseButtonLeft;
    [SerializeField] private GameObject pauseButtonRight;
    [SerializeField] private GameObject closeButtonLeft;
    [SerializeField] private GameObject closeButtonRight;
    [SerializeField] private GameObject pausePanel;

    private int levelID;

    private void Start()
    {
        pausePanel.SetActive(false);

        levelID = PlayerPrefs.GetInt("Control");

        if (levelID == 0)
            LeftControl();
        else
            RightControl();
    }

    public void OpenPenel()
    {
        pausePanel.SetActive(true);
        if (levelID == 0)
            pauseButtonLeft.SetActive(false);
        else
            pauseButtonRight.SetActive(false);
    }

    public void ClosePenel()
    {
        pausePanel.SetActive(false);
        if (levelID == 0)
            pauseButtonLeft.SetActive(true);
        else
            pauseButtonRight.SetActive(true);
    }

    private void LeftControl()
    {
        pauseButtonLeft.SetActive(true);
        pauseButtonRight.SetActive(false);
        closeButtonLeft.SetActive(true);
        closeButtonRight.SetActive(false);
    }

    private void RightControl()
    {
        pauseButtonLeft.SetActive(false);
        pauseButtonRight.SetActive(true);
        closeButtonLeft.SetActive(false);
        closeButtonRight.SetActive(true);
    }
}