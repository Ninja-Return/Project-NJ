using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Player/Data/Value")]
public class PlayerDataSO : ScriptableObject
{

    [field:SerializeField] public Stat MoveSpeed { get; private set; }
    [field:SerializeField] public Stat SitSpeed { get; private set; }
    [field:SerializeField] public Stat JumpPower { get; private set; }
    [field:SerializeField] public Stat LookSensitive { get; private set; }

    [field:Space]
    [field:Header("Interaction")]
    [field:SerializeField] public Stat InteractionRange { get; private set; }
    [field:SerializeField] public LayerMask InteractionLayer { get; private set; }

}
