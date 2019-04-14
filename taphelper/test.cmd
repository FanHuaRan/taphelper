@echo off
set i=1
:CLICK
echo click %i%...
 
::此处数字500 500表示需要点击的屏幕坐标，可根据需求自行更改
adb shell input tap 500 500
 
::此处数字5表示延时5秒，可根据需求自行更改
ping 127.0.0.1 -n 5 >nul
set /a i=i+1
goto CLICK