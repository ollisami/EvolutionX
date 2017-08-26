using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Blockwater : Block
{

    public Blockwater()
        : base()
    {

    }

    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();
        tile.x = 4;
        tile.y = 1;

        return tile;
    }
}
