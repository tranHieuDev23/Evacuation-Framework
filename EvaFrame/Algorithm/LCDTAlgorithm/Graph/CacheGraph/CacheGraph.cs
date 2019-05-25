using System.Collections.Generic;
using System.Collections.ObjectModel;

using EvaFrame.Models.Building;

namespace EvaFrame.Algorithm.LCDTAlgorithm.Cache {

    public class CacheGraph {

        private List<SubCacheGraph> subCacheGraphs;
        public ReadOnlyCollection<SubCacheGraph> SubCacheGraphs { get { return subCacheGraphs.AsReadOnly(); } }

        public CacheGraph(Building target, double lenBetweenCacheNode) {
            this.subCacheGraphs = new List<SubCacheGraph>();

            for(int i = 0; i < target.Floors.Count; ++i) {
                SubCacheGraph subCacheGraph = new SubCacheGraph();
                if (i == 0) subCacheGraph.initialize(target.Floors[0], lenBetweenCacheNode, true);
                else subCacheGraph.initialize(target.Floors[i], lenBetweenCacheNode);

                this.subCacheGraphs.Add(subCacheGraph);
            }
        }
    }
}