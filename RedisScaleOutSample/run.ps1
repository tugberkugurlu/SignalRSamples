$executingPath = (Split-Path -parent $MyInvocation.MyCommand.Definition)
$appPPath = (join-path $executingPath "RedisScaleOutSample")
$iisExpress = "c:\Program Files (x86)\IIS Express\iisexpress.exe"
$args1 = "/path:$appPPath /port:9090 /clr:v4.0"
$args2 = "/path:$appPPath /port:9091 /clr:v4.0"

start-process $iisExpress $args1 -windowstyle Normal
start-process $iisExpress $args2 -windowstyle Normal