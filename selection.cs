using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Selection
{
    Coordinate[] points;
    Path[] paths;
    //Chose 10 points
    //Get difficulty between all 10, then sort
    public Selection(int[,,] maze)
    {
        points = new Coordinate[10];
        for(int i = 0; i < 10; i++)
        {
            points[i] = generatePointOnPath(maze);
        }

        paths = new Path[45];
        int pos = 0;
        for(int i = 0; i < 10; i++)
        {
            for(int j = i+1; j < 10; j++)
            {
                paths[pos] = new Path(points[i],points[j],maze);
                pos += 1;
            }
        }

        for(int i = 0; i < 44; i++)
        {
            for(int j = 0; j < 44; j++)
            {
                if(paths[j].difficulty > paths[j+1].difficulty)
                {
                    Path temp = paths[j];
                    paths[j] = paths[j+1];
                    paths[j+1] = temp;
                }
            }
        }
        for(int i = 0; i < 45; i++)
        {
            Debug.Log(paths[i].toStr());
        }
    }
    
    public Path getPath()
    {
        return paths[0];
    }

    Coordinate generatePointOnPath(int[,,] maze)
    {
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
}

public class Path
{
    public Coordinate pointA,pointB;
    public int difficulty;
    public int x,y,z;
    public Path(Coordinate a, Coordinate b, int[,,] maze)
    {
        pointA = a;
        pointB = b;
        int[] temp = Difficulty.GetDifficulty(maze,a,b);

        x = temp[0];y = temp[1]; z = temp[2];

        difficulty = x + y*z;

    }
    public string toStr()
    {
        return difficulty+": "+x+" "+y+" "+z;
    }
}