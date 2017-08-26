using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : Living
{

    public Sun(WorldPos pos, Block block, TerrainGen terrain)
        : base(pos, block, terrain)
    {
        LivingPart[] part = new LivingPart[] { this.originPart, this.originPart, this.originPart, this.originPart, this.originPart, this.originPart };
        int d = 20;
        int r = d / 2;

        LivingPart pX = this.originPart;
        LivingPart pY = this.originPart;
        LivingPart pZ = this.originPart;

        Vector3 center = new Vector3(r,r,r);

        for (int x = 0; x < d; x++)
        {
            pX = pX.SetChildPart(Block.Direction.north, block);
            pY = pX;

            for (int y = 0; y < d; y++)
            {
                pY = pY.SetChildPart(Block.Direction.east, block);
                pZ = pY;
                for (int z = 0; z < d; z++)
                {
                    pZ = pZ.SetChildPart(Block.Direction.up, block);
                    //Debug.Log(Vector3.Distance(center, new Vector3(pZ.pos.x, pZ.pos.y + 1, pZ.pos.z)));
                    if (Vector3.Distance(center, new Vector3(pZ.pos.x, pZ.pos.y, pZ.pos.z)) <= r)
                    {
                        
                        base.SetBlock(pos.x + pZ.pos.x, pos.y + pZ.pos.y, pos.z + pZ.pos.z, block);
                    }
                }

            }
        }

        /*for (int i = 1; i < size; i++)
        {
            //TODO: Method to check block type (air, grass etc.) Also move all enums to one enum file.

            part[0] = part[0].SetChildPart(Block.Direction.down, block);
            base.SetBlock(pos.x + part[0].pos.x, pos.y + part[0].pos.y, pos.z + part[0].pos.z, block);

            part[1] = part[1].SetChildPart(Block.Direction.up, block);
            base.SetBlock(pos.x + part[1].pos.x, pos.y + part[1].pos.y, pos.z + part[1].pos.z, block);


            part[2] = part[2].SetChildPart(Block.Direction.west, block);
            base.SetBlock(pos.x + part[2].pos.x, pos.y + part[2].pos.y, pos.z + part[2].pos.z, block);
            setUpAndDown(part[2], i, size, block, pos);


            part[3] = part[3].SetChildPart(Block.Direction.east, block);
            base.SetBlock(pos.x + part[3].pos.x, pos.y + part[3].pos.y, pos.z + part[3].pos.z, block);
            setUpAndDown(part[3], i, size, block, pos);


            part[4] = part[4].SetChildPart(Block.Direction.north, block);
            base.SetBlock(pos.x + part[4].pos.x, pos.y + part[4].pos.y, pos.z + part[4].pos.z, block);
            //setUpAndDown(part[4], i, size, block, pos);


            part[5] = part[5].SetChildPart(Block.Direction.south, block);
            base.SetBlock(pos.x + part[5].pos.x, pos.y + part[5].pos.y, pos.z + part[5].pos.z, block);
            //setUpAndDown(part[5], i, size, block, pos);

        }*/
    }

    private void setUpAndDown (LivingPart part, int i, int size, Block block, WorldPos pos)
    {
        LivingPart lpS    = part;
        LivingPart lpN    = part;
        LivingPart lpUp   = part;
        LivingPart lpDown = part;

        for (int j = 1; j < size - i; j++)
        {
            lpS = lpS.SetChildPart(Block.Direction.south, block);
            base.SetBlock(pos.x + lpS.pos.x, pos.y + lpS.pos.y, pos.z + lpS.pos.z, block);

            lpN = lpN.SetChildPart(Block.Direction.north, block);
            base.SetBlock(pos.x + lpN.pos.x, pos.y + lpN.pos.y, pos.z + lpN.pos.z, block);

            lpUp = lpUp.SetChildPart(Block.Direction.up, block);
            base.SetBlock(pos.x + lpUp.pos.x, pos.y + lpUp.pos.y, pos.z + lpUp.pos.z, block);

            lpDown = lpDown.SetChildPart(Block.Direction.down, block);
            base.SetBlock(pos.x + lpDown.pos.x, pos.y + lpDown.pos.y, pos.z + lpDown.pos.z, block);
        }
    }

    public override void Live()
    {

    }

    public override void Die()
    {

    }
}
