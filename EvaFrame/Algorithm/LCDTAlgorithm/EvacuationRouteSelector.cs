using System;

namespace EvaFrame.Algorithm.LCDTAlgorithm {

    public class EvacuationRouteSelector {
        private CrossGraph crossGraph;

        EvacuationRouteSelector() {
            this.crossGraph = null;
        }
        EvacuationRouteSelector(CrossGraph crossGraph) {
            this.crossGraph = crossGraph;
        }
    }
}