## Setup Build Environment

### Download Android Studio

Link: https://developer.android.com/studio
Mở Android Studio lên vào File >> Setting >> System Setting >> Android SDK
SDK Platforms chọn tải Android 9, 10, 11, 12
SDK Tool chọn tải 30.0.2,   30.0.3,   31.0.0

### Download NDK

Link: https://drive.google.com/drive/folders/15-VG28C9Tn5mpL4PTF-X_4w8AVOd8tTa
Chỉ download file RAR extract ra một folder bất kì.

### Sử dụng JDK, SDK, NDK

vào Unity >> Edit >>  Preferences >> External Tools
<img width="274" alt="image" src="https://user-images.githubusercontent.com/1218572/183359010-d3c2c081-3aaa-4b9b-a16f-adb8b39aaae7.png">

- Uncheck như trong hình
- Riêng gradle sẽ sử dụng của Unity không cần Uncheck
- JDK >>Browse vào đường dẫn hồi lúc tải Java
- SDK >> Browse >> Mở Android Studio vào File >> Setting >> System Setting >> Android SDK >> Copy cái đường dẫn bỏ vào SDK
<img width="460" alt="image" src="https://user-images.githubusercontent.com/1218572/183359308-5006fc40-ee00-448a-a1e9-e0836068c40c.png">

- NDK >> Browse >> trỏ vào đường dẫn vào NDK đã tải

### Custom Gradle

Mở Unity >> Edit >> Project Setting >> Player >> Publishing Setting 
<img width="230" alt="image" src="https://user-images.githubusercontent.com/1218572/183359467-ae2627f7-91b6-42d0-91a4-47a1f3e57f18.png">

### Chỉnh Player Settings để Publish.
Note: Phải update Version tiếp theo nếu đã từng publish.

<img width="414" alt="image" src="https://user-images.githubusercontent.com/1218572/183364453-dc4c2ee4-cfc5-433e-815c-be5b45674ed3.png">

### Reset Unity:

Khi hoàn thành xong hết save xong tắt unity và mở lại tiến hành build.

Note: Có thể phải mở Unity bằng quyền Admin nếu các file SDK, JDK, NDK để ở ổ C ( mặc định)

## Build APK Files

File gửi cho bên để lấy feedback về chất lượng game sẽ có định dạng APK.

## Build AAB Files

File để up lên Google Play sẽ có định dạng AAB

