using System;
using System.Collections.Generic;

using EvaFrame.Models.Building;
using EvaFrame.Algorithm.LCDTAlgorithm.Cache;

namespace EvaFrame.Algorithm.LCDTAlgorithm {
    
    public class UpdateEvacuationWithCache {
        private Building target;
        private int k;
        private double lenghtThreshold;

        public UpdateEvacuationWithCache(Building target, int k, double lenghtThreshold) {
            this.target = target;
            this.k = k;
            this.lenghtThreshold = lenghtThreshold;
        }

        public void Run(double lenBetweenCacheNode) {
            CacheGraph cacheGraph = new CacheGraph(target, lenBetweenCacheNode);
            
            int cnt = 0;
            foreach (SubCacheGraph subCacheGraph in cacheGraph.SubCacheGraphs) {
                foreach (CacheNode node1 in subCacheGraph.CacheNodes) {
                    foreach (CacheNode node2 in subCacheGraph.CacheNodes) 
                    if (!node1.Equals(node2)) {
                        List<CachePath> listPath = subCacheGraph.FindKPath(k, node1, node2, lenghtThreshold);
                        Update(listPath);
                        //Console.WriteLine("Node {0} to Node {1} Done!", node1.CorrespondingIndicator.Id, node2.CorrespondingIndicator.Id);
                    }
                }
                Console.WriteLine("Floor {0} Done!", cnt++);
            }

        }

        private void Update(List<CachePath> listPath) {
            for (int i = 0; i < listPath.Count; ++i) {
                CachePath path = listPath[i];
                double density = 0;
                double capacity = 0;
                double minTrust = 1;

                foreach (Corridor cor in path.Corridors) {
                    density += cor.Density;
                    capacity += cor.Capacity;
                    if (minTrust > cor.Trustiness) {
                        minTrust = cor.Trustiness;
                    }
                }

                if (minTrust < Init.TrustnessThreshold || density > Init.Beta * capacity) {
                    if (i+1 == listPath.Count) continue;
                    path = listPath[i+1];
                    foreach (Corridor cor in path.Corridors) {
                        cor.From.Next = cor;
                    }
                }
            }
        }
    }
}