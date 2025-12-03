# 🏴‍☠️ Treasure Path Calculator  
Ứng dụng full-stack tính lượng nhiên liệu tối thiểu để thu thập kho báu trên bản đồ ma trận.  
Frontend sử dụng **React + TypeScript + Material-UI**, backend sử dụng **ASP.NET Core + EF Core + SQLite**.

---

## 📌 1. Mô tả bài toán
Vùng biển chứa kho báu được mô phỏng dưới dạng ma trận `n × m`.  
Mỗi ô chứa một rương mang số từ `1` đến `p`, trong đó:

- Rương `x` chứa chìa khoá để mở rương `x + 1`  
- Rương `p` là rương duy nhất chứa kho báu  
- Vị trí bắt đầu: `(1,1)` với chìa khoá `0`  
- Chi phí di chuyển giữa `(x1, y1)` → `(x2, y2)` là: √((x1 - x2)² + (y1 - y2)²)


**Mục tiêu:** Tính tổng nhiên liệu *nhỏ nhất* để mở được rương `p`.

---

## 🧱 2. Kiến trúc hệ thống

### 🔹 Backend – ASP.NET Core
- API chính: `POST /Awing/calculate-fuel`
- Lưu trữ dữ liệu gồm:
  - Input (TreasureRequest)
  - Output (TreasureResult)
- Database: **SQLite**
- ORM: **Entity Framework Core**
- Service tính toán tối ưu đường đi theo thứ tự rương `1 → 2 → … → p` .
- Ở mỗi bước từ rương `x`, thay vì xét toàn bộ vị trí có giá trị `x + 1` (có thể rất nhiều, đặc biệt với ma trận lớn), vậy nên hệ thống sẽ chỉ:

  - Tính khoảng cách từ vị trí hiện tại đến tất cả các ô mang giá trị `x + 1`.  
  - Chọn ra **K vị trí gần nhất** (Top-K candidates), giá trị K mặc định là 5.  
  - Chỉ mở rộng đường đi từ các ứng viên này, thay vì toàn bộ tập.  

  Điều này sẽ giúp:
  - Giảm mạnh số lượng phép tính khoảng cách
  - Giảm nhánh trong quá trình tìm kiếm
  - Tăng tốc độ khi `n × m` lớn và `p` có nhiều vị trí lặp lại

### 🔹 Frontend – React + TypeScript + Material-UI
- Form nhập `n`, `m`, và ma trận kích thước động  
- Trường `p` được **tính tự động** là giá trị lớn nhất trong ma trận  
- Validation:
  - `n` và `m` > 0  
  - Tất cả phần tử ma trận phải là số dương  
  - `p` phải xuất hiện đúng **1 lần**  
- Gọi API backend để tính kết quả  
- Hiển thị kết quả nhiên liệu sau khi tính toán

---

## 🚀 3. Chạy Backend & Frontend

### 🔧 Backend (ASP.NET Core)

Mở thư mục dự án API và chạy:

```bash
dotnet run
```
Dịch vụ API sẽ bắt đầu lắng nghe tại địa chỉ: http://localhost:5217

### 🌐 Frontend (React + TypeScript + Material-UI)

Mở thư mục dự án UI và chạy:
```bash
npm run dev
```
Ứng dụng frontend sẽ bắt đầu chạy tại địa chỉ: http://localhost:5173/

