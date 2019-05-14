using System;
using System.Collections.Generic;

namespace EvaFrame.Algorithm.NewAlgo
{
    /// <summary>
    /// Lớp gồm các phương thức hỗ trợ để thực hiện 
    /// <c>MainAlgo</c>
    /// </summary>
    public class Utility
    {
        const double Vtb = 5;

        /// <summary>
        /// Setup all data in order to re-execute Algorithm
        /// </summary>
        public void Setup()
        {

        }

        /// <summary>
        /// Tìm đỉnh xa nhất mà trong khoảng thời gian
        /// <c>t</c>, số người ở đỉnh <c>u</c> có thể đi tới được bằng việc đi qua cạnh
        /// <c>e</c> và dọc theo các chỉ thịcủa các đỉnh đã gán nhãn ngay sau cạnh <c>e</c>
        /// </summary>
        /// <param name="from">Đỉnh xuất phát</param>
        /// <param name="passing">Cạnh đi từ <c>u</c> tới đỉnh được gán nhãn</param>
        /// <returns>Đỉnh xa nhất mà những người từ <c>u</c> có thể tới được trong khoảng 
        /// thời gian <c>t</c></returns>
        public Node FindCrossNode(Node from, Edge passing)
        {
            /*Implement code in here */
            return null;
        }

        /// <summary>
        /// Tính toán trọng số của một đoạn đường.
        /// Đoạn đường đó được xác định bởi đỉnh đầu và cuối, đỉnh tiếp theo sẽ được đỉnh 
        /// đằng trước chỉ đến thông qua đặc tính <c>next</c>
        /// </summary>
        /// <param name="from">Đỉnh bắt đăù của đoạn đường</param>
        /// <param name="to">Đỉnh kết thúc của đoạn đường</param>
        /// <param name="numberPeople">Số người đi trên đoạn đường</param>
        /// <returns>Trọng số của của đoạn đường tương ứng với <c>numberPeople</c></returns>
        public double CalculateWeight(Node from, Node to, int numberPeople)
        {
            /*Implement code in here */
            return 0;
        }
        
        /// <summary>
        /// Cập nhật lại trọng số của các Node mà sẽ tới được <c>reachedNode</c> trong tương lai 
        /// nhưng vẫn chưa được gán nhãn
        /// </summary>
        /// <param name="reachedNode">Đỉnh đã được gán nhãn mà các đỉnh tới nó cần được update</param>
        public void UpdateComingPeople(Node reachedNode)
        {
            /*Implement code in here */
        }

        /// <summary>
        /// Cập nhật người từ các đỉnh nằm giữa các đỉnh đã được gán nhãn cho các đỉnh 
        /// phía trước trong cây khung Dijkstra
        /// </summary>
        /// <param name="edge">Cạnh nằm giữa hai đỉnh đã được gán nhãn</param>
        public void UpdateComingPeople(Edge edge)
        {
            /*Implement code in here */
        }
    }
}