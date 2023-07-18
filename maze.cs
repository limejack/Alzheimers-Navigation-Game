using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Maze
{
    public static int[,,] createMaze(int l, int w, int h) {
        int rows = l * 2 + 1;
        int cols = w * 2 + 1;
        int layers = h * 2 + 1;
        int[,,] arr = new int[rows,cols,layers];
        for(int x = 0; x < arr.GetLength(0); x++) {
            for(int y = 0; y < arr.GetLength(1); y++){
                for(int z = 0; z < arr.GetLength(2); z++){
                    arr[x,y,z] = 0;
                }
            }   
        }
        
        for (int r = 0; r < rows; r++) {
            for (int c = 0; c < cols; c++) {
                for (int i = 0; i < layers; i++) {
                    if (i % 2 == 0 || i == layers - 1) {
                        arr[r,c,i] = 1;
                    }
                    if (r % 2 == 0 || r == rows - 1) {
                        arr[r,c,i] = 1;
                    }
                    if (c % 2 == 0 || c == cols - 1) {
                        arr[r,c,i] = 1;
                    }            
                }
            }
        }
        
        return arr;
    }


    public static List<int[]> getAvailableNeighbors(int x, int y, int z, int[,,] maze, List<int[]> visited) {
        List<int[]> availableNeighbors = new List<int[]>();
        int[] a = {x-2,y,z};
        int[] b = {x, y-2, z};
        int[] c = {x+2, y, z};
        int[] d = {x, y+2, z};
        int[] e = {x, y, z-2};
        int[] f = {x, y, z+2};

        if (x != 1 && !isInVisited(a,visited)) {
            availableNeighbors.Add(a);
        }
        if (y != 1 && !isInVisited(b,visited)) {
            availableNeighbors.Add(b);
        }
        if (x != maze.GetLength(0) - 2 && !isInVisited(c,visited)) {
            availableNeighbors.Add(c);
        }
        if (y != maze.GetLength(1) - 2 && !isInVisited(d,visited)) {
            availableNeighbors.Add(d);
        }
        if (z != 1 & !isInVisited(e,visited)) {
            availableNeighbors.Add(e);
        }
        if (z != maze.GetLength(2) - 2 && !isInVisited(f,visited)) {
            availableNeighbors.Add(f);
        }
        
    
        return availableNeighbors;
    }

    public static bool isInVisited(int[] arr,List<int[]> visited) {
        foreach (int[] coords in visited) {
            if (coords[0] == arr[0] && coords[1] == arr[1] && coords[2] == arr[2]) {
                return true;
            }
        }
        
        return false;
    }
    public static int[,,] generateMaze(int h, int w, int l)
    {
        int[,,] maze = createMaze(h, w, l);
        List<int[]> visited = new List<int[]>();
        int visitIndex = 0;
        int x = 1;
        int y = 1;
        int z = 1;

        while (visited.Count < h*w*l) {
            int[] coords = {x,y,z};
            if (!isInVisited(coords,visited)) {
                visited.Add(coords);
            }
            List<int[]> neighbors = getAvailableNeighbors(x,y,z,maze, visited);
            if (neighbors.Count > 0) {
                int[] randNeighbor = neighbors[(int)(Random.Range(0,neighbors.Count))];
                maze[(randNeighbor[0] + x)/2,(randNeighbor[1] + y)/2,(randNeighbor[2] + z)/2] = 0;
                x = randNeighbor[0];
                y = randNeighbor[1];
                z = randNeighbor[2];
            } else {
                for (int i = visited.Count - 1; i >= 0; i--) {
                    x = visited[i][0];
                    y = visited[i][1];  
                    z = visited[i][2];
                    if (getAvailableNeighbors(x,y,z,maze,visited).Count > 0) {
                        break;
                    }
                }
            }
        }
        return maze;
    }
}