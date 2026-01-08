using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ar_assistant_manager : MonoBehaviour
{
    public GameObject arAssistantPrefab; // The prefab for the AR Assistant
    public GameObject hint;

    private GameObject arAssistantInstance;

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate the AR Assistant and set it as inactive initially
        arAssistantInstance = Instantiate(arAssistantPrefab);
        arAssistantInstance.SetActive(false);

        // Position the AR Assistant at a fixed position relative to the camera
        // Move the AR Assistant slightly to the right by increasing the x value
        arAssistantInstance.transform.SetParent(Camera.main.transform);
        arAssistantInstance.transform.localPosition = new Vector3(0.4f, 0, 2); // Adjust the x value as needed

        // Activate the AR Assistant
        ObjActive(true);
    }

    // Method to activate the AR Assistant
    public void ObjActive(bool isFirst = false)
    {
        arAssistantInstance.SetActive(true);
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
}