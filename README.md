# Service.Template

[![Build status](https://ci.appveyor.com/api/projects/status/178r91ywh63o2pby?svg=true)](https://ci.appveyor.com/project/bltzkrgmchn/service-template)

Шаблон для создания сервиса общего назначения.
  
## Требования

Шаблон использует стандартный механизм шаблонизации ```dotnet new``` и требует установленный ```.NET Core 2.2 SDK```, который можно взять [здесь](https://dotnet.microsoft.com/download/dotnet-core/2.2).

## Установка

Для установки шаблона необходимо склонировать содержимое репозитория и использовать команду

```
.\build.ps1 -target PackageTemplate
```

После выполнения посторения будет создан nuget пакет, содержащий шаблон, который может быть установлен с использованием команды ```dotnet new```.

Для установки шаблона необходимо использовать команду

```
dotnet new --install ".\artefacts\packages\Service.Template.{версия.шаблона}.nupkg"
```

Шаблон будет установлен под именем ```webapi-masstransit```.

Для просмотра всех установленных шаблонов необходимо использовать команду

```
dotnet new --list
```

## Использование

Для создания нового экземпляра сервиса с использованием шаблона необходимо использовать команду 

```
dotnet new --name {название.сервиса} --output путь/к/экземпляру/сервиса webapi-masstransit
```

## Используемые библиотеки

Сервис использует следующие библиотеки:
- MassTransit - абстракция над шиной обмана сообщений
- NUnit - юнит-тесты
- FakeItEasy - создание фейков в юнит-тестах
- FluentAssertions - человеко-читаемые условия в юнит-тестах
- Cake - скрипт посторения
- Paket - менеджер пакетов
- Microsoft.AspNetCore.Mvc - Web Api
- Microsoft.AspNetCore.Server.Kestrel - веб-сервер для резидентного размещения
- Microsoft.Extensions.Configuration - управление конфигурацией
- Microsoft.EntityFrameworkCore - объектно-реляционный модуль сопоставления для работы с базой данных

## Построение сервиса
Для построения сервиса необходимо запустить скрип построения командой

```
./build.ps1
```

## Запуск сервиса

В процессе построения в папке ```./artefacts/packages``` будет создан NuGet пакет с исполняемыми файлами сервиса.

Для запуска сервиса необходимо извлечь содержимое NuGet пакета и выполнить команду

```
dotnet lib/netcoreapp2.2/Service.Template.Instance.dll
```

или запустить исполняемые файлы, созданные в процессе посторения командой

```
dotnet sources/{название.сервиса}.Instance/release/netcoreapp2.2/{название.сервиса}.Instance.dll
```

После успешного запуска сервиса должны быть доступны следующие методы 

- ```GET localhost:5000/placeholders``` 
- ```GET localhost:5000/placeholders/{placeholderId}```  

Методы должны возвращать ответы с кодом ```200 OK``` и содержимым вида

```
{
    "name": "placeholder"
}
``` 