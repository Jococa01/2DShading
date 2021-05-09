using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FloorAlgorithm : MonoBehaviour
{
    [Header("Listas de Objetos")]
    public List<GameObject> Habitaciones = new List<GameObject>();
    public List<GameObject> Cámaras = new List<GameObject>();
    public List<Vector3> HPosiciones = new List<Vector3>();

    public int NHabitaciones, Dimensiones, IDHabitación;
    public Tile Textura1;
    public Tilemap MapaBase, Muro;
    public Tile[] SueloRandom;
    public GameObject AdministradorH, AdministradorCam;

    public int CordenadaID = 0;
    void Start()
    {
        for(int n=0; n<NHabitaciones; n++)
        {
            GenerarBase();
        }

        Relocate();
    }

    void GenerarBase()
    {

        //Creo el gameobject de tipo Habitación primero y le añado los componentes correspondientes y un nombre que varia en función de su tiempo de creación

        string Nombre = "Habitación" + IDHabitación;
        GameObject gameobj = new GameObject();
        gameobj.name = Nombre;
        //gameobj.AddComponent<>();
        gameobj.AddComponent<Grid>();

        Habitaciones.Add(gameobj);
        gameobj.transform.parent = AdministradorH.transform;

        //Creo el hijo del gameobject recién creado y le asigno una serie de componentes que usaré en adelante

        GameObject childofobj = new GameObject();
        childofobj.AddComponent<Tilemap>();
        childofobj.AddComponent<TilemapRenderer>();
        childofobj.GetComponent<Tilemap>().tileAnchor = Vector3.zero;
        childofobj.transform.parent = gameobj.transform;

        Tilemap LocalTM = childofobj.GetComponent<Tilemap>();

        //Creo el gameobject de tipo cámara

        GameObject Camobj = new GameObject();
        Camobj.name = Nombre;

        Camobj.AddComponent<RoomDetection>();
        Camobj.AddComponent<BoxCollider2D>();
        Camobj.tag = "Floor";

        Camobj.GetComponent<RoomDetection>().RoomNumber = IDHabitación;
        Camobj.GetComponent<BoxCollider2D>().size = new Vector2(38, 22);
        Camobj.GetComponent<BoxCollider2D>().isTrigger=true;

        Camobj.transform.parent = AdministradorCam.transform;
        Cámaras.Add(Camobj);


        int Ygen = Dimensiones / 2;

        //Generaión del espacio en el 1er cuadrante
        for (int y = 0; y < Ygen; y++)
        {
            int Y = y;
            for (int x = 0; x > -Dimensiones; x--)
            {
                Vector3Int Posicion = new Vector3Int(Mathf.FloorToInt(x), Mathf.FloorToInt(Y), 0);
                LocalTM.SetTile(Posicion, Textura1);
            }
        }
        //Generaión del espacio en el 2o cuadrante
        for (int y = 0; y < Ygen; y++)
        {
            int Y = y;
            for (int x = 0; x < Dimensiones; x++)
            {
                Vector3Int Posicion = new Vector3Int(Mathf.FloorToInt(x), Mathf.FloorToInt(Y), 0);
                LocalTM.SetTile(Posicion, Textura1);
            }
        }
        //Generaión del espacio en el 3er cuadrante
        for (int y = 0; y > -Ygen; y--)
        {
            int Y = y;
            for (int x = 0; x < Dimensiones; x++)
            {
                Vector3Int Posicion = new Vector3Int(Mathf.FloorToInt(x), Mathf.FloorToInt(Y), 0);
                LocalTM.SetTile(Posicion, Textura1);
            }
        }
        //Generaión del espacio en el 4o cuadrante
        for (int y = 0; y > -Ygen; y--)
        {
            int Y = y;
            for (int x = 0; x > -Dimensiones; x--)
            {
                Vector3Int Posicion = new Vector3Int(Mathf.FloorToInt(x), Mathf.FloorToInt(Y), 0);
                LocalTM.SetTile(Posicion, Textura1);
            }
        }

        IDHabitación++;
    }
    void GenerarMuro()
    {

    }
    void EmebellecerSuelo()
    {

    }
    void Relocate()
    {
        GameObject[] HArray = Habitaciones.ToArray();
        GameObject[] CArray = Cámaras.ToArray();
        for (int n =0; n < HArray.Length; n++)
        {
            if (n == 0)
            {
                HArray[n].transform.position = Vector3.zero;
            }
            else if(n > 0)
            {
                int RandomCoord = Random.Range(1, 5);
                Vector3 RelativePos = Vector3.zero;

                switch (RandomCoord)
                {
                    case 1:
                        RelativePos = new Vector3(HArray[n - 1].transform.position.x + 38, HArray[n - 1].transform.position.y, 0);
                        Debug.Log("Este con coords en " + RelativePos);
                        CordenadaID = 1;
                        break;
                    case 2:
                        RelativePos = new Vector3(HArray[n - 1].transform.position.x - 38, HArray[n - 1].transform.position.y, 0);
                        Debug.Log("Oeste con coords en " + RelativePos);
                        CordenadaID = 2;
                        break;
                    case 3:
                        RelativePos = new Vector3(HArray[n - 1].transform.position.x, HArray[n - 1].transform.position.y + 22, 0);
                        Debug.Log("Norte con coords en " + RelativePos);
                        CordenadaID = 3;
                        break;
                    case 4:
                        RelativePos = new Vector3(HArray[n - 1].transform.position.x, HArray[n - 1].transform.position.y - 22, 0);
                        CordenadaID = 4;
                        Debug.Log("Sur con coords en " + RelativePos);
                        break;
                    default:
                        Debug.Log("Valor imposible");
                        break;
                }

                HPosiciones.Add(HArray[n].transform.position);
                Vector3[] PosiArray = HPosiciones.ToArray();

                //for(int np =0; np < PosiArray.Length; np++)
                //{

                //}

                foreach(Vector3 hp in PosiArray)
                {
                    if (hp == HArray[n].transform.position)
                    {
                        Debug.Log("Repetido");
                        HArray[n].transform.position = RelativePos;
                    }
                    else
                    {
                        HArray[n].transform.position = RelativePos;
                    }
                }
            }

            CArray[n].transform.position = HArray[n].transform.position;
        }
    }

    void RandomDir()
    {

    }
}
