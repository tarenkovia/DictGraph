using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MainGraph
{
	public class Graph :IGraph
	{
		Dictionary<string, Dictionary<string, int>> MyRoad = new Dictionary<string, Dictionary<string, int>>();
		public int CountOfCities;
		protected string PathForFile = "C:/Users/taren/source/repos/DictGraph/DictGraph/outDictionary.txt";
		protected string PathForCopiedGraph = "C:/Users/taren/source/repos/DictGraph/DictGraph/outCopiedGraph.txt";
		protected string PathForSpecificGraph = "C:/Users/taren/source/repos/DictGraph/DictGraph/inSpecificGraph.txt";
		protected string PathForOutSpecificGraph = "C:/Users/taren/source/repos/DictGraph/DictGraph/outSpecificGraph.txt";
		private bool[] ArrayVerifiedVertex;
		private Dictionary<string, bool> ChekedList = new Dictionary<string, bool>();
		private Dictionary<string, int> ChekedListVal = new Dictionary<string, int>();

		public Graph() { }
		public Graph(string Path)
		{
			using (StreamReader file = new StreamReader(Path))
			{
				while (!file.EndOfStream)
				{
					CountOfCities++;
					string MainCity = file.ReadLine();
					Dictionary<string, int> Map = new Dictionary<string, int>();
					string str = file.ReadLine();
					if (str == "empty")
					{
						MyRoad.Add(MainCity, Map);
						ChekedList.Add(MainCity, true);
						ChekedListVal.Add(MainCity, 0);
					}
					else
					{
						List<string> Cities = str.Split(' ').ToList();
						for (int i = 0; i < Cities.Count; i += 2)
						{
							Map.Add(Cities[i], int.Parse(Cities[i + 1]));
						}
						MyRoad.Add(MainCity, Map);
						ChekedList.Add(MainCity, true);
						ChekedListVal.Add(MainCity, 0);
					}
				}
			}

			ArrayVerifiedVertex = new bool[CountOfCities];
		}

		public Graph(Graph graph)
		{
			Dictionary<string, Dictionary<string, int>> CopiedMyRoad = new Dictionary<string, Dictionary<string, int>>();
			foreach (var City in graph.MyRoad)
			{
				Dictionary<string, int> Map = new Dictionary<string, int>();
				foreach (var Cities in City.Value)
				{
					Map.Add(Cities.Key, Cities.Value);
				}
				CopiedMyRoad.Add(City.Key, Map);
			}

			Console.WriteLine("***Graph successfully copied***");

			using (StreamWriter file = new StreamWriter(PathForCopiedGraph))
			{
				foreach (var City in CopiedMyRoad)
				{
					file.WriteLine(City.Key);
					if (City.Value.Count == 0)
					{
						file.Write("empty");
					}
					else
					{
						foreach (var Cities in City.Value)
						{
							file.Write("{0} {1} ", Cities.Key, Cities.Value);
						}
					}
					file.WriteLine();
				}
			}

			Console.WriteLine("***Data has been successfully recorded***");
		}

		public Graph(int CountOfCities)
		{
			Dictionary<string, Dictionary<string, int>> SpecificGraph = new Dictionary<string, Dictionary<string, int>>();
			Dictionary<string, int> Map = new Dictionary<string, int>();
			int CurrentNumberOfCities = 0;
			List<string> Cities = new List<string>();
			using (StreamReader file = new StreamReader(PathForSpecificGraph))
			{
				while(CurrentNumberOfCities < CountOfCities)
				{
					string CityName = file.ReadLine();
					Cities.Add(CityName);
					SpecificGraph.Add(CityName, Map);
					CurrentNumberOfCities++;
				}
			}

			foreach(var City in SpecificGraph)
			{
				foreach(var CurCity in Cities)
				{
					if (!City.Value.ContainsKey(CurCity) && City.Key != CurCity)
					{
						City.Value.Add(CurCity, 0);
					}
				}
			}

			Console.WriteLine("***Graph has been successfully builded***");
			Console.WriteLine();

			using (StreamWriter file = new StreamWriter(PathForOutSpecificGraph))
			{
				foreach (var City in SpecificGraph)
				{
					file.WriteLine(City.Key);
					if (City.Value.Count == 0)
					{
						file.Write("empty");
					}
					else
					{
						foreach (var Citieses in City.Value)
						{
							file.Write("{0} {1} ", Citieses.Key, Citieses.Value);
						}
					}
					file.WriteLine();
				}
			}

			Console.WriteLine("***Data has been successfully recorded***");
			Console.WriteLine();
		}

		public void ShowAllGraph()
		{
			foreach (var City in MyRoad)
			{
				Console.WriteLine("{0} ",City.Key);
				foreach (var Citites in City.Value)
				{
					if (Citites.Value == 0)
					{
						Console.WriteLine("-> {0}", Citites.Key);
					}
					else
					{
						Console.WriteLine("-> {0} - {1}", Citites.Key, Citites.Value);
					}
				}
				Console.WriteLine();
			}

			Console.WriteLine("Total sending cities: {0}", CountOfCities);
			Console.WriteLine();
		}

		public void SetNewWayInUndirectedGraph()
		{
			Console.WriteLine("Enter first city:");
			string FirstCityRaw = Console.ReadLine();
			string FirstCity = FirstCityRaw.SetFirstLetterToUpper();
			Console.WriteLine("Enter second city:");
			string SecondCityRaw = Console.ReadLine();
			string SecondCity = SecondCityRaw.SetFirstLetterToUpper();
			Console.WriteLine("Enter distance between cities:");
			int Path = int.Parse(Console.ReadLine());
			bool CityExistInList = MyRoad.ContainsKey(SecondCity) && MyRoad.ContainsKey(FirstCity);

			if (CityExistInList)
			{
				foreach (var City in MyRoad)
				{
					if (FirstCity == City.Key)
					{
						City.Value.Add(SecondCity, Path);
						MyRoad[SecondCity].Add(City.Key, Path);
					}
				}
			}
			else
			{
				throw new Exception("This way already exist");
			}

			Console.WriteLine("***New road added successfully***");
		}

		public void SetNewWayWithoutWeightInUndirectedGraph()
		{
			Console.WriteLine("Enter first city:");
			string FirstCityRaw = Console.ReadLine();
			string FirstCity = FirstCityRaw.SetFirstLetterToUpper();
			Console.WriteLine("Enter second city:");
			string SecondCityRaw = Console.ReadLine();
			string SecondCity = SecondCityRaw.SetFirstLetterToUpper();
			bool CityExistInList = MyRoad.ContainsKey(SecondCity) && MyRoad.ContainsKey(FirstCity);

			if (CityExistInList)
			{
				foreach (var City in MyRoad)
				{
					if (FirstCity == City.Key)
					{
						City.Value.Add(SecondCity, default(int));
						MyRoad[SecondCity].Add(City.Key, default(int));
					}
				}
			}
			else
			{
				throw new Exception("This way already exist");
			}

			Console.WriteLine("***New road added successfully***");
		}

		public void SetNewCity()
		{
			string NewCity = Console.ReadLine();
			bool CityExistInList = MyRoad.ContainsKey(NewCity.SetFirstLetterToUpper());
			Dictionary<string, int> NewWay = new Dictionary<string, int>();

			if (CityExistInList)
			{
				Console.WriteLine("The city already exists");
			}
			else
			{
				MyRoad.Add(NewCity.SetFirstLetterToUpper(), NewWay);
				CountOfCities++;
			}

			Console.WriteLine("***New city added successfully***");
			ArrayVerifiedVertex = new bool[CountOfCities];
		}

		public void DeleteCityInDictionary()
		{
			string RemovableCity = Console.ReadLine();
			bool CityExistInList = MyRoad.ContainsKey(RemovableCity.SetFirstLetterToUpper());

			if (CityExistInList)
			{
				foreach (var City in MyRoad)
				{
					if (RemovableCity.ToLower() == City.Key.ToLower())
					{
						MyRoad.Remove(RemovableCity.SetFirstLetterToUpper());
						CountOfCities--;
					}
					foreach (var Cities in City.Value)
					{
						if (RemovableCity.ToLower() == Cities.Key.ToLower())
						{
							City.Value.Remove(RemovableCity.SetFirstLetterToUpper());
						}
					}
				}
			}
			else
			{
				throw new Exception("This city doesn't exist.");
			}

			Console.WriteLine("***City delete successfully***");
		}

		public void DeleteWayBetweenCities()
		{
			Console.WriteLine("Enter first city:");
			string FirstCity = Console.ReadLine();
			Console.WriteLine("Enter second city:");
			string SecondCity = Console.ReadLine();
			bool CityExistInList = MyRoad.ContainsKey(FirstCity.SetFirstLetterToUpper()) && MyRoad.ContainsKey(SecondCity.SetFirstLetterToUpper());

			if (CityExistInList)
			{
				foreach(var City in MyRoad)
				{
					if(FirstCity.ToLower() == City.Key.ToLower())
					{
						foreach(var Cities in City.Value)
						{
							if(SecondCity.ToLower() == Cities.Key.ToLower())
							{
								City.Value.Remove(SecondCity.SetFirstLetterToUpper());
							}
						}
					}

					if (SecondCity.ToLower() == City.Key.ToLower())
					{
						foreach (var Cities in City.Value)
						{
							if (FirstCity.ToLower() == Cities.Key.ToLower())
							{
								City.Value.Remove(FirstCity.SetFirstLetterToUpper());
							}
						}
					}
				}
			}
			else
			{
				throw new Exception("One or two cities do not exist in the dictionary.");
			}

			Console.WriteLine("***Road delete successfully***");
		}

		public void WrtiteToFile()
		{
			using (StreamWriter file = new StreamWriter(PathForFile))
			{
				foreach(var City in MyRoad)
				{
					file.WriteLine(City.Key);
					if (City.Value.Count == 0)
					{
						file.Write("empty");
					}
					else
					{
						foreach (var Cities in City.Value)
						{
							file.Write("{0} {1} ", Cities.Key, Cities.Value);
						}
					}
					file.WriteLine();
				}
			}

			Console.WriteLine("***Data has been successfully recorded***");	
		}

		public void SetNewWayInDirectedGraph()
		{
			Console.WriteLine("Enter initial city:");
			string InitialCityRaw = Console.ReadLine();
			string InitialCity = InitialCityRaw.SetFirstLetterToUpper();
			Console.WriteLine("Enter designated city:");
			string DesignatedCityRaw = Console.ReadLine();
			string DesignatedCity = DesignatedCityRaw.SetFirstLetterToUpper();
			Console.WriteLine("Enter distance between cities:");
			int Path = int.Parse(Console.ReadLine());
			bool CityExistInList = MyRoad.ContainsKey(InitialCity);

			if (CityExistInList)
			{
				foreach (var City in MyRoad)
				{
					if (InitialCity == City.Key)
					{
						City.Value.Add(DesignatedCity, Path);
					}
				}
			}
			else
			{
				throw new Exception("This way already exist");
			}

			Console.WriteLine("***New road added successfully***");
		}

		public void SetNewWayWithoutWeightInDirectedGraph()
		{
			Console.WriteLine("Enter first city:");
			string FirstCityRaw = Console.ReadLine();
			string FirstCity = FirstCityRaw.SetFirstLetterToUpper();
			Console.WriteLine("Enter second city:");
			string SecondCityRaw = Console.ReadLine();
			string SecondCity = SecondCityRaw.SetFirstLetterToUpper();
			bool CityExistInList = MyRoad.ContainsKey(SecondCity) && MyRoad.ContainsKey(FirstCity);

			if (CityExistInList)
			{
				foreach (var City in MyRoad)
				{
					if (FirstCity == City.Key)
					{
						City.Value.Add(SecondCity, default(int));
					}
				}
			}
			else
			{
				throw new Exception("This way already exist");
			}

			Console.WriteLine("***New road added successfully***");
		}

		public void DeleteWayBetweenCitiesInDirectedGraph()
		{
			Console.WriteLine("Enter main city:");
			string MainCity = Console.ReadLine();
			Console.WriteLine("Enter city to delete:");
			string CityToDelete = Console.ReadLine();
			bool CityExistInList = MyRoad.ContainsKey(MainCity.SetFirstLetterToUpper());
			if (CityExistInList)
			{
				foreach (var City in MyRoad)
				{
					if (MainCity.ToLower() == City.Key.ToLower())
					{
						foreach (var Cities in City.Value)
						{
							if (CityToDelete.ToLower() == Cities.Key.ToLower())
							{
								City.Value.Remove(CityToDelete.SetFirstLetterToUpper());
							}
						}
					}
				}
			}
			else
			{
				throw new Exception("One or two cities do not exist in the dictionary.");
			}

			Console.WriteLine("***Road delete successfully***");
		}

		
		public void ShowDegreesOfVertices()
		{
			int count = 0;
			foreach (var City in MyRoad)
			{
				CountOfIncomingVertex(City.Key, ref count);
				Console.WriteLine("{0} [degree - {1}]", City.Key, City.Value.Count + count);
				count = 0;
				foreach (var Citites in City.Value)
				{
					if (Citites.Value == 0)
					{
						Console.WriteLine("-> {0}", Citites.Key);
					}
					else
					{
						Console.WriteLine("-> {0} - {1}", Citites.Key, Citites.Value);
					}
				}
				Console.WriteLine();
			}

			Console.WriteLine("Total sending cities: {0}", CountOfCities);
			Console.WriteLine();
		}

		public void CountOfIncomingVertex(string SelectedCity, ref int count)
		{
			foreach(var City in MyRoad)
			{
				if (City.Value.ContainsKey(SelectedCity))
				{
					count++;
				}
			}
		}

		public void IsExistsPathFromUtoVThroughTheTop()
		{
			Console.WriteLine("Enter first city:");
			string FirstCityRaw = Console.ReadLine();
			string FirstCity = FirstCityRaw.SetFirstLetterToUpper();
			Console.WriteLine("Enter second city:");
			string SecondCityRaw = Console.ReadLine();
			string SecondCity = SecondCityRaw.SetFirstLetterToUpper();
			string DesiredCity = " ";
			bool CityExistInList = MyRoad.ContainsKey(SecondCity) && MyRoad.ContainsKey(FirstCity);
			bool CityIsFound = true;

			//сначала проверяем, в каком из городов находится второй город
			//как только нашли нужный город (DesiredCity), проверяем его наличение в списке первого города
			if (CityExistInList)
			{
				foreach(var City in MyRoad)
				{
					foreach(var Citites in City.Value)
					{
						if(SecondCity == Citites.Key)
						{
							DesiredCity = City.Key;
						}
					}
				}
				foreach(var City in MyRoad)
				{
					if(FirstCity == City.Key)
					{
						foreach(var Cities in City.Value)
						{
							if(Cities.Key == DesiredCity)
							{
								Console.WriteLine();
								Console.WriteLine("Path: {0} -> {1} -> {2}", FirstCity, DesiredCity, SecondCity);
								CityIsFound = false;
							}
						}
					}
				}
				if (CityIsFound)
				{
					Console.WriteLine();
					Console.WriteLine("***This path doesn't exist***");
				}
			}
			else
			{
				throw new Exception("This tops doen't exist in road");
			}

		}

		public void CreateGraphFromOrgraph()
		{
			foreach (var City in MyRoad)
			{
				foreach (var SecondCity in MyRoad)
				{
					
					if (!SecondCity.Value.Keys.Contains(City.Key))
					{
						City.Value.Remove(SecondCity.Key);
					}
				}
			}
			foreach(var City in MyRoad)
			{
				if(City.Value.Count == 0)
				{
					MyRoad.Remove(City.Key);
				}
			}

			Console.WriteLine("***Graph successfully transformed***");
			
		}


		public void FindRootOfGraph()
		{
			ResettingViewedVertices();
			bool Acyclic = false;
			CheckForAcyclicity(ref Acyclic);
			ResettingViewedVertices();
			int NumberOfEdges = 0;
			bool answer = true;

			if (Acyclic == false)
			{
				foreach(var City in MyRoad)
				{
					Bfs(City.Key, ref NumberOfEdges);
					if(NumberOfEdges == CountOfCities - 1)
					{
						Console.WriteLine("Digraph root found - {0}", City.Key);
						answer = false;
						break;
					}
					else
					{
						NumberOfEdges = 0;
					}
				}
			}
			else 
			{
				Console.WriteLine("***There is a cycle in the graph***");
			}

			if (answer)
			{
				Console.WriteLine("***The root does not exist***");
			}
		}
		public void ResettingViewedVertices()
		{
			for (int i = 0; i < CountOfCities; i++)
			{
				ArrayVerifiedVertex[i] = true;
			}
		}
		public void CheckForAcyclicity(ref bool Acyclic)
		{
			int numberOfCity = 0;
			foreach (var City in MyRoad)
			{
				string StartedCity = City.Key;
				DfsAndAcyclicity(City.Key, numberOfCity, ref Acyclic, ref StartedCity);
				if (Acyclic)
				{
					break;
				}
				ResettingViewedVertices();
				numberOfCity++;
			}
		}
		public void DfsAndAcyclicity(string SelectedCity, int NumberOfCity, ref bool Acyclic, ref string StartedCity)
		{
			ArrayVerifiedVertex[NumberOfCity] = false;
			NumberOfCity = 0;

			foreach (var City in MyRoad)
			{
				if (City.Key == SelectedCity)
				{
					foreach (var CheckCity in MyRoad)
					{
						if (City.Value.ContainsKey(CheckCity.Key) && ArrayVerifiedVertex[NumberOfCity])
						{
							DfsAndAcyclicity(CheckCity.Key, NumberOfCity, ref Acyclic, ref StartedCity);
						}
						if (City.Value.ContainsKey(StartedCity))
						{
							Acyclic = true;
						}

						NumberOfCity++;
					}
				}
			}
		}
		public void Bfs(string SelectedCity, ref int TestSum)
		{
			Queue q = new Queue();
			int CountOfCity = 0;
			q.Enqueue(SelectedCity);
			ArrayVerifiedVertex[CountOfCity] = false;
			ChekedList[SelectedCity] = false;
			bool ans = true;

			while (q.Count != 0)
			{
				SelectedCity = (string)q.Dequeue();
				foreach (var City in MyRoad)
				{
					if (City.Key == SelectedCity)
					{
						foreach (var CheckCity in MyRoad)
						{
							ChekedList.TryGetValue(CheckCity.Key, out ans);
							if (City.Value.ContainsKey(CheckCity.Key) && ans)
							{
								q.Enqueue(CheckCity.Key);
								TestSum++;
								ChekedList[CheckCity.Key] = false;
							}
						}
					}
				}
			}
			ClearCheckedList();
		}
		public void ClearCheckedList()
		{
			foreach (var City in ChekedList)
			{
				ChekedList[City.Key] = true;
			}
		}


		public void FindRadiusOfGraph()
		{
			int eccentricity = 0;
			int radius = int.MaxValue;

			foreach (var City in MyRoad)
			{
				Dfs(City.Key);
				foreach (var Cities in ChekedListVal)
				{
					if (ChekedListVal[Cities.Key] > eccentricity)
					{
						eccentricity = ChekedListVal[Cities.Key];
					}
				}

				if (eccentricity < radius)
				{
					radius = eccentricity;
				}

				Console.WriteLine("Eccentricity {0} - {1}", City.Key, eccentricity);
				//Console.WriteLine(eccentricity);
				eccentricity = 0;
				Console.WriteLine();
				ClearCheckedListVal();
			}

			Console.WriteLine("***Radius - {0}***", radius);
		}
		public void Dfs(string SelectedCity)
		{
			foreach(var City in MyRoad)
			{
				if(City.Key == SelectedCity)
				{
					foreach(var Cities in MyRoad)
					{
						if (City.Value.ContainsKey(Cities.Key))
						{
							if (ChekedListVal[Cities.Key] == 0)
							{
								ChekedListVal[Cities.Key] = ChekedListVal[SelectedCity] + 1;
								Dfs(Cities.Key);
							}
							else
							{
								if (ChekedListVal[Cities.Key] > ChekedListVal[SelectedCity] + 1)
								{
									ChekedListVal[Cities.Key] = ChekedListVal[SelectedCity] + 1;
									Dfs(Cities.Key);
								}
							}
						}
					}
				}
			}
		}
		public void ClearCheckedListVal()
		{
			foreach (var City in ChekedListVal)
			{
				ChekedListVal[City.Key] = 0;
			}
		}


		//до лучших времен
		public void BuildKraskalGraph()
		{

		}



		
		public void FindRadiusOfGraphWithDijkstr()
		{
			int eccentricity = 0;
			int radius = int.MaxValue;
			Dictionary<string, int> CitiesWithEcce = new Dictionary<string, int>();

			foreach (var City in MyRoad)
			{
				Dijkstr(City.Key);
				foreach (var City2 in ChekedListVal)
				{
					Console.WriteLine("{0} - {1}", City2.Key, City2.Value);
				}
				Console.WriteLine();
				foreach (var Cities in ChekedListVal)
				{
					if(eccentricity < ChekedListVal[Cities.Key])
					{
						eccentricity = ChekedListVal[Cities.Key];
					}
				}

				CitiesWithEcce.Add(City.Key, eccentricity);
				if (radius > eccentricity)
				{
					radius = eccentricity;
				}
				Console.WriteLine("Eccentricity {0} - {1}", City.Key, eccentricity);
				Console.WriteLine();
				eccentricity = 0;
				ClearCheckedListVal();
			}

			Console.WriteLine("***Radius - {0}***", radius);
			Console.Write("Centers - ");
			foreach(var City in CitiesWithEcce)
			{
				if(City.Value == radius)
				{
					Console.Write("{0} ", City.Key);
				}
			}
		}
		public void Dijkstr(string SelectedCity)
		{
			ChekedListVal[SelectedCity] = -1;

			foreach(var City in MyRoad)
			{
				if(City.Key == SelectedCity)
				{
					foreach(var Cities in ChekedListVal)
					{
						if (City.Value.ContainsKey(Cities.Key))
						{
							ChekedListVal[Cities.Key] = City.Value[Cities.Key];
						}
						else
						{
							if (Cities.Value == 0)
							{
								ChekedListVal[Cities.Key] = int.MaxValue;
							}
						}
					}
				}
			}

			foreach(var City in MyRoad)
			{
				if (City.Key != SelectedCity)
				{
					foreach(var Cities in City.Value)
					{
						if (Math.Abs(ChekedListVal[Cities.Key]) > Math.Abs(Cities.Value + ChekedListVal[City.Key]))
						{
							ChekedListVal[Cities.Key] = Cities.Value + ChekedListVal[City.Key];
						}
					}
				}
			}
		}


		public void FindRadiusOfGraphWithFloyd()
		{
			int eccentricity = 0;
			int radius = int.MaxValue;
			foreach (var City in MyRoad)
			{
				Floyd(City.Key);
				//foreach (var City2 in ChekedListVal)
				//{
				//	Console.WriteLine("{0} - {1}", City2.Key, City2.Value);
				//}
				//Console.WriteLine();
				foreach (var Cities in ChekedListVal)
				{
					if (eccentricity < ChekedListVal[Cities.Key])
					{
						eccentricity = ChekedListVal[Cities.Key];
					}
				}

				if (radius > eccentricity)
				{
					radius = eccentricity;
				}
				Console.WriteLine("Eccentricity {0} - {1}", City.Key, eccentricity);
				Console.WriteLine();
				eccentricity = 0;
				ClearCheckedListVal();
			}

			Console.WriteLine("***Radius - {0}***", radius);
		}
		public void Floyd(string SelectedCity)
		{
			foreach (var City in MyRoad)
			{
				if (City.Key == SelectedCity)
				{
					foreach (var Cities in ChekedListVal)
					{
						if (Cities.Key == SelectedCity)
						{
							ChekedListVal[SelectedCity] = 0;
						}
						else
						{
							if (Cities.Value == 0)
							{
								ChekedListVal[Cities.Key] = int.MaxValue;
							}
							else
							{
								ChekedListVal[Cities.Key] = City.Value[Cities.Key];
							}
						}
					}
				}
			}

			int distance = 0;
			int k;
			foreach (var City in MyRoad)
			{
				if (City.Key == SelectedCity)
				{
					foreach (var Cities in ChekedListVal)
					{
						foreach (var Citties1 in MyRoad)
						{
							if (Citties1.Value.TryGetValue(Cities.Key, out k) && SelectedCity != Citties1.Key)
							{
								distance = k + ChekedListVal[Citties1.Key];
							}
							if (Cities.Value > distance)
							{
								ChekedListVal[Cities.Key] = distance;
							}
						}
					}
				}
			}
		}

		//сделано!
		public void FindWayBetweenUtoV1andV2()
		{
			Console.Write("Enter the city -- ");
			string SourceCity = Console.ReadLine().SetFirstLetterToUpper();
			Console.Write("Enter the first city of destenation -- ");
			string FisrtDestCity = Console.ReadLine().SetFirstLetterToUpper();
			Console.Write("Enter the second city of destenation -- ");
			string SecondDestCity = Console.ReadLine().SetFirstLetterToUpper();
			int path = 0;
			int cyclic = 0;
			bool expath = true;
			Bellman_Ford(SourceCity, FisrtDestCity, ref path, ref cyclic, ref expath);
			if (cyclic == 1)
			{
				Console.WriteLine("***Graph contains negative weight cycle***");
			}
			else
			{
				if (expath)
				{
					Console.WriteLine("The shortest path from {0} to {1} - {2}", SourceCity, FisrtDestCity, path);
				}
			}
			cyclic = 0;
			path = 0;
			expath = true;
			Bellman_Ford(SourceCity, SecondDestCity, ref path, ref cyclic, ref expath);
			if (cyclic == 1)
			{
				Console.WriteLine("***Graph contains negative weight cycle***");
			}
			else
			{
				if (expath)
				{
					Console.WriteLine("The shortest path from {0} to {1} - {2}", SourceCity, SecondDestCity, path);
				}
			}
		}
		public void Bellman_Ford(string SourceCity, string DestCity, ref int path, ref int cyclic, ref bool expath)
		{
			ClearCheckedListValForBellmanFord();

			ChekedListVal[SourceCity] = 0;

			foreach (var City in MyRoad)
			{
				foreach (var Cities in City.Value)
				{
					if (ChekedListVal[Cities.Key] > Cities.Value + ChekedListVal[City.Key] && ChekedListVal[City.Key] != int.MaxValue)
					{
						ChekedListVal[Cities.Key] = Cities.Value + ChekedListVal[City.Key];
					}
				}
			}

			foreach(var City in ChekedListVal)
			{
				Console.WriteLine("{0} - {1}", City.Key, City.Value);
			}

			path = ChekedListVal[DestCity];
			if (path == int.MaxValue)
			{
				Console.WriteLine("***The path doent exist***");
				expath = false;
				return;
			}

			foreach (var City in MyRoad)
			{
				foreach (var Cities in City.Value)
				{
					if (ChekedListVal[Cities.Key] > Cities.Value + ChekedListVal[City.Key] && ChekedListVal[City.Key] != int.MaxValue)
					{
						cyclic = 1;
					}
				}
			}
		}
		public void ClearCheckedListValForBellmanFord()
		{
			foreach (var City in ChekedListVal)
			{
				ChekedListVal[City.Key] = int.MaxValue;
			}
		}


		public void FindMaxFlow()
		{
			//Используется алгоритм Форда - Фалкерсона
			string source = "";
			string stock= "";
			int cntVertex = 0;
			int cnt = int.MaxValue;
			int maxFlow = 0;

			foreach(var City in MyRoad)
			{
				CountOfIncomingVertex(City.Key, ref cntVertex);
				if(cntVertex == 0)
				{
					source = City.Key;
				}
				if(City.Value.Count == 0)
				{
					stock = City.Key;
				}
				cntVertex = 0;
			}
			cntVertex = 0;
			CountOfIncomingVertex(stock, ref cntVertex);
			//while (cntVertex != 0 || MyRoad[source].Count != 0)
			//{
			//	MinValInVertex(source, stock, ref cnt);
			//	maxFlow += cnt;
			//	DecWithMInValue(source, stock, ref cnt);
			//	cnt = int.MaxValue;
			//	cntVertex = 0;
			//	CountOfIncomingVertex(stock, ref cntVertex);
			//}
			MinValInVertex(source, stock, ref cnt);
			maxFlow += cnt;
			DecWithMInValue(source, stock, ref cnt);
			cnt = int.MaxValue;
			cntVertex = 0;
			CountOfIncomingVertex(stock, ref cntVertex);
			Console.WriteLine(maxFlow);

			MinValInVertex(source, stock, ref cnt);
			maxFlow += cnt;
			DecWithMInValue(source, stock, ref cnt);
			cnt = int.MaxValue;
			cntVertex = 0;
			CountOfIncomingVertex(stock, ref cntVertex);
			Console.WriteLine(maxFlow);


		}
		public void MinValInVertex(string Source, string Stock, ref int cnt)
		{
			foreach (var City in MyRoad)
			{
				if (City.Key == Source)
				{
					foreach (var Cities in City.Value)
					{
						if (MyRoad[Cities.Key].Count != 0)
						{
							if (Cities.Key != Stock)
							{
								if (Cities.Value < cnt)
								{
									cnt = Cities.Value;
								}
								MinValInVertex(Cities.Key, Stock, ref cnt);
							}
							else
							{
								return;
							}
						}
					}
				}
			}
		}

		public void DecWithMInValue(string Source, string Stock, ref int cnt)
		{
			foreach (var City in MyRoad)
			{
				if (City.Key == Source)
				{
					foreach (var Cities in City.Value)
					{
						if (MyRoad[Cities.Key].Count != 0)
						{
							if (Cities.Key != Stock)
							{
								if (Cities.Value == cnt)
								{
									City.Value.Remove(Cities.Key);
									MyRoad[Cities.Key].Add(City.Key, cnt);

								}
								DecWithMInValue(Cities.Key, Stock, ref cnt);
							}
							else
							{
								return;
							}
						}
						else
						{
							if (Cities.Key != Stock)
							{
								RemoveWay(City.Key, Cities.Key);
							}
						}
					}
				}
			}
		}

		public void RemoveWay(string MainCity, string SelectedCity)
		{
			foreach(var City in MyRoad)
			{
				if (City.Key == MainCity)
				{
					City.Value.Remove(SelectedCity);
				}
			}
		}
	}
}
