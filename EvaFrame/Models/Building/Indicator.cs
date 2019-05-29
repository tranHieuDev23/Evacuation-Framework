using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EvaFrame.Models.Building
{
    /// <summary>
    /// Interface mô tả đèn báo chỉ đường thoát hiểm thông minh trong tòa nhà.
    /// </summary>
    /// <remarks>
    /// Trong sử dụng thực tế, các đối tượng đèn báo sẽ được cung cấp thông qua class <c>Building</c> - người dùng không cần phải tự cài đặt interface này.
    /// </remarks>
    public interface Indicator
    {
        /// <value>String định danh của <c>Indicator</c>, có dạng [số thứ tự trong tầng]@[số tầng].</value>
        string Id { get; }

        /// <value>Tọa độ X của <c>Indicator</c> trên biểu diễn đồ họa.</value>
        int X { get; }

        /// <value>Tọa độ Y của <c>Indicator</c> trên biểu diễn đồ họa.</value>
        int Y { get; }

        /// <value>Chỉ số của tầng mà <c>Indicator</c> này đang ở trên. Là số nguyên trong khoảng [1, số tầng].</value>
        int FloorId { get; }

        /// <value>
        /// Danh sách các hành lang dẫn tới các đèn báo kế cận với đèn báo này. Giá trị read-only.
        /// </value>
        ReadOnlyCollection<Corridor> Neighbors { get; }

        /// <value>
        /// Hành lang mà đèn báo này đang chỉ lối tới. 
        /// Các thuật toán sẽ thực hiện thay đổi trên giá trị này nhằm đưa ra chỉ dẫn cho cư dân trong tòa nhà.
        /// Có giá trị bằng <c>null</c> nếu như đèn báo không chỉ đến đâu cả. 
        /// </value>
        /// <exception cref="System.InvalidOperationException">
        /// Throw nếu được gán bởi một giá trị khác <c>null</c> mà không nằm trong danh sách <c>Neighbors</c>.
        /// </exception>
        Corridor Next { get; set; }

        /// <value>
        /// Trả về true nếu như <c>Indicator</c> này là một Stair Node. Giá trị read-only.
        /// </value>
        bool IsStairNode { get; }

        /// <value>
        /// Trả về true nếu như <c>Indicator</c> này là một Exit Node. Giá trị read-only.
        /// </value>
        bool IsExitNode { get; }
    }

    public partial class Building
    {
        // Cài đặt cụ thể của interface Indicator bên trong Building.
        private class IndicatorImpl : Indicator
        {
            private string id;
            public string Id { get { return id; } }

            private int x;
            public int X { get { return x; } }

            private int y;
            public int Y { get { return y; } }

            private int floorId;
            public int FloorId { get { return floorId; } }

            public List<Corridor> neighbors;
            public ReadOnlyCollection<Corridor> Neighbors { get { return neighbors.AsReadOnly(); } }

            private Corridor next;
            public Corridor Next
            {
                get { return next; }

                set
                {
                    if (value != null && !neighbors.Contains(value))
                        throw new InvalidOperationException("Corridor not found in Neighbors.");
                    next = value;
                }
            }

            private bool isStairNode;
            public bool IsStairNode
            {
                get { return isStairNode; }
                set { isStairNode = value; }
            }

            private bool isExitNode;
            public bool IsExitNode
            {
                get { return isExitNode; }
                set { isExitNode = value; }
            }

            public IndicatorImpl(string id, int x, int y, int floorId)
            {
                if (x < 0)
                    throw new ArgumentOutOfRangeException("x", "x cannot be negative!");
                if (y < 0)
                    throw new ArgumentOutOfRangeException("y", "y cannot be negative!");
                if (floorId < 0)
                    throw new ArgumentOutOfRangeException("floorId", "floorId cannot be negative!");
                this.id = id;
                this.x = x;
                this.y = y;
                this.floorId = floorId;
                this.neighbors = new List<Corridor>();
                this.next = null;
            }
        }
    }
}