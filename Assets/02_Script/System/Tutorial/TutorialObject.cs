using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public abstract class TutorialObject : NetworkBehaviour
{
    public bool isTutorialOn;
    private bool isStarted;
    
    private void Update()
    {
        if (isTutorialOn)
        {
            if (!isStarted)
            {
                isStarted = true;
                Init();
            }

            IsClearTutorial();
        }
    }

    protected abstract void Init();

    protected abstract void IsClearTutorial();
}
