using System;
using System.Collections.Generic;

using EvaFrame.Models.Building;
using EvaFrame.Algorithm.LCDTAlgorithm.Cache;
using EvaFrame.Algorithm.LCDTAlgorithm.Utilities;

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

        public void Run(double lenBetweenCacheNode, List<int> srcInds, List<int> dstInds) {
            CacheGraph cacheGraph = new CacheGraph(target, lenBetweenCacheNode);
            
            int cnt = 0;
            foreach (SubCacheGraph subCacheGraph in cacheGraph.SubCacheGraphs) {
                List<String> stoppedNode = new List<String>() {
                    "16", "33", "50", "64", "75", "87", "101", "115", "138", "169" 
                };
                subCacheGraph.setCachePathFromStoppedNodes(stoppedNode);

                for (int i = 0; i < srcInds.Count; ++i) {
                    CacheNode node1 = subCacheGraph.CacheNodes.Find( node => node.CorrespondingIndicator.getIdNumber() == srcInds[i]);
                    CacheNode node2 = subCacheGraph.CacheNodes.Find( node => node.CorrespondingIndicator.getIdNumber() == dstInds[i]);
                    if (node1 == null || node2 == null) 
                        throw new NullReferenceException("Invalid Indicator");
                    List<CachePath> listPath = subCacheGraph.FindKPath(k, node1, node2, lenghtThreshold);
                    Update(listPath, subCacheGraph.CorrespondingFloor, node1.CorrespondingIndicator);
                }

            }

        }

        private void Update(List<CachePath> listPath, Floor floor, Indicator src) {
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

                /* if (minTrust < Init.TrustnessThreshold || density > Init.Beta * capacity) {
                    //if (i+1 == listPath.Count) continue;
                    Console.WriteLine("Path change {0}", i+1);
                    if (i+1 == listPath.Count) path = listPath[0];
                    else path = listPath[i+1];
                    foreach (Corridor cor in path.Corridors) {
                        src.Next = cor;
                        src = cor.To(src);
                    }
                }
                else*/
                if (path.isCongested()) {
                    Console.WriteLine("Path change {0}", i+1);
                    if (i+1 == listPath.Count) path = listPath[0];
                    else path = listPath[i+1];
                    if (path.getCachePathWeight() < path.getPhysicalWeight()) {
                        foreach (Corridor cor in path.Corridors) {
                            src.Next = cor;
                            src = cor.To(src);
                        }
                    }
                }
            }
        }
    }
}