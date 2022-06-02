# Install script for directory: D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/lib/oead/lib/rapidyaml/ext/c4core

# Set the install prefix
if(NOT DEFINED CMAKE_INSTALL_PREFIX)
  set(CMAKE_INSTALL_PREFIX "C:/Program Files (x86)/oead")
endif()
string(REGEX REPLACE "/$" "" CMAKE_INSTALL_PREFIX "${CMAKE_INSTALL_PREFIX}")

# Set the install configuration name.
if(NOT DEFINED CMAKE_INSTALL_CONFIG_NAME)
  if(BUILD_TYPE)
    string(REGEX REPLACE "^[^A-Za-z0-9_]+" ""
           CMAKE_INSTALL_CONFIG_NAME "${BUILD_TYPE}")
  else()
    set(CMAKE_INSTALL_CONFIG_NAME "Release")
  endif()
  message(STATUS "Install configuration: \"${CMAKE_INSTALL_CONFIG_NAME}\"")
endif()

# Set the component getting installed.
if(NOT CMAKE_INSTALL_COMPONENT)
  if(COMPONENT)
    message(STATUS "Install component: \"${COMPONENT}\"")
    set(CMAKE_INSTALL_COMPONENT "${COMPONENT}")
  else()
    set(CMAKE_INSTALL_COMPONENT)
  endif()
endif()

# Is this installation the result of a crosscompile?
if(NOT DEFINED CMAKE_CROSSCOMPILING)
  set(CMAKE_CROSSCOMPILING "FALSE")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  if("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Dd][Ee][Bb][Uu][Gg])$")
    file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/lib" TYPE STATIC_LIBRARY FILES "D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/.build-vs/lib/oead/lib/rapidyaml/subprojects/c4core/build/Debug/c4core.lib")
  elseif("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Rr][Ee][Ll][Ee][Aa][Ss][Ee])$")
    file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/lib" TYPE STATIC_LIBRARY FILES "D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/.build-vs/lib/oead/lib/rapidyaml/subprojects/c4core/build/Release/c4core.lib")
  elseif("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Mm][Ii][Nn][Ss][Ii][Zz][Ee][Rr][Ee][Ll])$")
    file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/lib" TYPE STATIC_LIBRARY FILES "D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/.build-vs/lib/oead/lib/rapidyaml/subprojects/c4core/build/MinSizeRel/c4core.lib")
  elseif("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Rr][Ee][Ll][Ww][Ii][Tt][Hh][Dd][Ee][Bb][Ii][Nn][Ff][Oo])$")
    file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/lib" TYPE STATIC_LIBRARY FILES "D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/.build-vs/lib/oead/lib/rapidyaml/subprojects/c4core/build/RelWithDebInfo/c4core.lib")
  endif()
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/c4" TYPE FILE FILES "D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/lib/oead/lib/rapidyaml/ext/c4core/src/c4/allocator.hpp")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/c4" TYPE FILE FILES "D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/lib/oead/lib/rapidyaml/ext/c4core/src/c4/base64.hpp")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/c4" TYPE FILE FILES "D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/lib/oead/lib/rapidyaml/ext/c4core/src/c4/blob.hpp")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/c4" TYPE FILE FILES "D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/lib/oead/lib/rapidyaml/ext/c4core/src/c4/bitmask.hpp")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/c4" TYPE FILE FILES "D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/lib/oead/lib/rapidyaml/ext/c4core/src/c4/charconv.hpp")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/c4" TYPE FILE FILES "D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/lib/oead/lib/rapidyaml/ext/c4core/src/c4/c4_pop.hpp")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/c4" TYPE FILE FILES "D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/lib/oead/lib/rapidyaml/ext/c4core/src/c4/c4_push.hpp")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/c4" TYPE FILE FILES "D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/lib/oead/lib/rapidyaml/ext/c4core/src/c4/char_traits.hpp")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/c4" TYPE FILE FILES "D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/lib/oead/lib/rapidyaml/ext/c4core/src/c4/common.hpp")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/c4" TYPE FILE FILES "D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/lib/oead/lib/rapidyaml/ext/c4core/src/c4/compiler.hpp")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/c4" TYPE FILE FILES "D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/lib/oead/lib/rapidyaml/ext/c4core/src/c4/config.hpp")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/c4" TYPE FILE FILES "D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/lib/oead/lib/rapidyaml/ext/c4core/src/c4/cpu.hpp")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/c4" TYPE FILE FILES "D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/lib/oead/lib/rapidyaml/ext/c4core/src/c4/ctor_dtor.hpp")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/c4" TYPE FILE FILES "D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/lib/oead/lib/rapidyaml/ext/c4core/src/c4/enum.hpp")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/c4" TYPE FILE FILES "D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/lib/oead/lib/rapidyaml/ext/c4core/src/c4/error.hpp")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/c4" TYPE FILE FILES "D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/lib/oead/lib/rapidyaml/ext/c4core/src/c4/export.hpp")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/c4" TYPE FILE FILES "D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/lib/oead/lib/rapidyaml/ext/c4core/src/c4/format.hpp")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/c4" TYPE FILE FILES "D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/lib/oead/lib/rapidyaml/ext/c4core/src/c4/hash.hpp")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/c4" TYPE FILE FILES "D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/lib/oead/lib/rapidyaml/ext/c4core/src/c4/language.hpp")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/c4" TYPE FILE FILES "D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/lib/oead/lib/rapidyaml/ext/c4core/src/c4/memory_resource.hpp")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/c4" TYPE FILE FILES "D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/lib/oead/lib/rapidyaml/ext/c4core/src/c4/memory_util.hpp")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/c4" TYPE FILE FILES "D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/lib/oead/lib/rapidyaml/ext/c4core/src/c4/platform.hpp")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/c4" TYPE FILE FILES "D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/lib/oead/lib/rapidyaml/ext/c4core/src/c4/preprocessor.hpp")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/c4" TYPE FILE FILES "D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/lib/oead/lib/rapidyaml/ext/c4core/src/c4/restrict.hpp")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/c4" TYPE FILE FILES "D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/lib/oead/lib/rapidyaml/ext/c4core/src/c4/span.hpp")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/c4/std" TYPE FILE FILES "D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/lib/oead/lib/rapidyaml/ext/c4core/src/c4/std/std.hpp")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/c4/std" TYPE FILE FILES "D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/lib/oead/lib/rapidyaml/ext/c4core/src/c4/std/string.hpp")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/c4/std" TYPE FILE FILES "D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/lib/oead/lib/rapidyaml/ext/c4core/src/c4/std/tuple.hpp")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/c4/std" TYPE FILE FILES "D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/lib/oead/lib/rapidyaml/ext/c4core/src/c4/std/vector.hpp")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/c4" TYPE FILE FILES "D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/lib/oead/lib/rapidyaml/ext/c4core/src/c4/substr.hpp")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/c4" TYPE FILE FILES "D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/lib/oead/lib/rapidyaml/ext/c4core/src/c4/szconv.hpp")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/c4" TYPE FILE FILES "D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/lib/oead/lib/rapidyaml/ext/c4core/src/c4/time.hpp")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/c4" TYPE FILE FILES "D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/lib/oead/lib/rapidyaml/ext/c4core/src/c4/type_name.hpp")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/c4" TYPE FILE FILES "D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/lib/oead/lib/rapidyaml/ext/c4core/src/c4/types.hpp")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/c4" TYPE FILE FILES "D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/lib/oead/lib/rapidyaml/ext/c4core/src/c4/unrestrict.hpp")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/c4" TYPE FILE FILES "D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/lib/oead/lib/rapidyaml/ext/c4core/src/c4/windows.hpp")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/c4" TYPE FILE FILES "D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/lib/oead/lib/rapidyaml/ext/c4core/src/c4/windows_pop.hpp")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/c4" TYPE FILE FILES "D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/lib/oead/lib/rapidyaml/ext/c4core/src/c4/windows_push.hpp")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/c4" TYPE FILE FILES "D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/lib/oead/lib/rapidyaml/ext/c4core/src/c4/c4core.natvis")
endif()

