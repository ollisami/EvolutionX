using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class LivingPart
{
    //Position from living origin
    public WorldPos pos;
    private Block block;

    LivingPart norht;
    LivingPart south;
    LivingPart east;
    LivingPart west;
    LivingPart up;
    LivingPart down;


    public LivingPart(WorldPos positionFromOrigin, Block block)
    {
        this.pos = positionFromOrigin;
        this.block = block;

        norht = null;
        south = null;
        east = null;
        west = null;
        up = null;
        down = null;
    }

    public LivingPart SetChildPart (Block.Direction dir, Block blockType)
    {
        if (dir == Block.Direction.north)
        {
            norht = new LivingPart(new WorldPos(pos.x + 1, pos.y, pos.z), blockType);
            return this.norht;
        }

        if (dir == Block.Direction.south)
        {
            south = new LivingPart(new WorldPos(pos.x - 1, pos.y, pos.z), blockType);
            return this.south;
        }

        if (dir == Block.Direction.east)
        {
            east = new LivingPart(new WorldPos(pos.x, pos.y, pos.z + 1), blockType);
            return this.east;
        }

        if (dir == Block.Direction.west)
        {
            west = new LivingPart(new WorldPos(pos.x, pos.y, pos.z - 1), blockType);
            return this.west;
        }

        if (dir == Block.Direction.up)
        {
            up = new LivingPart(new WorldPos(pos.x, pos.y + 1, pos.z), blockType);
            return this.up;
        }

        if (dir == Block.Direction.down)
        {
            down = new LivingPart(new WorldPos(pos.x, pos.y - 1, pos.z), blockType);
            return this.down;
        }

        return null;
    }
}