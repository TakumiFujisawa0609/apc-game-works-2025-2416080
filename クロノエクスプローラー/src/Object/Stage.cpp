#include "Stage.h"
#include <DxLib.h>
#include <fstream>
#include <sstream>

// JSONライブラリがない環境用に、CSVデータを直接解析する方法
void Stage::LoadFromTiled(const std::string& filename)
{
    blocks_.clear();

    std::ifstream file(filename);
    if (!file.is_open()) return;

    std::string line;
    bool inData = false;
    int y = 0;

    while (std::getline(file, line))
    {
        if (line.find("<data encoding=\"csv\">") != std::string::npos) {
            inData = true;
            continue;
        }
        if (line.find("</data>") != std::string::npos) {
            break;
        }

        if (inData) {
            std::stringstream ss(line);
            std::string value;
            int x = 0;

            while (std::getline(ss, value, ',')) {
                int tileId = std::stoi(value);
                if (tileId == 42) { // 床・壁として扱うID
                    Block b;
                    b.x = x * 32;
                    b.y = y * 32;
                    b.w = 32;
                    b.h = 32;
                    blocks_.push_back(b);
                }
                x++;
            }
            y++;
        }
    }
}
