using System;
using System.Collections.Generic;

namespace EvaFrame.Models.Building
{
    /// <summary>
    /// Class mô tả đèn báo chỉ đường thoát hiểm thông minh trong tòa nhà.
    /// </summary>
    public class Indicator
    {
        private string id;
        /// <value>String định danh của <c>Indicator</c>, có dạng [số thứ tự trong tầng]@[số tầng].</value>
        public string Id { get { return id; } }

        private List<Corridor> neighbors;
        /// <value>
        /// Danh sách các hành lang dẫn tới các đèn báo kế cận với đèn báo này.
        /// </value>
        public List<Corridor> Neighbors { get { return neighbors; } }

        private Corridor next;
        /// <value>
        /// Hành lang mà đèn báo này đang chỉ lối tới. <c>null</c> nếu như đây là đèn báo ở 
        /// Exit Node và không cần chỉ đến đâu cả. Nếu được gán bởi một giá trị khác null 
        /// mà không nằm trong danh sách <c>Neighbors</c>, <c>InvalidOperationException</c> sẽ 
        /// được throw.
        /// </value>
        public Corridor Next
        {
            get { return next; }

            set
            {
                if (value != null && !neighbors.Contains(value))
                    throw new InvalidOperationException("Corridor not found in neighbors.");
                next = value;
            }
        }

        private bool isStairNode;
        /// <value>
        /// Trả về true nếu như <c>Indicator</c> này là một Stair Node.
        /// </value>
        public bool IsStairNode
        {
            get { return isStairNode; }
            set { isStairNode = value; }
        }

        private bool isExitNode;
        /// <value>
        /// Trả về true nếu như <c>Indicator</c> này là một Stair Node.
        /// </value>
        public bool IsExitNode
        {
            get { return isExitNode; }
            set { isExitNode = value; }
        }

        /// <summary>
        /// Khởi tạo một đối tượng đèn báo không có hành lang nào kế cận, và không chỉ tới đâu cả.
        /// </summary>
        public Indicator(string id)
        {
            this.id = id;
            this.neighbors = new List<Corridor>();
            this.next = null;
        }
    }
}