using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILightCastable
{

    public event Action OnCastedEvent;
    public void Casting();

}
