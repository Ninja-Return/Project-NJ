using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MonsterAnimation : NetworkBehaviour
{
    public Animator anim;

    public void SetAnimation(string name)
    {
        anim.SetTrigger(name);
        SetAnimationClientRpc(name);
    }

    public void SetAnimation(string name, bool value)
    {
        anim.SetBool(name, value);
        SetAnimationClientRpc(name, value);
    }

    [ClientRpc]
    private void SetAnimationClientRpc(string name)
    {
        anim.SetTrigger(name);
    }

    [ClientRpc]
    private void SetAnimationClientRpc(string name, bool value)
    {
        anim.SetBool(name, value);
    }
}
