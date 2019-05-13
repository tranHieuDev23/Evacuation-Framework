using EvaFrame.Models.Building;

namespace EvaFrame.Models
{
    /// <summary>
    /// Class mô tả cư dân trong tòa nhà.
    /// </summary>
    class Person
    {
        private double speedMax;
        /// <value>
        /// Vận tốc tối đa của cư dân, trong môi trường tối ưu (hành lang có độ tin cậy 1, 
        /// không có người nào khác đi qua).
        /// </value>
        public double SpeedMax { get { return speedMax; } }

        private Corridor location;
        /// <summary>
        /// Hành lang mà người này đang di chuyển trên.
        /// </summary>
        /// <value></value>
        public Corridor Location
        {
            get { return location; }
            set { location = value; }
        }

        private double completedPercentage;
        /// <summary>
        /// Mức độ hoàn thành hành lang hiện tại của người này, nằm trong khoảng [0, 1]. 
        /// 0 có nghĩa là là vừa xuất phát tại <c>Indicator</c> From.
        /// </summary>
        /// <value></value>
        public double CompletedPercentage
        {
            get { return completedPercentage; }
            set { completedPercentage = value; }
        }
    }
}