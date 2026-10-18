using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFieldCamera : MonoBehaviour
{
    [SerializeField]
    private float _stickMinZoom, _stickMaxZoom;
    [SerializeField]
    private float _swivelMinZoom, _swivelMaxZoom;
    [SerializeField, Range(0f, 1f)]
    private float _zoomSensetivity;
    [SerializeField]
    private float _moveSpeedMinZoom, _moveSpeedMaxZoom;
    [SerializeField]
    [Header("(Для основной игры)")]
    private GameBoard _mapSize;
    [SerializeField]
    [Header("(Для режимов на карте мира и деревне)")]
    private float[] _customMapSize = new float[4];

    [SerializeField]
    private float _rotationSpeed;

    private Transform _swivel, _stick;
    private float _zoom = 1f;
    private float _rotationAngle;

    private void Awake()
    {
        _swivel = transform.GetChild(0);
        _stick = _swivel.GetChild(0);
    }

    private void Update()
    {
        float zoomDelta = -Input.GetAxis("Mouse ScrollWheel") * _zoomSensetivity;
        if (zoomDelta != 0f)
        {
            AdjustZoom(zoomDelta);
        }

        float rotationDelta = Input.GetAxis("Rotation");
        if (rotationDelta != 0f)
        {
            AdjustRotation(rotationDelta);
        }

        float xDelta = Input.GetAxis("Horizontal");
        float zDelta = Input.GetAxis("Vertical");
        if (xDelta != 0f || zDelta != 0f)
        {
            AdjustPosition(xDelta, zDelta);
        }
    }

    private void AdjustZoom(float delta)
    {
        _zoom = Mathf.Clamp01(_zoom + delta);

        float distance = Mathf.Lerp(_stickMinZoom, _stickMaxZoom, _zoom);
        _stick.localPosition = new Vector3(0f, 0f, distance);

        float angle = Mathf.Lerp(_swivelMinZoom, _swivelMaxZoom, _zoom);
        _swivel.localRotation = Quaternion.Euler(angle, 0f, 0f);
    }

    private void AdjustRotation(float delta)
    {
        _rotationAngle += delta * _rotationSpeed * Time.deltaTime;

        if (_rotationAngle < 0f)
        {
            _rotationAngle += 360f;
        }
        else if (_rotationAngle >= 360f)
        {
            _rotationAngle -= 360f;
        }

        transform.localRotation = Quaternion.Euler(0f, _rotationAngle, 0f);
    }

    private void AdjustPosition(float xDelta, float zDelta)
    {
        Vector3 direction = transform.localRotation * new Vector3(xDelta, 0f, zDelta).normalized;
        float damping = Mathf.Max(Mathf.Abs(xDelta), Mathf.Abs(zDelta));
        float distance = Mathf.Lerp(_moveSpeedMinZoom, _moveSpeedMaxZoom, _zoom) * damping * Time.deltaTime;

        Vector3 position = transform.localPosition;
        position += direction * distance;
        transform.localPosition = ClampPosition(position);
    }

    Vector3 ClampPosition(Vector3 position)
    {
        if (_mapSize != null)
        {
            position.x = Mathf.Clamp(position.x, _mapSize.MapSize[3], _mapSize.MapSize[1]);
            position.z = Mathf.Clamp(position.z, _mapSize.MapSize[2], _mapSize.MapSize[0]);
        }
        else
        {
            position.x = Mathf.Clamp(position.x, _customMapSize[3], _customMapSize[1]);
            position.z = Mathf.Clamp(position.z, _customMapSize[2], _customMapSize[0]);
        }

        return position;
    }
}