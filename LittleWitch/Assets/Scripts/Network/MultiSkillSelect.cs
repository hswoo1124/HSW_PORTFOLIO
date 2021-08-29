using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiSkillSelect : MonoBehaviour
{
    public List<int> selectSkillId_List = new List<int>();
    public string username;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
