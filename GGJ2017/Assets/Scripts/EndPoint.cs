using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPoint : MonoBehaviour
{
    public void LoadLevel (string level)
    {
        SceneManager.LoadScene(level, LoadSceneMode.Single);
    }
}
