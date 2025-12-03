using API.Interfaces;
using API.Models;

namespace API.Services;

public class AwingService : IAwingService
{
    public AwingService()
    {
    }
    
    private readonly int _topK = 5;
    
    public double CalculateFuel(TreasureRequest treasureRequest)
    {
        // B1: Gom tọa độ rương theo value
        var layers = BuildLayers(treasureRequest.N, treasureRequest.M, treasureRequest.P, treasureRequest.Matrix);

        // B2: Khởi tạo dp cho tầng 1 (đi từ start → L[1])
        var dpPrev = InitDpForFirstLayer(layers[1]);

        // B3: Lặp từ tầng 2 → p
        for (int k = 2; k <= treasureRequest.P; k++)
        {
            dpPrev = ComputeNextLayerDp(dpPrev, layers[k]);
        }

        // B4: Kết quả là min dp của tầng cuối
        return dpPrev.Min(d => d.Cost);
    }

    /// <summary>
    /// Tạo cấu trúc dữ liệu các tầng (layers) để nhóm toàn bộ tọa độ của ma trận theo giá trị rương tương ứng. <br/>
    /// Cơ chế:<br/>
    /// - Mỗi giá trị rương k sẽ có một danh sách các vị trí (x,y) chứa rương đó.<br/>
    /// - Giúp giảm chi phí xử lý về sau khi tính DP theo từng tầng k.
    /// </summary>
    /// <param name="n">Kích thước ma trận theo chiều dọc</param>
    /// <param name="m">Kích thước ma trận theo chiều ngang</param>
    /// <param name="p">Số lượng rương khác nhau</param>
    /// <param name="matrix">Ma trận rương</param>
    /// <returns>Trả về một <see cref="Dictionary{TKey,TValue}"/> với <c>TKEY</c> là số hiệu rương, <c>TValue</c> là danh sách tọa độ chứa các rương đó</returns>
    private static Dictionary<int, List<PointPos>> BuildLayers(int n, int m, int p, int[][] matrix)
    {
        var layers = new Dictionary<int, List<PointPos>>(p + 1);
        for (int i = 1; i <= p; i++)
            layers[i] = new List<PointPos>();

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                int val = matrix[i][j];
                layers[val].Add(new PointPos(i, j));
            }
        }

        return layers;
    }

    /// <summary>
    /// Tính chi phí (dp) ban đầu cho tầng rương số 1. <br/>
    /// Cơ chế:<br/>
    /// - Điểm xuất phát được cố định tại (0,0). <br/>
    /// - Với mỗi vị trí thuộc tầng rương 1, tính chi phí di chuyển ban đầu bằng khoảng cách Euclid từ điểm xuất phát. <br/>
    /// - Kết quả được biểu diễn dưới dạng danh sách DpPoint. <br/>
    /// - Áp dụng bước sàng lọc Top-K để giới hạn số điểm cần xét ở các tầng tiếp theo.
    /// </summary>
    /// <param name="firstLayer">Danh sách toàn bộ tọa độ chứa rương số 1.</param>
    /// <returns>Danh sách <see cref="DpPoint"/> đã được rút gọn theo tiêu chí <c>Top-K</c>.</returns>
    private List<DpPoint> InitDpForFirstLayer(List<PointPos> firstLayer)
    {
        var dp = new List<DpPoint>();

        var start = new PointPos(0, 0);

        foreach (var pt in firstLayer)
        {
            double dist = Euclid(start, pt);
            dp.Add(new DpPoint(pt, dist));
        }
        
        return TakeTopK(dp);
    }

    /// <summary>
    /// Tính tập giá trị dp cho tầng rương kế tiếp dựa trên kết quả của tầng trước. <br/>
    /// Cơ chế:<br/>
    /// - Lấy tập Top-K từ tầng trước nhằm giảm số lượng điểm cần xét. <br/>
    /// - Với mỗi tọa độ thuộc tầng hiện tại, xác định chi phí tối ưu bằng cách
    ///   cộng chi phí dp của từng điểm Top-K ở tầng trước với khoảng cách Euclid đến điểm hiện tại. <br/>
    /// - Lựa chọn giá trị nhỏ nhất làm chi phí dp tương ứng cho điểm hiện tại. <br/>
    /// - Trả về danh sách DpPoint thể hiện chi phí tối ưu của toàn bộ điểm trong tầng hiện tại.
    /// </summary>
    /// <param name="prevLayerDp">Danh sách dp của tầng trước.</param>
    /// <param name="currentLayer">Danh sách toàn bộ tọa độ thuộc tầng rương hiện tại.</param>
    /// <returns>Danh sách <see cref="DpPoint"/> biểu diễn chi phí tối ưu của tầng hiện tại.</returns>
    private List<DpPoint> ComputeNextLayerDp(List<DpPoint> prevLayerDp, List<PointPos> currentLayer)
    {
        var topPrev = TakeTopK(prevLayerDp);

        var result = new List<DpPoint>();

        foreach (var cur in currentLayer)
        {
            double best = double.MaxValue;

            foreach (var prev in topPrev)
            {
                double cost = prev.Cost + Euclid(prev.Pos, cur);
                if (cost < best) best = cost;
            }

            result.Add(new DpPoint(cur, best));
        }

        return result;
    }

    /// <summary>
    /// Lựa chọn tập K điểm có chi phí dp nhỏ nhất từ danh sách hiện tại. <br/>
    /// Cơ chế:<br/>
    /// - Sắp xếp toàn bộ danh sách theo giá trị chi phí tăng dần. <br/>
    /// - Trích ra K phần tử đầu tiên để giữ lại các trạng thái có khả năng tạo
    ///   đường đi tối ưu ở các tầng tiếp theo. <br/>
    /// - Trả về danh sách rút gọn nhằm giảm độ phức tạp khi kết hợp với tầng kế tiếp.
    /// </summary>
    /// <param name="dpList">Danh sách dp của một tầng.</param>
    /// <returns>Danh sách <see cref="DpPoint"/> gồm K điểm có chi phí thấp nhất.</returns>
    private List<DpPoint> TakeTopK(List<DpPoint> dpList)
    {
        return dpList
            .OrderBy(d => d.Cost)
            .Take(_topK)
            .ToList();
    }

    /// <summary>
    /// Tính khoảng cách Euclid giữa hai tọa độ trong mặt phẳng ma trận. <br/>
    /// Cơ chế:<br/>
    /// - Lấy độ lệch theo trục X và trục Y giữa hai điểm. <br/>
    /// - Áp dụng công thức căn bậc hai của tổng bình phương khoảng cách theo hai trục. <br/>
    /// - Kết quả được sử dụng để xác định chi phí di chuyển giữa hai điểm trong không gian 2D.
    /// </summary>
    /// <param name="a">Tọa độ điểm thứ nhất.</param>
    /// <param name="b">Tọa độ điểm thứ hai.</param>
    /// <returns>Giá trị khoảng cách Euclid giữa hai điểm.</returns>
    private static double Euclid(PointPos a, PointPos b)
    {
        int dx = a.X - b.X;
        int dy = a.Y - b.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }
}