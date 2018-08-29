using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class Preset
{
    protected abstract void Init();  // Initialize a Preset. If already initialized, resets the object
    protected abstract bool[,] GetMap();  // Returns the block map
    protected abstract void GenerateObjects();  // Creates the objects, adding them to the list

    private int x, y, width, height, rotation;

    // Bottom left corner of the preset
    public Vector2 Origin
    {
        set { x = (int)value.x;  y = (int)value.y; }
    }
    public int Width
    {
        get { return rotation % 2 == 0 ? width : height; }
    }
    public int Height
    {
        get { return rotation % 2 == 0 ? height : width; }
    }

    private bool[,] map;  // reference to the map
    // rotation: 0 is normal, 1 is 90 degrees clockwise
    private List<GameObject> objects;

    public Preset(int x, int y, List<GameObject> objects)
    {
        Init();
        this.objects = objects;
        this.x = x;
        this.y = y;
        map = GetMap();
        width = map.GetLength(1);
        height = map.GetLength(0);
    }

    public int Rotation
    {
        set { rotation = value; }
    }

    private Vector2 TranslatePosition(float x, float y)
    {
        switch (rotation)
        {
            case 1: return new Vector2(y + this.x, width - x + this.y);
            case 2: return new Vector2(width - x + this.x, height - y + this.y);
            case 3: return new Vector2(height - y + this.x, x + this.y);
            default: return new Vector2(x + this.x, y + this.y);
        }
    }

    // Sets the map with the bottom left corner being (x, y)
    public void GeneratePreset(bool[,] blocks)
    {
        Refit(blocks.GetLength(1), blocks.GetLength(0));
        int size = height - 1;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 pos = TranslatePosition(i, j);
                // Map is read this way for easier programming
                blocks[(int)pos.x, (int)pos.y] = map[size - j, i];
            }
        }
        GenerateObjects();
    }

    // Shifts the origin to the closest location that is within boundaries
    private void Refit(int maxW, int maxH)
    {
        int newX = x;
        int difX = maxW - (Width + x + 3);
        if (difX < 0)
        {
            newX += difX;
        }

        int newY = y;
        int difY = maxH - (Height + y + 3);
        if (difY < 0)
        {
            newY += difY;
        }

        Origin = new Vector2(newX, newY);
    }

    // Move Object to (x, y) on the preset
    public void PutObject(float x, float y, GameObject obj)
    {
        obj.transform.position = TranslatePosition(x, y);
        objects.Add(obj);
    }
}