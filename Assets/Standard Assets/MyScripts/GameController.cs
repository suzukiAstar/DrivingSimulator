using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;
using UnityStandardAssets.CrossPlatformInput;

public enum GameState
{
    Ready,
    Game,
    Pause,
    Result
}

public enum Brightness
{
    Day,
    Night
}

public enum CarColor
{
    White,
    Gray,
    Black
}

public enum BackGround
{

    Grass,
    Town
}

public class RigidbodyVelocity
{
    public Vector3 velocity;
    public Vector3 angularVelocity;
    public RigidbodyVelocity(Rigidbody rigidbody)
    {
        velocity = rigidbody.velocity;
        angularVelocity = rigidbody.angularVelocity;
    }
}

public class GameController : MonoBehaviour
{

    public GameState state;
    public float gameTime;
    private float logTimer = 0f;
    public int experimentTime;
    public List<int> brakingTimeList = new List<int>();
    public List<int> personTimeList = new List<int>();
    public int brakeCount = 0;
    public Brightness gameBright;
    public CarColor carColor;
    public BackGround backGround;

    public string inputFileName = "";
    public string outputFileName = "";

    public Texture2D meterBackTexture;
    public Texture2D meterArrowTexture;

    public GameObject playerCar;
    public GameObject frontCar;
    public GameObject sunLight;

    public RigidbodyVelocity[] rigidbodyVelocities = new RigidbodyVelocity[2];
    public Rigidbody[] pausingRigidbodies = new Rigidbody[2];
    public GameObject[] pausingGameObjects = new GameObject[2];


    //public string[] selBrightness = new string[]
    //{
    //    "Day",
    //    "Night",
    //};

    //public string[] selColor = new string[]
    //{
    //    "White",
    //    "Gray",
    //    "Black",
    //};

    //public string[] selBackground = new string[]
    //{
    //    "Grass",
    //    "Town",
    //};

    // Use this for initialization
    void Start()
    {
        state = GameState.Ready;
        playerCar = GameObject.FindWithTag("PlayerCar");
        frontCar = GameObject.FindWithTag("FrontCar");
        sunLight = GameObject.Find("SunLight");
        outputFileName = "log_" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + ".csv";
        //FirstLog();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == GameState.Game || state == GameState.Pause)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                state = GameState.Result;
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                Pause();
            }
        }
    }

    void FixedUpdate()
    {
        if (state == GameState.Game)
        {
            gameTime += Time.deltaTime;
            logTimer += Time.deltaTime;
            if (logTimer >= 0.1f)
            {
                OutputLog();
                logTimer = 0f;
            }
            if (brakeCount <= brakingTimeList.Count - 1 && gameTime >= brakingTimeList[brakeCount])
            {
                brakeCount++;
                frontCar.GetComponent<UnityStandardAssets.Vehicles.Car.FrontCarControl>().StartBraking();
            }

            if (gameTime >= experimentTime)
            {
                state = GameState.Result;
            }

        }
    }

    void SetOption()
    {
        FileInfo fi = new FileInfo(inputFileName);
        StreamReader sr = new StreamReader(fi.OpenRead());

        if (sr.Peek() >= 0)
        {
            string options = sr.ReadLine();
        }
        if (sr.Peek() >= 0)
        {
            string firstBuffer = sr.ReadLine();
            string[] firstValues = firstBuffer.Split(',');
            experimentTime = int.Parse(firstValues[0]);
            switch (firstValues[1])
            {
                case "Black":

                    frontCar.transform.Find("SkyCar/SkyCarBody").GetComponent<Renderer>().material.color = Color.black;
                    break;
                case "Gray":
                    Debug.Log("ok");
                    frontCar.transform.Find("SkyCar/SkyCarBody").GetComponent<Renderer>().material.color = Color.gray;
                    break;
                case "White":
                    frontCar.transform.Find("SkyCar/SkyCarBody").GetComponent<Renderer>().material.color = Color.white;
                    break;
                default:
                    Debug.Log("dame");
                    break;
            }
            switch (firstValues[2])
            {
                case "Day":
                    sunLight.transform.rotation = Quaternion.Euler(100, -30, 0);
                    break;
                case "Night":
                    sunLight.transform.rotation = Quaternion.Euler(-12, -30, 0);
                    break;
                default:
                    break;
            }
            brakingTimeList.Add(int.Parse(firstValues[3]));
            personTimeList.Add(int.Parse(firstValues[4]));
        }
        while (sr.Peek() >= 0)
        {
            string stBuffer = sr.ReadLine();
            string[] values = stBuffer.Split(',');
            brakingTimeList.Add(int.Parse(values[3]));
            personTimeList.Add(int.Parse(values[4]));

        }

        sr.Close();
    }

    void FirstLog()
    {
        string logText = "";
        FileInfo fi = new FileInfo(Application.dataPath + "/" + outputFileName);
        StreamWriter sw = fi.AppendText();
        logText = System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + System.DateTime.Now.Second.ToString();
        sw.WriteLine(logText);
        logText = "Time,Accel,Brake,Distance,Driven,Gap,PlayerSpeed,FrontBrake,FrontSpeed";
        sw.WriteLine(logText);
        sw.Close();
    }

    void OutputLog()
    {
        string logText = "";
        FileInfo fi = new FileInfo(Application.dataPath + "/" + outputFileName);
        StreamWriter sw = fi.AppendText();
        logText = "" + System.Environment.TickCount + ","
                // + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "." + DateTime.Now.Millisecond + ", "
                //    + "Vertical = " + Input.GetAxisRaw("Vertical") + ", "
                + Math.Max(Input.GetAxisRaw("Vertical"), 0) + ","
                + Math.Abs(Math.Min(Input.GetAxisRaw("Vertical"), 0)) + ","
                + Vector3.Distance(frontCar.transform.position, playerCar.transform.position) + ","
                + Vector3.Distance(new Vector3(-1.5f, 0.1f, -12f), playerCar.transform.position) + ","
                + (float)(playerCar.transform.position.x + 1.5f) + ","
                + playerCar.GetComponent<Rigidbody>().velocity.magnitude + ","
                + Convert.ToInt32(frontCar.GetComponent<UnityStandardAssets.Vehicles.Car.FrontCarControl>().isBraking) + ","
                + frontCar.GetComponent<Rigidbody>().velocity.magnitude;
        sw.WriteLine(logText);
        sw.Close();
    }

    void OnGUI()
    {
        int sh = Screen.height;
        int sw = Screen.width;

        switch (state)
        {
            case GameState.Ready:
                inputFileName = GUI.TextField(new Rect(sw * 3 / 8, sh / 20, sw / 16, sh / 20), inputFileName);
                if (GUI.Button(new Rect(sw * 3 / 8, sh / 9, sw / 24, sh / 18), "Input"))
                {
                    SetOption();
                }
                if (GUI.Button(new Rect(sw * 20 / 48, sh / 2, sw / 24, sh / 8), "Start"))
                {
                    FirstLog();
                    state = GameState.Game;
                }
                break;
            case GameState.Game:
                int x = sw * 11 / 24;
                int y = sh * 4 / 5;
                int w = sw / 24;
                int h = sw / 24;
                float rot = playerCar.GetComponent<Rigidbody>().velocity.magnitude * 5f + 17;

                Vector2 pivotPoint = new Vector2(x + w / 2, y + h / 2);
                GUI.DrawTexture(new Rect(x, y, w, h), meterBackTexture);
                GUIUtility.RotateAroundPivot(rot, pivotPoint);
                GUI.DrawTexture(new Rect(x, y, w, h), meterArrowTexture);
                GUIUtility.RotateAroundPivot(-rot, pivotPoint);
                break;
            case GameState.Pause:
                if (GUI.Button(new Rect(sw * 20 / 48, sh / 2, sw / 24, sh / 8), "Resume"))
                {
                    Pause();
                }
                break;
            case GameState.Result:
                if (GUI.Button(new Rect(sw * 20 / 48, sh / 2, sw / 24, sh / 8), "Finish"))
                {
                    Application.LoadLevel(0);
                }
                break;
            default:
                break;
        }
    }

    void Pause()
    {
        switch (state)
        {
            case GameState.Game:
                state = GameState.Pause;
                for (int i = 0; i < pausingGameObjects.Length; i++)
                {
                    pausingRigidbodies[i] = pausingGameObjects[i].GetComponent<Rigidbody>();
                }
                for (int i = 0; i < pausingRigidbodies.Length; i++)
                {
                    rigidbodyVelocities[i] = new RigidbodyVelocity(pausingRigidbodies[i]);
                    pausingRigidbodies[i].isKinematic = true;
                }
                break;
            case GameState.Pause:
                state = GameState.Game;
                for (int i = 0; i < pausingRigidbodies.Length; i++)
                {
                    pausingRigidbodies[i].isKinematic = false;
                    pausingRigidbodies[i].velocity = rigidbodyVelocities[i].velocity;
                    pausingRigidbodies[i].angularVelocity = rigidbodyVelocities[i].angularVelocity;
                }
                break;
            default:
                break;
        }
    }
}
