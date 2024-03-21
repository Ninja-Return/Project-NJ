using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public abstract class TutorialObject : NetworkBehaviour
{
    public bool isTutorialOn;

    private void Update()
    {
        if (isTutorialOn)
        {
            IsClearTutorial();
        }
    }

    protected abstract void IsClearTutorial();
}
