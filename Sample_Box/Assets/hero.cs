using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class hero : MonoBehaviour {
    float speed = 0.1f;
    float hor, ver;
    float bonus = 0f;
    float healthy = 3;
    float posx, posy, posz;
    Box cubex = new Box();
    Box player = new Box();

    public GameObject cube;
	// Use this for initialization
	void Start () {
        posx = this.gameObject.transform.position.x;
        posy = this.gameObject.transform.position.y;
        posz = this.gameObject.transform.position.z;

        StartCoroutine(CreateCube());
        StartCoroutine(ChangeBox());

    }
	
	// Update is called once per frame
	void Update () {
        hor = Input.GetAxis("Horizontal");
        ver = Input.GetAxis("Vertical");

        Vector3 tr = this.gameObject.transform.position;
        this.gameObject.transform.position = new Vector3(tr.x + (speed * hor), 
                                                         tr.y, 
                                                         tr.z + (speed * ver));


        if (Input.GetKeyDown(KeyCode.Space))
        {
            Application.Quit();
        }
	}

    IEnumerator ChangeBox()
    {
        while(true)
        {
            WWWForm form = new WWWForm();
            form.AddField("id", "2");
            form.AddField("x", this.gameObject.transform.position.x.ToString());
            form.AddField("y", this.gameObject.transform.position.y.ToString());
            form.AddField("z", this.gameObject.transform.position.z.ToString());
            using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:12345/boxes/2", form))
            {
                yield return www.Send();
                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    if (www.isDone)
                    {
                        string jsonResult =
                            System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                        Debug.Log(jsonResult);
                        //ddlCountries.options.AddRange(entities.
                    }
                }
            }
            yield return new WaitForSeconds(float.Parse("0.1"));
        }

        //StopCoroutine(ChangeBox());
    }

    IEnumerator CreateCube()
    {

        WWWForm form = new WWWForm();
        form.AddField("id", "1");
        form.AddField("x", this.gameObject.transform.position.x.ToString());
        form.AddField("y", this.gameObject.transform.position.y.ToString());
        form.AddField("z", this.gameObject.transform.position.z.ToString());
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:12345/boxes/1/create", form))
        {
            yield return www.Send();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (www.isDone)
                {
                    string jsonResult =
                        System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                    Debug.Log(jsonResult);
                    //ddlCountries.options.AddRange(entities.
                }
            }
        }
        WWWForm form1 = new WWWForm();
        form1.AddField("id", "2");
        form1.AddField("x", cube.transform.position.x.ToString());
        form1.AddField("y", cube.transform.position.y.ToString());
        form1.AddField("z", cube.transform.position.z.ToString());
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:12345/boxes/2/create", form1))
        {
            yield return www.Send();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (www.isDone)
                {
                    string jsonResult =
                        System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                    Debug.Log(jsonResult);
                    //ddlCountries.options.AddRange(entities.
                }
            }
        }
        StartCoroutine(GetCube());
        StopCoroutine(CreateCube());
    }

    IEnumerator GetCube()
    {
        while (true)
        {
            string GetCube = "http://localhost:12345/boxes/2";
            using (UnityWebRequest www = UnityWebRequest.Get("http://localhost:12345/boxes/2"))
            {
                yield return www.Send();
                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    if (www.isDone)
                    {
                        string jsonResult =
                            System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                        Debug.Log(jsonResult);
                        Box entities =
                            JsonHelper.getJsonArray<Box>(jsonResult);
                        Debug.Log(entities.x);
                        //ddlCountries.options.AddRange(
                        //    entities.OrderBy(p => p.name).Select(x =>
                        //                  new UnityEngine.UI.Dropdown.OptionData()
                        //                  {
                        //                      text = x.name
                        //                  }).ToList());
                        //ddlCountries.value = 0;
                        Vector3 vec = new Vector3(float.Parse(entities.x), float.Parse(entities.y), float.Parse(entities.z));
                        cube.transform.position = vec;
                        //ddlCountries.options.AddRange(entities.
                    }
                }
            }
            yield return new WaitForSeconds(float.Parse("0.1"));
        }
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "bonus")
        {
            Destroy(other.gameObject);
            bonus++;
        }
        if(other.gameObject.tag == "Die")
        {
            if (healthy > 0)
            {
                this.gameObject.transform.position = new Vector3(posx, posy, posz);
                healthy--;
            }
            else
            {
                Application.LoadLevel(Application.loadedLevel);
            }
        }
    }
    private void OnGUI()
    {
        GUI.Box(new Rect(10f, 10f, 100f, 25f), "Bonus: " + bonus);
        GUI.Box(new Rect(10f, 35f, 100f, 25f), "Healthy: " + healthy);
    }
}
