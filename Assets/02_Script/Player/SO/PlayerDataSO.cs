using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Player/Data")]
public class PlayerDataSO : ScriptableObject
{

    [field:SerializeField] public Stats MoveSpeed { get; private set; }
    [field:SerializeField] public Stats JumpPower { get; private set; }

}
