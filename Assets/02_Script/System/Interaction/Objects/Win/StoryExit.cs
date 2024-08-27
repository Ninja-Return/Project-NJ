using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryExit : InteractionObject
{

    [SerializeField] private string sceneName;

    protected override void DoInteraction()
    {

        HostSingle.Instance.GameManager.ShutdownAsync();
        SceneManager.LoadScene(sceneName);

    }

}
