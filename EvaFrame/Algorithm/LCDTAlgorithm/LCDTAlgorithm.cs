using System;
using System.Collections.Generic;
using EvaFrame.Algorithm;
using EvaFrame.Models.Building;

namespace EvaFrame.Algorithm.LCDTAlgorithm {
    
    class MainAlgo : IAlgorithm {
        private Building target;
        void IAlgorithm.Initialize(Building target) {
            this.target = target;
        }

        void IAlgorithm.Run() {

        }


    }
}