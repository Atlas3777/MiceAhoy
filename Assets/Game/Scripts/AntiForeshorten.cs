using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class AntiForeshorten : MonoBehaviour
{
    private Camera _cam;
    private float _lastAspect;
    private float _lastFov;
    
    //private const float AspectModifier = 1; 
    private const float AspectModifier = 1.41421356f; 

    void Awake()
    {
        _cam = GetComponent<Camera>();
        UpdateMatrix();
    }

    void OnValidate() => UpdateMatrix();

    void LateUpdate()
    {
        // if (_cam.aspect != _lastAspect || _cam.fieldOfView != _lastFov)
        // {
            UpdateMatrix();
        //}
    }

    [ContextMenu("Update Matrix")]
    public void UpdateMatrix()
    {
        if (!_cam) 
            _cam = GetComponent<Camera>();
        
        _cam.ResetProjectionMatrix();
        Matrix4x4 mat = _cam.projectionMatrix;
        mat[1, 1] *= AspectModifier;
        _cam.projectionMatrix = mat;
        
        _lastAspect = _cam.aspect;
        _lastFov = _cam.fieldOfView;
    }
}