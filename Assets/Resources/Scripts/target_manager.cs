using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class target_manager : MonoBehaviour
{
    public List<GameObject> objPrefabList;
    public GameObject hint;

    // currently not used, but maybe used later
    private Dictionary<string, int> objMap = new Dictionary<string, int>
    {
        { "tv monitor", 0 },
        { "chair", 1 },
        { "keyboard", 2 },
        { "book", 3 },
        { "laptop", 4 },
        { "cell phone", 5 },
        { "mouse", 6 },
        { "apple", 7 },
        { "bottle", 8 }
     };

    //private int selectedObjNum = 3;
    private int selectedObjNum = 9;
    private List<GameObject> gameObjects = new List<GameObject>();
    private int ObjIndex = 3;

    // Start is called before the first frame update
    void Start()
    {
        List<int> selectedIndex = SelectRandomNumbers(selectedObjNum, 9);

        foreach (int ind in selectedIndex)
        {
            GameObject obj = GameObject.Instantiate(objPrefabList[ind]);
            obj.SetActive(false);
            gameObjects.Add(obj);
        }
        ObjActive(true);
    }

    public void ObjActive(bool isFirst = false)
    {
        if (isFirst)
        {
            for (int i = 0; i < ObjIndex; i++)
            {
                gameObjects[i].SetActive(true);
            }
            return;
        }
        if (ObjIndex < 9)
            gameObjects[ObjIndex++].SetActive(true);
        Debug.Log("ObjIndex" + ObjIndex);
    }

    // Update is called once per frame
    void Update()
    {
        // Create a ray from the camera's position along its forward vector
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        // Create a RaycastHit variable to store information about the hit object
        RaycastHit hit;
        // Set the maximum raycast distance (adjust this as needed)
        float maxRaycastDistance = 100f;
        // Perform the raycast
        if (Physics.Raycast(ray, out hit, maxRaycastDistance))
        {
            // Check if the ray hit a GameObject
            GameObject hitObject = hit.collider.gameObject;
            // Do something with the hitObject
            floatingTarget floattarget = hitObject.GetComponent<floatingTarget>();
            if (floattarget != null)
            {
                // If the component exists, get the "label" field
                string label = floattarget.label;
                label = LanguageDictionary.TranslateToOther(label, LanguageSelection.Get_Language());
                Debug.Log(label);
                hint.GetComponent<Text>().text = "Targeting At\n" + label;
            }
        }
    }

    private List<int> SelectRandomNumbers(int x, int k)
    {
        List<int> selectedNumbers = new List<int>();
        List<int> availableNumbers = new List<int>();

        // Fill the availableNumbers list with numbers from 1 to k
        for (int i = 0; i < k; i++)
        {
            availableNumbers.Add(i);
        }

        for (int i = 0; i < x; i++)
        {
            int randomIndex = Random.Range(0, availableNumbers.Count);
            int selectedNumber = availableNumbers[randomIndex];

            selectedNumbers.Add(selectedNumber);
            availableNumbers.RemoveAt(randomIndex);
        }

        return selectedNumbers;
    }
}