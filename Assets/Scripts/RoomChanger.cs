using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RoomChanger : MonoBehaviour
{
    public CinemachineVirtualCamera Maincam;
    public List<Transform> Rooms = new List<Transform>();
    public int RoomNumber;

    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform child in transform.GetComponentsInChildren<Transform>())
        {
            Rooms.Add(child);
            child.gameObject.AddComponent<WallSimulation>();
        }
        Rooms.Remove(transform);
        Destroy(GetComponent<WallSimulation>());
    }

    // Update is called once per frame
    void Update()
    {
        Maincam.Follow = Rooms[RoomNumber].transform;
    }
}
