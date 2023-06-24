3 phần này không nhất thiết phải làm trong API mà có thể làm trong MVC cũng được
# Sorting
# Filter
# Paging
- Gọi p: số trang hiện tại
- m: số phần tử tối đa mỗi trang
- Không sợ Take nhiều quá sẽ bị lỗi, nếu nhiều quá thì sẽ lấy số phần tử
tối đa có thể lấy thông
- Công thức: query.Skip((p-1)*m).Take(m)