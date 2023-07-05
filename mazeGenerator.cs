using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class mazeGenerator : MonoBehaviour
{

    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;


    // Start is called before the first frame update
    void Start()
    {
        int[,,] maze = generateMaze();
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;


        int h,w,d;
        w = maze.GetLength(0);
        d = maze.GetLength(1);
        h = maze.GetLength(2);
        vertices     = new Vector3[w*d*h];
        triangles    = new int[6*w*d*h*2];


        mesh.Clear();

        
        for(int x = 0; x < maze.GetLength(0); x++)
        {
            for(int y = 0; y < maze.GetLength(1); y++)
            {
                for(int z = 0; z < maze.GetLength(2); z++)
                {
                    vertices[z*(w*d)+y*w+x] = new Vector3(x,y,z);
                }
            }
        }

        for(int i = 0; i < w-1; i++)
        {
            for(int j = 0; j < d-1; j++)
            {
                for(int k = 0; k < h-1; k++)
                {
                    if(maze[i,j,k] == 1)
                    {
                        for(int l = 0; l < 3; l++)
                        {
                            int[] first = {0,0,0};
                            first[l] += 1;
                            int[] second = {0,0,0};
                            if(l == 2) second[0] += 1;
                            else second[l+1] += 1;

                            int[] third = {0,0,0};
                            if(l == 0)
                            {
                                third[2] += 1;
                            }
                            else
                            {
                                third[l-1] += 1;
                            }

                            triangles[getIndex(i,j,k,w,d,h)*6*2*3 + 12*l  + 0] = getIndex(i,j,k,w,d,h);
                            triangles[getIndex(i,j,k,w,d,h)*6*2*3 + 12*l + 1] = getIndex(i+second[0],j+second[1],k+second[2],w,d,h);
                            triangles[getIndex(i,j,k,w,d,h)*6*2*3 + 12*l + 2] = getIndex(i+first[0],j+first[1],k+first[2],w,d,h);

                            triangles[getIndex(i,j,k,w,d,h)*6*2*3 + 3 + 12*l + 0] = getIndex(i+first[0]+second[0],j+first[1]+second[1],k+first[2]+second[2],w,d,h);
                            triangles[getIndex(i,j,k,w,d,h)*6*2*3 + 3 + 12*l + 1] = getIndex(i+first[0],j+first[1],k+first[2],w,d,h);
                            triangles[getIndex(i,j,k,w,d,h)*6*2*3 + 3 + 12*l + 2] = getIndex(i+second[0],j+second[1],k+second[2],w,d,h);

                            triangles[getIndex(i,j,k,w,d,h)*6*2*3 + 6 + 12*l + 0] = getIndex(i+third[0],j+third[1],k+third[2],w,d,h);
                            triangles[getIndex(i,j,k,w,d,h)*6*2*3 + 6 + 12*l + 1] = getIndex(i+first[0]+third[0],j+first[1]+third[1],k+third[2]+first[2],w,d,h);
                            triangles[getIndex(i,j,k,w,d,h)*6*2*3 + 6 + 12*l + 2] = getIndex(i+second[0]+third[0],j+second[1]+third[1],k+third[2]+second[2],w,d,h);

                            triangles[getIndex(i,j,k,w,d,h)*6*2*3 + 9 + 12*l + 0] = getIndex(i+first[0]+second[0]+third[0],j+third[1]+first[1]+second[1],k+third[2]+first[2]+second[2],w,d,h);
                            triangles[getIndex(i,j,k,w,d,h)*6*2*3 + 9 + 12*l + 1] = getIndex(i+second[0]+third[0],j+third[1]+second[1],k+third[2]+second[2],w,d,h);
                            triangles[getIndex(i,j,k,w,d,h)*6*2*3 + 9 + 12*l + 2] = getIndex(i+first[0]+third[0],j+third[1]+first[1],k+third[2]+first[2],w,d,h);
                        }

                    }
                }
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();

    }

    int getIndex(int x, int y, int z, int w, int d, int h)
    {
        return z*w*d+y*w+x;
    }
    int[,,] generateMaze()
    {
        int[,,] temp = new int[5,5,5];
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
        temp[0,0,0] = 1;
        temp[1,0,0] = 1;
        temp[2,0,0] = 1;
        temp[3,0,0] = 1;
        return temp;
    }
    void OnGizmosDraw()
    {
        Debug.Log(vertices.Length);
        for(int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i],0.4f);
        }
    }
}
