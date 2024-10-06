using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndScript : MonoBehaviour
{
    public static Dictionary<string, bool> End = new Dictionary<string, bool>();

    public GameObject panel;
    // Start is called before the first frame update
    void Start()
    {
        End.Add("chel_red", false);
        End.Add("chel_blue", false);
        End.Add("chel_violet", false);
    }

    // Update is called once per frame
    void Update()
    {
        bool ended = true;
        foreach (var b in End)
        {
            ended = ended && b.Value;
        }

        if (ended)
        {
            Time.timeScale = 0;
            panel.SetActive(true);
        }
            
    }
}
