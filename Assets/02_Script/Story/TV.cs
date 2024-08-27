using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TV : MonoBehaviour
{

    [SerializeField] private MeshRenderer render;
    [SerializeField] private Material onMaterial;
    private List<Material> on = new();
    private void Start()
    {

        render.GetMaterials(on);
        on[1] = Instantiate(onMaterial);

    }

    public void On()
    {

        render.SetMaterials(on);

    }

}
