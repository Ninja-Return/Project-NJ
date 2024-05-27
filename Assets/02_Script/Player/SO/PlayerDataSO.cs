using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Player/Data/Value")]
public class PlayerDataSO : ScriptableObject
{

    [field: SerializeField] public Stat MoveSpeed { get; private set; }
    [field:SerializeField] public Stat SitSpeed { get; private set; }
    [field:SerializeField] public Stat JumpPower { get; private set; }
    [field:SerializeField] public Stat LookSensitive { get; private set; }

    [field:Space]
    [field:Header("Interaction")]
    [field:SerializeField] public Stat InteractionRange { get; private set; }
    [field:SerializeField] public LayerMask InteractionLayer { get; private set; }

    public PlayerDataSO Copy()
    {

        var data = Instantiate(this);
        data.MoveSpeed = MoveSpeed.Copy();
        data.SitSpeed = SitSpeed.Copy();
        data.JumpPower = JumpPower.Copy();
        data.InteractionRange = InteractionRange.Copy();
        data.LookSensitive = LookSensitive.Copy();
        data.InteractionLayer = InteractionLayer;

        return data;


    }

}
