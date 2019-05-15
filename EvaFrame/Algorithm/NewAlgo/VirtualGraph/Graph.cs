using System;
using System.Collections.Generic;
using EvaFrame.Models.Building;

namespace EvaFrame.Algorithm.NewAlgo.VirtualGraph
{
    class Graph
    {
        /// <summary>
        /// Đỉnh giả lập được nối với các ExitNode 
        /// </summary>
        public Node root;

        public Graph()
        {
            root = new Node(null);
        }
        /// <summary>
        /// Phương thức <c> MakeGraph() </c> thực hiện khởi tạo đồ thị biểu 
        /// diễn <c>building</c> để thực hiện thuật toán.
        /// </summary>
        /// <param name="building"> đối tượng building cần cài đặt</param>
        public void MakeGraph(Building building){

        }

        /// <summary>
        /// Phương thức <c> FetchInforFromBuilding() </c> nhận thông tin cập 
        /// nhật các thông số của tòa nhà tại thời điểm gọi.
        /// </summary>

        public void FetchInforFromBuilding(){

        }
        /// <summary>
        /// Phương thức <c> TackleInformation() </c> xử lí thông tin nhận được
        /// từ building thành các trọng số phù hợp với yêu cầu của thuật toán.
        /// </summary>

        public void TackleInformation(){

        }
        /// <summary>
        /// Trả lại thông tin về hướng chỉ của các <c> Indicator </c> cho
        /// <c> building </c>
        /// </summary>

        public void UpdateResultToBuilding(){

        }
    }
}