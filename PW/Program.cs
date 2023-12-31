﻿using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Drawing;

namespace PW;

public class Program
{
    private static readonly string dataFile = @"C:\Users\Public\MyToolsSave\PW\PW_Save.json";
    public static string DataFile
    {
        get => dataFile;
    }

    public static void Main(string[] args)
    {
        #region 读模式 从文件hash得到密码
        if (args[0] == "R") // 读模式
        {
            Dictionary<string, string> pwDict = JsonToDict();
            string fh = FileHash(args[1]);
            if (pwDict.ContainsKey(fh))
            {
                Console.WriteLine("该文件密码:{0}", pwDict[fh]);
                AddTextClipboard(pwDict[fh]);
            }
            else
            {
                WriteWithColor("[Error]:", ConsoleColor.Red);
                Console.WriteLine("未找到该文件密码。");
            }
            Console.Write("按任意键退出...");
            Console.ReadKey();
        }
        #endregion
        #region 写模式 将密码保存到json
        if (args[0] == "W") // 写模式
        {
            Dictionary<string, string> pwDict = JsonToDict();
            string fh = FileHash(args[1]);
            Console.WriteLine("请输入密码");
            if (!pwDict.ContainsKey(fh)) // 找不到 则添加一条
            {
                string pw = Console.ReadLine();
                if (!string.IsNullOrEmpty(pw))
                {
                    pwDict.Add(fh, pw);
                    Console.WriteLine("密码保存成功");
                    // 写入文件
                    DictToJson(pwDict);
                }
            }
            else // 已经有了 触发警告 或 修改这一条
            {
                WriteWithColor("[Warning]:", ConsoleColor.Yellow);
                Console.WriteLine("本文件已保存了密码,密码为 {0} ,密码已复制到剪切板", pwDict[fh]);
                AddTextClipboard(pwDict[fh]);
                Console.WriteLine("输入 Q 回车退出，或者输入新密码");
                string pw = Console.ReadLine();
                if (!string.IsNullOrEmpty(pw))
                {
                    if (pw == "Q" || pw == "q")
                        return;
                    else
                        pwDict[fh] = pw;
                }
                Console.WriteLine("密码保存成功");
                // 写入文件
                DictToJson(pwDict);
            }

            Console.Write("按任意键退出...");
            Console.ReadKey();
        }
        #endregion
        #region help
        if (args[0] == "help")
        {
            Console.WriteLine("密码文件保存在 {0}", dataFile);
            Console.WriteLine("以下为所有密码");
            Dictionary<string, string> pwDict = JsonToDict();
            foreach (var kvp in pwDict)
            {
                Console.WriteLine($"文件标记: {kvp.Key}, 密码: {kvp.Value}");
            }
            Console.Write("按任意键退出...");
            Console.ReadKey();
        }
        #endregion
    }

    #region 文件 hash
    public static string FileHash(string filePath)
    {
        using var md5 = MD5.Create();
        using var stream = File.OpenRead(filePath);
        var hash = md5.ComputeHash(stream);
        var hashValue = BitConverter.ToString(hash).Replace("-", "").ToLower();
        return hashValue;
    }
    #endregion
    #region json 和 Dict 互转
    public static void DictToJson(Dictionary<string, string> data)
    {
        // 将Dictionary转换为JSON字符串
        string json = JsonConvert.SerializeObject(data);

        // 保存JSON字符串到文件
        File.WriteAllText(DataFile, json);
    }

    public static Dictionary<string, string> JsonToDict()
    {
        // 从JSON文件中读取JSON字符串
        string json = File.ReadAllText(DataFile);
        // 将JSON字符串转化为Dictionary
        Dictionary<string, string> data = JsonConvert.DeserializeObject<Dictionary<string, string>>(
            json
        );
        return data!;
    }
    #endregion

    #region 由于不能使用using System.Windows.Forms; 所以要做复制到剪切板的底层内存管理
    [DllImport("user32.dll")]
    private static extern bool OpenClipboard(IntPtr hWndNewOwner);

    [DllImport("user32.dll")]
    private static extern bool CloseClipboard();

    [DllImport("user32.dll")]
    private static extern bool EmptyClipboard();

    [DllImport("user32.dll")]
    private static extern IntPtr SetClipboardData(uint uFormat, IntPtr hMem);

    [DllImport("kernel32.dll")]
    private static extern IntPtr GlobalAlloc(uint uFlags, IntPtr dwBytes);

    [DllImport("kernel32.dll")]
    private static extern IntPtr GlobalLock(IntPtr hMem);

    [DllImport("kernel32.dll")]
    private static extern bool GlobalUnlock(IntPtr hMem);

    private const uint CF_UNICODETEXT = 13;

    static void AddTextClipboard(string textToCopy)
    {
        if (OpenClipboard(IntPtr.Zero))
        {
            EmptyClipboard();

            IntPtr hGlobal = GlobalAlloc(0x0042, (IntPtr)((textToCopy.Length + 1) * 2));
            IntPtr lpStr = GlobalLock(hGlobal);

            Marshal.Copy(textToCopy.ToCharArray(), 0, lpStr, textToCopy.Length);
            GlobalUnlock(hGlobal);

            SetClipboardData(CF_UNICODETEXT, hGlobal);
            CloseClipboard();
            Console.WriteLine("已复制到剪切板: " + textToCopy);
        }
        else
        {
            Console.WriteLine("无法访问剪切板。");
        }
    }
    #endregion

    public static void WriteWithColor(string str, ConsoleColor color)
    {
        ConsoleColor currentForeColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.Write(str);
        Console.ForegroundColor = currentForeColor;
    }
}
