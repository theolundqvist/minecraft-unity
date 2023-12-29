using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textureCoords : MonoBehaviour {

    public static textureCoords Map;

    private void Awake()
    {
        if (Map != null) return;
        Map = this;
    }



    public const int Air   =   0;
    public const int Grass =   1;
    public const int Dirt  =   2;
    public const int Stone =   3;
    public const int Water =   4;
    public const int Sand  =   5;

    public Vector2[] getTexture(int blockID, Vector3 direction)
    {

        float offset = 1 / 16f;
        float pixel = 1 / 1024f;

        switch (blockID)
        {
            case Grass:
                #region

                //Top
                if (direction == -Vector3.up)
                {
                    return new Vector2[]{
                        new Vector2(offset * 0 + pixel, offset * 16 - pixel),
                        new Vector2(offset * 1 - pixel, offset * 16 - pixel),
                        new Vector2(offset * 1 - pixel, offset * 15 + pixel),
                        new Vector2(offset * 0 + pixel, offset * 15 + pixel),
                    };
                }
                //X axis
                else if (direction == Vector3.right || direction == Vector3.left)
                {
                    return new Vector2[]{
                    new Vector2(offset * 1 + pixel, offset * 15 + pixel),
                    new Vector2(offset * 2 - pixel, offset * 15 + pixel),
                    new Vector2(offset * 2 - pixel, offset * 16 - pixel),
                    new Vector2(offset * 1 + pixel, offset * 16 - pixel)};
                }

                //Z Axis
                else if (direction == Vector3.forward || direction == Vector3.back)
                {
                    return new Vector2[]{
                    new Vector2(offset * 1 + pixel, offset * 16 - pixel),    
                    new Vector2(offset * 1 + pixel, offset * 15 + pixel),    
                    new Vector2(offset * 2 - pixel, offset * 15 + pixel),    
                    new Vector2(offset * 2 - pixel, offset * 16 - pixel) };  
                }

                //Bottom
                else
                {
                    return getTexture(Dirt, Vector3.down);
                }
#endregion
            case Dirt:
                return new Vector2[]{
                    new Vector2(offset * 2 + pixel, offset * 16 - pixel),
                    new Vector2(offset * 3 - pixel, offset * 16 - pixel),
                    new Vector2(offset * 3 - pixel, offset * 15 + pixel),
                    new Vector2(offset * 2 + pixel, offset * 15 + pixel)};

            case Stone:
                return new Vector2[]{
                    new Vector2(offset * 3 + pixel, offset * 16 - pixel),
                    new Vector2(offset * 4 - pixel, offset * 16 - pixel),
                    new Vector2(offset * 4 - pixel, offset * 15 + pixel),
                    new Vector2(offset * 3 + pixel, offset * 15 + pixel)};

            case Water:
                return new Vector2[]{
                    new Vector2(offset * 15 + pixel, offset * 3 - pixel),
                    new Vector2(offset * 16 - pixel, offset * 3 - pixel),
                    new Vector2(offset * 16 - pixel, offset * 2 + pixel),
                    new Vector2(offset * 15 + pixel, offset * 2 + pixel)};

            case Sand:
                return new Vector2[]{
                    new Vector2(offset * 4 + pixel, offset * 16 - pixel),
                    new Vector2(offset * 5 - pixel, offset * 16 - pixel),
                    new Vector2(offset * 5 - pixel, offset * 15 + pixel),
                    new Vector2(offset * 4 + pixel, offset * 15 + pixel)};
            default: return new Vector2[4];
        }
    }






















//    switch (blockID)
//        {
//            case Grass:
//                #region
//                if (direction == -Vector3.up)
//                {
//                    return new Vector2[]{
//                        new Vector2(offset* 0 + pixel, offset* 16 - pixel),
//                        new Vector2(offset* 1 - pixel, offset* 16 - pixel),
//                        new Vector2(offset* 1 - pixel, offset* 15 + pixel),
//                        new Vector2(offset* 0 + pixel, offset* 15 + pixel),
//                    };
//                }

//                else if (direction == Vector3.right || direction == Vector3.left)
//                {
//                    return new Vector2[]{

//                    new Vector2(offset* 3 + pixel, offset* 15 + pixel*2),
//                    new Vector2(offset* 4 - pixel* 2, offset* 15 + pixel* 2),
//                    new Vector2(offset* 4 - pixel* 2, offset* 16),
//                    new Vector2(offset* 3 + pixel, offset* 16)};
//                }

//                else if (direction == Vector3.forward || direction == Vector3.back)
//                {
//                    return new Vector2[]{

//                    new Vector2(offset* 3 + pixel, offset* 16),                    //2
//                    new Vector2(offset* 3 + pixel, offset* 15 + pixel* 2),              //3
//                    new Vector2(offset* 4 - pixel* 2, offset* 15 + pixel* 2),      //1
//                    new Vector2(offset* 4 - pixel* 2, offset* 16) };                     //0

                    
                    
                    
                    
//                }

//                else
//                {
//                    return new Vector2[]{
//                    new Vector2(offset* 2 + pixel, offset* 16),
//                    new Vector2(offset* 3 - pixel*2, offset* 16),
//                    new Vector2(offset* 3 - pixel*2, offset* 15 + pixel*2),
//                    new Vector2(offset* 2 + pixel, offset* 15 + pixel*2)};
//                }
//#endregion
//            case Dirt:
//                return new Vector2[]{
//                    new Vector2(offset* 2 + pixel, offset* 16),
//                    new Vector2(offset* 3 - pixel*2, offset* 16),
//                    new Vector2(offset* 3 - pixel*2, offset* 15 + pixel*2),
//                    new Vector2(offset* 2 + pixel, offset* 15 + pixel*2)};

//            case Stone:
//                return new Vector2[]{
//                    new Vector2(offset* 1 + pixel, offset* 16 - pixel),
//                    new Vector2(offset* 2 - pixel, offset* 16 - pixel),
//                    new Vector2(offset* 2 - pixel, offset* 15 + pixel),
//                    new Vector2(offset* 1 + pixel, offset* 15 + pixel)};

//            case Water:
//                return new Vector2[]{
//                new Vector2(offset* 15 + pixel, offset* 3 - pixel),
//                new Vector2(offset* 16 - pixel, offset* 3 - pixel),
//                new Vector2(offset* 16 - pixel, offset* 2 + pixel),
//                new Vector2(offset* 15 + pixel, offset* 2 + pixel)};

//            case Sand:
//                return new Vector2[]{
//                    new Vector2(offset* 2 + pixel, offset* 15 - pixel),
//                    new Vector2(offset* 3 - pixel, offset* 15 - pixel),
//                    new Vector2(offset* 3 - pixel, offset* 14 + pixel),
//                    new Vector2(offset* 2 + pixel, offset* 14 + pixel)};
//            default: return new Vector2[4];
//        }
//    }
}
