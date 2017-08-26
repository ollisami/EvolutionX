using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class BlockMountain : Block
{

    public BlockMountain()
        : base()
    {

    }

    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();

        switch (direction)
        {
            case Direction.up:
                tile.x = 4;
                tile.y = 4;
                return tile;
            case Direction.down:
                tile.x = 2;
                tile.y = 4;
                return tile;
        }

        tile.x = 3;
        tile.y = 4;

        return tile;
    }
}