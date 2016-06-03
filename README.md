<div id="table-of-contents">
<h2>Table of Contents</h2>
<div id="text-table-of-contents">
<ul>
<li><a href="#orgheadline1">1. Union Find Data Structure</a></li>
<li><a href="#orgheadline7">2. Applications</a>
<ul>
<li><a href="#orgheadline2">2.1. Connected Components</a></li>
<li><a href="#orgheadline6">2.2. Maze Generation</a>
<ul>
<li><a href="#orgheadline3">2.2.1. Example</a></li>
<li><a href="#orgheadline4">2.2.2. Larger Example</a></li>
<li><a href="#orgheadline5">2.2.3. An Even Larger Example</a></li>
</ul>
</li>
</ul>
</li>
</ul>
</div>
</div>


# Union Find Data Structure<a id="orgheadline1"></a>

    using System.Collections.Generic;
    
    public class UnionFind<T> {
    
      public class SetElement {
        public SetElement parent;
        public readonly T value;
        public int size; 
    
        public SetElement(T value) {
          this.value = value;
          parent = this; 
          size = 1; 
        }
    
        override public string ToString() {
         return string.Format("{0}, size:{1}", value, size);
        }
      }
    
      Dictionary<T, SetElement> dict;
    
      public UnionFind() {
        dict = new Dictionary<T, SetElement>(); 
      }
    
      public SetElement MakeSet(T value) {
        SetElement element = new SetElement(value); 
        dict[value] = element;
        return element;
      }
    
      public SetElement Find(T value) {
        SetElement element = dict[value];
        SetElement root = element; 
        while(root.parent != root) {
          root = root.parent; 
        }
        SetElement z = element; 
        while(z.parent != z) {
          SetElement temp = z; 
          z = z.parent;
          temp.parent = root;
        }
        return root; 
      }
    
      public SetElement Union(SetElement root1, SetElement root2) {
        if (root2.size > root1.size) {
          root2.size += root1.size;
          root1.parent = root2;
          return root2;
        } else {
          root1.size += root2.size;
          root2.parent = root1;
          return root1;
        }
      }
    }

# Applications<a id="orgheadline7"></a>

## Connected Components<a id="orgheadline2"></a>

    using System; 
    using Pair = System.Collections.Generic.KeyValuePair<string, string>;
    
    public class ConnectedComponents {
    
      public static void Main() {
    
        string[] names = {"John", "George", "Paul", "Mick", "Ringo", "Bill", "Brian", "Charlie", "Keith" };
        Pair[] pairs = { new Pair("John", "George"), new Pair("Paul", "Ringo"), new Pair("Ringo", "George"), new Pair("Charlie", "Bill"), new Pair("Keith", "Mick"), new Pair("Brian", "Bill"), new Pair("Charlie", "Keith")}; 
        var UF = new UnionFind<string>(); 
    
        foreach(var name in names) 
          UF.MakeSet(name);
    
        foreach(var pair in pairs) {
          if (UF.Find(pair.Key) != UF.Find(pair.Value))
            UF.Union(UF.Find(pair.Key), UF.Find(pair.Value));
        }
    
        foreach(var name in names) 
          Console.WriteLine("{0} belongs to set {1}", name, UF.Find(name)); 
      }
    }

    mcs demo/connectedcomponents.cs src/unionfind.cs
    mono demo/connectedcomponents.exe

    John belongs to set Paul, size:4
    George belongs to set Paul, size:4
    Paul belongs to set Paul, size:4
    Mick belongs to set Charlie, size:5
    Ringo belongs to set Paul, size:4
    Bill belongs to set Charlie, size:5
    Brian belongs to set Charlie, size:5
    Charlie belongs to set Charlie, size:5
    Keith belongs to set Charlie, size:5

## Maze Generation<a id="orgheadline6"></a>

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
            Console.WriteLine("  \\draw({0},{1}) -- ({2},{3});", cell2.Key, cell1.Value, cell2.Key, cell1.Value+1);
          else 
            Console.WriteLine("  \\draw({0},{1}) -- ({2},{3});", cell1.Key, cell2.Value, cell1.Key+1, cell2.Value);
        }
        Console.WriteLine("\\end{tikzpicture}");
      }
    
      public static void Main(string[] args) {
    
        int width, height;
        if (!Int32.TryParse(args[0], out width)) width = 7;
        if (!Int32.TryParse(args[0], out height)) height = 7;
    
        var UF = new UnionFind<Cell>(); 
        var grid = Enumerable.Range(0, width*height).Select( x => new Cell(x%width,x/width));
        foreach (var cell in grid) // works! 
          UF.MakeSet(cell);
    
       var edges = new List<Edge>();
        foreach(var cell in grid) {
          if (cell.Key < width-1) edges.Add(new Edge(cell, new Cell(cell.Key+1, cell.Value)));
          if (cell.Value < height-1) edges.Add(new Edge(cell, new Cell(cell.Key, cell.Value+1)));
        }
    
        var random = new Random();
        int removedEdges = 0; 
        while (removedEdges < grid.Count()-1) {
          int next = random.Next(edges.Count);
          var edge = edges[next];
          if (UF.Find(edge.Key) != UF.Find(edge.Value)) {
            UF.Union(UF.Find(edge.Key),UF.Find(edge.Value));
            edges.RemoveAt(next);
            removedEdges++;
          }
        }
        DrawMaze(edges, width, height); 
      }
    }

### Example<a id="orgheadline3"></a>

    mcs demo/mazegenerator.cs src/unionfind.cs
    mono demo/mazegenerator.exe 7 7

![img](images/maze.png)

### Larger Example<a id="orgheadline4"></a>

    mcs demo/mazegenerator.cs src/unionfind.cs
    mono demo/mazegenerator.exe 21 21

### An Even Larger Example<a id="orgheadline5"></a>

    mcs demo/mazegenerator.cs src/unionfind.cs
    mono demo/mazegenerator.exe 54 54