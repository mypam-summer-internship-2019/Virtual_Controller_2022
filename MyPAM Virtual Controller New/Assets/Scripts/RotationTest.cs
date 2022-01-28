using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotationTest : MonoBehaviour
{
    public Camera mainCam;
    public Camera zoomCam;

    public Canvas mainCanvas;
    public Canvas zoomCanvas;

    private static float targetX = 0;
    private static float targetY = 0;

    Vector2 targetPos;

    float gridTLX;
    float gridTLY;

    float gridBRX;
    float gridBRY;

    float armA;
    float armB;
    float armC;

    float r1;
    float A;
    float theta1;
    float theta2;

    float j1Angle;
    float j2Angle;

    private static bool move = true;

    float armLength2;
    float armLength;

    public GameObject joint1;
    public GameObject joint2;
    public GameObject endEffector;
    public GameObject endEffectorZoom;
    public GameObject gameFrame;
    public GameObject traget;
    public GameObject join1PosHolder;
    public Slider speed;

    Vector3 endEffectorPos;
    Vector3 gridPos;
    Vector3 startMousePos;
    Vector3 endEffectorStartPos;
    Vector3 currentMousePos;
    Vector3 moveArm;
    Vector3 joint1Pos;
    Vector3 startPos;
    public Toggle assistanceToggle;

    float gameFramWidth = 300;
    static public float gameFramHeight = 210;
    static public float frameScalar = 100;

    static public Vector3 origin;

    // Start is called before the first frame update
    void Start()
    {
       // gameFrame.transform.localScale = new Vector3(10/ frameScalar, 10 / frameScalar, 1);
        joint1Pos = joint1.transform.position;

        //Arm length in pixels

        armA = joint2.transform.position.y;
        armB = endEffector.transform.position.y;
        armC = joint1.transform.position.y;

        armLength = armA - armB;
        armLength2 = armC - armA;

        float gFY = - Mathf.Sqrt((Mathf.Pow(armLength, 2) + Mathf.Pow(armLength2, 2))) + joint1.transform.position.y;
        origin = gameFrame.transform.position;
        gameFrame.transform.position = origin;
        endEffector.transform.position = origin;

        ///Debugging
        Debug.Log(gridTLX);
        Debug.Log(gridTLY);
        Debug.Log(gridBRX);
        Debug.Log(gridBRY);
        Debug.Log( "X Length: " + (gridTLX - gridBRX) );
        Debug.Log( "Y Length: " + (gridBRY - gridTLY) );
        Debug.Log( "Arm Length 1: " + (armLength));
        Debug.Log("Arm Length 2: " + (armLength2));

        endEffector.transform.position = origin;
        endEffectorPos = endEffector.transform.position - joint1.transform.position;


        /// Inverse Kiematics//////////////////////////////////////
        r1 = Mathf.Sqrt(Mathf.Pow(endEffectorPos.x, 2) + Mathf.Pow(endEffectorPos.y, 2));

        A = Mathf.Acos((Mathf.Pow(armLength2, 2) + Mathf.Pow(armLength, 2) - Mathf.Pow(r1, 2)) / (2 * armLength2 * armLength));
        theta2 = Mathf.PI - A;
        theta1 = Mathf.Asin(Mathf.Sin(A) / r1 * armLength) + Mathf.Atan(-endEffectorPos.x / endEffectorPos.y);

        j1Angle = theta1 / Mathf.PI * 180;
        j2Angle = (-theta2 + theta1) / Mathf.PI * 180;

        UDP_Handler.Pot1 = j1Angle;
        UDP_Handler.Pot2 = j2Angle;
        UDP_Handler.Theta0 = j1Angle;
        UDP_Handler.Theta1 = j2Angle;

        joint1.transform.rotation = Quaternion.Euler(0.0f, 0.0f, j1Angle);
        joint2.transform.rotation = Quaternion.Euler(0.0f, 0.0f, j2Angle);

    }
    // Update is called once per frame
    void Update()
    {
        joint2.transform.position = join1PosHolder.transform.position;
        targetX = (((float)(UDP_Handler.Xtarget)) / frameScalar) + origin.x;
        targetY = (((float)(UDP_Handler.Ytarget)) / frameScalar) + origin.y;
        targetPos = new Vector3(targetX, targetY, -6);
        traget.transform.position = targetPos;

        if (!settigsHandler.zoomBool)
        {
            mainCanvas.enabled = true;
            zoomCanvas.enabled = false;
            //joint1.SetActive(true);
            //endEffectorZoom.SetActive(false);
        }

        else
        {
            mainCanvas.enabled = false;
            zoomCanvas.enabled = true;
            //joint1.SetActive(false);
            //endEffectorZoom.SetActive(true);
        }

        if (Input.GetKeyDown("mouse 0"))
        {
            if (!settigsHandler.zoomBool){startMousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);}
            else { startMousePos = zoomCam.ScreenToWorldPoint(Input.mousePosition);}

            endEffectorStartPos = endEffector.transform.position;
        }
        if (Input.GetKey("mouse 0"))
        {

            if (Input.GetKey("mouse 0"))
            {
                if (!settigsHandler.zoomBool) { currentMousePos = mainCam.ScreenToWorldPoint(Input.mousePosition); }
                else { currentMousePos = zoomCam.ScreenToWorldPoint(Input.mousePosition); }
            }

            moveArm = startMousePos - currentMousePos;
            endEffectorPos = endEffectorStartPos - moveArm;
            if (endEffectorPos.x <= origin.x - 1.5f)
            {
                endEffectorPos.x = origin.x - 1.5f;
            }
            else if (endEffectorPos.x >= origin.x + 1.5f)
            {
                endEffectorPos.x = origin.x + 1.5f;
            }

            if (endEffectorPos.y <= origin.y - 1.05f)
            {
                endEffectorPos.y = origin.y - 1.05f;
            }
            else if (endEffectorPos.y >= origin.y + 1.05f)
            {
                endEffectorPos.y = origin.y + 1.05f;
            }

            /*
            if (endEffectorPos.x <= -gameFramWidth / (frameScalar * 2) + origin.x)
            {
                endEffectorPos.x = -gameFramWidth / (frameScalar * 2) + origin.x;
            }
            else if (endEffectorPos.x >= gameFramWidth / (frameScalar * 2) + origin.x)
            {
                endEffectorPos.x = gameFramWidth / (frameScalar * 2) + origin.x;
            }

            if (endEffectorPos.y <= -gameFramHeight / (frameScalar * 2) + origin.y)
            {
                endEffectorPos.y = -gameFramHeight / (frameScalar * 2) + origin.y;
            }

            else if (endEffectorPos.y >= gameFramHeight / (frameScalar * 2) + origin.y)
            {
                endEffectorPos.y = gameFramHeight / (frameScalar * 2) + origin.y;
            }
            */
            endEffector.transform.position = endEffectorPos;
            endEffectorZoom.transform.position = endEffectorPos;
            endEffectorPos = endEffectorPos - joint1.transform.position;

            /// Inverse Kiematics//////////////////////////////////////
            r1 = Mathf.Sqrt(Mathf.Pow(endEffectorPos.x, 2) + Mathf.Pow(endEffectorPos.y, 2));

            A = Mathf.Acos((Mathf.Pow(armLength2, 2) + Mathf.Pow(armLength, 2) - Mathf.Pow(r1, 2)) / (2 * armLength2 * armLength));
            theta2 = Mathf.PI - A;
            theta1 = Mathf.Asin(Mathf.Sin(A) / r1 * armLength) + Mathf.Atan(-endEffectorPos.x / endEffectorPos.y);

            j1Angle = theta1 / Mathf.PI * 180;
            j2Angle = (-theta2 + theta1) / Mathf.PI * 180;

            UDP_Handler.Pot1 = j1Angle;
            UDP_Handler.Pot2 = j2Angle;
            UDP_Handler.Theta0 = j1Angle;
            UDP_Handler.Theta1 = j2Angle;

            joint1.transform.rotation = Quaternion.Euler(0.0f, 0.0f, j1Angle);
            joint2.transform.rotation = Quaternion.Euler(0.0f, 0.0f, j2Angle);
        }

        else if (assistanceToggle.isOn)
        {

            endEffector.transform.position = Vector3.MoveTowards(endEffector.transform.position, targetPos, speed.value);
            endEffectorPos = endEffector.transform.position - joint1.transform.position;

            /// Inverse Kiematics//////////////////////////////////////
            r1 = Mathf.Sqrt(Mathf.Pow(endEffectorPos.x, 2) + Mathf.Pow(endEffectorPos.y, 2));

            A = Mathf.Acos((Mathf.Pow(armLength2, 2) + Mathf.Pow(armLength, 2) - Mathf.Pow(r1, 2)) / (2 * armLength2 * armLength));
            theta2 = Mathf.PI - A;
            theta1 = Mathf.Asin(Mathf.Sin(A) / r1 * armLength) + Mathf.Atan(-endEffectorPos.x / endEffectorPos.y);

            j1Angle = theta1 / Mathf.PI * 180;
            j2Angle = (-theta2 + theta1) / Mathf.PI * 180;

            UDP_Handler.Pot1 = j1Angle;
            UDP_Handler.Pot2 = j2Angle;
            UDP_Handler.Theta0 = j1Angle;
            UDP_Handler.Theta1 = j2Angle;

            joint1.transform.rotation = Quaternion.Euler(0.0f, 0.0f, j1Angle);
            joint2.transform.rotation = Quaternion.Euler(0.0f, 0.0f, j2Angle);
        }
    }
}