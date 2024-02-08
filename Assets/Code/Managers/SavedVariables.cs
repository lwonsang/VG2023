using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavedVariables : MonoBehaviour
{
    /// <summary>
    /// this script holds variables between different scenes, if you want something transferred over
    /// to another scene, put the variable here and make it static.
    /// </summary>
    public static SavedVariables Instance;

    public static List<GameObject> Players;

    private void Awake()
    {
        //singleton behavior for these reference variables
        if (Instance == null)
        {
            Instance = this;
            Players = new List<GameObject>();
            Debug.Log(Players.Count);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
