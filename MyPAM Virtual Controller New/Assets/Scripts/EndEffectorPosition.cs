using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndEffectorPosition : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 endEffectorPosition;
    Vector3 mappedPosition;

    public Text textX;
    public Text textY;

    public GameObject joint1;
    void Start() 
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        endEffectorPosition = RotationTest.origin - transform.position;

        mappedPosition.x = -(endEffectorPosition.x * RotationTest.frameScalar);
        UDP_Handler.X2pos = mappedPosition.x;
        mappedPosition.y = -(endEffectorPosition.y * RotationTest.frameScalar);
        UDP_Handler.Y2pos = mappedPosition.y;

        textX.text = "X: " + mappedPosition.x.ToString("F2");
        textY.text = "Y: " + mappedPosition.y.ToString("F2");
    }
}
