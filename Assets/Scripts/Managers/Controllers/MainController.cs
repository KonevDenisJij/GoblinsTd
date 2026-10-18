using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    [SerializeField]
    private DefenseController _defController;

    [SerializeField]
    private RestController _restController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeState()
    {
        if (_defController.Active)
        {
            _defController.StopState();
            _restController.StartState();
            return;
        }

        if (_restController.Active)
        {
            _restController.StopState();
            _defController.StartState();
            return;
        }
    }
}
