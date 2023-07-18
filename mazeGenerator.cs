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
    public float blockWidth = 2;

    public Transform goal;


    // Start is called before the first frame update
    void Start()
    {
        int[,,] maze = Maze.generateMaze(5,5,5);
        mesh = new Mesh();


        int h,w,d;
        w = maze.GetLength(0);
        d = maze.GetLength(1);
        h = maze.GetLength(2);
        vertices     = new List<Vector3>();
        uvs     = new List<Vector2>();
        triangles    = new List<int>();
        

        mesh.Clear();

        Coordinate start = new Coordinate(1,1,1);
        Transform player = GameObject.Find("Player").GetComponent<Transform>();
        player.position = new Vector3(blockWidth,blockWidth,blockWidth);

        
        Coordinate end = generateGoal(maze);
        goal.position = new Vector3(end.x*blockWidth,end.z*blockWidth,end.y*blockWidth);
        goal.localScale    = blockWidth * Vector3.one;

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
                } 
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.triangles = triangles.ToArray();


        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;

        Debug.Log(Difficulty.GetDifficulty(maze,start,end));
    

        
    }

    int getIndex(int x, int y, int z, int w, int d, int h)
    {
        return z*w*d+y*w+x;
    }
    Coordinate generateGoal(int[,,] maze)
    {
        Debug.Log(maze.GetLength(0));
        Debug.Log(maze.GetLength(1));
        Debug.Log(maze.GetLength(2));
        int a,b,c;
        a = Random.Range(1,maze.GetLength(0)-1);
        b = Random.Range(1,maze.GetLength(1)-1);
        c = Random.Range(1,maze.GetLength(2)-1);

        while(maze[a,b,c] != 0)
        {
            a = Random.Range(1,maze.GetLength(0)-1);
            b = Random.Range(1,maze.GetLength(1)-1);
            c = Random.Range(1,maze.GetLength(2)-1);
        }
        return new Coordinate(a,b,c);
        

    }
    void drawCube(float x, float y, float z,int[,,] maze, int i, int j, int k)
    {



        //Top
        if(k != maze.GetLength(2)-1 && maze[i,j,k+1] != 1)
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
        if(j != 0 && maze[i,j-1,k] != 1)
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
        if(i != maze.GetLength(0)-1 && maze[i+1,j,k] != 1)
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
        if(j != maze.GetLength(1)-1 && maze[i,j+1,k] != 1)
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
        if(i != 0 && maze[i-1,j,k] != 1)
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
        if(k != 0 && maze[i,j,k-1] != 1)
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
