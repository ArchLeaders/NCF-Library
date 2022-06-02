#include <nonstd/span.h>
#include <oead/byml.h>

#include <fstream>

void ConvertToYml(const char* file, char* text) {
    std::ifstream stream(file, std::ios::binary);
    std::vector<u8> buffer(std::istreambuf_iterator<char>(stream), {});
    auto byml = oead::Byml::FromBinary(buffer);
    std::strcpy(text, byml.ToText().c_str());
}