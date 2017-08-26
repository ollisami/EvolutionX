using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLiving : MonoBehaviour {

    public World world;
    private TerrainGen terrain;

	void Start () {
        terrain = world.getTerrain();
        if (terrain == null)
        {
            Debug.LogError("ERR!! Terrain not found.");
        }
        addLivingObjectsToWorld();
    }
	
	void Update () {
		
	}

    void addLivingObjectsToWorld ()
    {
        Sun sun = new Sun(new WorldPos(-25, 25, -25), new BlockSand(), terrain);
    }
}
