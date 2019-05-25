using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EvaFrame.Models.Building
{
    /// <summary>
    /// Class mô tả một tầng trong mô hình LCDT.
    /// </summary>
    public class Floor
    {
        List<Indicator> indicators, stairs;
        List<Corridor> corridors, stairways;

        /// <value>
        /// Danh sách các <c>Indicator</c> trên tầng này. Giá trị read-only.
        /// </value>
        public ReadOnlyCollection<Indicator> Indicators { get { return indicators.AsReadOnly(); } }

        /// <value>
        /// Danh sách các <c>Indicator</c> là Stair Node trên tầng này. Là một tập hợp con của danh sách <c>Indicators</c>. Giá trị Read-Only.
        /// </value>
        public ReadOnlyCollection<Indicator> Stairs { get { return stairs.AsReadOnly(); } }

        /// <value>
        /// Danh sách các <c>Corridor</c> giữa các <c>Indicator</c> trên tầng này. Giá trị read-only.
        /// </value>
        public ReadOnlyCollection<Corridor> Corridors { get { return corridors.AsReadOnly(); } }

        /// <value>
        /// Danh sách các <c>Corridor</c> giữa <c>Indicator</c> trên tầng này với <c>Indicator</c> ở tầng dưới. Giá trị read-only.
        /// </value>
        public ReadOnlyCollection<Corridor> Stairways { get { return stairways.AsReadOnly(); } }

        /// <summary>
        /// Khởi tạo một tầng mới.
        /// </summary>
        /// <param name="indicators">Danh sách các <c>Indicator</c> trên tầng này.</param>
        /// <param name="stairs">Danh sách các <c>Indicator</c> là Stair Node trên tầng này.</param>
        /// <param name="corridors">Danh sách các <c>Corridor</c> giữa các <c>Indicator</c> trên tầng này.</param>
        /// <param name="stairways">Danh sách các <c>Corridor</c> giữa <c>Indicator</c> trên tầng này với <c>Indicator</c> ở tầng dưới.</param>
        public Floor(List<Indicator> indicators, List<Indicator> stairs, List<Corridor> corridors, List<Corridor> stairways)
        {
            this.indicators = indicators;
            this.stairs = stairs;
            this.corridors = corridors;
            this.stairways = stairways;
            foreach (Indicator i in stairs)
                i.IsStairNode = true;
        }
    }
}