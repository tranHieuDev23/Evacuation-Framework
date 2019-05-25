using System;

using EvaFrame.Models.Building;

namespace EvaFrame.Algorithm.LCDTAlgorithm.Cache {
    public class CacheNode {
        private Indicator correspondingIndicator;
        public Indicator CorrespondingIndicator { get { return correspondingIndicator; } }

        public int nPathThrough;

        public CacheNode(Indicator indicator) {
            this.correspondingIndicator = indicator;
            this.nPathThrough = 0;
        }

        public CacheNode Clone() {
            CacheNode newNode = new CacheNode(this.CorrespondingIndicator);
            newNode.nPathThrough = this.nPathThrough;
            return newNode;
        }
    }
}