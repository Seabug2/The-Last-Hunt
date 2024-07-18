using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rhythm_ButtonController : MonoBehaviour
{
    public void SceneLoader(string scene_name)
    {
        SceneManager.LoadScene(scene_name);
    }
}
