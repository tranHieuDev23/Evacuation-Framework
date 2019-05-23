using System;
using EvaFrame.Models;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI;

namespace EvaFrame.Visualization.WindowVisualization
{
    class GeneralTab : UserControl
    {
        private ViewModel viewModel;
        
        public GeneralTab()
        {
            this.viewModel = new ViewModel();
            this.DataContext = this.viewModel;
            AvaloniaXamlLoader.Load(this);
        }

        /// <summary>
        /// Cập nhật nội dung của tab.
        /// </summary>
        /// <param name="timeElapsed">Thời gian kể từ lúc bắt đầu mô phỏng thuật toán, tính theo đơn vị s.</param>
        /// <param name="remainingCount">Số người còn lại trong tòa nhà mục tiêu.</param>
        /// <param name="displayedPerson">Cư dân được chọn để hiển thị (người ở gần lối ra nhất).</param>
        public void UpdateContent(double timeElapsed, int remainingCount, Person displayedPerson)
        {
            viewModel.TimeElapsed = String.Format("Time elapsed: {0, 0:F5}s", timeElapsed);
            viewModel.TimeElapsed = String.Format("Time elapsed: {0, 0:F5}s", timeElapsed);
            viewModel.RemainingCount = String.Format("Remaining inhabitants: {0, 0:D}", remainingCount);
            viewModel.ClosestId = "Inhabitant Id: " + displayedPerson.Id;
            viewModel.ClosestSpeedMax = "Inhabitant SpeedMax: " + displayedPerson.SpeedMax;
            viewModel.ClosestFollowing = "Inhabitant is taking direction from indicator: " + displayedPerson.Following.Id;
            if (displayedPerson.Location == null)
            {
                viewModel.ClosestLocation = "Inhabitant is currently not running on any corridor.";
                viewModel.ClosestLocationLength =
                viewModel.ClosestLocationWidth =
                viewModel.ClosestLocationCapacity =
                viewModel.ClosestLocationDensity =
                viewModel.ClosestLocationTrustiness =
                viewModel.ClosestCompletedPercentage =
                viewModel.ClosestActualSpeed = "";
            }
            else
            {
                viewModel.ClosestLocation = "Inhabitant is currently running on corridor: " + displayedPerson.Location.Id;
                viewModel.ClosestLocationLength = "Corridor length: " + displayedPerson.Location.Length;
                viewModel.ClosestLocationWidth = "Corridor width: " + displayedPerson.Location.Width;
                viewModel.ClosestLocationCapacity = "Corridor capacity: " + displayedPerson.Location.Capacity;
                viewModel.ClosestLocationDensity = "Corridor density: " + displayedPerson.Location.Density;
                viewModel.ClosestLocationTrustiness = "Corridor trustiness: " + displayedPerson.Location.Trustiness;
                viewModel.ClosestCompletedPercentage = "Inhabitant completedPercentage: " + displayedPerson.CompletedPercentage;
                viewModel.ClosestActualSpeed = "Inhabitant actualSpeed: " + displayedPerson.CalculateActualSpeed(displayedPerson.Location);
            }
        }

        private class ViewModel : ReactiveObject
        {
            private string timeElapsed;
            public string TimeElapsed { get => timeElapsed; set => this.RaiseAndSetIfChanged(ref timeElapsed, value); }

            private string remainingCount;
            public string RemainingCount { get => remainingCount; set => this.RaiseAndSetIfChanged(ref remainingCount, value); }

            private string closestId;
            public string ClosestId { get => closestId; set => this.RaiseAndSetIfChanged(ref closestId, value); }

            private string closestSpeedMax;
            public string ClosestSpeedMax { get => closestSpeedMax; set => this.RaiseAndSetIfChanged(ref closestSpeedMax, value); }

            private string closestFollowing;
            public string ClosestFollowing { get => closestFollowing; set => this.RaiseAndSetIfChanged(ref closestFollowing, value); }

            private string closestLocation;
            public string ClosestLocation { get => closestLocation; set => this.RaiseAndSetIfChanged(ref closestLocation, value); }

            private string closestLocationLength;
            public string ClosestLocationLength { get => closestLocationLength; set => this.RaiseAndSetIfChanged(ref closestLocationLength, value); }

            private string closestLocationWidth;
            public string ClosestLocationWidth { get => closestLocationWidth; set => this.RaiseAndSetIfChanged(ref closestLocationWidth, value); }

            private string closestLocationCapacity;
            public string ClosestLocationCapacity { get => closestLocationCapacity; set => this.RaiseAndSetIfChanged(ref closestLocationCapacity, value); }

            private string closestLocationDensity;
            public string ClosestLocationDensity { get => closestLocationDensity; set => this.RaiseAndSetIfChanged(ref closestLocationDensity, value); }

            private string closestLocationTrustiness;
            public string ClosestLocationTrustiness { get => closestLocationTrustiness; set => this.RaiseAndSetIfChanged(ref closestLocationTrustiness, value); }

            private string closestCompletedPercentage;
            public string ClosestCompletedPercentage { get => closestCompletedPercentage; set => this.RaiseAndSetIfChanged(ref closestCompletedPercentage, value); }

            private string closestActualSpeed;
            public string ClosestActualSpeed { get => closestActualSpeed; set => this.RaiseAndSetIfChanged(ref closestActualSpeed, value); }
        }
    }
}