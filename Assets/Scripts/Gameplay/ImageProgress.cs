using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageProgress
{
    private bool[] m_tiles;

    public int Width
    {
        get;
        private set;
    }

    public int Height
    {
        get;
        private set;
    }

    public int TotalTiles
    {
        get { return m_tiles.Length; }
    }

    public int RevealedTiles
    {
        get;
        private set;
    }

    public bool IsCompleted
    {
        get { return TotalTiles == RevealedTiles; }
    }

    public void Init(int width, int height)
    {
        Width = width;
        Height = height;

        m_tiles = new bool[Width * Height];

        RevealedTiles = 0;
    }

    public void RevealTile(int x, int y)
    {
        int index = y * Width + x;
        if (index >= TotalTiles)
            return;

        if (m_tiles[index])
            return;

        m_tiles[index] = true;
        RevealedTiles++;
    }

    public bool IsRevealed(int x, int y)
    {
        int index = y * Width + x;
        if (index >= TotalTiles)
            return false;

        return m_tiles[index];
    }

    public void SetTiles(bool[] tiles)
    {
        if (tiles == null)
            return;

        if (tiles.Length < m_tiles.Length)
            return;

        System.Array.Copy(tiles, m_tiles, m_tiles.Length);

        CountRevealedTiles();
    }

    public bool[] GetTiles()
    {
        if (m_tiles == null)
            return null;

        var result = new bool[TotalTiles];
        System.Array.Copy(m_tiles, result, result.Length);
        return result;
    }

    private void CountRevealedTiles()
    {
        RevealedTiles = 0;

        for (int i = 0; i < m_tiles.Length; i++)
        {
            if (m_tiles[i])
                RevealedTiles++;
        }
    }
}
