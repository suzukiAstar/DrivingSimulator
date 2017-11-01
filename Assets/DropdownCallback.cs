using UnityEngine;
using System.Collections;

public enum Option
{
    Brightness,
    CarColor,
    BackGround
}

public class DropdownCallback : MonoBehaviour {

    public Option option;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnValueChanged(int result)
    {
        if (result != 0)
        {
            ValueChange((int)option, result);
        }
    }

    void ValueChange(int op, int num)
    {
        switch (op)
        {
            case (int)Option.Brightness:
                if(num == 1)
                {

                }
                else if(num == 2)
                {

                }
                break;
            case (int)Option.CarColor:
                if (num == 1)
                {
                    GameObject.FindWithTag("FrontCar").transform.Find("Body").GetComponent<Renderer>().material.color = Color.black;
                }
                else if (num == 2)
                {
                    GameObject.FindWithTag("FrontCar").transform.Find("Body").GetComponent<Renderer>().material.color = Color.gray;
                }
                else if(num == 3)
                {
                    
                }
                break;
            case (int)Option.BackGround:
                if (num == 1)
                {

                }
                else if (num == 2)
                {

                }
                break;
            default:
                break;
        }
    }
}
