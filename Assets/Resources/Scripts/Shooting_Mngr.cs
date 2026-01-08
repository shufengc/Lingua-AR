using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Shooting_Mngr : MonoBehaviour
{
    public GameObject bullet_prefab;
    public AudioSource Shootaudio;
    public Text ScoreTxt;
    public int ScoreNum;
    public int SeriesNum;
    private float speed = 400f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        ScoreTxt.text = $"Total Score : {ScoreNum}";
    }

    public void OnShooting()
    {
        GameObject obj = GameObject.Instantiate(bullet_prefab);

        Shootaudio.Play();

        Debug.Log(Camera.main.transform.position);

        Vector3 forwardVector = Camera.main.transform.forward;

        string inputWord = GameObject.Find("InputWord").GetComponent<InputField>().text;

        Debug.Log("inputword: " + inputWord);

        obj.GetComponent<Bullet_Mngr>().SetLabel(LanguageDictionary.TranslateToEnglish(inputWord));

        obj.GetComponent<Transform>().position = Camera.main.transform.position;
        obj.transform.rotation= Camera.main.transform.rotation;
        obj.GetComponent<Rigidbody>().velocity = forwardVector * speed /*20.0f*/;
        Debug.Log(obj.GetComponent<Rigidbody>().velocity);
        Debug.Log(LanguageDictionary.TranslateToEnglish(inputWord));


        GameObject.Destroy(obj, 5f);
    }
}
