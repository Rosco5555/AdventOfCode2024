using System.Runtime.CompilerServices;
using Day9;

var map = new Dictionary<int, List<int>>();

string AddBlock(int start, char c, int id, int fileId)
{
    var ans = "";
    for (int i = 0; i < (c-'0'); i++)
    {
        ans += id.ToString();
    }
   
    map[fileId] = new List<int>(Enumerable.Range(start, (c-'0')).ToList());
    
    return ans;
}

string GetFreeSpace(char c)
{
    var ans = "";
    for (int i = 0; i < (c - '0'); i++)
    {
        ans += '.';
    }

    return ans;
}

string GetBlocks(char[] diskMap)
{
    var blocks = "";
    var fileId = 0;
    for (int i = 0; i < diskMap.Length; i++)
    {
        if (i % 2 == 0) // This is a file block
        {
            blocks += AddBlock(blocks.Length, diskMap[i], fileId%10, fileId);
        }
        else // Free space block
        {
            blocks += GetFreeSpace(diskMap[i]);
            fileId++;
        }
    }
    
    return blocks;
}


// int RemoveIndex(int i)
// {
//     foreach (var (fileId, indices) in map)
//     {
//         if (indices.Contains(i))
//         {
//             indices.Remove(i);
//             map[fileId] = indices;
//             return fileId;
//         }
//     }
//
//     return 0;
// }

// string MoveBlocks(string blocks)
// {
//     var blocksArr = blocks.ToCharArray();
//
//     var p1 = 0;
//     var p2 = blocksArr.Length - 1;
//
//     while (p2 > p1)
//     {
//         if (blocksArr[p1] == '.' && blocksArr[p2] != '.')
//         {
//             var fileId = RemoveIndex(p2);
//             map[fileId].Add(p1);
//             // map[p1] = block;
//             // map[p2] = 0;
//             blocksArr[p1] = blocksArr[p2];
//             blocksArr[p2] = '.';
//             p1++;
//             p2--;
//         }
//         else
//         {
//             if (blocksArr[p1] != '.')
//             {
//                 p1++;
//             }
//             if (blocksArr[p2] == '.')
//             {
//                 p2--;
//             }
//         }
//     }
//
//     var ans = String.Join("", blocksArr);
//     // Console.WriteLine(String.Join("", blocksArr));
//     return ans;
// }

List<int> FindSpace(int n, char[] blocks, int start)
{
    var curr = new List<int>();
    for(int i = 0; i <= start; i++)
    {
        if (curr.Count == n)
        {
            return curr;
        }
        if (blocks[i] == '.')
        {
            curr.Add(i);
        }
        else
        {
            curr = new List<int>();
        }
    }

    return [];
}

char[] Replace(List<int> indices, char c, char[] blocksArr)
{
    foreach (var i in indices)
    {
        blocksArr[i] = c;
    }

    return blocksArr;
}

void MoveFiles()
{
    
    var blocks = GetBlocks(File.ReadAllText("../../../input.txt").ToCharArray());
    
    var blocksArr = blocks.ToCharArray();
    
    var files 
        = map.Select((x => new SysFile(x.Key, x.Value))).OrderByDescending(x=>x.Id).ToList();
    
    foreach(var file in files)
    {
        int length = file.Indices.Count;
        file.Indices.Sort();
        int start = file.Indices.First();
        
        var space = FindSpace(length, blocksArr, start);
        if (space.Count > 0)
        {
            
            var oldIndices = file.Indices;
            var tempValue = 'X';
            file.Indices = space;

            
            blocksArr = Replace(oldIndices, '.', blocksArr);
            blocksArr = Replace(space, tempValue, blocksArr);
            
        }
    }
    
    
    
    var ans = String.Join("", blocksArr);

    foreach (var file in files)
    {
        Console.WriteLine($"{file.Id} {String.Join(",", file.Indices)}");
    }
    
    Console.WriteLine(CheckSum(ans, files));
}

int GetFileId(int i, List<SysFile> files)
{
    foreach(var file in files)
    {
        if (file.Indices.Contains(i))
        {
            return file.Id;
        }
    }

    return 0;
}


long CheckSum(string blocks, List<SysFile> files)
{
    long total = 0;
    long id = 0;

    for(int i = 0; i < blocks.Length; i++)
    {
        if (blocks[i] != '.')
        {
            total += GetFileId(i, files) * i;
        }

    }

    return total;
}




MoveFiles();