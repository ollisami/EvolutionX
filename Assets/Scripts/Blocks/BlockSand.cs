using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class BlockSand : Block
{

    public BlockSand()
        : base()
    {

    }

    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();

        switch (direction)
        {
            case Direction.up:
                tile.x = 1;
                tile.y = 4;
                return tile;
        }

        tile.x = 0;
        tile.y = 4;

        return tile;
    }
}