using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FloorAlgorithm : MonoBehaviour
{
    GameObject[] Habitaciones;
    public int NHabitaciones, Dimensiones;
    public Tile Textura1;
    public Tilemap MapaBase, Muro;
    public Tile[] SueloRandom;

    void Start()
    {
        GenerarBase();
    }

    void GenerarBase()
    {
        int Ygen = Dimensiones / 2;
        //Generaión del espacio en el 1er cuadrante
        for (int y = 0; y < Ygen; y++)
        {
            int Y = y;
            for (int x = 0; x > -Dimensiones; x--)
            {
                Vector3Int Posicion = new Vector3Int(Mathf.FloorToInt(x), Mathf.FloorToInt(Y), 0);
                MapaBase.SetTile(Posicion, Textura1);
            }
        }
        //Generaión del espacio en el 2o cuadrante
        for (int y = 0; y < Ygen; y++)
        {
            int Y = y;
            for (int x = 0; x < Dimensiones; x++)
            {
                Vector3Int Posicion = new Vector3Int(Mathf.FloorToInt(x), Mathf.FloorToInt(Y), 0);
                MapaBase.SetTile(Posicion, Textura1);
            }
        }
        //Generaión del espacio en el 3er cuadrante
        for (int y = 0; y > -Ygen; y--)
        {
            int Y = y;
            for (int x = 0; x < Dimensiones; x++)
            {
                Vector3Int Posicion = new Vector3Int(Mathf.FloorToInt(x), Mathf.FloorToInt(Y), 0);
                MapaBase.SetTile(Posicion, Textura1);
            }
        }
        //Generaión del espacio en el 4o cuadrante
        for (int y = 0; y > -Ygen; y--)
        {
            int Y = y;
            for (int x = 0; x > -Dimensiones; x--)
            {
                Vector3Int Posicion = new Vector3Int(Mathf.FloorToInt(x), Mathf.FloorToInt(Y), 0);
                MapaBase.SetTile(Posicion, Textura1);
            }
        }
    }
    void GenerarMuro()
    {

    }
    void EmebellecerSuelo()
    {

    }
}
