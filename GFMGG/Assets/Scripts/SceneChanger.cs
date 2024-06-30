using UnityEngine.SceneManagement;
using HietakissaUtils;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] SceneReference scene;

    public void LoadScene()
    {
        SceneManager.LoadScene(scene);
    }
}