namespace ZhangShashaCSharp
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public class Tree
	{

		public Node root;
		public Dictionary<int, Node> all_nodes = new Dictionary<int, Node>();
		// function l() which gives the leftmost leaf for a given node (identified by post-order number).
		public List<int> l = new List<int>();
		// list of keyroots for a tree root, i.e., nodes with a left sibling, or in the case of the root, the root itself.
		public List<int> keyroots = new List<int>();
		// list of the labels of the nodes used for node comparison
		public List<string> labels = new List<string>();
		public Dictionary<Node, IParseTree> antlr_nodes = new Dictionary<Node, IParseTree>();

		// the following constructor handles s-expression notation. E.g., ( f a ( b c ) )
		public Tree(IParseTree t, Parser parser, Lexer lexer)
		{
			root = Convert(t, parser, lexer);
		}

		private Node Convert(IParseTree t, Parser parser, Lexer lexer)
        {
			Node n = new Node();
			antlr_nodes[n] = t;
			if (t is ParserRuleContext tt)
            {
				n.label = parser.RuleNames[tt.RuleIndex];
				if (tt.children != null)
				{
					var ch = tt.children.Select(c => Convert(c, parser, lexer)).ToList();
					n.children = ch;
				}
				else n.children = new List<Node>();
			}
            else if (t is TerminalNodeImpl ff)
            {
				n.label = lexer.Vocabulary.GetSymbolicName(ff.Symbol.Type);
			}
			return n;
		}

        public void Traverse()
		{
			// put together an ordered list of node labels of the tree
			Traverse(root, labels);
		}

		private static List<string> Traverse(Node node, List<string> labels)
		{
			for (int i = 0; i < node.children.Count; i++)
			{
				labels = Traverse(node.children[i], labels);
			}
			labels.Add(node.label);
			return labels;
		}

		public void ComputePostOrderNumber()
		{
			// index each node in the tree according to traversal method
			ComputePostOrderNumber(root, 0);
		}

		private int ComputePostOrderNumber(Node node, int index)
		{
			for (int i = 0; i < node.children.Count; i++)
			{
				index = ComputePostOrderNumber(node.children[i], index);
			}
			index++;
			node.postorder_number = index;
			all_nodes[index] = node;
			return index;
		}

		public void ComputeLeftMostLeaf()
		{
			// put together a function which gives l()
			Leftmost();
			l = ComputeLeftMostLeaf(root, new List<int>());
		}

		private List<int> ComputeLeftMostLeaf(Node node, List<int> l)
		{
			for (int i = 0; i < node.children.Count; i++)
			{
				l = ComputeLeftMostLeaf(node.children[i], l);
			}
			l.Add(node.leftmost.postorder_number);
			return l;
		}

		private void Leftmost()
		{
			Leftmost(root);
		}

		private static void Leftmost(Node node)
		{
			if (node == null)
				return;
			for (int i = 0; i < node.children.Count; i++)
			{
				Leftmost(node.children[i]);
			}
			if (node.children.Count == 0)
			{
				node.leftmost = node;
			}
			else
			{
				node.leftmost = node.children[0].leftmost;
			}
		}

		public void Keyroots()
		{
			// calculate the keyroots
			for (int i = 0; i < l.Count; i++)
			{
				int flag = 0;
				for (int j = i + 1; j < l.Count; j++)
				{
					if (l[j] == l[i])
					{
						flag = 1;
					}
				}
				if (flag == 0)
				{
					this.keyroots.Add(i + 1);
				}
			}
		}

		static int[,] tree_distance;
		static List<Operation>[,] tree_operations;

		public static (int, List<Operation>) ZhangShasha(Tree tree1, Tree tree2)
		{
			tree1.ComputePostOrderNumber();
			tree1.ComputeLeftMostLeaf();
			tree1.Keyroots();
			tree1.Traverse();

			tree2.ComputePostOrderNumber();
			tree2.ComputeLeftMostLeaf();
			tree2.Keyroots();
			tree2.Traverse();

			List<int> l1 = tree1.l;
			List<int> keyroots1 = tree1.keyroots;
			List<int> l2 = tree2.l;
			List<int> keyroots2 = tree2.keyroots;

			// space complexity of the algorithm
			tree_distance = new int[l1.Count + 1, l2.Count + 1];
			tree_operations = new List<Operation>[l1.Count + 1, l2.Count + 1];
			for (int m = 0; m < l1.Count + 1; ++m)
				for (int n = 0; n < l2.Count + 1; ++n)
					tree_operations[m, n] = new List<Operation>();

			// solve subproblems
			for (int i1 = 1; i1 < keyroots1.Count + 1; i1++)
			{
				for (int j1 = 1; j1 < keyroots2.Count + 1; j1++)
				{
					int i = keyroots1[i1 - 1];
					int j = keyroots2[j1 - 1];
					(tree_distance[i, j], tree_operations[i, j]) = Treedist(l1, l2, i, j, tree1, tree2);
				}
			}

			return (tree_distance[l1.Count, l2.Count], tree_operations[l1.Count, l2.Count]);
		}

		private static (int, List<Operation>) Treedist(List<int> l1, List<int> l2, int i, int j, Tree tree1, Tree tree2)
		{
			int[,] forest_distance = new int[i + 1, j + 1];
			List<Operation>[,] forest_operations = new List<Operation>[i + 1, j + 1];
			for (int m = 0; m < i + 1; ++m)
				for (int n = 0; n < j + 1; ++n)
					forest_operations[m, n] = new List<Operation>();

			// costs of the three atomic operations
			int Delete = 1;
			int Insert = 1;
			int Relabel = 1;

			forest_distance[0, 0] = 0;
			forest_distance[l1[i - 1] - 1, 0] = 0;
			forest_distance[0, l2[j - 1] - 1] = 0;

			for (int i1 = l1[i - 1]; i1 <= i; i1++)
			{
				forest_distance[i1, 0] = forest_distance[i1 - 1, 0] + Delete;
				forest_operations[i1, 0] = new List<Operation>(forest_operations[i1 - 1, 0]);
				forest_operations[i1, 0].Add(new Operation() { O = Operation.Op.Delete, N1 = i1 + 1});
			}
			for (int j1 = l2[j - 1]; j1 <= j; j1++)
			{
				forest_distance[0, j1] = forest_distance[0, j1 - 1] + Insert;
				forest_operations[0, j1] = new List<Operation>(forest_operations[0, j1 - 1]);
				forest_operations[0, j1].Add(new Operation() { O = Operation.Op.Insert, N2 = j1 + 1});
			}
			for (int i1 = l1[i - 1]; i1 <= i; i1++)
			{
				for (int j1 = l2[j - 1]; j1 <= j; j1++)
				{
					int i_temp = (l1[i - 1] > i1 - 1) ? 0 : i1 - 1;
					int j_temp = (l2[j - 1] > j1 - 1) ? 0 : j1 - 1;
					if ((l1[i1 - 1] == l1[i - 1]) && (l2[j1 - 1] == l2[j - 1]))
					{
						int Cost = (tree1.labels[i1 - 1].Equals(tree2.labels[j1 - 1])) ? 0 : Relabel;
						
						int test1;
						List<Operation> list1;
						if (forest_distance[i_temp, j1] + Delete > forest_distance[i1, j_temp] + Insert)
                        {
							test1 = forest_distance[i1, j_temp] + Insert;
							list1 = new List<Operation>(forest_operations[i1, j_temp]);
							list1.Add(new Operation() { O = Operation.Op.Insert, N2 = j_temp + 1 });
							if (list1.Count != test1) throw new Exception();
						}
						else
                        {
							test1 = forest_distance[i_temp, j1] + Delete;
							list1 = new List<Operation>(forest_operations[i_temp, j1]);
							list1.Add(new Operation() { O = Operation.Op.Delete, N1 = i_temp + 1 });
							if (list1.Count != test1) throw new Exception();
						}

						int test2;
						List<Operation> list2;
						if (test1 > forest_distance[i_temp, j_temp] + Cost)
						{
							test2 = forest_distance[i_temp, j_temp] + Cost;
							list2 = new List<Operation>(forest_operations[i_temp, j_temp]);
							if (Cost > 0)
								list2.Add(new Operation() { O = Operation.Op.Change, N1 = i1, N2 = j1 });
							if (list2.Count != test2) throw new Exception();
						}
						else
						{
							test2 = test1;
							list2 = new List<Operation>(list1);
							if (list2.Count != test2) throw new Exception();
						}

						var temp = Math.Min(
							Math.Min(forest_distance[i_temp, j1] + Delete, forest_distance[i1, j_temp] + Insert),
							forest_distance[i_temp, j_temp] + Cost);
						if (test2 != temp) throw new Exception();

						forest_distance[i1, j1] = test2;
						forest_operations[i1, j1] = list2;
						if (temp != test2) throw new Exception();
						
						tree_distance[i1, j1] = test2;
						tree_operations[i1, j1] = list2;
					}
					else
					{
						int i1_temp = l1[i1 - 1] - 1;
						int j1_temp = l2[j1 - 1] - 1;

						int i_temp2 = (l1[i - 1] > i1_temp) ? 0 : i1_temp;
						int j_temp2 = (l2[j - 1] > j1_temp) ? 0 : j1_temp;

						int test1;
						List<Operation> list1;
						if (forest_distance[i_temp, j1] + Delete > forest_distance[i1, j_temp] + Insert)
						{
							test1 = forest_distance[i1, j_temp] + Insert;
							list1 = new List<Operation>(forest_operations[i1, j_temp]);
							list1.Add(new Operation() { O = Operation.Op.Insert, N2 = j_temp + 1 });
							if (list1.Count != test1) throw new Exception();
						}
						else
						{
							test1 = forest_distance[i_temp, j1] + Delete;
							list1 = new List<Operation>(forest_operations[i_temp, j1]);
							list1.Add(new Operation() { O = Operation.Op.Delete, N1 = i_temp + 1 });
							if (list1.Count != test1) throw new Exception();
						}

						int test2;
						List<Operation> list2;
						if (test1 > forest_distance[i_temp2, j_temp2] + tree_distance[i1, j1])
						{
							test2 = forest_distance[i_temp2, j_temp2] + tree_distance[i1, j1];
							list2 = new List<Operation>(forest_operations[i_temp2, j_temp2]);
							list2.AddRange(tree_operations[i1, j1]);
							if (list2.Count != test2) throw new Exception();
						}
						else
						{
							test2 = test1;
							list2 = new List<Operation>(list1);
							if (list2.Count != test2) throw new Exception();
						}

						var temp = Math.Min(
							Math.Min(forest_distance[i_temp, j1] + Delete, forest_distance[i1, j_temp] + Insert),
							forest_distance[i_temp2, j_temp2] + tree_distance[i1, j1]);
						if (test2 != temp) throw new Exception();

						forest_distance[i1, j1] = test2;
						forest_operations[i1, j1] = list2;
					}
				}
			}
			return (forest_distance[i, j], tree_operations[i, j]);
		}
	}
}

