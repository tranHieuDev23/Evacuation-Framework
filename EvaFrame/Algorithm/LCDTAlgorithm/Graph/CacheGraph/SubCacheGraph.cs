using System.Collections.Generic;
using System.Collections.ObjectModel;

using EvaFrame.Models.Building;
using EvaFrame.Algorithm.LCDTAlgorithm;
using EvaFrame.Algorithm.LCDTAlgorithm.Utilities;
using EvaFrame.Utilities;

namespace EvaFrame.Algorithm.LCDTAlgorithm.Cache {

    public class SubCacheGraph {
        private Floor correspondingFloor;
        public Floor CorrespondingFloor { get { return correspondingFloor; } }
        
        private List<CacheNode> cacheNodes;
        public ReadOnlyCollection<CacheNode> CacheNodes { get { return cacheNodes.AsReadOnly(); } }

        private Dictionary<Indicator, bool> visited;

        public SubCacheGraph() {
            this.correspondingFloor = null;
            this.cacheNodes = new List<CacheNode>();
            this.visited = new Dictionary<Indicator, bool>();
        }

        public void initialize(Floor floor, double threshold, bool isFirstFloor = false) {
                this.correspondingFloor = floor;
                foreach (Indicator indicator in floor.Indicators) {
                    visited[indicator] = false;
                }
            
                List<CachePath> tempCachePaths = new List<CachePath>();

                if (isFirstFloor == true)
                    foreach (Indicator exitNode in floor.Indicators)
                    if (exitNode.IsExitNode) {
                        CacheNode cacheNode = new CacheNode(exitNode);
                        cacheNodes.Add(cacheNode);
                    }

                foreach (Indicator stairNode in floor.Stairs) {
                    CacheNode cacheNode = new CacheNode(stairNode);
                    cacheNodes.Add(cacheNode);
                }

                if (isFirstFloor == true) {
                    foreach (Indicator exitNode in floor.Indicators)
                    if (exitNode.IsExitNode == true) {
                        foreach (Indicator stairNode in floor.Stairs) {
                            List<Corridor> listLocalCor = new List<Corridor>();
                            Queue<Corridor> q = new Queue<Corridor>();

                            tempCachePaths.AddRange(FindAllPath(stairNode, exitNode, cacheNodes, listLocalCor, 0, 500));
                        }
                    }
                }
                else {
                    foreach (Indicator stairNode in floor.Indicators) {
                        int floorNumber = stairNode.getFloorNumber();
                        Corridor corridor = stairNode.Neighbors.Find( cor => cor.To.getFloorNumber() > floorNumber);
                        if (corridor == null) continue;

                        Indicator otherNode = corridor.To; 
                        foreach (Indicator otherStairNode in floor.Stairs) {
                            List<Corridor> listLocalCor = new List<Corridor>();
                            Queue<Corridor> q = new Queue<Corridor>();

                            tempCachePaths.AddRange(FindAllPath(otherStairNode, stairNode, cacheNodes, listLocalCor, 0, 500));
                        }
                    }
                }

                List<CacheNode> tempCacheNodes = new List<CacheNode>();

                foreach (Indicator indicator in floor.Indicators) {
                    CacheNode cacheNode = new CacheNode(indicator);
                    tempCacheNodes.Add(cacheNode);
                }

                foreach (CachePath cachePath in tempCachePaths) {
                    foreach (Corridor cor in cachePath.Corridors) {
                        CacheNode v = tempCacheNodes.Find( node => node.CorrespondingIndicator == cor.To);
                        if (v.CorrespondingIndicator.IsExitNode == false && v.CorrespondingIndicator.IsStairNode == false) {
                            v.nPathThrough += 1;
                        }
                    }
                }

                for (int i = 0; i < tempCacheNodes.Count; ++i) {
                    for(int j = i+1; j < tempCacheNodes.Count; ++j) 
                    if (tempCacheNodes[i].nPathThrough < tempCacheNodes[j].nPathThrough) {
                        CacheNode tempNode = tempCacheNodes[i].Clone();
                        tempCacheNodes[i] = tempCacheNodes[j].Clone();
                        tempCacheNodes[j] = tempNode;
                    }
                }

                foreach (Indicator exitNode in floor.Indicators) {
                    if (isFirstFloor == true) {
                        if (exitNode.IsExitNode == false) continue;
                    }
                    else {
                        int floorNumber = exitNode.getFloorNumber();
                        Corridor corridor = exitNode.Neighbors.Find( cor => cor.To.getFloorNumber() > floorNumber);
                        if (corridor == null) continue;
                    }
                    distanceDijkstra(exitNode);

                    //CacheNode cacheNode = new CacheNode(exitNode);

                    foreach (CacheNode node in tempCacheNodes)
                    if (node.CorrespondingIndicator.IsExitNode == false && node.CorrespondingIndicator.IsStairNode == false) {
                        double weight = 0;
                        CacheNode u = node;
                        while(true) {
                            weight += u.CorrespondingIndicator.Next.Length;
                            CacheNode uNew = tempCacheNodes.Find( v => v.CorrespondingIndicator == u.CorrespondingIndicator.Next.To);
                            if (weight > threshold || cacheNodes.Find( v => v.CorrespondingIndicator == uNew.CorrespondingIndicator) != null) {
                                CacheNode newCacheNode = new CacheNode(u.CorrespondingIndicator);
                                cacheNodes.Add(newCacheNode);
                                break;
                            }
                            u = uNew;
                        }
                    }
                }
        }

        private void distanceDijkstra(Indicator exitNode) {
            Dictionary<Indicator, double> dis = new Dictionary<Indicator, double>();
            MinHeap<DataI> heap = new MinHeap<DataI>();

            foreach(Indicator indicator in correspondingFloor.Indicators) 
            if (!indicator.IsExitNode) {
                dis[indicator] = double.PositiveInfinity;
            }

            dis[exitNode] = 0;
            heap.Push(new DataI(exitNode, 0));

            while(heap.Count > 0) {
                Indicator u = heap.Top().node;
                double du = heap.Top().weightToExit;
                heap.Pop();

                if (dis[u] != du) continue;

                foreach (Corridor e in u.Neighbors) {
                    Indicator v = e.To;
                    if (dis.ContainsKey(v) == false) continue;
                    double dv = dis[v];

                    if (dv > du + e.Length) {
                        dv = du + e.Length;
                        dis[v] = dv;
                        heap.Push(new DataI(v, dv));
                        v.Next = v.Neighbors.Find( cor => cor.To == u);
                    }
                }
            }
        }

        private List<CachePath> FindAllPath(Indicator src, Indicator dst, 
                                            List<CacheNode> stopNodes, List<Corridor> listLocalCor,
                                            double weight, double threshold){
            visited[src] = true;
            List<CachePath> tempPaths = new List<CachePath>();

            if (src.Equals(dst) == true) {
                List<Corridor> tmp = new List<Corridor>();
                foreach (Corridor cor in listLocalCor) {
                    tmp.Add(cor.CorClone());
                }

                CachePath cachePath = new CachePath(tmp);
                tempPaths.Add(cachePath);
                visited[src] = false;
                return tempPaths;

            }

            foreach (Corridor e in src.Neighbors)
            if (correspondingFloor.Indicators.Contains(e.To)) {
                Indicator v = e.To;
                if (!visited[v]) {
                    listLocalCor.Add(e);
                    weight += e.Length;

                    if (weight < threshold && (stopNodes.Find( node => node.CorrespondingIndicator == v) == null || v.Equals(dst))) {
                        tempPaths.AddRange(FindAllPath(v, dst, stopNodes, listLocalCor, weight, threshold));
                    }

                    listLocalCor.Remove(e);
                    weight -= e.Length;
                }
            }
            visited[src] = false;

            return tempPaths;
        }

        public List<CachePath> FindKPath(int k, CacheNode src, CacheNode dst, double threshold) {
            List<Corridor> listLocalCor = new List<Corridor>();
            double weight = 0;
            List<CachePath> allCachePath = FindAllPath(src.CorrespondingIndicator, dst.CorrespondingIndicator, cacheNodes, listLocalCor, weight, threshold);

            for (int i = 0; i < allCachePath.Count; ++i) {
                for(int j = i+1; j < allCachePath.Count; ++j)
                if (allCachePath[i].getLength() > allCachePath[j].getLength()) {
                    CachePath temp = allCachePath[j].Clone();
                    allCachePath[j] = allCachePath[i].Clone();
                    allCachePath[i] = temp;
                }
            }

            List<CachePath> listKPath = new List<CachePath>();
            if (allCachePath.Count < k) {
                //System.Console.WriteLine("Node {0} to Node {1} have {2} path", 
                //src.CorrespondingIndicator.Id, dst.CorrespondingIndicator.Id, allCachePath.Count);
                return allCachePath;
            }

            for (int i = 0; i < k; ++i) listKPath.Add(allCachePath[i]);

            double score = scoreKPath(listKPath);

            for (int i = k; i < allCachePath.Count; ++i) {
                for (int j = k-1; j > 0; --j) {
                    List<CachePath> tempKPath = new List<CachePath>();
                    for (int t = 0; t < k; ++t) tempKPath.Add(listKPath[i]);

                    tempKPath.Remove(listKPath[j]);
                    tempKPath.Add(allCachePath[i]);

                    double tempScore = scoreKPath(tempKPath);

                    if (tempScore > score) {
                        score = tempScore;
                        listKPath.Remove(listKPath[j]);
                        listKPath.Add(allCachePath[i]);
                    }
                }
            }

            return listKPath;
        }

        private double scoreKPath(List<CachePath> listCachePath) {
            double res = 0;
            for (int i = 0; i < listCachePath.Count; ++i) {
                for (int j = i+1; j < listCachePath.Count; ++j) {
                    res = res + Init.Alpha * (1 - listCachePath[i].CosineScore(listCachePath[j]))
                              + (1 - Init.Alpha) * listCachePath[i].CoefficientDeviation(listCachePath[j]) ;
                }
            }

            return res;
        }
    }
}