## EvaFrame - Thư viện thiết kế và phát triển thuật toán tìm đường thoát hiểm trong các tòa nhà thông minh.

### Hoàn cảnh ra đời

Việc tìm đường thoát hiểm trong các tòa nhà cao tầng không đơn thuần chỉ là việc chọn ra con đường ngắn nhất cho các cư dân trong tòa nhà - các yếu tố như tác động liên tục của thảm họa lên độ tin cậy của các hành lang, tình trạng tắc nghẽn do lượng lớn cư dân tòa nhà tràn vào các tuyến đường trọng yếu trong thời gian ngắn, vân vân... khiến cho các thuật toán tìm đường đơn giản hoạt động kém hiệu quả. Trong cùng một mô hình tính toán giống nhau, có vô số cách giải quyết khác nhau, với những lợi thế khác nhau về thời gian di tản, độ an toàn cho cư dân và khả năng giải quyết tắc nghẽn.

Xuất phát từ nhu cầu cần có một thư viện chung để dễ dàng thay đổi, tinh chỉnh các thuật toán tìm đường đi thoát hiểm trên cùng một mô hình thống nhất nhằm đưa đến đánh giá, so sánh khách quan, nhóm Ice Tea đã thiết kế và phát triển thư viện EvaFrame - thư viện thiết kế và phát triển thuật toán tìm đường thoát hiểm trên ngôn ngữ C#.

### Tư tưởng thiết kế

EvaFrame được thiết kế nhằm hướng đến một số mục tiêu:

- Module hóa các thành phần trong quá trình thiết kế và thử nghiệm các thuật toán tìm đường thoát hiểm. Mỗi module thực hiện một chức năng độc lập rõ ràng, không bị ràng buộc bởi cơ chế cài đặt bên trong của các module khác.
- Thiết kế các module dễ mở rộng, có thể nhanh chóng lắp đặt và chạy thử trên các hàm đã viết sẵn từ trước.
- Đưa ra một mô hình tính toán chung, thống nhất dành cho mọi thuật toán tìm đường thoát hiểm trong tòa nhà thông minh.
- Tương thích với nhiều hệ điều hành (Microsoft Windows, MacOS, Linux).

### Cấu hình yêu cầu

- ```.NET Core``` phiên bản 2.1 trở lên.
- Package ```Avalonia``` phiên bản ```0.8.0``` trở lên.
- Package ```SkiaSharp``` phiên bản ```1.68.0``` trở lên.

### Cài đặt thư viện EvaFrame

Sử dụng Git để clone lại mã nguồn của thư viện từ GitHub, sau đó build lại project bằng .Net.

```
git clone https://github.com/tranHieuDev23/Evacuation-Framework.git
cd Evacuation-Framework
dotnet build
```

File ```EvacuationFramework.dll``` sẽ được tạo ra trong folder ```bin``` của thư mục để người dùng có thể import vào trong project của mình.

### Chương trình mẫu sử dụng thư viện EvaFrame

```
using System;
using Avalonia;
using Avalonia.Logging.Serilog;
using EvaFrame.Models.Building;
using EvaFrame.Algorithm;
using EvaFrame.Algorithm.PFAlgorithm;
using EvaFrame.Simulator;
using EvaFrame.Simulator.Hazards;
using EvaFrame.Visualization.WindowVisualization;

class Program
{
    public static void Main(string[] args)
    {
        AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToDebug()
            .Start(AppMain, null);
    }

    private static void AppMain(Application app, string[] args)
    {
        // Load tòa nhà mục tiêu từ file dữ liệu
        Building target = Building.LoadFromFile("data.bld");
        // Sử dụng thuật toán PF với hai tham số 4 và 40
        IAlgorithm algorithm = new PFAlgorithm(4, 40);
        // Mô phỏng với thảm họa tại các vị trí không trọng yếu ở tầng 6, 7, 8, 9
        IHazard hazard = new RandomNonCriticalHazard(new int[] {5, 6, 7, 8}, 0.3, 3);
        // Sử dụng giao diện đồ họa của Avalonia, với kích thước màn hình 1280 * 720
        WindowVisualization visualization = new WindowVisualization(1280,  720);

        Simulator simulator = new Simulator(target, algorithm, hazard, visualization);
        // Chạy giả lập, cập nhật tình trạng thảm họa sau 0.1s, chạy lại thuật toán sau 10s và in dữ liệu chạy ra file SimulationData.csv
        simulator.RunSimulatorAsync(0.1, 10, "SimulationData.csv");

        app.Run(visualization.MainWindow);
    }
}
```

### Các module trong thư viện EvaFrame

EvaFrame cung cấp 4 module tương ứng với 4 yếu tố cần thiết trong việc thử nghiệm một thuật toán tìm đường thoát hiểm:

- ```Models```: Mô hình tính toán của thuật toán, bao gồm ```Building``` - tòa nhà nơi xảy ra thảm họa và ```Person``` - những cư dân sống trong tòa nhà.
- ```Algorithm```: Thuật toán tìm đường thoát hiểm được sử dụng.
- ```Visualization```: Công cụ mô tả lại tình trạng của tòa nhà, giúp cho lập trình viên nắm rõ được tình hình đang diễn ra và đưa ra các thiết kế hợp lý.
- ```Simulator```: Chương trình mô phỏng thảm họa (được mô tả dưới dạng các đối tượng ```Hazards```) và quá trình chạy thuật toán trên tòa nhà.

Ngoài các module trên, EvaFrame còn có một module thứ 5 là ```Utilities```, cung cấp các class và interface phụ trở phổ biến dùng trong thuật toán, ví dụ như interface ```IWeightFunction``` - hàm tính trọng số của hành lang.

5 module này được tổ chức trong các namespace đặt tên tương ứng. Người dùng có thể sử dụng các tính năng được cài đặt sẵn có trong chương trình, hoặc mở rộng theo nhu cầu của bản thân.

### ```Models```

Namespace ```EvaFrame.Models``` cung cấp các class liên quan tới mô hình tính toán của thuật toán, bao gồm:

- Namespace con ```Building``` chứa các class và interface mô tả một đối tượng tòa nhà:
    - ```Indicator```: Interface mô tả đèn báo thông minh, chỉ dẫn đường đi cho cư dân trong tòa nhà theo tính toán của thuật toán.
    - ```Corridor```: Interface mô tả hành lang nối giữa các đèn báo, cung cấp số liệu đo đạc để thuật toán thực hiện tính toán.
    - ```Floor```: Interface mô tả một tầng trong tòa nhà, bao gồm danh sách các ```Indicator``` có trên tầng này, trong đó ```Indicator``` nào là cầu thang, các hành lang có trên tầng và các hàng lang dẫn sang các tầng khác (cầu thang).
    - ```Building```: Class chính mà các thuật toán sẽ thực hiện tính toán lên, mô tả tòa nhà thông minh tổng quát bao gồm nhiều tầng.
- Class ```Person``` mô tả cư dân trong tòa nhà, với các thông tin như tốc độ chạy tối đa, vị trí ban đầu và các hàm cập nhật sự di chuyển của họ.

Thông thường, người dùng sẽ không khởi tạo hay mở rộng các class và interface trên, mà chúng sẽ được xây dựng nội bộ và cung cấp cho người dùng từ file thông tin tòa nhà. Để load một đối tượng ```Building``` từ file, ta sử dụng lệnh:

```
Building building = Building LoadFromFile(filepath);
```

Điều này giúp bảo vệ tính đóng gói của thông tin tòa nhà, ngăn chặn các thay đổi bất hợp pháp từ bên ngoài. Việc mở rộng các class và interface trên là hoàn toàn có thể, tuy nhiên sẽ mất công sức không cần thiết, và không tuân theo mục tiêu thiết kế của thư viện - một mô hình tính toán chung cho mọi thuật toán.

#### Cấu trúc file thông tin tòa nhà

File thông tin tòa nhà là một file text bình thường, có thể có định dạng bất kì. Nội dung của file thông tin tòa nhà có dạng:

- Dòng đầu tiên chứa một số nguyên ```numFloor``` - số tầng trong tòa nhà.
- ```numFloor``` cụm dòng tiếp theo, mỗi cụm mô tả thông tin của một tầng, bắt đầu từ tầng 1:
    - Dòng đầu tiên là một số nguyên ```numIndicator``` - số lượng ```Indicator``` trên tầng này.
    - Nếu như ```numIndicator``` khác 0, dòng tiếp theo bao gồm ```numIndicator``` string con - mỗi string mô tả tọa độ của một ```Indicator```, có dạng ```[tọa độ x];[tọa độ y]```. Các string con này được phân tách bằng dấu ```,```. Tọa độ này được sử dụng trong biểu diễn đồ họa của tòa nhà.
    - Dòng tiếp theo là một số nguyên ```numCorridor``` - số hành lang giữa các ```Indicator``` trên tầng này.
    - Nếu như ```numCorridor``` khác 0, dòng tiếp theo bao gồm ```numCorridor``` string con - mỗi string mô tả một ```Corridor```, có dạng ```[Id của Indicator thứ nhất];[Id của Indicator thứ hai];[độ dài tính theo mét];[độ rộng tính theo mét];[độ tin cậy ban đầu, nằm trong khoảng [0, 1]]```. Id của các ```Indicator``` có dạng ```[Số thứ tự trong danh sách Indicator]@[Số tầng, đếm từ 1]```. Các string con được phân tách bằng dấu ```,```.
    - Dòng tiếp theo là một số nguyên ```numStairNode``` - số lượng Stair Node (điểm bắt đầu cầu thang) trên tầng này.
    - Nếu như ```numStairNode``` khác 0, dòng tiếp theo chứa các Id của các Stair Node trên tầng, phân cách nhau bằng dấu ```,```.
    - Dòng tiếp theo là một số nguyên ```numStairway``` - số lượng cầu thang đi từ tầng này xuống các tầng thấp hơn.
    - Nếu như ```numStairway``` khác 0, dòng tiếp theo mô tả các hành lang cầu thang đi từ tầng này xuống các tầng thấp hơn, theo format giống như dòng mô tả các hành lang trên tầng.
- Dòng tiếp theo sau đó chứa một số nguyên ```numExitNode``` - số lượng Exit Node (điểm thoát khỏi tòa nhà).
- Dòng tiếp theo chứa danh sách Id của các Exit Node trong tòa nhà, phân cách nhau bằng dấu ```,```.
- Dòng tiếp theo sau đó chứa một số nguyên ```numInhabitant```, số lượng cư dân trong tòa nhà.
- ```numInhabitant``` dòng cuối cùng, mỗi dòng mô tả một cư dân trong tòa nhà theo dạng ```[Indicator xuất phát];[tốc độ chạy tối đa tính theo m/s];[tọa độ x ban đầu];[tọa độ y ban đầu]```.

### ```Algorithm```

Namespace ```EvaFrame.Algorithm``` cung cấp giao diện ```IAlgorithm```, mô tả giao diện tối thiểu cần có của thuật toán tìm đường thoát hiểm. Interface này yêu cầu cài đặt hai hàm:

- ```void Initialize(Building target)```: Khởi tạo thuật toán với một tòa nhà mục tiêu cụ thể. Tại bước này, thuật toán có thể thực hiện các thao tác chuẩn bị như xây dựng đồ thị ảo, lựa chọn các vị trí để cache tăng tốc độ, vân vân... trước khi bước vào tính toán.
- ```void Run()```: Chạy một lần thuật toán tìm đường. Hàm này được gọi sau hàm ```Initialize()```, do đó mục tiêu tính toán của đối tượng triển khai ```IAlgorithm``` cũng phải được xác định từ thời điểm gọi hàm ```Initialize()```.

Namespace cũng cung cấp một số thuật toán viết sẵn theo interface ```IAlgorithm```:

- ```PlainDijikstra```: Class thuật toán Dijikstra cổ điển - tìm đường đi vật lý ngắn nhất trong tòa nhà. Được sử dụng để chỉ ra nhược điểm của các thuật toán tìm đường cơ bản trong bài toán tìm đường thoát hiểm.
- ```LiveUpdateDijiksra```: Class thuật toán Dijikstra có cập nhật lại - thuật toán sẽ được chạy lại sau mỗi khoảng thời gian nhất định. Hàm tính toán trọng số hành lang của tòa nhà cũng có thể thay đổi được, giúp người dùng dễ dàng thử nghiệm các hàm số khác nhau, thay vì chỉ phụ thuộc duy nhất vào độ dài vật lý.
- ```LCDTAlgorithm```: Thuật toán LCDT-GV dựa trên paper *A Scalable Approach for Dynamic Evacuation Routing in Large Smart Buildings*.
- ```PFAlgorithm```: Thuật toán do nhóm tự cải tiến, có khả năng dự đoán và tránh các tuyến đường có khả năng ùn tắc trong tương lai (do đó có tên là PF Algorithm - Predict Future).

### ```Visualization```

Namespace ```EvaFrame.Visualization``` cung cấp các class và interface liên quan tới việc mô tả tình trạng của tòa nhà trong quá trình chạy mô phỏng thuật toán. Người dùng có thể  sử dụng hai class đã được cài đặt sẵn là ```TextVisualization``` - mô tả bằng việc in ra một số thông tin đơn giản của tòa nhà trên giao diện command line, hoặc ```WindowVisualization``` - mô tả bằng cửa sổ đồ họa. Giao diện đồ họa của thư viện được cài đặt bằng hai thư viện [```Avalonia```](http://avaloniaui.net) và [```SkiaSharp```](https://github.com/mono/SkiaSharp), hỗ trợ hiển thị trên đa nền tảng.

![Ảnh chụp giao diện đồ họa của class ```WindowVisualization```](Screenshot.jpeg)

Người dùng cũng có thể tự cài đặt interface ```IVisualization``` để cài đặt giao diện mô tả tình trạng tòa nhà của mình. Interface này yêu cầu cài đặt hai hàm:

- ```void Initialize(Building target)```: Khởi tạo giao diện đồ họa với một tòa nhà mục tiêu cụ thể.
- ```void Update(double timeElapsed)```: Cập nhật lại giao diện đồ họa. Tương tự như ```IAlgorithm```, hàm này được gọi sau hàm ```Initialize()```, do đó mục tiêu của đối tượng triển khai cũng phải được xác định từ thời điểm gọi hàm ```Initialize()```. ```timeElapsed``` là thời gian tính từ thời điểm bắt đầu chạy thuật toán, được truyền vào để giao diện có thể hiển thị thời gian trôi qua.

### ```Simulator```

Namespace ```EvaFrame.Simulator``` cung cấp các class và interface liên quan tới việc chạy mô phỏng thuật toán. Hai đối tượng quan trọng nhất trong namespace này là:

- Interface ```IHazard```: Interface mô tả tình trạng thảm họa của tòa nhà. Các lớp triển khai interface này cần cài đặt hai hàm:
    - ```void Initialize(Building target)```: Khởi tạo thảm họa với một tòa nhà mục tiêu cụ thể.
    - ```void Update(double updatePeriod)```: Cập nhật tình trạng thảm họa (ví dụ, làm cho vùng bị ảnh hưởng lan rộng hơn). Tương tự như ```IAlgorithm``` và ```IVisualization```, hàm này được gọi sau hàm ```Initialize()```, do đó mục tiêu của đối tượng triển khai cũng phải được xác định từ thời điểm gọi hàm ```Initialize()```. ```updatePeriod``` là thời gian tính từ thời điểm cập nhật tình trạng thảm họa lần cuối cùng.
- Class ```Simulator``` thực hiện việc chạy mô phỏng thuật toán với một đối tượng ```Building``` mục tiêu, sử dụng một thuật toán ```IAlgorithm``` đã được cài đặt từ trước, dưới tác động của một thảm họa ```IHazard``` và biểu diễn lại cho người dùng thông qua một giao diện ```IVisualization```.


Để mô tả thảm họa, người dùng có thể cài đặt interface ```IHazard```, hoặc sử dụng các class đã được cài đặt sẵn:
- ```NoHazard```: Không có thảm họa xảy ra.
- ```RandomNonCriticalHazard```: Thảm họa xảy ra ngẫu nhiên ở các khu vực không trọng yếu (các điểm không phải cầu thang hay lối thoát). Bằng cách cố định hạt giống ngẫu nhiên, người dùng có thể cố định cùng một tình trạng thảm họa để kiểm tra nhiều lần.
- ```RandomCriticalHazard```: Thảm họa xảy ra ngẫu nhiên ở các khu vực trọng yếu (xung quanh cầu thang). Có thể cố định hạt giống ngẫu nhiên tương tự như ```RandomNonCriticalHazard```.

### Todo

- Thư viện ```Avalonia``` vẫn đang trong giai đoạn beta, do đó giao diện đồ họa của chương trình có thể gặp lỗi và dừng lại trong quá trình chạy.

### Giấy phép

MIT