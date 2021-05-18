using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WallSimulation : MonoBehaviour
{
    //public GameObject Player;
    public FloorAlgorithm algorithm;
    public RoomDetection RoomDetection;

    Tilemap tilemap;
    TilemapRenderer Tilerenderer;

    public bool Inside = false;
    public int ID;

    // Start is called before the first frame update
    void Start()
    {
        //Asignando a cada variable su componente
        RoomDetection = transform.GetComponent<RoomDetection>();
        gameObject.AddComponent<Grid>();
        algorithm = FindObjectOfType<FloorAlgorithm>();

        GameObject paco = new GameObject();
        paco.transform.parent = transform;

        tilemap = paco.AddComponent<Tilemap>();
        Tilerenderer = paco.AddComponent<TilemapRenderer>();

        ID = RoomDetection.RoomNumber;
        tilemap.tileAnchor = Vector3.zero;

        int Dimensiones = algorithm.Dimensiones;
        int Ygen = algorithm.Dimensiones - 6;

        Tile DaTile = algorithm.Muro_d;
        GameObject[] HArray = algorithm.Habitaciones.ToArray();

        for(int y=0; y < Ygen; y++)
        {
            Vector3Int border = new Vector3Int(Mathf.FloorToInt(HArray[ID].transform.position.x + Dimensiones), Mathf.FloorToInt(HArray[ID].transform.position.y + y), 0);
            tilemap.SetTile(border, DaTile);

            border = new Vector3Int(Mathf.FloorToInt(HArray[ID].transform.position.x + Dimensiones), Mathf.FloorToInt(HArray[ID].transform.position.y - y), 0);
            tilemap.SetTile(border, DaTile);

            border = new Vector3Int(Mathf.FloorToInt(HArray[ID].transform.position.x - Dimensiones), Mathf.FloorToInt(HArray[ID].transform.position.y + y), 0);
            tilemap.SetTile(border, DaTile);

            border = new Vector3Int(Mathf.FloorToInt(HArray[ID].transform.position.x - Dimensiones), Mathf.FloorToInt(HArray[ID].transform.position.y - y), 0);
            tilemap.SetTile(border, DaTile);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            Debug.Log("Entra el jugador");
            Inside = true;
            StopAllCoroutines();
            StartCoroutine(FadeWapo(.5f, 0, 1));
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            Debug.Log("El jugador sale de la habitación");
            Inside = false;
            StopAllCoroutines();
            StartCoroutine(FadeWapo(2, 1, 0));
        }
    }

    IEnumerator FadeWapo(float duration, float start, float end)
    {
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;
            //right here, you can now use normalizedTime as the third parameter in any Lerp from start to end
            tilemap.color = new Color(1, 1, 1, Mathf.Lerp(start, end, normalizedTime));
            yield return null;
        }
    }
}
