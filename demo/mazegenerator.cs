using System; 
using System.Linq; 
using System.Collections.Generic; 

using Cell = System.Collections.Generic.KeyValuePair<int, int>;
using Edge = System.Collections.Generic.KeyValuePair<System.Collections.Generic.KeyValuePair<int, int>, System.Collections.Generic.KeyValuePair<int, int>>;

public class MazeGenerator {

  public static void DrawMaze(List<Edge> edges, int width, int height) {
    Console.WriteLine("\\begin{tikzpicture}[>=latex]");
    Console.WriteLine("  \\draw[thick](0,0) -- ({0},0);",width); 
    Console.WriteLine("  \\draw[thick](0,{0}) -- ({1},{0});",width, height); 
    Console.WriteLine("  \\draw[thick](0,0) -- (0,{0});",height-1);     
    Console.WriteLine("  \\draw[thick]({0},1) -- ({0},{1});",width, height); 
    foreach(var edge in edges) {
      var cell1 = edge.Key;
      var cell2 = edge.Value;
      if (cell1.Value == cell2.Value)
        Console.WriteLine("  \\draw({0},{1}) -- ++(0,1);", cell2.Key, cell1.Value);
      else 
        Console.WriteLine("  \\draw({0},{1}) -- ++(1,0);", cell1.Key, cell2.Value);
    }
    Console.WriteLine("\\end{tikzpicture}");
  }

  public static void Main(string[] args) {

    int width, height;
    if (!(args.Length > 0 && Int32.TryParse(args[0], out width))) width = 7;
    if (!(args.Length > 1 && Int32.TryParse(args[1], out height))) height = width;

    var grid = Enumerable.Range(0, width*height).Select( x => new Cell(x%width,x/width));

    var edges = new List<Edge>();
    var UF = new UnionFind<Cell>(); 
    foreach(var cell in grid) {
      if (cell.Key < width-1) edges.Add(new Edge(cell, new Cell(cell.Key+1, cell.Value)));
      if (cell.Value < height-1) edges.Add(new Edge(cell, new Cell(cell.Key, cell.Value+1)));
      UF.MakeSet(cell);
    }

    var random = new Random();
    var candidates = new List<Edge>(edges);
    int edgesToRemove = grid.Count()-1;
    while(edgesToRemove > 0) {
      int next = random.Next(candidates.Count);
      var edge = candidates[next];
      if (UF.Find(edge.Key) != UF.Find(edge.Value)) {
        UF.Union(UF.Find(edge.Key),UF.Find(edge.Value));
        edges.Remove(edge);
        edgesToRemove--;
      }
      candidates.RemoveAt(next); 
    }
    DrawMaze(edges, width, height); 
  }
}
