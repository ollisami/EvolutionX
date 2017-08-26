using UnityEngine;
using System.Collections;
using System;


[Serializable]
public class Living
{
    public LivingPart originPart;
    TerrainGen terrain;
    WorldPos originPos;

 
    public Living(WorldPos pos, Block block, TerrainGen terrain)
    {
        this.terrain = terrain;
        originPos = pos;
        this.originPart = new LivingPart(new WorldPos(0, 0, 0), block);
    }

    public virtual void SetBlock(int x, int y, int z, Block block)
    {
        terrain.SetBlock(x, y, z, block);
    }

    public virtual Block GetBlock(int x, int y, int z)
    {
        return terrain.GetBlock(x, y, z, true);
    }

    public virtual void Live()
    {
        //Do movement etc.
    }

    public virtual void Die()
    {
        //die or reproduce etc.
    }
    
}