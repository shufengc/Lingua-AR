using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Mngr : MonoBehaviour
{
    static private int seq;
    private string label;
    private int seq_num;
    // Start is called before the first frame update
    void Start()
    {
        seq_num = Bullet_Mngr.seq;
        Bullet_Mngr.seq++;
        Debug.Log("Start Called, Seq: " + Bullet_Mngr.seq);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLabel(string new_label)
    {
        Debug.Log("set new label to" + new_label + "Seq" + seq_num);
        label = new_label;
        Debug.Log("New Label is" + label);
    }

    public string GetLabel()
    {
        Debug.Log("Get label" + label + "Seq" + seq_num);
        return label;
    }
}
