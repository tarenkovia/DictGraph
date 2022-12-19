using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainGraph
{
	internal interface IGraph
	{
		public void ShowAllGraph();
		public void SetNewWayInUndirectedGraph();
		public void SetNewCity();
		public void DeleteCityInDictionary();
		public void DeleteWayBetweenCities();
		public void WrtiteToFile();
		public void SetNewWayWithoutWeightInUndirectedGraph();
		public void SetNewWayInDirectedGraph();
		public void SetNewWayWithoutWeightInDirectedGraph();
		public void DeleteWayBetweenCitiesInDirectedGraph();

		//методы выполнения заданий к графу
		public void ShowDegreesOfVertices();
		public void IsExistsPathFromUtoVThroughTheTop();
		public void CreateGraphFromOrgraph();
		public void FindRootOfGraph();
		public void FindRadiusOfGraph();
		public void BuildKraskalGraph();
		public void FindRadiusOfGraphWithDijkstr();
		public void FindRadiusOfGraphWithFloyd();
		public void FindWayBetweenUtoV1andV2();

	}
}
