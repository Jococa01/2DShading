using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class WallSimulation : MonoBehaviour
{
    //public GameObject Player;
    public FloorAlgorithm algorithm;
    public RoomDetection RoomDetection;

    Tilemap tilemap;
    TilemapRenderer Tilerenderer;
    GameObject paco;

    public bool Inside = false;
    public int ID;
    public int Ygen;

    public bool start = true;

    public List<Vector3> SalaNorte = new List<Vector3>();
    public List<Vector3> SalaSur = new List<Vector3>();
    public List<Vector3> SalaEste = new List<Vector3>();
    public List<Vector3> SalaOeste = new List<Vector3>();

    [Header("SalasContiguas")]
    public bool Norte = false;
    public bool Sur = false;
    public bool Este = false;
    public bool Oeste = false;

    // Start is called before the first frame update
    void Start()
    {
        //Asignando a cada variable su componente
        RoomDetection = transform.GetComponent<RoomDetection>();
        gameObject.AddComponent<Grid>();
        algorithm = FindObjectOfType<FloorAlgorithm>();

        paco = new GameObject();
        paco.name = "paco";
        paco.transform.parent = transform;
        paco.transform.position = Vector3.zero;

        tilemap = paco.AddComponent<Tilemap>();
        Tilerenderer = paco.AddComponent<TilemapRenderer>();
        paco.AddComponent<TilemapCollider2D>();

        ID = RoomDetection.RoomNumber;
        tilemap.tileAnchor = Vector3.zero;

        int Dimensiones = algorithm.Dimensiones;
        Ygen = algorithm.Ygen;

        Tile DaTile = algorithm.Muro_Blank;
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
        for (int x = 0; x < (Dimensiones); x++)
        {
            Vector3Int border = new Vector3Int(Mathf.FloorToInt(HArray[ID].transform.position.x + x), Mathf.FloorToInt(HArray[ID].transform.position.y + Ygen), 0);
            tilemap.SetTile(border, DaTile);

            border = new Vector3Int(Mathf.FloorToInt(HArray[ID].transform.position.x + x), Mathf.FloorToInt(HArray[ID].transform.position.y - Ygen), 0);
            tilemap.SetTile(border, DaTile);

            border = new Vector3Int(Mathf.FloorToInt(HArray[ID].transform.position.x - x), Mathf.FloorToInt(HArray[ID].transform.position.y + Ygen), 0);
            tilemap.SetTile(border, DaTile);

            border = new Vector3Int(Mathf.FloorToInt(HArray[ID].transform.position.x - x), Mathf.FloorToInt(HArray[ID].transform.position.y - Ygen), 0);
            tilemap.SetTile(border, DaTile);
        }

        for (int e = 0; e < 4; e++)
        {
            Vector3Int border = new Vector3Int(Mathf.FloorToInt(HArray[ID].transform.position.x - Dimensiones), Mathf.FloorToInt(HArray[ID].transform.position.y + Ygen), 0);
            if (e == 0)
            {
                tilemap.SetTile(border, DaTile);
            }
            if (e == 1)
            {
                border = new Vector3Int(Mathf.FloorToInt(HArray[ID].transform.position.x + Dimensiones), Mathf.FloorToInt(HArray[ID].transform.position.y + Ygen), 0);
                tilemap.SetTile(border, DaTile);
            }
            if (e == 2)
            {
                border = new Vector3Int(Mathf.FloorToInt(HArray[ID].transform.position.x - Dimensiones), Mathf.FloorToInt(HArray[ID].transform.position.y - Ygen), 0);
                tilemap.SetTile(border, DaTile);
            }
            if (e == 3)
            {
                border = new Vector3Int(Mathf.FloorToInt(HArray[ID].transform.position.x + Dimensiones), Mathf.FloorToInt(HArray[ID].transform.position.y - Ygen), 0);
                tilemap.SetTile(border, DaTile);
            }
        }

        start = false;

        StartCoroutine(LateStart());

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
            if (start==false)
            {
                StartCoroutine(FadeWapo(.5f, 0, 1));
            }
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

    void Puertas()
    {
        GameObject[] HArray = algorithm.Habitaciones.ToArray();

        if (Norte == true)
        {
            tilemap.SetTile(new Vector3Int(Mathf.FloorToInt(HArray[ID].transform.position.x), Mathf.FloorToInt(HArray[ID].transform.position.y +Ygen), 0), null);
            tilemap.SetTile(new Vector3Int(Mathf.FloorToInt(HArray[ID].transform.position.x+1), Mathf.FloorToInt(HArray[ID].transform.position.y + Ygen), 0), null);
            tilemap.SetTile(new Vector3Int(Mathf.FloorToInt(HArray[ID].transform.position.x-1), Mathf.FloorToInt(HArray[ID].transform.position.y + Ygen), 0), null);

            if (algorithm.Ygen < 10)
            {
                int dif = 10 - Ygen;

                for(int RP=0; RP<=dif; RP++)
                {
                    tilemap.SetTile(new Vector3Int(Mathf.FloorToInt(HArray[ID].transform.position.x + 2), Mathf.FloorToInt(HArray[ID].transform.position.y + Ygen + RP), 0), algorithm.Muro_Blank);
                    tilemap.SetTile(new Vector3Int(Mathf.FloorToInt(HArray[ID].transform.position.x - 2), Mathf.FloorToInt(HArray[ID].transform.position.y + Ygen + RP), 0), algorithm.Muro_Blank);
                }
            }
        }
        if (Sur == true)
        {
            tilemap.SetTile(new Vector3Int(Mathf.FloorToInt(HArray[ID].transform.position.x), Mathf.FloorToInt(HArray[ID].transform.position.y - Ygen), 0), null);
            tilemap.SetTile(new Vector3Int(Mathf.FloorToInt(HArray[ID].transform.position.x + 1), Mathf.FloorToInt(HArray[ID].transform.position.y - Ygen), 0), null);
            tilemap.SetTile(new Vector3Int(Mathf.FloorToInt(HArray[ID].transform.position.x - 1), Mathf.FloorToInt(HArray[ID].transform.position.y - Ygen), 0), null);

            if (algorithm.Ygen < 10)
            {
                int dif = 10 - Ygen;

                for (int RP = 0; RP <= dif; RP++)
                {
                    tilemap.SetTile(new Vector3Int(Mathf.FloorToInt(HArray[ID].transform.position.x + 2), Mathf.FloorToInt(HArray[ID].transform.position.y - Ygen - RP), 0), algorithm.Muro_Blank);
                    tilemap.SetTile(new Vector3Int(Mathf.FloorToInt(HArray[ID].transform.position.x - 2), Mathf.FloorToInt(HArray[ID].transform.position.y - Ygen - RP), 0), algorithm.Muro_Blank);
                }
            }
        }
        if (Este == true)
        {
            tilemap.SetTile(new Vector3Int(Mathf.FloorToInt(HArray[ID].transform.position.x +algorithm.Dimensiones), Mathf.FloorToInt(HArray[ID].transform.position.y), 0), null);
            tilemap.SetTile(new Vector3Int(Mathf.FloorToInt(HArray[ID].transform.position.x + algorithm.Dimensiones), Mathf.FloorToInt(HArray[ID].transform.position.y - 1), 0), null);
            tilemap.SetTile(new Vector3Int(Mathf.FloorToInt(HArray[ID].transform.position.x + algorithm.Dimensiones), Mathf.FloorToInt(HArray[ID].transform.position.y +1), 0), null);

            if (algorithm.Dimensiones < 18)
            {
                int dif = 18 - algorithm.Dimensiones;

                for (int RP = 0; RP <= dif; RP++)
                {
                    tilemap.SetTile(new Vector3Int(Mathf.FloorToInt(HArray[ID].transform.position.x + algorithm.Dimensiones + RP), Mathf.FloorToInt(HArray[ID].transform.position.y - 2), 0), algorithm.Muro_Blank);
                    tilemap.SetTile(new Vector3Int(Mathf.FloorToInt(HArray[ID].transform.position.x + algorithm.Dimensiones + RP), Mathf.FloorToInt(HArray[ID].transform.position.y + 2), 0), algorithm.Muro_Blank);
                }
            }
        }
        if (Oeste == true)
        {
            tilemap.SetTile(new Vector3Int(Mathf.FloorToInt(HArray[ID].transform.position.x - algorithm.Dimensiones), Mathf.FloorToInt(HArray[ID].transform.position.y), 0), null);
            tilemap.SetTile(new Vector3Int(Mathf.FloorToInt(HArray[ID].transform.position.x - algorithm.Dimensiones), Mathf.FloorToInt(HArray[ID].transform.position.y - 1), 0), null);
            tilemap.SetTile(new Vector3Int(Mathf.FloorToInt(HArray[ID].transform.position.x - algorithm.Dimensiones), Mathf.FloorToInt(HArray[ID].transform.position.y + 1), 0), null);

            if (algorithm.Dimensiones < 18)
            {
                int dif = 18 - algorithm.Dimensiones;

                for (int RP = 0; RP <= dif; RP++)
                {
                    tilemap.SetTile(new Vector3Int(Mathf.FloorToInt(HArray[ID].transform.position.x - algorithm.Dimensiones - RP), Mathf.FloorToInt(HArray[ID].transform.position.y - 2), 0), algorithm.Muro_Blank);
                    tilemap.SetTile(new Vector3Int(Mathf.FloorToInt(HArray[ID].transform.position.x - algorithm.Dimensiones - RP), Mathf.FloorToInt(HArray[ID].transform.position.y + 2), 0), algorithm.Muro_Blank);
                }
            }
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

    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(.15f);
        Puertas();
    }
}
