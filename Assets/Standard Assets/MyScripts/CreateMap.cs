using UnityEngine;
using System.Collections;

public class CreateMap: MonoBehaviour
{

    public GameObject green1;
    public GameObject green2;
    public GameObject frontCar;

    int border = 1000;

    void Awake()
    {
        green1 = GameObject.Find("Green1");
        green2 = GameObject.Find("Green2");
        frontCar = GameObject.FindWithTag("FrontCar");
    }

    void Update()
    {
        if (frontCar.transform.position.z > border)
        {
            MoveMap();
        }
    }

    void MoveMap()
    {
        if (green1.transform.position.z < border)
        {
            border += 2000;
            Vector3 temp = new Vector3(0, 0.05f, border);
            green1.transform.position = temp;
        }
        else if (green2.transform.position.z < border)
        {
            border += 2000;
            Vector3 temp = new Vector3(0, 0.05f, border);
            green2.transform.position = temp;
        }
    }
}