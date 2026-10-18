using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BuildingPreview : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 10f;
    [SerializeField]
    [Range(1f, 90f)]
    private float _startingAngle = 45f;

    private float _rotationAngleX = 30f;
    private float _rotationAngleY = 180f;
    private float _rotationAngleZ = 0f;

    private float _rotationModX = 1f;
    private float _rotationModY => 90f / _startingAngle;
    private float _rotationModZ = 1f;

    private void Start()
    {
        _rotationAngleX = _startingAngle;
        transform.localRotation = Quaternion.Euler(_rotationAngleX, _rotationAngleY, _rotationAngleZ);
    }
    void Update()
    {
        _rotationAngleX += _rotationSpeed * Time.deltaTime * _rotationModX;
        _rotationAngleY += _rotationSpeed * Time.deltaTime * _rotationModY;
        _rotationAngleZ += _rotationSpeed * Time.deltaTime * _rotationModZ;

        if (_rotationAngleX >= _startingAngle)
        {
            _rotationModX = -1f;
        } 
        else if (_rotationAngleX <= -_startingAngle)
        {
            _rotationModX = 1f;
        }

        if (_rotationAngleY >= 360f)
        {
            _rotationAngleY -= 360f;
        }

        if (_rotationAngleZ >= _startingAngle)
        {
            _rotationModZ = -1f;
        }
        else if (_rotationAngleZ <= -_startingAngle)
        {
            _rotationModZ = 1f;
        }

        transform.localRotation = Quaternion.Lerp(
            transform.localRotation, 
            Quaternion.Euler(_rotationAngleX, _rotationAngleY, _rotationAngleZ), 
            Time.deltaTime);
    }
}
