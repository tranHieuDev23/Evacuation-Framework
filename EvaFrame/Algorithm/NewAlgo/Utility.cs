using System;
using System.Collections.Generic;
using EvaFrame.Algorithm.NewAlgo.VirtualGraph;

namespace EvaFrame.Algorithm.NewAlgo
{
    /// <summary>
    /// Lớp gồm các phương thức hỗ trợ để thực hiện 
    /// <c>MainAlgo</c>
    /// </summary>
    public class Utility
    {
        private const double V_TB = 5;
        private const double TIME = 10;

        /// <summary>
        /// Setup all data in order to re-execute Algorithm
        /// </summary>
        public void Setup()
        {
            
        }

        /// <summary>
        /// Hàm tính toán sự ảnh hưởng của ngoại cảnh tới vận tốc trên đoạn đường
        /// </summary>
        /// <param name="trustness">chỉ số trustness của đoạn đường</param> 
        /// <param name="density">mật độ người đi trên đoạn đường</param>
        /// <returns>Chỉ số ảnh hưởng</returns>
        private double ContextFunction(double trustness, double density)
        {

        }

        /// <summary>
        /// Hàm tính trọng số của con đường
        /// </summary>
        /// <param name="edge"></param> Đọan đường đang xét 
        /// <param name="numberPeople"></param>Số người đi trên con đường
        /// <returns>Giá trị trọng số của đoạn đường</returns>
        private double GetWeight(Edge edge, int numberPeople)
        {
            
        }

        /// <summary>
        /// Hàm tính mật độ của đoạn đường
        /// </summary>
        /// <param name="edge"></param> Đoạn đường đang xét
        /// <param name="numberPeople"></param>Số người đang đi trên con đường
        /// <returns>Giá trị mật độ người trên con đường</returns>
        private double GetDensity(Edge edge, int numberPeople)
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
        /// thời gian <c>TIME</c></returns>
        public Node FindCrossNode(Node from, Edge passing)
        {
            /*Implement code in here */
            double sumWeight = V_TB * TIME;
            int numberPeople = passing.numberPeople;
            Node reach = from;
            Edge next = passing;
            while (sumWeight > 0)
            {
                sumWeight = sumWeight - GetWeight(next, numberPeople);
                reach = next.To;
                next = reach.nextEdge;
                if (next == null)
                {
                    break;
                }
            }
            return reach;
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
            double weight = 0;
            Edge current = from.nextEdge;
            do
            {
                double density = GetDensity(current, numberPeople);
                weight = weight + current.CorrespondingCorridor.Length 
                                * ContextFunction(current.CorrespondingCorridor.Trustiness, 
                                                  density);
                current = current.To.nextEdge;
            } while (current.To != to);
            return weight;
        }
        
        /// <summary>
        /// Cập nhật lại trọng số của các Node mà sẽ tới được <c>reachedNode</c> trong tương lai 
        /// nhưng vẫn chưa được gán nhãn
        /// </summary>
        /// <param name="reachedNode">Đỉnh đã được gán nhãn mà các đỉnh tới nó cần được update</param>
        /// <param name="root">Đỉnh nguồn mà các đỉnh khác tìm đường ngắn nhất tới</param>
        public void UpdateComingNode(Node reachedNode, Node root)
        {
            /*Implement code in here */
            foreach (var comingNode in reachedNode.comingNodes)
            {
                if (comingNode.label)
                {
                    continue;
                }
                /*Tìm đỉnh trung gian giữa reachedNode và comingNode */
                Adjacence intermediate = new Adjacence();
                foreach (var adjacence in comingNode.adjacences)
                {
                    if (adjacence.reaching == reachedNode)
                    {
                        intermediate = adjacence;
                        break;
                    }
                }
                /*Cập nhật lại trọng số con đường của comingNode mà đi tới được reachedNode */
                double w1 = CalculateWeight(intermediate.node, 
                                            reachedNode, 
                                            intermediate.edge.numberPeople);
                double w2 = CalculateWeight(reachedNode, root, reachedNode.nComingPeople);
                intermediate.passingWeight = intermediate.edge.weight + w1 + w2;
                GetNextNode(comingNode);
            }
        }

        /// <summary>
        /// Duyệt trong những đỉnh kề với <c>node</c> để đặt lại quãng đường tốt nhất
        /// </summary>
        /// <param name="node">Đỉnh cần được cập nhật được tốt nhất</param>
        private void GetNextNode(Node node)
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