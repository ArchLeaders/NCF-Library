cmake_minimum_required(VERSION 3.5)
add_subdirectory("./lib/oead")

project(coead VERSION 0.1.0 DESCRIPTION "C# Bindings for OEAD" LANGUAGES CXX)
add_library(coead SHARED src/BymlLibrary.cpp include/BymlLibrary.h)
add_definitions(-DBUILD_MY_DLL)
target_compile_features(coead PRIVATE cxx_std_17)
target_include_directories(coead PRIVATE ${CMAKE_CURRENT_SOURCE_DIR}/include)
target_link_libraries(coead PRIVATE oead)
target_compile_options(coead PRIVATE /clr)
target_compile_options(coead PRIVATE /fp:precise) # /fp:strict is incompatible with /clr