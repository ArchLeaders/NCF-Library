#pragma once

#include <oead/byml.h>

#define BYML_LIBRARY_H

#ifdef __cplusplus
extern "C" {
#endif

#ifdef BUILD_MY_DLL
#define BYML_LIBRARY __declspec(dllexport)
#else
#define BYML_LIBRARY __declspec(dllimport)
#endif

void BYML_LIBRARY ConvertToYml(const char* file, char* text);

#ifdef __cplusplus
}
#endif