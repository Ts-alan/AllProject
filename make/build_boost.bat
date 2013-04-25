pushd %~dp0..\3rd\boost 

bjam -j 7 toolset=msvc-9.0 runtime-link=static link=static --with-date_time

