using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventSystem : MonoBehaviour
{

    public event Action OnTweenAnimeEvent;

    public void TweenAnimeInvoke()
    {

        OnTweenAnimeEvent?.Invoke();

    }

}
