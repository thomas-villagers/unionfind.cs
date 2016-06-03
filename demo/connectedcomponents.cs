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
