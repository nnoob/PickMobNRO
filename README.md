# PickMobNRO
 
Hỗ trợ tự động đánh quái và tự động nhặt vật phẩm

***Lưu ý:***
- Tự động đánh quái sẽ không né siêu quái khi bật TDLT
- Tự động nhặt mà có bật TDLT sẽ dịch chuyển tới vật phẩm để nhặt
- Chỉ nhặt ngọc chỉ là cài đặt lại danh sách lọc vật phẩm của tự động nhặt, tắt cài đặt chỉ nhặt ngọc dùng lệnh "clri" bộ lọc được đặt lại về mặc định (nhặt tất cả vật phẩm).

## Lệnh chat
- add (trỏ vào quái hay vật phẩm): Thêm/Xoá quái hoặc vật phẩm ở danh sách tương ứng

### Tự động đánh quái
- ts: Bật/Tắt tự động đánh quái
- nsq: Bật/Tắt né siêu quái khi tự động đánh quái (Mặc định bật)
- addm***X***: Thêm vào/Xoá khỏi dánh sách đánh quái id ***X*** (***X*** là id quái)
- clrm: Xoá danh sách tàn sát

### Lệnh tự động nhặt vật phẩm
- anhat: Bật/Tắt tự động nhặt vật phẩm
- itm: Bật/Tắt lọc không nhặt vật phẩm của người khác (Mặc định bật)
- sln: Bật/Tắt giới hạn số lần tự động nhặt một vật phẩm (Mặc định bật)
- sln***X***: Thay đổi giới hạn số lần nhặt là ***X*** (Mặc định ***X*** = 7)
- addt***X***: Thêm vào/Xoá khỏi dánh sách chỉ nhặt vật phẩm id ***X*** (***X*** là id vật phẩm)
- blocki (trỏ vào vật phẩm): Thêm vật phẩm vào danh sách chặn
- blocki***X***: Thêm vào/Xoá khỏi dánh sách chặn vật phẩm id ***X***
- cnn: Cài đặt chỉ nhặt ngọc
- clri: Xoá tất cả danh sách lọc vật phẩm (danh sách chỉ nhặt và danh sách chặn)

## Phím tắt
- T: Bật/Tắt tự động đánh quái
- N: Bật/Tắt tự động nhặt vật phẩm
- A: Tương ứng lệnh chat "add"