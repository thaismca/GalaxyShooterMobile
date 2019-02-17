using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadSiglePlayerMode()
    {
        SceneManager.LoadScene("Single_Player");
    }

    public void LoadCoopMode()
    {
        SceneManager.LoadScene("Coop");
    }

}
