using UnityEngine;

public class ColorDisplay : MonoBehaviour
{
    [SerializeField] private ColorManager _colorManager;
    [SerializeField] private SkinnedMeshRenderer _displayColorServer;
    [SerializeField] private SkinnedMeshRenderer _displayColorClient;

    private void Start()
    {
        HandlePlayerColorChanged(Color.clear, _colorManager.playerColor.Value);

        _colorManager.playerColor.OnValueChanged += HandlePlayerColorChanged;
    }

    private void HandlePlayerColorChanged(Color oldColor, Color newColor)
    {
        _displayColorServer.material.color = newColor;
        _displayColorClient.material.color = newColor;
    }

    private void OnDestroy()
    {
        _colorManager.playerColor.OnValueChanged -= HandlePlayerColorChanged;
    }
}
