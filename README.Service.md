# Service.Template

{{serviceDescription}}

## Построение сервиса
Для построения сервиса необходимо запустить скрип построения командой

```
./build.ps1
```

## Запуск сервиса

В процессе построения в папке ```./artifacts/packages``` будет создан NuGet пакет с исполняемыми файлами сервиса.

Для запуска сервиса необходимо извлечь содержимое NuGet пакета и выполнить команду

```
dotnet lib/netcoreapp2.2/Service.Template.Instance.dll
```

или запустить исполняемые файлы, созданные в процессе посторения командой

```
dotnet sources/Service.Template.Instance/release/netcoreapp2.2/Service.Template.Instance.dll
```

После успешного запуска сервиса должнен быть доступен следующий метод: 

- ```GET localhost:5000/health``` 

В случае успешного запуска сервиса будет получен ответ ```200 - OK```. 