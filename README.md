# Wordel

![App preview](img/preview.png)

## Build

C# sdk and runtime are required to build and run native app:

```sh
pacman -Sy dotnet-sdk dotnet-runtime
```

Build

### Android

Android build additionaly requires android dotnet workload:

```sh
sudo dotnet workload install android
```

### Web

Browser build requires wasm-tools:

```sh
sudo dotnet workload install wasm-tools
```

## License

Code for this app is licensed under MIT license.
A copy of the license is available in the [LICENSE](./LICENSE) file.
