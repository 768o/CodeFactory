﻿using CodeFactory.Core.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.Service
{
    public class CodeService : ICodeService
    {
        private readonly static string Path = Environment.CurrentDirectory + "\\Data";
        private static List<CodeInfo> CodeInfos { get; set; } = ReadJsonFileToList<CodeInfo>();
        private static List<CodePath> CodePaths { get; set; } = ReadJsonFileToList<CodePath>();

        public async Task<bool> CreateCodeInfo(CodeInfo info)
        {
            info.Id = Guid.NewGuid();
            CodeInfos.Add(info);
            return await Write(CodeInfos);
        }

        public async Task<bool> CreateCodePath(CodePath path)
        {
            path.Id = Guid.NewGuid();
            CodePaths.Add(path);
            return await Write(CodePaths);
        }

        public async Task<bool> DeleteCodeInfo(CodeInfo info)
        {
            CodeInfos = CodeInfos.Where(d => d.Id != info.Id).ToList();
            return await Write(CodeInfos);
        }

        public async Task<bool> DeleteCodePath(CodePath path)
        {
            if (CodePaths.Any(d => d.ParentId == path.Id) || CodeInfos.Any(d => d.PathId == path.Id)) 
            {
                return false;
            }
            CodePaths = CodePaths.Where(d => d.Id != path.Id).ToList();
            return await Write(CodePaths);
        }

        public async Task<List<CodeInfo>> GetCodeInfos(Guid pathId)
        {
            return await Task.FromResult(CodeInfos.Where(d => d.PathId == pathId).ToList());
        }

        public async Task<CodeInfo> GetCodeInfo(CodeInfo info)
        {
            return await Task.FromResult(CodeInfos.FirstOrDefault(d => d.Id == info.Id));
        }

        public async Task<CodePath> GetPathTree(CodePath path)
        {
            var parent = CodePaths.Select(d => new CodePath { 
                Childrens = null, 
                Id = d.Id, 
                Name = d.Name, 
                ParentId = d.ParentId}).ToList().FirstOrDefault(d => d.Id == path.ParentId);
            if (parent == null) return new CodePath();
            GetPathTreeRecursion(parent);
            return await Task.FromResult(parent);
        }

        private void GetPathTreeRecursion(CodePath parent) 
        {
            var childrens = CodePaths.Where(d => d.ParentId == parent.Id).Select(d => new CodePath
            {
                Childrens = null,
                Id = d.Id,
                Name = d.Name,
                ParentId = d.ParentId
            }).ToList();
            if (childrens.Any()) 
            {
                parent.Childrens = childrens;
                parent.Childrens.ForEach(d => {
                    GetPathTreeRecursion(d);
                });
            }
        }

        public async Task<bool> UpdateCodeInfo(CodeInfo info)
        {
            CodeInfos.ForEach(d => {
                if (d.Id == info.Id) 
                {
                    d.Title = info.Title;
                    d.Context = info.Context;
                }
            });
            return await Write(CodeInfos);
        }

        public async Task<bool> UpdateCodePath(CodePath path)
        {
            CodePaths.ForEach(d => {
                if (d.Id == path.Id)
                {
                    d.Name = path.Name;
                }
            });
            return await Write(CodePaths);
        }

        private static async Task<bool> Write<T>(List<T> jsonData)
        {
            string strFileName = Path + $"\\{GetFileName<T>()}";
            string ListJson = JsonConvert.SerializeObject(jsonData);

            writeJsonFile(strFileName, ListJson);

            //将序列化的json字符串内容写入Json文件，并且保存
            static void writeJsonFile(string path, string jsonConents)
            {
                using FileStream fs = new FileStream(path, FileMode.Truncate, System.IO.FileAccess.ReadWrite, FileShare.ReadWrite);
                //如果json文件中有中文数据，可能会出现乱码的现象，那么需要加上如下代码
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                using StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("GB2312"));
                sw.WriteLine(jsonConents);
            }
            return await Task.FromResult(true);
        }

        private static string GetFileName<T>()
        {
            if (typeof(T) == typeof(CodeInfo))
            {
                return "CodeInfo.json";
            }
            else
            {
                return "CodePath.json";
            }
        }

        private static List<T> ReadJsonFileToList<T>()
        {
            //将Json转换回列表
            string strFileName = Path + $"\\{GetFileName<T>()}";
            string jsonData = GetJsonFile(strFileName);
            //反序列化Json字符串内容为对象
            List<T> jsondata = JsonConvert.DeserializeObject<List<T>>(jsonData);
            return jsondata;

        }
        //获取到本地的Json文件并且解析返回对应的json字符串
        private static string GetJsonFile(string filepath)
        {
            string json = string.Empty;
            using (FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                using StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("GB2312"));
                json = sr.ReadToEnd().ToString();
            }
            return json;
        }
    }
}
