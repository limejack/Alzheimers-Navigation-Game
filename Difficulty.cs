using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Difficulty
{
    public static int GetDifficulty(int[,,] maze, Coordinate start, Coordinate end)
    {
        // 0 = maze, 1 = wall
        // coordinates of start and end points are represented by an array

        // fill up span tree using dfs
        Dictionary<Coordinate, List<Coordinate>> children = new Dictionary<Coordinate, List<Coordinate>>();
        Dictionary<Coordinate, List<Coordinate>> backEdges = new Dictionary<Coordinate, List<Coordinate>>();
        bool[,,] visited = new bool[maze.GetLength(0),maze.GetLength(1),maze.GetLength(2)];
        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                for (int k = 0; k < maze.GetLength(2); k++)
                {
                    visited[i,j,k] = false;
                    if (maze[i,j,k] == 0)
                    {
                        children[new Coordinate(i, j, k)] = new List<Coordinate>();
                        backEdges[new Coordinate(i, j, k)] = new List<Coordinate>();
                    }
                }
            }
        }
        //Dfs(start, maze, visited, children, backEdges);

        // from start, the root of the span tree, mark which nodes you can reach end by only going from a node to its children
        // then, mark every node that is directly a node that is marked through a back edge
        // x = number of nodes not marked
        bool[,,] reachable = new bool[maze.GetLength(0),maze.GetLength(1),maze.GetLength(2)];
        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                for(int k = 0; k < maze.GetLength(2); k++)
                {
                    reachable[i,j,k] = false;
                }
            }
        }
        //IsReachableThroughChild(start, end, children, reachable);
        foreach (Coordinate c in backEdges.Keys)
        {
            if (backEdges[c].Count > 0)
            {
                bool works = reachable[c.x,c.y,c.z];
                List<Coordinate> curr = backEdges[c];
                for (int i = 0; i < curr.Count; i++)
                {
                    works = works || reachable[curr[i].x,curr[i].y,curr[i].z];
                }
            }
        }
        int x = 0;
        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                for (int k = 0; k < maze.GetLength(2); k++)
                {
                    if (maze[i,j,k] == 0 && !reachable[i,j,k])
                    {
                        x++;
                    }
                }
            } 
        }
        // use bfs to get shortest path
        // y = length of shortest path
        int[,,] minDist = new int[maze.GetLength(0),maze.GetLength(1),maze.GetLength(2)];
        for (int i = 0; i < minDist.GetLength(0); i++)
        {
            for (int j = 0; j < minDist.GetLength(1); j++)
            {
                for(int k = 0; k < minDist.GetLength(2); k++)
                {
                    minDist[i,j,k] = -1;
                }
            }
        }
        minDist[start.x,start.y,start.z] = -1;
        Coordinate[,,] prevNode = new Coordinate[maze.GetLength(0),maze.GetLength(1),maze.GetLength(2)];

        Queue<Coordinate> bfs = new Queue<Coordinate>();
        bfs.Enqueue(start);
        while (bfs.Count > 0)
        {
            Coordinate currNode = bfs.Dequeue();
            List<Coordinate> adj = AdjMazeCoord(currNode, maze);
            for (int i = 0; i < adj.Count; i++)
            {
                Coordinate adjCoord = adj[i];
                if (minDist[adjCoord.x,adjCoord.y,adjCoord.z] == -1)
                {
                    minDist[adjCoord.x,adjCoord.y,adjCoord.z] = minDist[currNode.x,currNode.y,currNode.z] + 1;
                    prevNode[adjCoord.x,adjCoord.y,adjCoord.z] = currNode;
                    bfs.Enqueue(new Coordinate(adjCoord.x, adjCoord.y, adjCoord.z));
                }
            }
        }
        int y = minDist[end.x,end.y,end.z];

        // mark intersections in the shortest path
        // z = number of intersections marked
        int currX = end.x;
        int currY = end.y;
        int currZ = end.z;
        int z = 0;
        while (true)
        {
            if (!(currX == start.x && currY == start.y && currZ == start.z))
            {
                if (AdjMazeCoord(new Coordinate(currX, currY, currZ), maze).Count > 2)
                {
                    z++;
                }
                Coordinate next = prevNode[currX,currY,currZ];
                currX = next.x;
                currY = next.y;
                currZ = next.z;
            }
            else
            {
                if (AdjMazeCoord(start, maze).Count > 2)
                {
                    z++;
                }
                break;
            }
        }
        return x + y * z;
    }

    public static bool IsReachableThroughChild(Coordinate start, Coordinate end, Dictionary<Coordinate, List<Coordinate>> children, bool[][][] reachable)
    {
        bool curr = (start.x == end.x) && (start.y == end.y) && (start.z == end.z);
        if (children.ContainsKey(start))
        {
            List<Coordinate> currChildren = children[start];
            for (int i = 0; i < currChildren.Count; i++)
            {
                curr = curr || IsReachableThroughChild(currChildren[i], end, children, reachable);
            }
        }
        reachable[start.x][start.y][start.z] = curr;
        return curr;
    }

    public static void Dfs(Coordinate cell, int[,,] maze, bool[,,] visited, Dictionary<Coordinate, List<Coordinate>> children, Dictionary<Coordinate, List<Coordinate>> backEdges)
    {
        List<Coordinate> adj = AdjMazeCoord(cell, maze);
        for (int i = 0; i < adj.Count; i++)
        {
            Coordinate adjCell = adj[i];
            if (!visited[adjCell.x,adjCell.y,adjCell.z])
            {
                children[new Coordinate(cell.x, cell.y, cell.z)].Add(new Coordinate(adjCell.x, adjCell.y, adjCell.z));
                visited[adjCell.x,adjCell.y,adjCell.z] = true;
                Dfs(adjCell, maze, visited, children, backEdges);
            }
            else
            {
                backEdges[new Coordinate(cell.x, cell.y, cell.z)].Add(new Coordinate(adjCell.x, adjCell.y, adjCell.z));
            }
        }
    }

    public static List<Coordinate> AdjMazeCoord(Coordinate cell, int[,,] maze)
    {
        List<Coordinate> adj = new List<Coordinate>();
        int[] dx = { 1, 0, 0, -1, 0, 0 };
        int[] dy = { 0, 0, 1, 0, 0, -1 };
        int[] dz = { 0, 1, 0, 0, -1, 0 };
        for (int i = 0; i < dx.Length; i++)
        {
            int newX = cell.x + dx[i];
            int newY = cell.y + dy[i];
            int newZ = cell.z + dz[i];
            if (newX >= 0 && newY >= 0 && newZ >= 0 &&
                newX < maze.GetLength(0) && newY < maze.GetLength(1) && newZ < maze.GetLength(2)
                && maze[newX,newY,newZ] == 0)
            {
                adj.Add(new Coordinate(newX, newY, newZ));
            }
        }
        return adj;
    }
}

public class Coordinate
{
    public int x;
    public int y;
    public int z;
    public Coordinate(int a, int b, int c)
    {
        x = a;
        y = b;
        z = c;
    }
}
