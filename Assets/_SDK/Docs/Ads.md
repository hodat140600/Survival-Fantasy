# Ads and Analytics

**NOTE:** QUY TRÌNH NÀY RẤT QUAN TRỌNG VÌ NÓ LIÊN QUAN ĐẾN TIỀN, PHẢI ĐẢM BẢO CÓ DOUBLE CHECK (2 NGƯỜI KHÁC NHAU) KHI RELEASE.

## Quy trình

### INPUT từ Marketing Team:
- Google Sheet các Config KEYS để gắn Ads. Ví dụ:
 <img width="300" alt="image" src="https://user-images.githubusercontent.com/1218572/192181425-5f07251d-b064-43ca-92c6-48ba57719472.png">
 
- File google-services.json để thiết lập connect tới Firebase, đây là file export từ Firebase Console:
<img width="300" alt="image" src="https://user-images.githubusercontent.com/1218572/194008791-e24bb67b-638f-4c7d-b17d-e61086f5ada4.png">

- Danh sách các Events cần Log:

<img width="300" alt="image" src="https://user-images.githubusercontent.com/1218572/192414300-0cc705d9-31c7-407f-af62-3b0519a80bb7.png">

### Setup CheckList
 * [ ] Đảm bảo thư mục **Ads** và **Analytics** và file _ApplicationStart.cs_ trong SDK và đã init Firebase và AppsFlyer Service
 * [ ] Thiết lập Package Name: ví dụ _com.bucket.crash.rsg_
 * [ ] Copy keys vào [Ads Config] (https://github.com/rocketsaigon/RocketSgSdk/blob/master/Assets/_SDK/Ads/AdsConfig.cs)
 * [ ] Copy google-services.json và ReImport để đảm bảo file đã được add vào build.
 * [ ] Enable các Ads Networks và Enable MAX Ad Review + AppLoving SDK Key
![image](https://user-images.githubusercontent.com/1218572/192258992-26a87855-5b79-414b-b017-62d5e9fce28f.png)
 * [ ] Log các [Analytics Event](https://github.com/rocketsaigon/RocketSgSdk/blob/master/Assets/_SDK/Analytics/AnalyticsEvent.cs#L6) tương ứng trong game để log trên Firebase và AppsFlyer

### Release - thử nghiệm khép kín trên Google Play trước
<img width="1260" alt="image" src="https://user-images.githubusercontent.com/1218572/194019617-25b74a37-76d1-499d-a0b1-2634b5e849fe.png">
- Sau khi đưa file aab lên đây, có thể check ngay lập tức với Firebase và Ads Intergration

### Test Ads Device
Để loại trừ tình huống Ads Network không trả về Inter nên ads không show -> add device vào test mode theo hướng dẫn này:
https://dash.applovin.com/documentation/mediation/max/get-started-with-max#test-mode


### Google Policy
```
Từ 30/9, toàn bộ interstitals ads xuất hiện trong 1 trong 3 trường hợp sau đây sẽ bị cấm:
1. Xuất hiện đột ngột giữa màn chơi (ví dụ như các dòng Tycoon không chia level và chỉ có capping time hoặc show ở check point)
2. Xuất hiện ở đầu level (ví dụ như FNF show đầu level)
3. Xuất hiện đúng vị trí nhưng không thể tắt sau 15s 
```

## Giải thích Sequence Diagram
https://sequencediagram.org/

```
title Ads and Analytics

participant ApplicationStart
participant Game

participant MaxAdsClient

participant "AppLoving Max(Mediation-Platform)" as AppLoving MAX

participant Firebase

participant AppsFlyer

ApplicationStart -> ApplicationStart: Init Firebase và AppsFlyer
Game -> MaxAdsClient:Setup Client với AdsConfig
MaxAdsClient -> AppLoving MAX: Load Banner, Inter, RV

Game -> Firebase:Lấy Frequency capping trong RemoteConfig
Game -> MaxAdsClient: Show Banner ở Bottom khi Game Start
note over Game:Dựa theo Google Policy, Frequency Capping, Game Rules
Game -> MaxAdsClient:Show Inter khi có thể
Game -> MaxAdsClient: Show RV khi player muốn reward
MaxAdsClient ->Firebase:Log Show Inter, RV
MaxAdsClient -> AppsFlyer:Log Show Inter, RV

```



