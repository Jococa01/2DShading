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

    public bool Inside = false;
    public int ID;

    public bool start = true;

    public List<Vector3> SalaNorte = new List<Vector3>();
    public List<Vector3> SalaSur = new List<Vector3>();
    public List<Vector3> SalaEste = new List<Vector3>();
    public List<Vector3> SalaOeste = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        //Asignando a cada variable su componente
        RoomDetection = transform.GetComponent<RoomDetection>();
        gameObject.AddComponent<Grid>();
        algorithm = FindObjectOfType<FloorAlgorithm>();

        GameObject paco = new GameObject();
        paco.transform.parent = transform;
        paco.transform.position = Vector3.zero;

        tilemap = paco.AddComponent<Tilemap>();
        Tilerenderer = paco.AddComponent<TilemapRenderer>();
        paco.AddComponent<TilemapCollider2D>();

        ID = RoomDetection.RoomNumber;
        tilemap.tileAnchor = Vector3.zero;

        int Dimensiones = algorithm.Dimensiones;
        int Ygen = algorithm.Dimensiones - 6;

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
        for (int y2 = 0; y2 < Ygen; y2++)
        {
            Vector3Int border = new Vector3Int(Mathf.FloorToInt(HArray[ID].transform.position.x + (Dimensiones+1)), Mathf.FloorToInt(HArray[ID].transform.position.y + y2), 0);
            tilemap.SetTile(border, DaTile);

            border = new Vector3Int(Mathf.FloorToInt(HArray[ID].transform.position.x + (Dimensiones+1)), Mathf.FloorToInt(HArray[ID].transform.position.y - y2), 0);
            tilemap.SetTile(border, DaTile);

            border = new Vector3Int(Mathf.FloorToInt(HArray[ID].transform.position.x - (Dimensiones+1)), Mathf.FloorToInt(HArray[ID].transform.position.y + y2), 0);
            tilemap.SetTile(border, DaTile);

            border = new Vector3Int(Mathf.FloorToInt(HArray[ID].transform.position.x - (Dimensiones+1)), Mathf.FloorToInt(HArray[ID].transform.position.y - y2), 0);
            tilemap.SetTile(border, DaTile);
        }
        for (int y3 = 0; y3 < Ygen; y3++)
        {
            Vector3Int border = new Vector3Int(Mathf.FloorToInt(HArray[ID].transform.position.x + (Dimensiones + 2)), Mathf.FloorToInt(HArray[ID].transform.position.y + y3), 0);
            tilemap.SetTile(border, DaTile);

            border = new Vector3Int(Mathf.FloorToInt(HArray[ID].transform.position.x + (Dimensiones + 2)), Mathf.FloorToInt(HArray[ID].transform.position.y - y3), 0);
            tilemap.SetTile(border, DaTile);

            border = new Vector3Int(Mathf.FloorToInt(HArray[ID].transform.position.x - (Dimensiones + 2)), Mathf.FloorToInt(HArray[ID].transform.position.y + y3), 0);
            tilemap.SetTile(border, DaTile);

            border = new Vector3Int(Mathf.FloorToInt(HArray[ID].transform.position.x - (Dimensiones + 2)), Mathf.FloorToInt(HArray[ID].transform.position.y - y3), 0);
            tilemap.SetTile(border, DaTile);
        }
        for (int x = 0; x < (Dimensiones+2); x++)
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


        for (int n=0; n<algorithm.Habitaciones.ToArray().Length; n++)
        {
                foreach(GameObject sala in HArray)
                {
                    //Si tiene sala contigua al norte
                    if(sala.transform.position.y - HArray[n].transform.position.y == algorithm.Altura && sala.transform.position.x - HArray[n].transform.position.x ==0)
                    {
                        SalaNorte.Add(HArray[n].transform.position);
                        Debug.Log("Sala al norte");
                    }
                    //Si tiene sala contigua al sur
                    if (sala.transform.position.y - HArray[n].transform.position.y == -algorithm.Altura && sala.transform.position.x - HArray[n].transform.position.x == 0)
                    {
                        SalaSur.Add(HArray[n].transform.position);
                        Debug.Log("Sala al sur");
                    }
                    //Si tiene sala contigua al este
                    if (sala.transform.position.y - HArray[n].transform.position.y == 0 && sala.transform.position.x - HArray[n].transform.position.x == algorithm.Anchura)
                    {
                        SalaEste.Add(HArray[n].transform.position);
                        Debug.Log("Sala al este");
                    }
                    //Si tiene sala contigua al oeste
                    if (sala.transform.position.y - HArray[n].transform.position.y == 0 && sala.transform.position.x - HArray[n].transform.position.x == -algorithm.Anchura)
                    {
                        SalaOeste.Add(HArray[n].transform.position);
                        Debug.Log("Sala al oeste");
                    }
            }
        }

        SalaNorte = SalaNorte.Distinct().ToList();
        SalaSur = SalaSur.Distinct().ToList();
        SalaEste = SalaEste.Distinct().ToList();
        SalaOeste = SalaOeste.Distinct().ToList();

        start = false;

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
