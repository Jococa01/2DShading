using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class FloorAlgorithm : MonoBehaviour
{
    [Header("Listas de Objetos")]
    public List<GameObject> Habitaciones = new List<GameObject>();
    public List<GameObject> Cámaras = new List<GameObject>();
    public List<Vector3> HPosiciones = new List<Vector3>();
    public List<int> DireccionesVetadas = new List<int>();

    public int Dimensiones, IDHabitación;
    public Tile HierbaBase, Muro_d, Muro_U, Muro_R, Muro_L, Muro_Blank;
    public Tilemap MapaBase, Muro;
    public Tile[] SueloRandom;
    public GameObject AdministradorH, AdministradorCam;
    public int CordenadaID = 0;

    [Header("Parámetros modificables en Editor")]
    [Range(1, 8)]
    public int NHabitaciones;
    public int Altura, Anchura;

    [Header("Parámetros del minimapa")]
    public Sprite MinimapSprite;
    public Material UnlitMat;
    public int RoomHeight, RoomWidth;

    [Header("Generación de Muros")]
    public List<Vector3> DoorDirections = new List<Vector3>();

    void Start()
    {
        for(int n=0; n<NHabitaciones; n++)
        {
            GenerarBase();
        }

        Relocate();
        MinimapIcons();
        //GenerarMuro();
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
        childofobj.name = "Suelo";

        Tilemap LocalTM = childofobj.GetComponent<Tilemap>();

        //Creo el gameobject de tipo cámara

        GameObject Camobj = new GameObject();
        Camobj.name = Nombre;

        Camobj.AddComponent<RoomDetection>();
        Camobj.AddComponent<BoxCollider2D>();
        Camobj.tag = "Floor";

        Camobj.GetComponent<RoomDetection>().RoomNumber = IDHabitación;
        Camobj.GetComponent<BoxCollider2D>().size = new Vector2(Anchura, Altura);
        Camobj.GetComponent<BoxCollider2D>().isTrigger=true;

        Camobj.transform.parent = AdministradorCam.transform;
        Cámaras.Add(Camobj);


        int Ygen = Dimensiones-6;

        //Generaión del espacio en el 1er cuadrante
        for (int y = 0; y < Ygen; y++)
        {
            int Y = y;
            for (int x = 0; x > -Dimensiones; x--)
            {
                Vector3Int Posicion = new Vector3Int(Mathf.FloorToInt(x), Mathf.FloorToInt(Y), 0);
                LocalTM.SetTile(Posicion, HierbaBase);
            }
        }
        //Generaión del espacio en el 2o cuadrante
        for (int y = 0; y < Ygen; y++)
        {
            int Y = y;
            for (int x = 0; x < Dimensiones; x++)
            {
                Vector3Int Posicion = new Vector3Int(Mathf.FloorToInt(x), Mathf.FloorToInt(Y), 0);
                LocalTM.SetTile(Posicion, HierbaBase);
            }
        }
        //Generaión del espacio en el 3er cuadrante
        for (int y = 0; y > -Ygen; y--)
        {
            int Y = y;
            for (int x = 0; x < Dimensiones; x++)
            {
                Vector3Int Posicion = new Vector3Int(Mathf.FloorToInt(x), Mathf.FloorToInt(Y), 0);
                LocalTM.SetTile(Posicion, HierbaBase);
            }
        }
        //Generaión del espacio en el 4o cuadrante
        for (int y = 0; y > -Ygen; y--)
        {
            int Y = y;
            for (int x = 0; x > -Dimensiones; x--)
            {
                Vector3Int Posicion = new Vector3Int(Mathf.FloorToInt(x), Mathf.FloorToInt(Y), 0);
                LocalTM.SetTile(Posicion, HierbaBase);
            }
        }

        IDHabitación++;
    }

    void EmebellecerSuelo()
    {

    }

    void Relocate()
    {
        GameObject[] HArray = Habitaciones.ToArray();
        GameObject[] CArray = Cámaras.ToArray();
        HPosiciones.Add(Vector3.zero);
        for (int n =0; n < HArray.Length; n++)
        {
            if (n == 0)
            {
                HArray[n].transform.position = Vector3.zero;
            }
            else if(n > 0)
            {
                NuevoLoop:

                int index = RandomInt();
                Debug.Log(index);
                Vector3 RelativePos = Vector3.zero;

                switch (index)
                {
                    case 1:
                        RelativePos = new Vector3(HArray[n - 1].transform.position.x + Anchura, HArray[n - 1].transform.position.y, 0);
                        Debug.Log("Este con coords en " + RelativePos);
                        CordenadaID = 1;
                        break;
                    case 2:
                        RelativePos = new Vector3(HArray[n - 1].transform.position.x - Anchura, HArray[n - 1].transform.position.y, 0);
                        Debug.Log("Oeste con coords en " + RelativePos);
                        CordenadaID = 2;
                        break;
                    case 3:
                        RelativePos = new Vector3(HArray[n - 1].transform.position.x, HArray[n - 1].transform.position.y + Altura, 0);
                        Debug.Log("Norte con coords en " + RelativePos);
                        CordenadaID = 3;
                        break;
                    case 4:
                        RelativePos = new Vector3(HArray[n - 1].transform.position.x, HArray[n - 1].transform.position.y - Altura, 0);
                        Debug.Log("Sur con coords en " + RelativePos);
                        CordenadaID = 4;

                        break;
                    default:
                        Debug.Log("Valor imposible");
                        break;
                }

                HArray[n].transform.position = RelativePos;
                Vector3[] PosiArray = HPosiciones.ToArray();

                int VectorActual = 0;

                foreach(Vector3 hp in PosiArray)
                {
                    if (hp == HArray[n].transform.position)
                    {
                        Debug.Log("Repetido");
                        Debug.Log(hp + " es el vector ya existente");
                        DireccionesVetadas.Add(index);
                        goto NuevoLoop;
                    }
                    else if(hp != HArray[n].transform.position)
                    {
                        Debug.Log("Nueva Posición");
                        DireccionesVetadas.Clear();
                    }
                    VectorActual++;
                }

                HPosiciones.Add(HArray[n].transform.position);
            }

            CArray[n].transform.position = HArray[n].transform.position;
        }
    }

    public int RandomInt()
    {
        int[] ValoresBase = new int[4] { 1, 2, 3, 4 };

        //Resta de valores según las opciones vetadas
        int[] ValoresVetados = DireccionesVetadas.ToArray();

        int[] ValorVerdadero = ValoresBase.Where(x => !ValoresVetados.Contains(x)).ToArray();

        int indice = ValoresBase[Random.Range(0, ValorVerdadero.Length)];

        //Cambiar esta wea por el Sistema de multiples reintentos en 1 frame, es una mierda pero es lo que hay || Mirar si se pueden quitar X int de un array de ints

        //var exclude = new HashSet<int>(DireccionesVetadas);
        //var CustomRange = Enumerable.Range(1, 5).Where(i => !exclude.Contains(i));

        //var RandomCoord = new System.Random();
        //int index = RandomCoord.Next(1, 4);
        return indice;
    }

    void GenerarMuro()
    {
        int Ygen = Dimensiones / 2;

        for(int n=0; n < Habitaciones.ToArray().Length; n++)
        {
            GameObject[] HArray = Habitaciones.ToArray();
            GameObject LayerMuro = new GameObject();
            LayerMuro.name="Muro";
            LayerMuro.transform.parent = HArray[n].transform;
            LayerMuro.AddComponent<Tilemap>();
            LayerMuro.AddComponent<TilemapRenderer>();

            Tilemap tilemap = LayerMuro.GetComponent<Tilemap>();
            tilemap.tileAnchor = Vector3.zero;

            //Genero las paredes superiores
            for(int x=0; x < Dimensiones; x++)
            {
                Vector3Int border = new Vector3Int(Mathf.FloorToInt(HArray[n].transform.position.x+x),Mathf.FloorToInt(HArray[n].transform.position.y + Ygen), 0);
                tilemap.SetTile(border, Muro_d);

                border = new Vector3Int(Mathf.FloorToInt(HArray[n].transform.position.x - x), Mathf.FloorToInt(HArray[n].transform.position.y + Ygen), 0);
                tilemap.SetTile(border, Muro_d);

                border = new Vector3Int(Mathf.FloorToInt(HArray[n].transform.position.x - x), Mathf.FloorToInt(HArray[n].transform.position.y - Ygen), 0);
                tilemap.SetTile(border, Muro_U);

                border = new Vector3Int(Mathf.FloorToInt(HArray[n].transform.position.x + x), Mathf.FloorToInt(HArray[n].transform.position.y - Ygen), 0);
                tilemap.SetTile(border, Muro_U);
            }

            //Genero las paredes laterales
            for(int y=0; y<Ygen; y++)
            {
                Vector3Int border = new Vector3Int(Mathf.FloorToInt(HArray[n].transform.position.x + Dimensiones), Mathf.FloorToInt(HArray[n].transform.position.y + y), 0);
                tilemap.SetTile(border, Muro_L);

                border = new Vector3Int(Mathf.FloorToInt(HArray[n].transform.position.x - Dimensiones), Mathf.FloorToInt(HArray[n].transform.position.y + y), 0);
                tilemap.SetTile(border, Muro_R);

                border = new Vector3Int(Mathf.FloorToInt(HArray[n].transform.position.x - Dimensiones), Mathf.FloorToInt(HArray[n].transform.position.y - y), 0);
                tilemap.SetTile(border, Muro_R);

                border = new Vector3Int(Mathf.FloorToInt(HArray[n].transform.position.x + Dimensiones), Mathf.FloorToInt(HArray[n].transform.position.y - y), 0);
                tilemap.SetTile(border, Muro_L);
            }

            //Genero las esquinas
            for(int e=0; e<4; e++)
            {
                Vector3Int border = new Vector3Int(Mathf.FloorToInt(HArray[n].transform.position.x - Dimensiones), Mathf.FloorToInt(HArray[n].transform.position.y + Ygen), 0);
                if (e == 0)
                {
                    tilemap.SetTile(border, Muro_L);
                }
                if (e == 1)
                {
                    border = new Vector3Int(Mathf.FloorToInt(HArray[n].transform.position.x + Dimensiones), Mathf.FloorToInt(HArray[n].transform.position.y + Ygen), 0);
                    tilemap.SetTile(border, Muro_L);
                }
                if (e == 2)
                {
                    border = new Vector3Int(Mathf.FloorToInt(HArray[n].transform.position.x - Dimensiones), Mathf.FloorToInt(HArray[n].transform.position.y - Ygen), 0);
                    tilemap.SetTile(border, Muro_L);
                }
                if (e == 3)
                {
                    border = new Vector3Int(Mathf.FloorToInt(HArray[n].transform.position.x + Dimensiones), Mathf.FloorToInt(HArray[n].transform.position.y - Ygen), 0);
                    tilemap.SetTile(border, Muro_L);
                }
            }


            //Dimensiones es la variable de la anchura de la habitación, la altura es Dimensiones/2

            //Coso de la generación de puertas
            //if(n< Habitaciones.ToArray().Length - 1)
            //{
            //    Vector3 DoorDir = HArray[n + 1].transform.position - HArray[n].transform.position;
            //    DoorDirections.Add(DoorDir);
            //}
        }
    }

    public void MinimapIcons()
    {
        for(int n=0; n<Habitaciones.ToArray().Length; n++)
        {
            GameObject[] Harray= Habitaciones.ToArray();
            GameObject Icon = new GameObject();
            Icon.transform.parent = Harray[n].transform;
            Icon.layer = 9;
            Icon.transform.localScale = new Vector3(RoomWidth, RoomHeight, 1);
            Icon.AddComponent<SpriteRenderer>();
            SpriteRenderer localsprite = Icon.GetComponent<SpriteRenderer>();
            localsprite.sprite =MinimapSprite;
            localsprite.material =UnlitMat;
            Icon.transform.position = HPosiciones[n];
            Icon.name = "Icon";
        }
    }
}
