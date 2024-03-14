using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class ConeAnim : MonoBehaviour
{
    [SerializeField] private GameObject coneUP;
    [SerializeField] private GameObject coneDown;
    [SerializeField] private GameObject coneLeft;
    [SerializeField] private GameObject coneRight;
    [SerializeField] private GameObject coneUPLeft;
    [SerializeField] private GameObject coneUPRight;
    [SerializeField] private GameObject coneDownLeft;
    [SerializeField] private GameObject coneDownRight;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnConeUP()
    {
        coneUP.SetActive(true);

        if (coneUPLeft.activeSelf == true)
        {
            coneUPLeft.SetActive(false);
        }
        //else if (coneRight.activeSelf == true)
        //coneRight.SetActive(false);
        if (coneUPRight.activeSelf == true)
            coneUPRight.SetActive(false);
        //else if (coneLeft.activeSelf == true)
        //coneLeft.SetActive(false);
        else if (coneLeft.activeSelf == true)
        {
            coneLeft.SetActive(false);
        }
        else if (coneDownRight.activeSelf == true)
            coneDownRight.SetActive(false);
    }

    public void OnConeDown()
    {
        coneDown.SetActive(true);

        if (coneDownLeft.activeSelf == true && coneRight.activeSelf == true)
        {
            coneDownLeft.SetActive(false);
            coneRight.SetActive(false);
        }
        //else if (coneRight.activeSelf == true)
            //coneRight.SetActive(false);
        if (coneDownRight.activeSelf == true)
            coneDownRight.SetActive(false);
        else if (coneDownLeft.activeSelf == true)
            coneDownLeft.SetActive(false);
        //else if (coneLeft.activeSelf == true)
        //  coneLeft.SetActive(false);
    }

    public void OnConeLeft()
    {
        coneLeft.SetActive(true);

        if (coneUPLeft.activeSelf == true && coneDown.activeSelf == true)
        {
            coneUPLeft.SetActive(false);
            coneDown.SetActive(false);
        }
        //else if (coneUP.activeSelf == true)
        //  coneUP.SetActive(false);
        /*if (coneDownRight.activeSelf == true)
            coneDownRight.SetActive(false);*/
        else if (coneDownLeft.activeSelf == true && coneUP.activeSelf == true)
        {
            coneDownLeft.SetActive(false);
            coneUP.SetActive(false);
        }
        else if (coneDownLeft.activeSelf == true)
        {
            coneDownLeft.SetActive(false);
        }
        else if (coneDown.activeSelf == true)
        {
            coneDown.SetActive(false);
        }
        else if (coneUP.activeSelf == true)
            coneUP.SetActive(false);
        /**/
        //else if (coneDown.activeSelf == true)
        //   coneDown.SetActive(false);
    }

    public void OnConeRight()
    {
        coneRight.SetActive(true);

        if (coneDownRight.activeSelf == true && coneUPRight.activeSelf == true)
        {
            coneDownRight.SetActive(false);
            coneUPRight.SetActive(false);

        }
        else if (coneUPLeft.activeSelf == true)
        {
            coneUPLeft.SetActive(false);
        }
        else if (coneDownLeft.activeSelf == true)
            coneDownLeft.SetActive(false);
        /*if (coneUPRight.activeSelf == true)
            coneUPRight.SetActive(false);*/
        /*else if (coneDown.activeSelf == true)
            coneDown.SetActive(false);*/
        //else if (coneUP.activeSelf == true)
        //  coneUP.SetActive(false);
    }

    public void OnConeUpLeft()
    {
        coneUPLeft.SetActive(true);

        if (coneUP.activeSelf == true && coneLeft.activeSelf == true)
        {
            coneUP.SetActive(false);
            coneLeft.SetActive(false);
        }
    }

    public void OnConeUPRight()
    {
        coneUPRight.SetActive(true);

        if (coneUP.activeSelf == true && coneRight.activeSelf == true)
        {
            coneUP.SetActive(false);
            coneRight.SetActive(false);
        }
    }

    public void OnConeDownLeft()
    {
        coneDownLeft.SetActive(true);

        if (coneLeft.activeSelf == true && coneDown.activeSelf == true)
        {
            coneLeft.SetActive(false);
            coneDown.SetActive(false);
        }
    }

    public void OnConeDownRight()
    {
        coneDownRight.SetActive(true);

        if (coneDown.activeSelf == true && coneRight.activeSelf == true)
        {
            coneDown.SetActive(false);
            coneRight.SetActive(false);
        }
    }


}
