using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : HandItemRoot
{

    [SerializeField] private GameObject flashlight;
    [SerializeField] private LayerMask obstacleLayer;

    public override void DoUse()
    {

        SoundManager.Play3DSound("FlashLight", transform.position, 0.1f, 5);
        flashlight.SetActive(!flashlight.activeSelf);

        if (flashlight.activeSelf)
        {

            var cast = Physics.OverlapBox(
                Camera.main.transform.position + Camera.main.transform.forward * 10f,
                Vector3.one * 10f,
                Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up),
                ~obstacleLayer);

            foreach(var item in cast)
            {

                var dir = Camera.main.transform.position - item.transform.position;

                if(!Physics.Raycast(Camera.main.transform.position, dir.normalized, 10, obstacleLayer))
                {

                    if (item.TryGetComponent<ILightCastable>(out var compo))
                    {

                        compo.Casting(transform.position);

                    }

                }

            }

        }

    }

}
