using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net.WebSockets;

namespace MainGraph
{
	public class Program
	{
		public static void GetSelectedGraph(out string path, out int Graphtype, char answerOne, char answerTwo)
		{
			if (Char.ToLower(answerOne) == 'y' && Char.ToLower(answerTwo) == 'y')
			{
				Graphtype = 1; //Oriented and weighted graph
				path = "C:/Users/taren/source/repos/DictGraph/DictGraph/inOrientedAndWeight.txt";
			}
			else
			{
				if (Char.ToLower(answerOne) == 'y' && Char.ToLower(answerTwo) == 'n')
				{
					Graphtype = 2; //Oriented and not weighted graph
					path = "C:/Users/taren/source/repos/DictGraph/DictGraph/inOrientedAndNotWeight.txt";
				}
				else
				{
					if (Char.ToLower(answerOne) == 'n' && Char.ToLower(answerTwo) == 'y')
					{
						Graphtype = 3; //Not oriented and weighted graph
						path = "C:/Users/taren/source/repos/DictGraph/DictGraph/inNotOrientedAndWeight.txt";
					}
					else
					{
						Graphtype = 4; //Not oriented and not weighted graph
						path = "C:/Users/taren/source/repos/DictGraph/DictGraph/inNotOrientedAndNotWeight.txt";
					}
				}
			}
		}

		public static void CreateSpecificGraph(char answer)
		{
			GetCorrectAnswer(out answer);
			Console.WriteLine();
			if (Char.ToLower(answer) == 'y')
			{
				Console.WriteLine("Please, enter count of cities from 1 to 23: ");
				int CountOfCities = int.Parse(Console.ReadLine());
				GetCorrectValueForSpecificGraph(ref CountOfCities);
				Console.WriteLine();

				if (CountOfCities == 0)
				{
					Console.WriteLine("***You have closed the construction of a specific graph***");
					Console.WriteLine();
				}
				else
				{
					Graph SpecificGraph = new Graph(CountOfCities);
				}
			}
		}

		public static void GetCorrectAnswer(out char answer)
		{
			answer = char.Parse(Console.ReadLine());
			while (Char.ToLower(answer) != 'n' && Char.ToLower(answer) != 'y')
			{
				Console.WriteLine("Incorrectly entered answer! Please, try again:");
				answer = char.Parse(Console.ReadLine());
			}
		}

		public static void GetCorrectValueForSpecificGraph(ref int CountOfCities)
		{
			while (CountOfCities > 24 || CountOfCities < -1)
			{
				Console.WriteLine();
				Console.WriteLine("You entered the wrong value. Please, try again");
				Console.WriteLine("Enter 0 to exit");
				CountOfCities = int.Parse(Console.ReadLine());
			}
		}


		static void Main()
		{
			string path = "";
			int Graphtype = 0;
			char answerOne = ' ';
			char answerTwo = ' ';
			char answerThree = ' ';

			Console.WriteLine("Press any key to continue...");
			var k = Console.ReadKey();

			Console.WriteLine("***A directed graph? [Y/N]***");
			GetCorrectAnswer(out answerOne);
			Console.WriteLine("***Is the graph weighted? [Y/N]***");
			GetCorrectAnswer(out answerTwo);
			Console.WriteLine("***Do you want to create a specific graph? [Y/N]***");
			CreateSpecificGraph(answerThree);

			GetSelectedGraph(out path, out Graphtype, answerOne, answerTwo);
			Graph MyGraph = new Graph(path);

			string[] page1 = {"[1] - Output the graph in the console", "[2] - Write a graph to a file", "[3] - Add a city",
								"[4] - Add a road with a direction", "[5] - Delete a city", "[6] - Delete a path between cities",
									"[8] - Output a graph with degrees of vertices", "[0] - Prev page\t\t[9] - Next page"};

			string[] page2Digraph = { "[A] - Output a graph with degrees of vertices", "[B] - Find a city between u and v", "[C] - Сonstruct a graph from a digraph",
										"[D] - Find the root of the digraph", "[E] - Find the radius of the graph","[F] - Find the radius of the graph with Dijkstr",
											"[G] - Find the radius of the graph with Floyd","[H] - Find the path from vertex U to V1 and V2", "[I] - Find max flow", 
												"[0] - Prev page\t\t[9] - Next page"};

			string[] page2Graph = { "[A] - Output a graph with degrees of vertices", "[B] - Find the radius of the graph","[C] - Find the path from vertex U to V1 and V2", 
										"[0] - Prev page\t\t[9] - Next page" };
			int numberActivePage = 1;
			string[] activePage = page1;
			
			while (k.Key != ConsoleKey.Enter)
			{
				if (Graphtype == 1) //Oriented and weighted graph
				{
					if (numberActivePage == 1)
					{
						activePage = page1;
					}
					if (numberActivePage == 2) 
					{
						activePage = page2Digraph;
					}
					Console.WriteLine("\t\t Operations console");
					for (int i = 0; i < activePage.Length; i++)
					{
						Console.WriteLine("\t {0}", activePage[i]);
					}
					Console.WriteLine("\t Press <Enter> to escape..");
					Console.WriteLine("\n\npress button, please..");
					k = Console.ReadKey();

					switch (k.Key)
					{
						case ConsoleKey.D1:
							Console.WriteLine();
							MyGraph.ShowAllGraph();
							Console.WriteLine();
							break;
						case ConsoleKey.D2:
							Console.WriteLine("\n");
							MyGraph.WrtiteToFile();
							Console.WriteLine("\n\n\n");
							break;
						case ConsoleKey.D3:
							Console.WriteLine("\n");
							Console.WriteLine("Enter a new city:");
							MyGraph.SetNewCity();
							Console.WriteLine("\n");
							break;
						case ConsoleKey.D4:
							Console.WriteLine("\n");
							MyGraph.SetNewWayInDirectedGraph();
							Console.WriteLine("\n");
							break;
						case ConsoleKey.D5:
							Console.WriteLine("\n");
							Console.WriteLine("Enter a city:");
							MyGraph.DeleteCityInDictionary();
							Console.WriteLine("\n");
							break;
						case ConsoleKey.D6:
							Console.WriteLine("\n");
							MyGraph.DeleteWayBetweenCitiesInDirectedGraph();
							Console.WriteLine("\n");
							break;
						case ConsoleKey.D7:
							Console.WriteLine("\n");
							Graph CopiedMyGraph = new Graph(MyGraph);
							Console.WriteLine("\n");
							break;
						case ConsoleKey.D8:
							Console.WriteLine("\n");
							MyGraph.ShowDegreesOfVertices();
							Console.WriteLine("\n");
							break;
						case ConsoleKey.D9:
							Console.WriteLine("\n");
							if (numberActivePage < 2)
							{
								numberActivePage++;
							}
							Console.WriteLine("\n");
							break;
						case ConsoleKey.D0:
							Console.WriteLine("\n");
							if (numberActivePage > 1)
							{
								numberActivePage--;
							}
							Console.WriteLine("\n");
							break;
						case ConsoleKey.A:
							Console.WriteLine("\n");
							MyGraph.ShowDegreesOfVertices();
							Console.WriteLine("\n");
							break;
						case ConsoleKey.B:
							Console.WriteLine("\n");
							MyGraph.IsExistsPathFromUtoVThroughTheTop();
							Console.WriteLine("\n");
							break;
						case ConsoleKey.C:
							Console.WriteLine("\n");
							MyGraph.CreateGraphFromOrgraph();
							Console.WriteLine("\n");
							break;
						case ConsoleKey.D:
							Console.WriteLine("\n");
							MyGraph.FindRootOfGraph();
							Console.WriteLine("\n");
							break;
						case ConsoleKey.E:
							Console.WriteLine("\n");
							MyGraph.FindRadiusOfGraph();
							Console.WriteLine("\n");
							break;
						case ConsoleKey.F:
							Console.WriteLine("\n");
							MyGraph.FindRadiusOfGraphWithDijkstr();
							Console.WriteLine("\n");
							break;
						case ConsoleKey.G:
							Console.WriteLine("\n");
							MyGraph.FindRadiusOfGraphWithFloyd();
							Console.WriteLine("\n");
							break;
						case ConsoleKey.H:
							Console.WriteLine("\n");
							MyGraph.FindWayBetweenUtoV1andV2();
							Console.WriteLine("\n");
							break;
						case ConsoleKey.I:
							Console.WriteLine("\n");
							MyGraph.FindMaxFlow();
							Console.WriteLine("\n");
							break;
						case ConsoleKey.Enter:
							Console.WriteLine("\n***The program has ended***");
							break;
						default:
							Console.WriteLine();
							Console.WriteLine("There is no such command!");
							Console.WriteLine();
							break;
					}
				}

				if (Graphtype == 2) //Oriented and not weighted graph
				{
					if (numberActivePage == 1)
					{
						activePage = page1;
					}
					if (numberActivePage == 2)
					{
						activePage = page2Digraph;
					}
					Console.WriteLine("\t\t Operations console");
					for (int i = 0; i < activePage.Length; i++)
					{
						Console.WriteLine("\t {0}", activePage[i]);
					}

					Console.WriteLine("\t Press <Enter> to escape..");
					Console.WriteLine("\n\npress button, please..");
					k = Console.ReadKey();

					switch (k.Key)
					{
						case ConsoleKey.D1:
							Console.WriteLine();
							MyGraph.ShowAllGraph();
							Console.WriteLine();
							break;
						case ConsoleKey.D2:
							Console.WriteLine("\n");
							MyGraph.WrtiteToFile();
							Console.WriteLine("\n\n\n");
							break;
						case ConsoleKey.D3:
							Console.WriteLine("\n");
							Console.WriteLine("Enter a new city:");
							MyGraph.SetNewCity();
							Console.WriteLine("\n");
							break;
						case ConsoleKey.D4:
							Console.WriteLine("\n");
							MyGraph.SetNewWayWithoutWeightInDirectedGraph();
							Console.WriteLine("\n");
							break;
						case ConsoleKey.D5:
							Console.WriteLine("\n");
							Console.WriteLine("Enter a city:");
							MyGraph.DeleteCityInDictionary();
							Console.WriteLine("\n");
							break;
						case ConsoleKey.D6:
							Console.WriteLine("\n");
							MyGraph.DeleteWayBetweenCitiesInDirectedGraph();
							Console.WriteLine("\n");
							break;
						case ConsoleKey.D7:
							Console.WriteLine("\n");
							Graph CopiedMyGraph = new Graph(MyGraph);
							Console.WriteLine("\n");
							break;
						case ConsoleKey.D8:
							Console.WriteLine("\n");
							MyGraph.ShowDegreesOfVertices();
							Console.WriteLine("\n");
							break;
						case ConsoleKey.D9:
							Console.WriteLine("\n");
							if (numberActivePage < 2)
							{
								numberActivePage++;
							}
							Console.WriteLine("\n");
							break;
						case ConsoleKey.D0:
							Console.WriteLine("\n");
							if (numberActivePage > 1)
							{
								numberActivePage--;
							}
							Console.WriteLine("\n");
							break;
						case ConsoleKey.A:
							Console.WriteLine("\n");
							MyGraph.ShowDegreesOfVertices();
							Console.WriteLine("\n");
							break;
						case ConsoleKey.B:
							Console.WriteLine("\n");
							MyGraph.IsExistsPathFromUtoVThroughTheTop();
							Console.WriteLine("\n");
							break;
						case ConsoleKey.C:
							Console.WriteLine("\n");
							MyGraph.CreateGraphFromOrgraph();
							Console.WriteLine("\n");
							break;
						case ConsoleKey.D:
							Console.WriteLine("\n");
							MyGraph.FindRootOfGraph();
							Console.WriteLine("\n");
							break;
						case ConsoleKey.E:
							Console.WriteLine("\n");
							MyGraph.FindRadiusOfGraph();
							Console.WriteLine("\n");
							break;
						case ConsoleKey.Enter:
							Console.WriteLine("\n***The program has ended***");
							break;
						default:
							Console.WriteLine();
							Console.WriteLine("There is no such command!");
							Console.WriteLine();
							break;
					}
				}
				if (Graphtype == 3) //Not oriented and weighted graph
				{
					if (numberActivePage == 1)
					{
						activePage = page1;
					}
					if (numberActivePage == 2)
					{
						activePage = page2Graph;
					}
					Console.WriteLine("\t\t Operations console");
					for (int i = 0; i < activePage.Length; i++)
					{
						Console.WriteLine("\t {0}", activePage[i]);
					}

					Console.WriteLine("\t Press <Enter> to escape..");
					Console.WriteLine("\n\npress button, please..");
					k = Console.ReadKey();

					switch (k.Key)
					{
						case ConsoleKey.D1:
							Console.WriteLine();
							MyGraph.ShowAllGraph();
							Console.WriteLine();
							break;
						case ConsoleKey.D2:
							Console.WriteLine("\n");
							MyGraph.WrtiteToFile();
							Console.WriteLine("\n\n\n");
							break;
						case ConsoleKey.D3:
							Console.WriteLine("\n");
							Console.WriteLine("Enter a new city:");
							MyGraph.SetNewCity();
							Console.WriteLine("\n");
							break;
						case ConsoleKey.D4:
							Console.WriteLine("\n");
							MyGraph.SetNewWayInUndirectedGraph();
							Console.WriteLine("\n");
							break;
						case ConsoleKey.D5:
							Console.WriteLine("\n");
							Console.WriteLine("Enter a city:");
							MyGraph.DeleteCityInDictionary();
							Console.WriteLine("\n");
							break;
						case ConsoleKey.D6:
							Console.WriteLine("\n");
							MyGraph.DeleteWayBetweenCities();
							Console.WriteLine("\n");
							break;
						case ConsoleKey.D7:
							Console.WriteLine("\n");
							Graph CopiedMyGraph = new Graph(MyGraph);
							Console.WriteLine("\n");
							break;
						case ConsoleKey.D8:
							Console.WriteLine("\n");
							MyGraph.ShowDegreesOfVertices();
							Console.WriteLine("\n");
							break;
						case ConsoleKey.D9:
							Console.WriteLine("\n");
							if (numberActivePage < 2)
							{
								numberActivePage++;
							}
							Console.WriteLine("\n");
							break;
						case ConsoleKey.D0:
							Console.WriteLine("\n");
							if (numberActivePage > 1)
							{
								numberActivePage--;
							}
							Console.WriteLine("\n");
							break;
						case ConsoleKey.A:
							Console.WriteLine("\n");
							MyGraph.ShowDegreesOfVertices();
							Console.WriteLine("\n");
							break;
						case ConsoleKey.B:
							Console.WriteLine("\n");
							MyGraph.FindRadiusOfGraph();
							Console.WriteLine("\n");
							break;
						case ConsoleKey.C:
							Console.WriteLine("\n");
							MyGraph.FindWayBetweenUtoV1andV2();
							Console.WriteLine("\n");
							break;
						case ConsoleKey.Enter:
							Console.WriteLine("\n***The program has ended***");
							break;
						default:
							Console.WriteLine();
							Console.WriteLine("There is no such command!");
							Console.WriteLine();
							break;
					}

				}
				if (Graphtype == 4) //Not oriented and not weighted graph
				{
					if (numberActivePage == 1)
					{
						activePage = page1;
					}
					if (numberActivePage == 2)
					{
						activePage = page2Graph;
					}
					Console.WriteLine("\t\t Operations console");
					for (int i = 0; i < activePage.Length; i++)
					{
						Console.WriteLine("\t {0}", activePage[i]);
					}

					Console.WriteLine("\t Press <Enter> to escape..");
					Console.WriteLine("\n\npress button, please..");
					k = Console.ReadKey();

					switch (k.Key)
					{
						case ConsoleKey.D1:
							Console.WriteLine();
							MyGraph.ShowAllGraph();
							Console.WriteLine();
							break;
						case ConsoleKey.D2:
							Console.WriteLine("\n");
							MyGraph.WrtiteToFile();
							Console.WriteLine("\n\n\n");
							break;
						case ConsoleKey.D3:
							Console.WriteLine("\n");
							Console.WriteLine("Enter a new city:");
							MyGraph.SetNewCity();
							Console.WriteLine("\n");
							break;
						case ConsoleKey.D4:
							Console.WriteLine("\n");
							MyGraph.SetNewWayWithoutWeightInUndirectedGraph();
							Console.WriteLine("\n");
							break;
						case ConsoleKey.D5:
							Console.WriteLine("\n");
							Console.WriteLine("Enter a city:");
							MyGraph.DeleteCityInDictionary();
							Console.WriteLine("\n");
							break;
						case ConsoleKey.D6:
							Console.WriteLine("\n");
							MyGraph.DeleteWayBetweenCities();
							Console.WriteLine("\n");
							break;
						case ConsoleKey.D7:
							Console.WriteLine("\n");
							Graph CopiedMyGraph = new Graph(MyGraph);
							Console.WriteLine("\n");
							break;
						case ConsoleKey.D8:
							Console.WriteLine("\n");
							MyGraph.ShowDegreesOfVertices();
							Console.WriteLine("\n");
							break;
						case ConsoleKey.D9:
							Console.WriteLine("\n");
							if (numberActivePage < 2)
							{
								numberActivePage++;
							}
							Console.WriteLine("\n");
							break;
						case ConsoleKey.D0:
							Console.WriteLine("\n");
							if (numberActivePage > 1)
							{
								numberActivePage--;
							}
							Console.WriteLine("\n");
							break;
						case ConsoleKey.A:
							Console.WriteLine("\n");
							MyGraph.ShowDegreesOfVertices();
							Console.WriteLine("\n");
							break;
						case ConsoleKey.B:
							Console.WriteLine("\n");
							MyGraph.FindRadiusOfGraph();
							Console.WriteLine("\n");
							break;
						case ConsoleKey.Enter:
							Console.WriteLine("\n***The program has ended***");
							break;
						default:
							Console.WriteLine();
							Console.WriteLine("There is no such command!");
							Console.WriteLine();
							break;
					}

				}
			}
		}
	}
}