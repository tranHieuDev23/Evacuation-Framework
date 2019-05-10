using System;
using System.Collections.Generic;

namespace EvaFrame.Algorithm.NewAlgo
{
    /// <summary>
    /// Class thu gọn của Indicator với mục đích chỉ dành cho
    /// thực hiện thuật toán
    /// </summary>
    class Node
    {
        internal struct adjacentEdge
        {
            Edge edge;
            double passingWeight;
        }

        /// <summary>
        /// Danh sách các cạnh kề 
        /// </summary>
        /// <value></value>
        internal List<adjacentEdge> adjacences;

        internal Edge next;
        /// <summary>
        /// Cạnh tiếp theo trong đường đi ngắn nhất tới <c>root</c>
        /// </summary>
        /// <value></value>
        public Edge Next
        {
            get{
                return next;
            }
            set{
                next = value;
            }
        }

        
        public Node()
        {
            
        }
    }
}