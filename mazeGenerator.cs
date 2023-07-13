using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class mazeGenerator : MonoBehaviour
{

    Mesh mesh;
    List<Vector3> vertices;
    List<int> triangles;
    List<Vector2> uvs;
    float blockWidth = 2;

    public Transform goal;


    // Start is called before the first frame update
    void Start()
    {
        int[,,] maze = generateMaze();
        mesh = new Mesh();


        int h,w,d;
        w = maze.GetLength(0);
        d = maze.GetLength(1);
        h = maze.GetLength(2);
        vertices     = new List<Vector3>();
        uvs     = new List<Vector2>();
        triangles    = new List<int>();
        

        mesh.Clear();



        for(int i = 0; i < w; i++)
        {
            for(int j = 0; j < d; j++)
            {
                for(int k = 0; k < h; k++)
               {
                    if(maze[i,j,k] == 1)
                    {
                        drawCube(i*blockWidth,j*blockWidth,-k*blockWidth,maze,i,j,k);
                    }
                    else if(maze[i,j,k] == 2)
                    {
                        //Remember to switch j and k
                        goal.position = new Vector3(i*blockWidth,k*blockWidth,j*blockWidth);
                        goal.localScale    = blockWidth * Vector3.one;
                    }
                    else if(maze[i,j,k] == 3)
                    {
                        //Remember to switch j and k
                        Transform player = GameObject.Find("Player").GetComponent<Transform>();
                        player.position = new Vector3(i*blockWidth,k*blockWidth,j*blockWidth);
                    }
                } 
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.triangles = triangles.ToArray();


        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;



        
        // for(int x = 0; x < maze.GetLength(0); x++)
        // {
        //     for(int y = 0; y < maze.GetLength(1); y++)
        //     {
        //         for(int z = 0; z < maze.GetLength(2); z++)
        //         {
        //             vertices[z*(w*d)+y*w+x] = new Vector3(x*blockWidth,y*blockWidth,z*blockWidth);
        //         }
        //     }
        // }

        // for(int i = 0; i < w-1; i++)
        // {
        //     for(int j = 0; j < d-1; j++)
        //     {
        //         for(int k = 0; k < h-1; k++)
        //         {
        //             if(maze[i,j,k] == 1)
        //             {
        //                 for(int l = 0; l < 3; l++)
        //                 {
        //                     int[] first = {0,0,0};
        //                     first[l] += 1;
        //                     int[] second = {0,0,0};
        //                     if(l == 2) second[0] += 1;
        //                     else second[l+1] += 1;

        //                     int[] third = {0,0,0};
        //                     if(l == 0)
        //                     {
        //                         third[2] += 1;
        //                     }
        //                     else
        //                     {
        //                         third[l-1] += 1;
        //                     }

        //                     triangles[getIndex(i,j,k,w,d,h)*6*2*3 + 12*l  + 0] = getIndex(i,j,k,w,d,h);
        //                     triangles[getIndex(i,j,k,w,d,h)*6*2*3 + 12*l + 1] = getIndex(i+second[0],j+second[1],k+second[2],w,d,h);
        //                     triangles[getIndex(i,j,k,w,d,h)*6*2*3 + 12*l + 2] = getIndex(i+first[0],j+first[1],k+first[2],w,d,h);

        //                     triangles[getIndex(i,j,k,w,d,h)*6*2*3 + 3 + 12*l + 0] = getIndex(i+first[0]+second[0],j+first[1]+second[1],k+first[2]+second[2],w,d,h);
        //                     triangles[getIndex(i,j,k,w,d,h)*6*2*3 + 3 + 12*l + 1] = getIndex(i+first[0],j+first[1],k+first[2],w,d,h);
        //                     triangles[getIndex(i,j,k,w,d,h)*6*2*3 + 3 + 12*l + 2] = getIndex(i+second[0],j+second[1],k+second[2],w,d,h);

        //                     triangles[getIndex(i,j,k,w,d,h)*6*2*3 + 6 + 12*l + 0] = getIndex(i+third[0],j+third[1],k+third[2],w,d,h);
        //                     triangles[getIndex(i,j,k,w,d,h)*6*2*3 + 6 + 12*l + 1] = getIndex(i+first[0]+third[0],j+first[1]+third[1],k+third[2]+first[2],w,d,h);
        //                     triangles[getIndex(i,j,k,w,d,h)*6*2*3 + 6 + 12*l + 2] = getIndex(i+second[0]+third[0],j+second[1]+third[1],k+third[2]+second[2],w,d,h);

        //                     triangles[getIndex(i,j,k,w,d,h)*6*2*3 + 9 + 12*l + 0] = getIndex(i+first[0]+second[0]+third[0],j+third[1]+first[1]+second[1],k+third[2]+first[2]+second[2],w,d,h);
        //                     triangles[getIndex(i,j,k,w,d,h)*6*2*3 + 9 + 12*l + 1] = getIndex(i+second[0]+third[0],j+third[1]+second[1],k+third[2]+second[2],w,d,h);
        //                     triangles[getIndex(i,j,k,w,d,h)*6*2*3 + 9 + 12*l + 2] = getIndex(i+first[0]+third[0],j+third[1]+first[1],k+third[2]+first[2],w,d,h);
        //                 }

        //             }
        //         }
        //     }
        // }


    }

    int getIndex(int x, int y, int z, int w, int d, int h)
    {
        return z*w*d+y*w+x;
    }
    int[,,] generateMaze()
    {
        int[,,] temp = new int[21,21,5];
        for(int x = 0; x < temp.GetLength(0); x++)
        {
            for(int y = 0; y < temp.GetLength(1); y++)
            {
                for(int z = 0; z < temp.GetLength(2); z++)
                {
                    temp[x,y,z] = 0;
                }
            }   
        }

        temp[0,0,1] = 1;
        temp[0,1,1] = 1;
        temp[0,2,1] = 1;
        temp[0,3,1] = 1;
        temp[0,4,1] = 1;
        temp[0,5,1] = 1;
        temp[0,6,1] = 1;
        temp[0,7,1] = 1;
        temp[0,8,1] = 1;
        temp[0,9,1] = 1;
        temp[0,10,1] = 1;
        temp[0,11,1] = 1;
        temp[0,12,1] = 1;
        temp[0,13,1] = 1;
        temp[0,14,1] = 1;
        temp[0,15,1] = 1;
        temp[0,16,1] = 1;
        temp[0,17,1] = 1;
        temp[0,18,1] = 1;
        temp[0,19,1] = 1;
        temp[0,20,1] = 1;
        temp[1,0,1] = 1;
        temp[1,2,1] = 1;
        temp[1,6,1] = 1;
        temp[1,12,1] = 1;
        temp[1,14,1] = 1;
        temp[1,20,1] = 1;
        temp[2,0,1] = 1;
        temp[2,2,1] = 1;
        temp[2,4,1] = 1;
        temp[2,6,1] = 1;
        temp[2,8,1] = 1;
        temp[2,9,1] = 1;
        temp[2,10,1] = 1;
        temp[2,12,1] = 1;
        temp[2,14,1] = 1;
        temp[2,16,1] = 1;
        temp[2,17,1] = 1;
        temp[2,18,1] = 1;
        temp[2,20,1] = 1;
        temp[3,0,1] = 1;
        temp[3,4,1] = 1;
        temp[3,8,1] = 1;
        temp[3,12,1] = 1;
        temp[3,16,1] = 1;
        temp[3,18,1] = 1;
        temp[3,20,1] = 1;
        temp[4,0,1] = 1;
        temp[4,1,1] = 1;
        temp[4,2,1] = 1;
        temp[4,3,1] = 1;
        temp[4,4,1] = 1;
        temp[4,5,1] = 1;
        temp[4,6,1] = 1;
        temp[4,7,1] = 1;
        temp[4,8,1] = 1;
        temp[4,10,1] = 1;
        temp[4,11,1] = 1;
        temp[4,12,1] = 1;
        temp[4,14,1] = 1;
        temp[4,15,1] = 1;
        temp[4,16,1] = 1;
        temp[4,18,1] = 1;
        temp[4,20,1] = 1;
        temp[5,0,1] = 1;
        temp[5,8,1] = 1;
        temp[5,10,1] = 1;
        temp[5,18,1] = 1;
        temp[5,20,1] = 1;
        temp[6,0,1] = 1;
        temp[6,2,1] = 1;
        temp[6,3,1] = 1;
        temp[6,4,1] = 1;
        temp[6,5,1] = 1;
        temp[6,6,1] = 1;
        temp[6,8,1] = 1;
        temp[6,10,1] = 1;
        temp[6,11,1] = 1;
        temp[6,12,1] = 1;
        temp[6,13,1] = 1;
        temp[6,14,1] = 1;
        temp[6,15,1] = 1;
        temp[6,16,1] = 1;
        temp[6,18,1] = 1;
        temp[6,20,1] = 1;
        temp[7,0,1] = 1;
        temp[7,2,1] = 1;
        temp[7,8,1] = 1;
        temp[7,16,1] = 1;
        temp[7,18,1] = 1;
        temp[7,20,1] = 1;
        temp[8,0,1] = 1;
        temp[8,1,1] = 1;
        temp[8,2,1] = 1;
        temp[8,4,1] = 1;
        temp[8,5,1] = 1;
        temp[8,6,1] = 1;
        temp[8,8,1] = 1;
        temp[8,9,1] = 1;
        temp[8,10,1] = 1;
        temp[8,11,1] = 1;
        temp[8,12,1] = 1;
        temp[8,13,1] = 1;
        temp[8,14,1] = 1;
        temp[8,16,1] = 1;
        temp[8,17,1] = 1;
        temp[8,18,1] = 1;
        temp[8,20,1] = 1;
        temp[9,0,1] = 1;
        temp[9,4,1] = 1;
        temp[9,12,1] = 1;
        temp[9,16,1] = 1;
        temp[9,20,1] = 1;
        temp[10,0,1] = 1;
        temp[10,2,1] = 1;
        temp[10,3,1] = 1;
        temp[10,4,1] = 1;
        temp[10,5,1] = 1;
        temp[10,6,1] = 1;
        temp[10,7,1] = 1;
        temp[10,8,1] = 1;
        temp[10,9,1] = 1;
        temp[10,10,1] = 1;
        temp[10,12,1] = 1;
        temp[10,14,1] = 1;
        temp[10,15,1] = 1;
        temp[10,16,1] = 1;
        temp[10,18,1] = 1;
        temp[10,19,1] = 1;
        temp[10,20,1] = 1;
        temp[11,0,1] = 1;
        temp[11,4,1] = 1;
        temp[11,10,1] = 1;
        temp[11,12,1] = 1;
        temp[11,16,1] = 1;
        temp[11,20,1] = 1;
        temp[12,0,1] = 1;
        temp[12,1,1] = 1;
        temp[12,2,1] = 1;
        temp[12,4,1] = 1;
        temp[12,6,1] = 1;
        temp[12,7,1] = 1;
        temp[12,8,1] = 1;
        temp[12,10,1] = 1;
        temp[12,11,1] = 1;
        temp[12,12,1] = 1;
        temp[12,13,1] = 1;
        temp[12,14,1] = 1;
        temp[12,16,1] = 1;
        temp[12,17,1] = 1;
        temp[12,18,1] = 1;
        temp[12,20,1] = 1;
        temp[13,0,1] = 1;
        temp[13,4,1] = 1;
        temp[13,6,1] = 1;
        temp[13,8,1] = 1;
        temp[13,14,1] = 1;
        temp[13,18,1] = 1;
        temp[13,20,1] = 1;
        temp[14,0,1] = 1;
        temp[14,2,1] = 1;
        temp[14,3,1] = 1;
        temp[14,4,1] = 1;
        temp[14,6,1] = 1;
        temp[14,8,1] = 1;
        temp[14,9,1] = 1;
        temp[14,10,1] = 1;
        temp[14,12,1] = 1;
        temp[14,14,1] = 1;
        temp[14,15,1] = 1;
        temp[14,16,1] = 1;
        temp[14,18,1] = 1;
        temp[14,20,1] = 1;
        temp[15,0,1] = 1;
        temp[15,4,1] = 1;
        temp[15,10,1] = 1;
        temp[15,12,1] = 1;
        temp[15,16,1] = 1;
        temp[15,18,1] = 1;
        temp[15,20,1] = 1;
        temp[16,0,1] = 1;
        temp[16,2,1] = 1;
        temp[16,4,1] = 1;
        temp[16,5,1] = 1;
        temp[16,6,1] = 1;
        temp[16,8,1] = 1;
        temp[16,9,1] = 1;
        temp[16,10,1] = 1;
        temp[16,12,1] = 1;
        temp[16,13,1] = 1;
        temp[16,14,1] = 1;
        temp[16,15,1] = 1;
        temp[16,16,1] = 1;
        temp[16,18,1] = 1;
        temp[16,20,1] = 1;
        temp[17,0,1] = 1;
        temp[17,2,1] = 1;
        temp[17,4,1] = 1;
        temp[17,8,1] = 1;
        temp[17,12,1] = 1;
        temp[17,18,1] = 1;
        temp[17,20,1] = 1;
        temp[18,0,1] = 1;
        temp[18,2,1] = 1;
        temp[18,4,1] = 1;
        temp[18,6,1] = 1;
        temp[18,7,1] = 1;
        temp[18,8,1] = 1;
        temp[18,10,1] = 1;
        temp[18,11,1] = 1;
        temp[18,12,1] = 1;
        temp[18,14,1] = 1;
        temp[18,15,1] = 1;
        temp[18,16,1] = 1;
        temp[18,17,1] = 1;
        temp[18,18,1] = 1;
        temp[18,20,1] = 1;
        temp[19,0,1] = 1;
        temp[19,2,1] = 1;
        temp[19,8,1] = 1;
        temp[19,20,1] = 1;
        temp[20,0,1] = 1;
        temp[20,1,1] = 1;
        temp[20,2,1] = 1;
        temp[20,3,1] = 1;
        temp[20,4,1] = 1;
        temp[20,5,1] = 1;
        temp[20,6,1] = 1;
        temp[20,7,1] = 1;
        temp[20,8,1] = 1;
        temp[20,9,1] = 1;
        temp[20,10,1] = 1;
        temp[20,11,1] = 1;
        temp[20,12,1] = 1;
        temp[20,13,1] = 1;
        temp[20,14,1] = 1;
        temp[20,15,1] = 1;
        temp[20,16,1] = 1;
        temp[20,17,1] = 1;
        temp[20,18,1] = 1;
        temp[20,19,1] = 1;
        temp[20,20,1] = 1;
        temp[19,19,1] = 2;
        temp[1,1,1] = 3;

        for(int i = 0; i < 21; i++)
        {
            for(int j = 0; j < 21; j++)
            {
                temp[i,j,2] = 1;
                temp[i,j,0] = 1;
            }
        }
        
        return temp;
    }
    void drawCube(float x, float y, float z,int[,,] maze, int i, int j, int k)
    {



        //Top
        if(k == 0 || maze[i,j,k-1] != 1)
        {
            vertices.Add(new Vector3(x-blockWidth/2.0f,y-blockWidth/2.0f,z-blockWidth/2.0f));
		    triangles.Add(vertices.Count-1);
            vertices.Add(new Vector3(x-blockWidth/2.0f,y+blockWidth/2.0f,z-blockWidth/2.0f));
		    triangles.Add(vertices.Count-1);
            vertices.Add(new Vector3(x+blockWidth/2.0f,y-blockWidth/2.0f,z-blockWidth/2.0f));
		    triangles.Add(vertices.Count-1);
		    triangles.Add(vertices.Count-1);
		    triangles.Add(vertices.Count-2);
            vertices.Add(new Vector3(x+blockWidth/2.0f,y+blockWidth/2.0f,z-blockWidth/2.0f));
		    triangles.Add(vertices.Count-1);

            uvs.Add(new Vector2(0,0));
            uvs.Add(new Vector2(1,0));
            uvs.Add(new Vector2(0,1));
            uvs.Add(new Vector2(1,1));

        }

        //South
        if(j == 0 || maze[i,j-1,k] != 1)
        {
            vertices.Add(new Vector3(x-blockWidth/2.0f,y-blockWidth/2.0f,z-blockWidth/2.0f));
		    triangles.Add(vertices.Count-1);
            vertices.Add(new Vector3(x+blockWidth/2.0f,y-blockWidth/2.0f,z-blockWidth/2.0f));
		    triangles.Add(vertices.Count-1);
            vertices.Add(new Vector3(x-blockWidth/2.0f,y-blockWidth/2.0f,z+blockWidth/2.0f));
		    triangles.Add(vertices.Count-1);
		    triangles.Add(vertices.Count-2);
            vertices.Add(new Vector3(x+blockWidth/2.0f,y-blockWidth/2.0f,z+blockWidth/2.0f));
		    triangles.Add(vertices.Count-1);
		    triangles.Add(vertices.Count-2);

            uvs.Add(new Vector2(0,0));
            uvs.Add(new Vector2(1,0));
            uvs.Add(new Vector2(0,1));
            uvs.Add(new Vector2(1,1));

        }

        //East
        if(i == maze.GetLength(0)-1 || maze[i+1,j,k] != 1)
        {
            vertices.Add(new Vector3(x+blockWidth/2.0f,y+blockWidth/2.0f,z-blockWidth/2.0f));
		    triangles.Add(vertices.Count-1);
            vertices.Add(new Vector3(x+blockWidth/2.0f,y-blockWidth/2.0f,z+blockWidth/2.0f));
		    triangles.Add(vertices.Count-1);
            vertices.Add(new Vector3(x+blockWidth/2.0f,y-blockWidth/2.0f,z-blockWidth/2.0f));
		    triangles.Add(vertices.Count-1);
            vertices.Add(new Vector3(x+blockWidth/2.0f,y+blockWidth/2.0f,z+blockWidth/2.0f));
		    triangles.Add(vertices.Count-1);
		    triangles.Add(vertices.Count-3);
		    triangles.Add(vertices.Count-4);
            uvs.Add(new Vector2(1,0));
            uvs.Add(new Vector2(0,1));
            uvs.Add(new Vector2(0,0));
            uvs.Add(new Vector2(1,1));
        }
        //North
        if(j == maze.GetLength(1)-1 || maze[i,j+1,k] != 1)
        {
            vertices.Add(new Vector3(x+blockWidth/2.0f,y+blockWidth/2.0f,z+blockWidth/2.0f));
		    triangles.Add(vertices.Count-1);
            vertices.Add(new Vector3(x+blockWidth/2.0f,y+blockWidth/2.0f,z-blockWidth/2.0f));
		    triangles.Add(vertices.Count-1);
            vertices.Add(new Vector3(x-blockWidth/2.0f,y+blockWidth/2.0f,z+blockWidth/2.0f));
		    triangles.Add(vertices.Count-1);
		    triangles.Add(vertices.Count-2);
            vertices.Add(new Vector3(x-blockWidth/2.0f,y+blockWidth/2.0f,z-blockWidth/2.0f));
		    triangles.Add(vertices.Count-1);
		    triangles.Add(vertices.Count-2);

            uvs.Add(new Vector2(1,1));
            uvs.Add(new Vector2(1,0));
            uvs.Add(new Vector2(0,1));
            uvs.Add(new Vector2(0,0));
        }

        //West
        if(i == 0 || maze[i-1,j,k] != 1)
        {
            vertices.Add(new Vector3(x-blockWidth/2.0f,y+blockWidth/2.0f,z+blockWidth/2.0f));
		    triangles.Add(vertices.Count-1);
            vertices.Add(new Vector3(x-blockWidth/2.0f,y+blockWidth/2.0f,z-blockWidth/2.0f));
		    triangles.Add(vertices.Count-1);
            vertices.Add(new Vector3(x-blockWidth/2.0f,y-blockWidth/2.0f,z+blockWidth/2.0f));
		    triangles.Add(vertices.Count-1);
            vertices.Add(new Vector3(x-blockWidth/2.0f,y-blockWidth/2.0f,z-blockWidth/2.0f));
		    triangles.Add(vertices.Count-1);
		    triangles.Add(vertices.Count-2);
		    triangles.Add(vertices.Count-3);

            uvs.Add(new Vector2(1,1));
            uvs.Add(new Vector2(1,0));
            uvs.Add(new Vector2(0,1));
            uvs.Add(new Vector2(0,0));
        }


        //Bottom
        if(k == maze.GetLength(2) || maze[i,j,k+1] != 1)
        {
            vertices.Add(new Vector3(x+blockWidth/2.0f,y+blockWidth/2.0f,z+blockWidth/2.0f));
		    triangles.Add(vertices.Count-1);
            vertices.Add(new Vector3(x-blockWidth/2.0f,y+blockWidth/2.0f,z+blockWidth/2.0f));
		    triangles.Add(vertices.Count-1);
            vertices.Add(new Vector3(x+blockWidth/2.0f,y-blockWidth/2.0f,z+blockWidth/2.0f));
		    triangles.Add(vertices.Count-1);
		    triangles.Add(vertices.Count-2);
            vertices.Add(new Vector3(x-blockWidth/2.0f,y-blockWidth/2.0f,z+blockWidth/2.0f));
		    triangles.Add(vertices.Count-1);
		    triangles.Add(vertices.Count-2);

            uvs.Add(new Vector2(0,0));
            uvs.Add(new Vector2(1,0));
            uvs.Add(new Vector2(0,1));
            uvs.Add(new Vector2(1,1));
        }
    }
}
