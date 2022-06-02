# Install script for directory: D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/lib/oead

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

# Set default install directory permissions.
if(NOT DEFINED CMAKE_OBJDUMP)
  set(CMAKE_OBJDUMP "C:/MinGW64/bin/objdump.exe")
endif()

if(NOT CMAKE_INSTALL_LOCAL_ONLY)
  # Include the install script for each subdirectory.
  include("D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/.build/lib/oead/lib/abseil/cmake_install.cmake")
  include("D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/.build/lib/oead/lib/EasyIterator/cmake_install.cmake")
  include("D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/.build/lib/oead/lib/libyaml/cmake_install.cmake")
  include("D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/.build/lib/oead/lib/ordered-map/cmake_install.cmake")
  include("D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/.build/lib/oead/lib/rapidyaml/cmake_install.cmake")
  include("D:/Visual Studio/Libraries/EADLibrary/BymlLibrary/Interop/.build/lib/oead/lib/zlib-ng/cmake_install.cmake")

endif()

