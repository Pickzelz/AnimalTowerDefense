@echo off
set APK_NAME=%1
set OUT_NO_EXT=%APK_NAME:~0,-4%
set OUT_UNPACKED=%OUT_NO_EXT%_unpacked
set OUT_HACKED=%OUT_NO_EXT%_hacked.apk 
set OUT_ALIGNED=%OUT_NO_EXT%_hacked_aligned.apk
set LIB_DIR=%~dp0%OUT_UNPACKED%\lib
set X86_DIR=%LIB_DIR%\x86

rem 7. sign new hacked apk
call jarsigner.exe -sigalg MD5withRSA -digestalg SHA1 -keystore .\GAMELOFT_KEY.keystore -storepass 123456 .\%APK_NAME% Gameloft
rem 8. delete temp 
call zipalign.exe -v -f 4 %APK_NAME% %OUT_ALIGNED%
pause