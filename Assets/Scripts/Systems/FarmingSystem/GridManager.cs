using UnityEngine;
using System.Collections.Generic;

namespace FarmManagement.Systems.Farming
{
    public class GridManager : MonoBehaviour
    {
        [Header("Grid Settings")]
        public Vector2Int gridSize = new Vector2Int(20, 20);
        public float cellSize = 1.0f;
        
        [Header("Visual")]
        public Transform gridParent;
        public GameObject plotPrefab;

        private Dictionary<Vector3Int, Plot> plots;
        private Vector3 gridOrigin;

        public static GridManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            plots = new Dictionary<Vector3Int, Plot>();
            gridOrigin = transform.position;
        }

        private void Start()
        {
            GenerateGrid();
        }

        private void GenerateGrid()
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int z = 0; z < gridSize.y; z++)
                {
                    Vector3Int coord = new Vector3Int(x, 0, z);
                    CreatePlotAt(coord);
                }
            }
        }

        private void CreatePlotAt(Vector3Int coord)
        {
            Vector3 worldPos = GridToWorld(coord);
            GameObject plotObj = Instantiate(plotPrefab, worldPos, Quaternion.identity, gridParent);
            Plot plot = plotObj.GetComponent<Plot>();
            plot.Initialize(coord);
            plots[coord] = plot;
        }

        public Vector3 GridToWorld(Vector3Int coord)
        {
            return gridOrigin + new Vector3(coord.x * cellSize, 0, coord.z * cellSize);
        }

        public Vector3Int WorldToGrid(Vector3 worldPos)
        {
            Vector3 offset = worldPos - gridOrigin;
            int x = Mathf.RoundToInt(offset.x / cellSize);
            int z = Mathf.RoundToInt(offset.z / cellSize);
            return new Vector3Int(x, 0, z);
        }

        public bool GetPlotAt(Vector3Int coord, out Plot plot)
        {
            return plots.TryGetValue(coord, out plot);
        }

        public bool IsValidCoordinate(Vector3Int coord)
        {
            return coord.x >= 0 && coord.x < gridSize.x && 
                   coord.z >= 0 && coord.z < gridSize.y;
        }

        public List<Plot> GetAllPlots()
        {
            return new List<Plot>(plots.Values);
        }
    }
}