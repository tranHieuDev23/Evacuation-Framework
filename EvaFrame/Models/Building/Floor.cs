using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EvaFrame.Models.Building
{
    /// <summary>
    /// Interface mô tả một tầng trong mô hình LCDT.
    /// </summary>
    /// <remarks>
    /// Trong sử dụng thực tế, các đối tượng tầng sẽ được cung cấp thông qua class <c>Building</c> - người dùng không cần phải tự cài đặt interface này.
    /// </remarks>
    public interface Floor
    {
        /// <value>
        /// Danh sách các <c>Indicator</c> trên tầng này. Giá trị read-only.
        /// </value>
        ReadOnlyCollection<Indicator> Indicators { get; }

        /// <value>
        /// Danh sách các <c>Indicator</c> là Stair Node trên tầng này. Là một tập hợp con của danh sách <c>Indicators</c>. Giá trị Read-Only.
        /// </value>
        ReadOnlyCollection<Indicator> Stairs { get; }

        /// <value>
        /// Danh sách các <c>Corridor</c> giữa các <c>Indicator</c> trên tầng này. Giá trị read-only.
        /// </value>
        ReadOnlyCollection<Corridor> Corridors { get; }

        /// <value>
        /// Danh sách các <c>Corridor</c> giữa <c>Indicator</c> trên tầng này với <c>Indicator</c> ở tầng dưới. Giá trị read-only.
        /// </value>
        ReadOnlyCollection<Corridor> Stairways { get; }
    }

    public partial class Building
    {
        // Cài đặt cụ thể của interface Floor bên trong Building.
        private class FloorImpl : Floor
        {
            List<Indicator> indicators, stairs;
            List<Corridor> corridors, stairways;

            public ReadOnlyCollection<Indicator> Indicators { get { return indicators.AsReadOnly(); } }

            public ReadOnlyCollection<Indicator> Stairs { get { return stairs.AsReadOnly(); } }

            public ReadOnlyCollection<Corridor> Corridors { get { return corridors.AsReadOnly(); } }

            public ReadOnlyCollection<Corridor> Stairways { get { return stairways.AsReadOnly(); } }

            public FloorImpl(List<Indicator> indicators, List<Indicator> stairs, List<Corridor> corridors, List<Corridor> stairways)
            {
                this.indicators = indicators;
                this.stairs = stairs;
                this.corridors = corridors;
                this.stairways = stairways;
            }
        }
    }
}