# Service.Template

[![Build status](https://ci.appveyor.com/api/projects/status/178r91ywh63o2pby?svg=true)](https://ci.appveyor.com/project/bltzkrgmchn/service-template)

Шаблон для создания сервиса общего назначения.

## Требования

Шаблон использует стандартный механизм шаблонизации ```dotnet new``` и требует установленный ```.NET Core 2.2 SDK```, который можно взять [на оффициальном сайте Microsoft](https://dotnet.microsoft.com/download/dotnet-core/2.2).

Для построения и запуска сервиса необходимо наличие установленной версии исполняемой среды ```.Net Core 2.2.8``` и исполняемая среда ```win-x64```.

## Установка

Для установки шаблона необходимо склонировать содержимое репозитория и использовать команду

```powershell
.\build.ps1
```

После выполнения посторения будет создан nuget пакет, содержащий шаблон, который может быть установлен с использованием команды ```dotnet new```.

Для установки шаблона необходимо использовать команду

```powershell
dotnet new --install ".\artifacts\packages\Service.Template.{версия.шаблона}.nupkg"
```

Шаблон будет установлен под именем ```webapi-masstransit```.

Для просмотра всех установленных шаблонов необходимо использовать команду

```powershell
dotnet new --list
```

## Использование

Для создания нового экземпляра сервиса с использованием шаблона необходимо использовать команду

```powershell
dotnet new --name "{название.сервиса}" --output "путь\к\экземпляру\сервиса" webapi-masstransit
```

## Используемые библиотеки

Сервис использует следующие библиотеки:

- MassTransit - абстракция над шиной обмена сообщений
- NUnit - юнит-тесты
- FakeItEasy - создание фейков в юнит-тестах
- FluentAssertions - человеко-читаемые условия в юнит-тестах
- Cake - скрипт посторения
- Microsoft.AspNetCore.Mvc - Web Api
- Microsoft.AspNetCore.Server.Kestrel - веб-сервер для резидентного размещения
- Microsoft.Extensions.Configuration - управление конфигурацией
- Microsoft.EntityFrameworkCore - объектно-реляционный модуль сопоставления для работы с базой данных

## Построение сервиса

Для построения сервиса необходимо запустить скрип построения командой

```powershell
./build.ps1
```

## Запуск сервиса

В процессе построения в папке ```./artifacts/packages``` будет создан NuGet пакет с шаблоном сервиса, а в папку ```./artifacts/publish``` будут опубликованны исполняемые файлы сервиса.

Для запуска сервиса необходимо запустить исполняемый файл сервиса ```Service.Template.exe```, находящийся в папке ```./artifacts/publish```.

```powershell
./artifacts/publish/Service.Template.exe
```

Для запуска сервиса как windows-службы необходимо установить исполняемый файл сервиса ```Service.Template.exe``` с флагом ```--service``` или ```-s``` как windows-службу.

```powershell
New-Service -Name "Service.Temlate" -BinaryPathName "путь\к\папке\с\исходным\кодом\сервиса\artifacts\publish\Service.Template.Instance.exe --service"
```

После успешного запуска сервиса должен быть доступен метод ```GET /health```, который должен возвращать ответ с кодом ```200 OK```.

## Известные проблемы

### .nuspec файлы имет расширение .template

На данный момент ```nuget.exe``` игнорирует все файлы с расширением ```.nuspec``` при сборке пакета.
Подробности можно посмотреть в описании проблемы на [GitHub](https://github.com/NuGet/Home/issues/6862).
