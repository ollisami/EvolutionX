using UnityEngine;
using System.Collections;
using SimplexNoise;
using System.Collections.Generic;

public class TerrainGen
{
    private World world;
    //TODO: Some way to save and reload this on startup? Maybe it could be in server...
    private Dictionary<WorldPos, Block> editedBlocks = new Dictionary<WorldPos, Block>();

    float stoneBaseHeight = -24;
    float stoneBaseNoise = 0.0005f;
    float stoneBaseNoiseHeight = 4;

    float stoneMountainHeight = 48;
    float stoneMountainFrequency = 0.008f;
    float stoneMinHeight = -12;

    float dirtBaseHeight = 2;
    float dirtNoise = 0.04f;
    float dirtNoiseHeight = 3;

    float caveFrequency = 0.025f;
    int caveSize = 7;

    float treeFrequency = 0.2f;
    int treeDensity = 3;

    //Assigned later
    int stoneHeight = 0;
    int dirtHeight  = 0;

    public TerrainGen(World world)
    {
        this.world = world;
    }

    public Chunk ChunkGen(Chunk chunk)
    {
        for (int x = chunk.pos.x - 3; x < chunk.pos.x + Chunk.chunkSize + 3; x++) //Change this line
        {
            for (int z = chunk.pos.z - 3; z < chunk.pos.z + Chunk.chunkSize + 3; z++)//and this line
            {
                chunk = ChunkColumnGen(chunk, x, z);
            }
        }
        return chunk;
    }

    private int getIntFromNoise(int min, int max, float noise)
    {
        return Mathf.RoundToInt(Mathf.Clamp((min + ((max - min) * noise)), min, max));
    }

    private void CalculateTerrainValues(int x, int z)
    {
        //TODO: Remove division inn floats

        float noise = GetNoise(x, 0, z, 0.00001F, 3);

        stoneBaseHeight        = -24 + getIntFromNoise(-5,5,noise);
        
        stoneBaseNoise         = 0.0005f + getIntFromNoise(-5, 5, noise) / 1000;
        stoneBaseNoiseHeight   = 4 + getIntFromNoise(-2, 2, noise); ;
        
        stoneMountainHeight    = 48 + getIntFromNoise(-20, 5, noise); ;
        stoneMountainFrequency = 0.008f + getIntFromNoise(-5, 5, noise) / 1000 ;
        stoneMinHeight         = -12 + getIntFromNoise(-3, 3, noise); ;
        
        dirtBaseHeight         = 2 +  getIntFromNoise(-1 , 1, noise);
        dirtNoise              = 0.04f + getIntFromNoise(-2, 2, noise) / 10; 
        dirtNoiseHeight        = 3 + getIntFromNoise(-2, 2, noise);
        
        caveFrequency          = 0.025f + getIntFromNoise(-2, 2, noise) / 10;
        caveSize               = 27 + getIntFromNoise(-2, 2, noise);

        stoneHeight  = Mathf.FloorToInt(stoneBaseHeight);
        stoneHeight += GetNoise(x, 0, z, stoneMountainFrequency, Mathf.FloorToInt(stoneMountainHeight));

        if (stoneHeight < stoneMinHeight)
            stoneHeight = Mathf.FloorToInt(stoneMinHeight);

        stoneHeight += GetNoise(x, 0, z, stoneBaseNoise, Mathf.FloorToInt(stoneBaseNoiseHeight));
        dirtHeight   = stoneHeight + Mathf.FloorToInt(dirtBaseHeight);
        dirtHeight  += GetNoise(x, 100, z, dirtNoise, Mathf.FloorToInt(dirtNoiseHeight));

    }

    public Block GetBlock (int x, int y, int z, bool calculateValues = false)
    {
        WorldPos pos = new WorldPos(x, y, z);
        if (editedBlocks.ContainsKey(pos))
            return editedBlocks[pos];

        if (calculateValues)
            CalculateTerrainValues(x, z);

        //Get a value to base cave generation on
        int caveChance = GetNoise(x, y, z, caveFrequency, 100);

        //Cave
        if (y <= stoneHeight && caveSize < caveChance)
        {
            if (y < dirtBaseHeight - 15 || y < -40)
            {
                //Water
                return new Blockwater();
            }
            else
            {
                //Cave 
                if (GetNoise(x, y, z, 0.0005F, 1) > 0.7F)
                    return new BlockSand();
                else
                    //Underground
                    return new Block();
            }
        }


        else if (y <= dirtHeight && caveSize < caveChance)
        {
            //Mountain
            if (y > 15)
            {
                //Mountain snow
                if (y > 25)
                {
                return new BlockMountainSnow();
                }
                else if (y > 20)
                {
                return new BlockMountain();
                }
                else
                {
                return new BlockGrassToMountain();
                }
            }
            else
            {
            //Grass
            return new BlockGrass();

                //if (y == dirtHeight && GetNoise(x, 0, z, treeFrequency, 100) < treeDensity)
                //  CreateTree(x, y + 1, z, chunk);
            }
        }

        //Air
        return new BlockAir();
    }

    public Chunk ChunkColumnGen(Chunk chunk, int x, int z)
    {
        //TODO: If player goes too low, no new ground is created
        CalculateTerrainValues(x, z);
        for (int y = chunk.pos.y - 8; y < chunk.pos.y + Chunk.chunkSize; y++)
        {
            SetBlock(x, y, z, GetBlock(x,y,z), chunk);
        }
        return chunk;
    }

    void CreateTree(int x, int y, int z, Chunk chunk)
    {
        //create leaves
        for (int xi = -2; xi <= 2; xi++)
        {
            for (int yi = 4; yi <= 8; yi++)
            {
                for (int zi = -2; zi <= 2; zi++)
                {
                    SetBlock(x + xi, y + yi, z + zi, new BlockLeaves(), chunk, true);
                }
            }
        }

        //create trunk
        for (int yt = 0; yt < 6; yt++)
        {
            SetBlock(x, y + yt, z, new BlockWood(), chunk, true);
        }
    }

    public static void SetBlock(int x, int y, int z, Block block, Chunk chunk, bool replaceBlocks = false)
    {
        x -= chunk.pos.x;
        y -= chunk.pos.y;
        z -= chunk.pos.z;

        if (Chunk.InRange(x) && Chunk.InRange(y) && Chunk.InRange(z))
        {
            if (replaceBlocks || chunk.blocks[x, y, z] == null)
                chunk.SetBlock(x, y, z, block);
        }
    }

    public void SetBlock(int x, int y, int z, Block block)
    {
        if (!world.SetBlock(x, y, z, block))
        {
            editedBlocks.Add(new WorldPos(x,y,z), block);
        }
    }

    public static int GetNoise(int x, int y, int z, float scale, int max)
    {
        return Mathf.FloorToInt((Noise.Generate(x * scale, y * scale, z * scale) + 1f) * (max / 2f));
    }
}