using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [SerializeField] private float widthUnit = 6;   // World 화면의 너비
    private Camera _camera;

    void Start()
    {
        _camera = GetComponent<Camera>();
        _camera.orthographicSize = widthUnit / _camera.aspect / 2f;
    }
}
