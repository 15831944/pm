:: 启动redis服务器
start D:\Redis-x64-3.2.100\redis-server.exe D:\Redis-x64-3.2.100\redis.windows.conf
:: 启动consul注册中心，本地模式，将会使用127.0.0.1 的ip地址
start D:\consul.exe agent -dev
:: 启动influxd数据库
start D:\pm\tools\influxdb\influxdb-1.7.1-1\influxd.exe -config D:\pm\tools\influxdb\influxdb-1.7.1-1\influxdb.conf
:: 启动IdentityServer服务器
cd D:\pm\Sample\SampleService.IdentityServer\bin\Debug\netcoreapp2.1
start dotnet SampleService.IdentityServer.dll
:: 启动API网关
cd D:\pm\src\Common\NanoFabric.Ocelot\bin\Debug\netcoreapp2.1
start dotnet NanoFabric.Ocelot.dll
:: 启动微服务
cd D:\pm\Sample\SampleService.Kestrel\bin\Debug\netcoreapp2.1
start dotnet SampleService.Kestrel.dll