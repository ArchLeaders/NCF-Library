        #include <cmrc/cmrc.hpp>
#include <map>
#include <utility>

namespace cmrc {
namespace oead::res {

namespace res_chars {
// These are the files which are available in this resource library
// Pointers to data/aglenv_file_info.json
extern const char* const f_d5c4_data_aglenv_file_info_json_begin;
extern const char* const f_d5c4_data_aglenv_file_info_json_end;
// Pointers to data/botw_hashed_names.txt
extern const char* const f_ef8b_data_botw_hashed_names_txt_begin;
extern const char* const f_ef8b_data_botw_hashed_names_txt_end;
// Pointers to data/botw_numbered_names.txt
extern const char* const f_26c0_data_botw_numbered_names_txt_begin;
extern const char* const f_26c0_data_botw_numbered_names_txt_end;
// Pointers to data/botw_resource_factory_info.tsv
extern const char* const f_00b9_data_botw_resource_factory_info_tsv_begin;
extern const char* const f_00b9_data_botw_resource_factory_info_tsv_end;
}

namespace {

const cmrc::detail::index_type&
get_root_index() {
    static cmrc::detail::directory root_directory_;
    static cmrc::detail::file_or_directory root_directory_fod{root_directory_};
    static cmrc::detail::index_type root_index;
    root_index.emplace("", &root_directory_fod);
    struct dir_inl {
        class cmrc::detail::directory& directory;
    };
    dir_inl root_directory_dir{root_directory_};
    (void)root_directory_dir;
    static auto f_8d77_data_dir = root_directory_dir.directory.add_subdir("data");
    root_index.emplace("data", &f_8d77_data_dir.index_entry);
    root_index.emplace(
        "data/aglenv_file_info.json",
        f_8d77_data_dir.directory.add_file(
            "aglenv_file_info.json",
            res_chars::f_d5c4_data_aglenv_file_info_json_begin,
            res_chars::f_d5c4_data_aglenv_file_info_json_end
        )
    );
    root_index.emplace(
        "data/botw_hashed_names.txt",
        f_8d77_data_dir.directory.add_file(
            "botw_hashed_names.txt",
            res_chars::f_ef8b_data_botw_hashed_names_txt_begin,
            res_chars::f_ef8b_data_botw_hashed_names_txt_end
        )
    );
    root_index.emplace(
        "data/botw_numbered_names.txt",
        f_8d77_data_dir.directory.add_file(
            "botw_numbered_names.txt",
            res_chars::f_26c0_data_botw_numbered_names_txt_begin,
            res_chars::f_26c0_data_botw_numbered_names_txt_end
        )
    );
    root_index.emplace(
        "data/botw_resource_factory_info.tsv",
        f_8d77_data_dir.directory.add_file(
            "botw_resource_factory_info.tsv",
            res_chars::f_00b9_data_botw_resource_factory_info_tsv_begin,
            res_chars::f_00b9_data_botw_resource_factory_info_tsv_end
        )
    );
    return root_index;
}

}

cmrc::embedded_filesystem get_filesystem() {
    static auto& index = get_root_index();
    return cmrc::embedded_filesystem{index};
}

} // oead::res
} // cmrc
    