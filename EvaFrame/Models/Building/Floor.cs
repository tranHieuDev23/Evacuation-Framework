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

        /// <value>
        /// Danh sách các <c>Indicator</c> trên tầng này. Giá trị Read-Only.
        /// </value>
        public ReadOnlyCollection<Indicator> Indicators { get { return indicators.AsReadOnly(); } }

        /// <value>
        /// Danh sách các <c>Indicator</c> là Stair Node trên tầng này. Giá trị Read-Only.
        /// </value>
        public List<Indicator> Stairs { get { return stairs; } }

        /// <summary>
        /// Khởi tạo một tầng mới.
        /// </summary>
        /// <param name="indicators">Danh sách các <c>Indicator</c> trên tầng này.</param>
        /// <param name="stairs">Danh sách các <c>Indicator</c> là Stair Node trên tầng này.</param>
        public Floor(List<Indicator> indicators, List<Indicator> stairs)
        {
            this.indicators = indicators;
            this.stairs = stairs;
            foreach (Indicator i in stairs)
                i.IsStairNode = true;
        }
    }
}