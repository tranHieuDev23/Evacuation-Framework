using System;
using System.Collections.Generic;
using EvaFrame.Utilities;
using EvaFrame.Algorithm.NewAlgo.VirtualGraph;

namespace EvaFrame.Algorithm.NewAlgo
{
    /// <summary>
    /// Lớp gồm các phương thức hỗ trợ để thực hiện 
    /// <c>MainAlgo</c>
    /// </summary>
    public class Utility
    {
        private const double V_TB = 4;
        private const double TIME = 50;

        /// <summary>
        /// Hàm tính toán sự ảnh hưởng của ngoại cảnh tới vận tốc trên đoạn đường
        /// </summary>
        /// <param name="trustness">chỉ số trustness của đoạn đường</param> 
        /// <param name="density">mật độ người đi trên đoạn đường</param>
        /// <returns>Chỉ số ảnh hưởng</returns>
        private double ContextFunction(double trustness, double density)
        {
            return 1 /(trustness * (1.0001 - density));
        }

        /// <summary>
        /// Hàm tính trọng số của con đường
        /// </summary>
        /// <param name="edge"></param> Đọan đường đang xét 
        /// <param name="numberPeople"></param>Số người đi trên con đường
        /// <returns>Giá trị trọng số của đoạn đường</returns>
        private double GetWeight(Edge edge, int numberPeople)
        {
            double density = GetDensity(edge, numberPeople);
            return edge.CorrespondingCorridor.Length
                 * ContextFunction(edge.CorrespondingCorridor.Trustiness, density);

        }

        /// <summary>
        /// Hàm tính mật độ của đoạn đường
        /// </summary>
        /// <param name="edge"></param> Đoạn đường đang xét
        /// <param name="numberPeople"></param>Số người đang đi trên con đường
        /// <returns>Giá trị mật độ người trên con đường</returns>
        public double GetDensity(Edge edge, int numberPeople)
        {
            double density;
            density = numberPeople / (edge.CorrespondingCorridor.Capacity);
            return density;

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
            int numberPeople = (int)passing.CorrespondingCorridor.Density;
            Node reach = from;
            Edge next = passing;
            while (sumWeight - GetWeight(next, numberPeople) > 0)
            {
                sumWeight = sumWeight - GetWeight(next, numberPeople);
                reach = next.To;
                next = reach.nextEdge;
                if(next == null || next.CorrespondingCorridor == null) break;
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
            /*Xử lý trường hợp đoạn đường có độ dài = 0 và trường hợp đỉnh from đứng trước to */
            if(from == to || from.label == false)
            {
                return 0;
            }
            /*------------------------------------------------------------------------------- */

            double weight = 0;
            Edge current = from.nextEdge;
            Edge preEdge;
            do
            {
                if(current == null || current.CorrespondingCorridor == null)
                {
                    break;
                }
                preEdge = current;
                double density = GetDensity(current, numberPeople);
                weight = weight + current.CorrespondingCorridor.Length 
                                * ContextFunction(current.CorrespondingCorridor.Trustiness, 
                                                  density);
                current = current.To.nextEdge;
            } while (preEdge.To != to);
            return weight;
        }
        
        /// <summary>
        /// Cập nhật lại trọng số của các Node mà sẽ tới được <c>reachedNode</c> trong tương lai 
        /// nhưng vẫn chưa được gán nhãn
        /// </summary>
        /// <param name="reachedNode">Đỉnh đã được gán nhãn mà các đỉnh tới nó cần được update</param>
        /// <param name="root">Đỉnh nguồn mà các đỉnh khác tìm đường ngắn nhất tới</param>
        /// <param name="heap">Cấu trúc dữ liệu để các đỉnh mới được update push vào</param>
        /// 
        //private MainAlgo mainAlgo = new MainAlgo();
        //private MainAlgo.Data data = new mainAlgo.Data();
        public void UpdateComingNode(Node reachedNode, Node root, MinHeap<MainAlgo.Data> heap)
        {
            /*Implement code in here */
            foreach (var comingNode in reachedNode.comingNodes)
            {
                if (comingNode.label)
                {
                    continue;
                }
                /*Tìm đỉnh trung gian giữa reachedNode và comingNode */
                Adjacence intermediate;
                intermediate = comingNode.adjacences.Find(adj => adj.reaching == reachedNode);

                /*Cập nhật lại trọng số con đường của comingNode mà đi tới được reachedNode */
                int numberPeople = (int) intermediate.edge.CorrespondingCorridor.Density;
                double w1 = CalculateWeight(intermediate.node, 
                                            reachedNode, 
                                            numberPeople);
                double w2 = CalculateWeight(reachedNode, root, reachedNode.nComingPeople);
                intermediate.passingWeight = intermediate.edge.weight + w1 + w2;
                bool isChaged = GetNextNode(comingNode);
                if (isChaged)
                {
                    heap.Push(new MainAlgo.Data(comingNode, comingNode.weight));
                }
            }
        }

        /// <summary>
        /// Duyệt trong những đỉnh kề với <c>node</c> để đặt lại quãng đường tốt nhất
        /// </summary>
        /// <param name="node">Đỉnh cần được cập nhật được tốt nhất</param>
        /// <return>Giá trị <c>boolen</c> thể hiện <c>node</c> có thay đổi trọng số?</return> 
        private bool GetNextNode(Node node)
        {
            /*Implement code in here */
            bool isChanged = false; // biến cho biết node có thay đổi trọng số ?
            foreach (var adjacence in node.adjacences)
            {
                if(adjacence.node.label)
                {
                    if (adjacence.passingWeight < node.weight)
                    {
                        node.weight = adjacence.passingWeight;
                        node.next = adjacence.node;
                        node.nextEdge = adjacence.edge;
                        node.reachedNode = adjacence.reaching;
                        isChanged = true;
                    }
                }
            }
            if (isChanged)
            {
                /*Nếu trọng số  của node thay đổi cần phải lưu lại giá trị mới này
                vào mảng dictionary */
            }
            return isChanged;
        }

        /// <summary>
        /// Cập nhật người từ các đỉnh nằm giữa các đỉnh đã được gán nhãn cho các đỉnh 
        /// phía trước trong cây khung Dijkstra
        /// </summary>
        /// <param name="node">Đỉnh xuất phát</param>
        /// <param name="edge">Cạnh nằm giữa hai đỉnh đã được gán nhãn</param>
        /// <param name="root">Đỉnh đích nguồn tìm đường ngắn nhất tới các đỉnh khác trong 
        /// đồ thị</param>
        /// <param name="heap">Heap hiện tại đang được thực hiện tương ứng với thuật toán</param>
        
        public void UpdateComingPeople(Node node, Edge edge, Node root, MinHeap<MainAlgo.Data> heap)
        {
            /*Implement code in here */
            Node reachedNode = FindCrossNode(node, edge);
            reachedNode.nComingPeople = reachedNode.nComingPeople 
                                + (int) edge.CorrespondingCorridor.Density;
            UpdateComingNode(reachedNode, root, heap);
        }

        /// <summary>
        /// Hàm xử lý trong trường hợp các đỉnh đẳng trước đột nhiên mật độ người tăng cao hoặc 
        /// chỉ số <c>trustiness</c> của con đường bị giảm mạnh.
        /// </summary>
        /// <param name="checkNode">Đỉnh cần chuyển hướng</param>
        /// <param name="root">Đỉnh nguồn</param>
        public void TackleIncidence(Node checkNode, Node root)
        {
            Edge changeDirectionTo = checkNode.nextEdge;
            double changedWeight = 100000000;
            foreach (var adj in checkNode.adjacences)
            {
                int numberComing = (int) adj.node.nComingPeople
                                 + checkNode.nComingPeople;
                double weight = CalculateWeight(adj.node, root, numberComing) 
                              + GetWeight(adj.edge, checkNode.nComingPeople);
                if (weight < changedWeight){
                    changeDirectionTo = adj.edge;
                    changedWeight = weight;
                }
            }
            checkNode.next = changeDirectionTo.To;
        }
    }
}