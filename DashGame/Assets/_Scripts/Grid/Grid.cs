using UnityEngine;
using System;
using TMPro;

public class Grid
{

    public static TextMeshPro CreateWorldText(string text, Vector3 localPosition = default(Vector3))
    {
        GameObject gameObject = new GameObject("World Text", typeof(TextMeshPro));
        gameObject.transform.localPosition = localPosition;
        gameObject.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        TextMeshPro textMeshPro = gameObject.GetComponent<TextMeshPro>();
        //textMeshPro.anchor = TextAnchor.MiddleCenter;
        textMeshPro.enableWordWrapping = false;
        textMeshPro.alignment = TextAlignmentOptions.MidlineLeft;
        textMeshPro.fontSize = 2;
        textMeshPro.text = text;
        textMeshPro.GetComponent<MeshRenderer>().sortingOrder = 5000;
        return textMeshPro;
    }

    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged; //Could use Action<OnGrid...> but EventHandler is the standard
    public class OnGridObjectChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    } 

    private int _width;
    private int _height;
    private float _cellSize;
    private Vector3 _originPosition;
    private PathNode[,] _gridArray;

    public Grid(int width, int height, float cellSize, Func<Grid, int, int, PathNode> createGridObject, Vector3 originPosition = default(Vector3))
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;
        _originPosition = originPosition;
        
        _gridArray = new PathNode[width,height];

        for(int x = 0; x < _gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < _gridArray.GetLength(1); y++)
            {
                _gridArray[x,y] = createGridObject(this, x, y);
            }
        }

        bool showDebug = true;
        if(showDebug)
        {
            TextMeshPro[,] _debugTextArray = new TextMeshPro[_width,_height];

            for(int x = 0; x < _gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < _gridArray.GetLength(1); y++)
                {
                    _debugTextArray[x,y] = CreateWorldText(_gridArray[x,y]?.ToString(), GetWorldPosition(x,y) + new Vector3(_cellSize,_cellSize) * 0.1f);
                    Color color = _gridArray[x,y].isWalkable ? Color.white : Color.red;
                    Debug.DrawLine(GetWorldPosition(x,y),GetWorldPosition(x,y+1),color,100f);
                    Debug.DrawLine(GetWorldPosition(x,y),GetWorldPosition(x+1,y),color,100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0,_height),GetWorldPosition(_width,_height),Color.white,100f);
            Debug.DrawLine(GetWorldPosition(_width,_height),GetWorldPosition(_width,0),Color.white,100f);

            OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventsArgs) => 
            {
                _debugTextArray[eventsArgs.x, eventsArgs.y].text = _gridArray[eventsArgs.x, eventsArgs.y]?.ToString(); 
            };
        }
    }
    public int GetHeight()
    {
        return _height;
    }

    public int GetWidth()
    {
        return _width;
    }

    public float GetCellSize()
    {
        return _cellSize;
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x,y) * _cellSize + _originPosition;
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition-_originPosition).x/_cellSize);
        y = Mathf.FloorToInt((worldPosition-_originPosition).y/_cellSize);
    }

    public void SetGridObject(int x, int y, PathNode value)
    {
        if(x >= 0 && y >= 0 & x < _width && y < _height)
        {
            _gridArray[x,y] = value;
            if(OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
        }
    }

    public void SetGridObject(Vector3 worldPosition, PathNode value)
    {
        int x,y;
        GetXY(worldPosition,out x, out y);
        SetGridObject(x,y,value);
    }

    public PathNode GetGridObject(int x, int y)
    {
        if(x >= 0 && y >= 0 && x < _width && y < _height)
        {
            return _gridArray[x,y];
        }
        return default(PathNode);
    }

    public PathNode GetGridObject(Vector3 worldPosition)
    {
        int x,y;
        GetXY(worldPosition,out x, out y);
        return GetGridObject(x,y);
    }

    public void TriggerGridObjectChange(int x, int y)
    {
        if(OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
    }
}
