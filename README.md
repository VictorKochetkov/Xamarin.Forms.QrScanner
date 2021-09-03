# Xamarin.Forms.QrScanner

<a href="https://www.nuget.org/packages/Xamarin.Forms.QrScanner"><img alt="Nuget" src="https://img.shields.io/nuget/v/Xamarin.Forms.QrScanner"></a> <a href="https://www.nuget.org/packages/Xamarin.Forms.QrScanner"><img alt="Nuget" src="https://img.shields.io/nuget/dt/Xamarin.Forms.QrScanner"></a>

Fast and lightweight QR scanner for Xamarin.Forms (works on Android and iOS) 🚀

> 💡 Install via [NuGet](https://www.nuget.org/packages/Xamarin.Forms.QrScanner)

![ezgif com-gif-maker (1)](https://user-images.githubusercontent.com/11313401/131913282-52b40221-8ae2-45e5-a7a9-5951c32b949f.gif)
![ezgif com-gif-maker](https://user-images.githubusercontent.com/11313401/131913338-8de7ed6a-a8fd-4dac-ae95-dae0edaf95eb.gif)

This implementation uses `GooglePlayServices.Vision` for Android and `AVFoundation` for iOS. 

No overhead dependencies! 🔥

> 💡 Launch [Sample](https://github.com/VictorKochetkov/Xamarin.Forms.QrScanner/tree/main/Sample) app which demonstrates library usage!

# Quick start

Easy to use control - just add `QrScannerView` into your XAML. Supports MVVM pattern!

```xaml
<controls:QrScannerView Result="{Binding ResultCommand}"
                        IsScanning="{Binding IsScanning}"
                        HorizontalOptions="FillAndExpand"
                        VerticalOptions="FillAndExpand"/>
```

# Features

`Result` - command will be executed when QR has recognized

`IsScanning` - enable or disable scan (while you processing a result, for example)

# Donate

If you like this project you can support it by making a donation 🤗 Thank you!

<a href="https://www.buymeacoffee.com/bananadev" target="_blank"><img src="https://www.buymeacoffee.com/assets/img/custom_images/orange_img.png" alt="Buy Me A Coffee"></a>

