using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelList : MonoBehaviour {

    public List<int> allTheLevels = new List<int>();
    public int selectedLevel;
    public Text levelText;

    // Use this for initialization
    void Start() {
        for (int i = 2; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            allTheLevels.Add(i);
        }
        selectedLevel = 2;
    }

    // Update is called once per frame
    void Update() {
        levelText.text = (selectedLevel-1).ToString();
    }

    public void AddLevel()
    {
        if (selectedLevel <= allTheLevels.Count)
        {
            selectedLevel += 1;
        }
    }
    public void BackLevel()
    {
        if (selectedLevel >= 3)
        {
            selectedLevel -= 1;
        }
    }
    public void LaunchLevel()
    {
        SceneManager.LoadScene(selectedLevel, LoadSceneMode.Single);
    }

    public void OpenWebsite(string website)
    {
        Application.OpenURL(website);
    }
}
