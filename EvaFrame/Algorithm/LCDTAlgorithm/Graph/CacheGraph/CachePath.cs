using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

using EvaFrame.Models.Building;
using EvaFrame.Algorithm.LCDTAlgorithm.Utilities;

namespace EvaFrame.Algorithm.LCDTAlgorithm.Cache {
    
    public class CachePath {
        private List<Corridor> corridors;
        public List<Corridor> Corridors { get { return corridors; } }

        public CachePath(List<Corridor> list) {
            this.corridors = new List<Corridor>();
            foreach (Corridor cor in list)
                this.corridors.Add(cor);
        }

        public CachePath Clone() {
            CachePath newCachePath = new CachePath(this.corridors);
            return newCachePath;
        }

        public double getLength() {
            double len = 0;
            foreach (Corridor cor in corridors) {
                len += cor.Length;
            }
            return len;
        }

        public double CosineScore(CachePath otherPath) {
            double wshared = 0, w2 = 0, w3 = 0;
            foreach (Corridor cor in corridors) {
                double w = cor.CacheWeight();
                w2 += w*w;
            }

            foreach (Corridor cor in otherPath.Corridors) {
                double w = cor.CacheWeight();
                w3 += w*w;

                if (corridors.Contains(cor)) {
                    wshared += w*w;
                }
            }
            return wshared / Math.Sqrt(w2 * w3);
        }

        public double CoefficientDeviation(CachePath otherPath) {
            double w1 = this.getLength();
            double w2 = otherPath.getLength();

            return (w2 - w1) / w1;
        }

        public double getCachePathWeight() {
            double res = 0.0;
            
            foreach (Corridor cor in corridors) {
                double trustness = cor.Trustiness;
                if (trustness < Init.Alpha)
                    trustness = 0.00001f;

                res += 1.0*(cor.Length * cor.Capacity) / (trustness * (cor.Capacity -  cor.Density + 1));
            }

            return res;
        }

        public double getPhysicalWeight() {
            double res = 0;
            foreach (Corridor cor in corridors) {
                res += cor.Length * cor.Capacity;
            }
            return res;
        }

        public bool isCongested() {
            bool isCongested = false;
            int count = 0, numOfSegment = corridors.Count;

            foreach (Corridor cor in corridors) {
                if (cor.Density > Init.Beta * cor.Capacity) {
                    ++count;
                }
                
                if (count > numOfSegment/3) {
                    isCongested = true;
                    break;
                }
            }
            
            return isCongested;
        }
    }
}