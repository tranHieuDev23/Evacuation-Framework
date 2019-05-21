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
            this.corridors = list;
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
            double w1 = 0, w2 = 0, w3 = 0;
            foreach (Corridor cor in corridors) {
                double w = cor.CacheWeight();
                w2 += w*w;
            }

            foreach (Corridor cor in otherPath.Corridors) {
                double w = cor.CacheWeight();
                w3 += w*w;

                if (corridors.Contains(cor)) {
                    w1 += w*w;
                }
            }
            return w1 / Math.Sqrt(w2 * w3);
        }

        public double CoefficientDeviation(CachePath otherPath) {
            double w1 = this.getLength();
            double w2 = otherPath.getLength();

            return (w2 - w1) / w1;
        }
    }
}