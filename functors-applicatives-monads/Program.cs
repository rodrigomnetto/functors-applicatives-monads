using functors_applicatives_monads;
using Newtonsoft.Json;

var tree1 = new Node(new Node(new Leaf(0), new Node(new Leaf(0), new Leaf(0))), new Node(new Leaf(0), new Leaf(0)));

var tree2 = RelabelTree(tree1).Run(1);

Console.WriteLine(JsonConvert.SerializeObject(tree2, Formatting.Indented));

ST<Tree> RelabelTree(Tree t) 
{
    return t switch
    {
        Node { Left: var l, Rigth: var r } => 
            RelabelTree(l)
            .Bind((lr) =>
                RelabelTree(r)
                .Bind((rl) => (new Node(lr, rl)).Return())
            ).Cast(),
        _ => Fresh().Bind((n) => (new Leaf(n)).Return()).Cast(),
    };
}

ST<int> Fresh() => ST<int>.Pack((int n) => (n, n + 1));

public record Tree();
public record Node(Tree Left, Tree Rigth) : Tree;
public record Leaf(int Value) : Tree;

public static class TreeExtensions
{
    public static ST<Tree> Cast(this ST<Leaf> st) => ST<Tree>.Pack((s) =>
    {
        (Tree r, var s1) = st.Run(s);
        return (r, s1);
    });

    public static ST<Tree> Cast(this ST<Node> st) => ST<Tree>.Pack((s) =>
    {
        (Tree r, var s1) = st.Run(s);
        return (r, s1);
    });
}


